//-----------------------------------------------------------------------------
// Inline IL tool. 
// Author Mike Stall (http://blogs.msdn.com/jmstall)
//
//  Allow inline IL in arbitrary .NET programs (such as C#). This is done
//  via stripping the IL snippets from the source and then injecting them
//  in during an Il roundtrip.
// 
// This is a purely academic tool and not intended for production use.
// See http://blogs.msdn.com/jmstall/archive/2005/02/21/377806.aspx for more details
// about this project.
//-----------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using StringCollection = System.Collections.Specialized.StringCollection;
using System.Collections;

#endregion

namespace InlineIL
{
    // Various generic utility functions. Ideally, these would be things in the BCL.
    static class Util
    {
        // Run a process and block waiting for it to finish.
        // Output to our console.
        // Throws exception if process fails (has non-zero exception code).
        public static void Run(string cmd, string args)
        {
            ProcessStartInfo info = new ProcessStartInfo(cmd, args);
            info.CreateNoWindow = false; // share our window.
            info.UseShellExecute = false;

            Console.WriteLine("   About to run '{0}'", cmd);
            Console.WriteLine("   args:{0}", args);
            Process proc = Process.Start(info);
            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new ArgumentException("Process '" + cmd + "' failed with exit code '" + proc.ExitCode + ")");
            }
        }

        // Insert the snippet array into the source array at the given idx in the source array.
        // Returns the new array. After this returns:
        //    ReturnValue[idxSource] == arraySnippet[0];
        //    ReturnValue.Length == arraySource.Length + arraySnippet.Length;
        // Unfortunately, there doesn't appear to be a good array-insert function, so we write our own.
        public static string[] InsertArrayIntoArray(string[] arraySource, string[] arraySnippet, int idxSource)
        {
            Debug.Assert(arraySource != null);
            Debug.Assert(arraySnippet != null);
            Debug.Assert(idxSource >= 0);
            Debug.Assert(idxSource <= arraySource.Length);

            string[] temp = new string[arraySnippet.Length + arraySource.Length];
            Array.Copy(arraySource, 0, temp, 0, idxSource);
            Array.Copy(arraySnippet, 0, temp, idxSource, arraySnippet.Length);
            Array.Copy(arraySource, idxSource, temp, idxSource + arraySnippet.Length, arraySource.Length - idxSource);

            return temp;
        }
    }

    // Document to represent ILasm output.
    // This class encapsulates all knowledge of the textual representation of the IL. Thus this class
    // is the only place that has to be changed if ilasm/ildasm change.
    class ILDocument
    {
        // We represent the ILasm document as a set of lines. This makes it easier to manipulate it
        // (particularly to inject snippets)
        string[] m_lines;

        // Create a new ildocument for the given module.
        public ILDocument(string pathModule)
        {
            // ILDasm the file to produce a textual IL file.
            //   /linenum  tells ildasm to preserve line-number information. This is needed so that we don't lose
            // the source-info when we round-trip the il.
                        
            string pathTempIl = Path.GetTempFileName();

            // We need to invoke ildasm, which is in the sdk. 
            string pathIldasm = Program.SdkDir + @"\Bin\ildasm.exe";

            // We'd like to use File.Exists to make sure ildasm is available, but that function appears broken.
            // It doesn't allow spaces in the filenam, even if quoted. Perhaps just a beta 1 bug.

            Console.WriteLine("Invoking ildasm on file '{0}' to temp file '{1}'", pathModule, pathTempIl);
            Util.Run(pathIldasm, "\"" + pathModule + "\" /linenum /out=\"" + pathTempIl + "\"");


            // Now read the temporary file into a string list.
            StringCollection temp = new StringCollection();
            using (TextReader reader = new StreamReader(new FileStream(pathTempIl, FileMode.Open)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Remove .maxstack since the inline IL will very likely increase stack size.
                    if (line.Trim().StartsWith(".maxstack"))
                    {
                        line = "// removed .maxstack declaration";
                    }

                    temp.Add(line);
                }
            }
            m_lines = new string[temp.Count];
            temp.CopyTo(m_lines, 0);
        }

        // Save the IL document back out to a file.
        public void EmitToFile(string pathOutputModule)
        {
            string pathTempIl = Path.GetTempFileName();

            // Dump to file.
            using (TextWriter writer = new StreamWriter(new FileStream(pathTempIl, FileMode.Create)))
            {
                foreach (string line in m_lines)
                {
                    string x = line;
                    // ilasm thinks different casings are different source files.
                    if (line.Trim().StartsWith(".line"))
                    {
                        x = line.ToLower();
                    }
                    writer.WriteLine(x);
                }
            }


            string pathIlasm = RuntimeEnvironment.GetRuntimeDirectory() + @"\" + "ilasm.exe";

            // Run ilasm to re-emit it. It will look something like this:
            // 	ilasm t.il /output=t2.exe /optimize /debug 
            //   /optimize tells ilasm to convert long instructions to short forms (eg "ldarg 0 --> ldarg.0")
            //   /debug (instead of /debug=impl) tells the runtime to use explicit sequence points 
            // (which are necessary to single-step the IL instructions that we're inlining)
            Util.Run(pathIlasm, "\"" +pathTempIl + "\" /output=\"" + pathOutputModule + "\" /optimize /debug");
        }

        // Insert a snippet of IL into the document.
        public void InsertSnippet(InlineILSnippet snippet)
        {
            int idxStartLine = snippet.StartLine;
            int idxEndLine = snippet.EndLine;

            // We need to find where to place the IL snippet in our ildasm output.
            // If the IL snippet is at source line (f, g) (inclusive), the ildasm should contain 
            // consecutive .line directives such '.line x' and '.line y' such that (x > f) && (g > y).
            // Once we find such a pair, we can inject the ilasm snippet into the ilasm document at
            // the line before the one containing '.line y'.            

            // intentionally pick MaxValue so that (idxLast < idxStartLine) is false until we initialize idxLast
            int idxLast = int.MaxValue;

            int idxInsertAt = -1;
            int idxIlasmLine = 1; // source files are 1-based.
            foreach (string line in m_lines)
            {
                int idxCurrent = GetLineMarker(line);
                if (idxCurrent != 0)
                {
                    if ((idxLast < idxStartLine) && (idxEndLine < idxCurrent))
                    {                        
                        // What if there are multiple such values of (x,y)? 
                        // Probably should inject at each spot. - which means we may need a while-loop here instead of foreach
                        if (idxInsertAt != -1)
                        {
                            throw new NotImplementedException("ILAsm snippet needs to be inserted at multiple spots.");
                        }

                        // Found snippets location! Insert before ilasm source line idxIlasmLine 
                        // (which is index idxIlasmLine-1, since the array is 0-based)                        
                        idxInsertAt = idxIlasmLine - 1;
                    }

                    idxLast = idxCurrent;
                }

                idxIlasmLine++;
            }

            if (idxInsertAt == -1)
            {
                throw new ArgumentException("Can't find where to place " + snippet);
            }

            m_lines = Util.InsertArrayIntoArray(m_lines, snippet.Lines, idxInsertAt);
        }

        #region IL Text parsing Utility 
        // Is this a line number marker? ".line #,"
        // Returns 0 if this isn't a line marker.
        // Note that this does not work with multiple source files.
        Regex m_reLine = new Regex(@"\s*\.line (\d+),");
        int GetLineMarker(string line)
        {            
            int i = 0;
            Match m = m_reLine.Match(line);
            if (m.Success)
            {
                string val = m.Groups[1].Value;
                if (int.TryParse(val, out i))
                {
                    return i;
                }
            }
            return 0;
        }

        // Get a ILasm string for a sequence point marker. Eg, should look something like:
        //     .line 7,7 : 2,6 'c:\\temp\\t.cs'
        public static string GetILSequenceMarker(string pathSourceFile, int idxLineStart, int idxLineEnd, int idxColStart, int idxColEnd)
        {
            string pathEscapedSourceFile = pathSourceFile.Replace(@"\", @"\\");
            return ".line " + idxLineStart + "," + idxLineEnd + " : " + idxColStart + "," + idxColEnd + " '" + pathEscapedSourceFile + "'";
        }

        // Given a line of IL, determine if it's a statement.
        // We can't add markers to none-statement lines.
        static public bool IsStatement(string line)
        {
            // Parsing the ilasm text lines seems very hacky here. This is one place it would be great if we had
            // a codedom for ilasm.
            // It would be best to run this through a compiled regular expression instead of the adhoc string operations.
            string t = line.Trim();

            // skip blank lines and comments.
            if (t.Length <= 1) return false;
            if (t.StartsWith("//")) return false;

            // The '.' includes all sorts of metacommands that we wan't to skip.
            char ch = t[0];
            if (ch == '.' || ch=='{' || ch == '}') return false;
                    
            if (t.StartsWith("catch")) return false;
            if (t.StartsWith("filter")) return false;

            return true;
        }

        #endregion IL Text parsing Utility
    }

    // Class to represent a snippet of Inline IL
    class InlineILSnippet
    {
        // Create an IL snippet that matches the given range (startLine to endLine, inclusive) in the
        // given source file. 
        // Although we could compute the lines collection from the other 3 parameters, we pass it in for perf reasons
        // since we already have it available.
        public InlineILSnippet(string pathSourceFile, int idxStartLine, int idxEndLine, StringCollection lines)
        {
            m_pathSourcefile    = pathSourceFile;
            m_idxStartLine      = idxStartLine;
            m_idxEndLine        = idxEndLine;

            // This assert would be false if the incoming lines collection has been preprocessed (Eg, if the caller
            // already inject .line directives to map back to the source).
            Debug.Assert(idxEndLine - idxStartLine + 1 == lines.Count);

            // Marshal into an array. Since we're already copying, we'll also inject the sequence point info.
            m_lines = new string[lines.Count * 2];
            
            for (int i = 0; i < lines.Count; i ++)
            {
                int idxSourceLine = idxStartLine + i;
                string sequenceMarker;

                // ILAsm only lets us add sequence points to statements.
                if (ILDocument.IsStatement(lines[i]))
                {
                    sequenceMarker= ILDocument.GetILSequenceMarker(
                     pathSourceFile, idxSourceLine, idxSourceLine, 1, lines[i].Length + 1);
                }
                else
                {
                    sequenceMarker = "// skip sequence marker";
                }

                m_lines[2*i] = sequenceMarker;
                m_lines[2*i + 1] = lines[i];
            }
        }

        public override string ToString()
        {
 	        return "Snippet in file '" + m_pathSourcefile + "' at range (" + m_idxStartLine + "," + m_idxEndLine + ")";
        }


        #region Properties
        // First line of the IL snippet within the source document.
        private int m_idxStartLine;
        public int StartLine
        {
            get { return m_idxStartLine; }
        }

        // Last line (inclusive) of the IL snippet in the source document.
        // Total number of lines in IL snippet is (EndLine - StartLine + 1)
        private int m_idxEndLine;
        public int EndLine
        {
            get { return m_idxEndLine; }
        }

        // Path to source file that IL snippet originally occured in.
        // This can be used to generate sequence points from the snippet back to the original source file.
        private string m_pathSourcefile;
        public string Sourcefile
        {
            get { return m_pathSourcefile; }
        }

        private string[] m_lines;
        public string[] Lines
        {
            get { return m_lines; }
        }

        #endregion Properties

    }

    class Program
    {
        // Searches the given source file for inline IL snippets.
        static InlineILSnippet [] FindInlineILSnippets(ILanguage lang, string pathSourceFile)
        {
            ArrayList snippets = new ArrayList();
            
            // An IL snippet begins at the first line prefixed with the startMarker
            // and goes until the first endMarker after that.
            string stStartMarker = lang.StartMarker.ToLower();
            string stEndMarker   = lang.EndMarker.ToLower();

            TextReader reader = new StreamReader(new FileStream(pathSourceFile, FileMode.Open));

            StringCollection list = null; 

            int idxStartLine = 0; // 0 means we're not tracking a snippet.
            int idxLine = 0; // current line into source file.
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string lineLowercase = line.ToLower();
                idxLine++;
                if (idxStartLine != 0)
                {
                    if (lineLowercase == stEndMarker)
                    {
                        // We found the end of the IL snippet.
                        int idxEndLine = idxLine - 1; // end line was the previous line
                        InlineILSnippet snippet = new InlineILSnippet(pathSourceFile, idxStartLine, idxEndLine, list);
                        snippets.Add(snippet);

                        idxStartLine = 0; // reset tracking the IL snippet.
                        list = null;
                    }
                    else
                    {
                        list.Add(line);
                    }
                }

                if (lineLowercase.StartsWith(stStartMarker))
                {
                    // We found the start of an IL snippet. The actual snippet will start at the next line.
                    list = new StringCollection();
                    idxStartLine = idxLine + 1;
                }
            }

            // If we got to the end of the file and are still tracking, then we have an unterminated inline IL segment.
            if (idxStartLine != 0)
            {
                throw new ArgumentException("Unterminated Inline IL segment in file '" + pathSourceFile + "' starting with '" +
                       stStartMarker + "' at line '" + idxStartLine +
                      "'. Expecting to find closing '" + stEndMarker + "'");
            }

            return (InlineILSnippet[]) snippets.ToArray(typeof(InlineILSnippet));
        }

        #region Language abstractions
        // Abstraction for different languages.
        interface ILanguage
        {
            string PrettyName
            {
                get;
            }
            void Compile(string pathSourceFile, string pathOutFile);

            // Source line that indicates an inline IL snippet is starting. 
            string StartMarker
            {
                get;
            }

            // Source line that ends an inline IL snippet. Must match with StartMarker.
            string EndMarker
            {
                get;
            }
        }

        // Language service for the VB.Net compiler.
        class VisualBasicLanguage : ILanguage
        {
            public string PrettyName
            {
                get { return "VB.Net Compiler (vbc.exe)"; }
            }
            public void Compile(string pathSourceFile, string pathOutFile)
            {   
                // VB's /out switch is broken because it will automatically append a ".exe" extension if it's not already present.
                // This means VB won't actually write out to the file we tell it to.
                string pathToolRoot = RuntimeEnvironment.GetRuntimeDirectory();
                string pathCompiler = pathToolRoot + "vbc.exe";
                Util.Run(pathCompiler, "/out:\"" + pathOutFile + "\" \"" + pathSourceFile + "\" /debug+");                
                
            }

            public string StartMarker
            {
                get { return "#If IL Then"; }
            }
            public string EndMarker
            {
                get { return "#End If"; }
            }
        }

        // Language service for the C# compiler.
        class CSharpLanguage : ILanguage
        {
            public string PrettyName
            {
                get { return "C# Compiler (CSC.exe)"; }
            }
            public void Compile(string pathSourceFile, string pathOutFile)
            {
                string pathToolRoot = RuntimeEnvironment.GetRuntimeDirectory();
                string pathCompiler = pathToolRoot + "csc.exe";
                Util.Run(pathCompiler, "/out:\"" + pathOutFile + "\" \"" + pathSourceFile + "\" /debug+");
            }

            public string StartMarker
            {
                get { return "#if IL"; }
            }
            public string EndMarker
            {
                get { return "#endif"; }
            }
        }
        #endregion // Language abstractions


        #region SDK Property
        public static string SdkDir
        {
            get { return m_pathSdkDir; }
        }
        static string m_pathSdkDir;
        static void InitSdkDir()
        {
            Console.WriteLine(@"Determining where the SDK is. Looking at %FrameworkSDKDir% environment var.");

            // Need to determine Sdk path. The vcvars batch script will set the env var FrameworkSDKDir            
            m_pathSdkDir = Environment.GetEnvironmentVariable("FrameworkSDKDir");
            if (m_pathSdkDir == null)
            {
                // If we can't find it, use the default location for V2.0 installed with VS.
                m_pathSdkDir = @"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0";
                Console.WriteLine("Var is not set. Assuming sdk is at '{0}'. (The default location provided by VS 2005).", m_pathSdkDir);
            }
            else
            {
                Console.WriteLine(@"Var is set. SDK is at '{0}'.", m_pathSdkDir);
            }

        }
        #endregion // SDK Property

        // Main entry point.
        static int Main(string[] args)
        {
            Console.WriteLine("Inline IL post-compiler tool");
            Console.WriteLine("This is an academic project only and not meant for commerical use.");
            Console.WriteLine(@"See http://blogs.msdn.com/jmstall/archive/2005/02/21/377806.aspx for details.");
            Console.WriteLine();


            if (args.Length != 1)
            {
                // It would be nice to have a real Help screen here.                
                Console.WriteLine("Error: Expecting exactly 1 input filename on command line.");
                Console.WriteLine("    example: 'Inlineil.exe Foo.cs'");
                return 1;
            }

            InitSdkDir();

            string pathSourceFile = Path.GetFullPath(args[0]);
            string pathFinalOut = Path.ChangeExtension(pathSourceFile, ".exe");
            ILanguage lang = GetLanguageForFile(pathSourceFile);

            try
            {
                return MainWorker(lang, pathSourceFile, pathFinalOut);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return 1;
            }
            catch
            {
                Console.WriteLine("Unknown error:");
                return 2;
            }
        }

        // Get a language service for the source file.
        static ILanguage GetLanguageForFile(string pathSourceFile)
        {
            string ext = Path.GetExtension(pathSourceFile);
            if (String.Compare(ext, ".vb", true) == 0)
            {
                return new VisualBasicLanguage();
            }
            if (String.Compare(ext, ".cs" ,true) == 0)
            {
                return new CSharpLanguage();
            }
            Console.WriteLine("** Can't identify language for '{0}', using C#.", pathSourceFile);
            return new CSharpLanguage();
        }

        static void WriteMarker()
        {
            Console.WriteLine("------------------------------------------------");
        }

        static int MainWorker(ILanguage lang, string pathSourceFile, string pathFinalOut)
        {
            // Some compilers (like VBC) get confused if the output extension isn't .exe.
            string pathTempOut = Path.ChangeExtension(Path.GetTempFileName(), ".exe");

            // First invoke the high-level compiler
            WriteMarker();
            Console.WriteLine("Compiling input file '" + pathSourceFile + "' with '" + lang.PrettyName + "' to output '" + pathFinalOut + "'");
            Console.WriteLine("Compiling to temporary file:" + pathTempOut);
            lang.Compile(pathSourceFile, pathTempOut);

            // Now, IL-disassemble it.
            WriteMarker();
            ILDocument doc = new ILDocument(pathTempOut);

            InlineILSnippet[] snippets = FindInlineILSnippets(lang, pathSourceFile);

            // Reinject the snippets.
            foreach (InlineILSnippet s in snippets)
            {
                Console.WriteLine("Found:" +s);
                foreach (string x in s.Lines)
                {
                    Console.WriteLine("   :" + x);
                }
                
                doc.InsertSnippet(s);
            }

            // Now re-emit the new IL.
            WriteMarker();
            doc.EmitToFile(pathFinalOut);

            // Since they're doing direct IL manipulation, we really should run peverify on the output. 
            WriteMarker();
            Console.WriteLine("runnning PEVerify on '{0}'.", pathFinalOut);
            Util.Run(SdkDir + @"\bin\PEverify.exe", pathFinalOut);
            Console.WriteLine("PEVerify passed!");

            return 0;
        } // end main
    }
}
