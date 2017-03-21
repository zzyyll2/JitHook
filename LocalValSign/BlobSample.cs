using System;
using System.IO;
using System.Reflection;

namespace LocalValSign
{
    class BlobSample
    {
        string[] paramnames;
        string[] typerefnames;
        string[] typedefnames;
        int tableoffset;
        int[] offset;
        long valid;
        byte[][] names;
        string[] streamnames;
        int baseofcode;
        int baseofdata;
        int sectiona;
        int filea;
        byte heapsizes;
        int offsetstring = 2;
        int offsetblob = 2;
        int offsetguid = 2;

        public bool tablepresent(int[] rows, byte i)
        {
            int p = (int)(valid >> i) & 1;
            byte[] sizes = { 
            10, 6, 14, 2, 6, 2, 14, 2, 6, 4, 6, 6, 6, 4, 
            6, 8, 6, 2, 4, 2, 6, 4, 2, 6, 6, 6, 2, 2, 8, 
            6, 8, 4, 22, 4, 12, 20, 6, 14, 8, 14, 12, 4 };

            for (int j = 0; j < i; j++)
            {
                int o = sizes[j] * rows[j];
                tableoffset = tableoffset + o;
            }

            if (p == 1)
                return true;
            else
                return false;
        }

        public int ReadBlobIndex(byte[] a, int o)
        {
            int z = 0;

            if (offsetblob == 2)
                z = BitConverter.ToUInt16(a, o);

            if (offsetblob == 4)
                z = (int)BitConverter.ToUInt32(a, o);

            return z;
        }

        public void abc()
        {
            Console.ReadKey();

            long startofmetadata;

            FileStream s = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\LocalValDll.dll", FileMode.Open);
            BinaryReader r = new BinaryReader(s);

            s.Seek(60, SeekOrigin.Begin);

            int ii = r.ReadInt32();
            ii = ii + 4 + 16;
            ii = ii + 24;

            s.Seek(ii, SeekOrigin.Begin);

            baseofcode = r.ReadInt32();
            baseofdata = r.ReadInt32();

            s.Seek(4, SeekOrigin.Current);

            sectiona = r.ReadInt32();
            filea = r.ReadInt32();
            ii = 52;

            s.Seek(ii, SeekOrigin.Current);

            int datad = r.ReadInt32();
            int rva, size;
            ii = 14 * 8;

            s.Seek(ii, SeekOrigin.Current);

            rva = r.ReadInt32();
            size = r.ReadInt32();

            Console.WriteLine("CLI Header RVA={0} Size={1}", rva.ToString("X"), size.ToString("X"));
            int where;

            if (filea != sectiona)
                where = rva % baseofcode + filea;
            else
                where = rva;

            s.Seek(where + 4 + 4, SeekOrigin.Begin);

            rva = r.ReadInt32();

            if (filea != sectiona)
                where = rva % baseofcode + filea;
            else
                where = rva;

            s.Seek(where, SeekOrigin.Begin);

            startofmetadata = s.Position;

            s.Seek(4 + 2 + 2 + 4 + 4 + 12 + 2, SeekOrigin.Current);

            int streams = r.ReadInt16();
            streamnames = new string[5];
            offset = new int[5];
            int[] ssize = new int[5];

            names = new byte[5][];
            names[0] = new byte[10];
            names[1] = new byte[10];
            names[2] = new byte[10];
            names[3] = new byte[10];
            names[4] = new byte[10];

            int i = 0; int j;

            for (i = 0; i < streams; i++)
            {
                offset[i] = r.ReadInt32();
                ssize[i] = r.ReadInt32();
                j = 0;
                byte bb;

                while (true)
                {
                    bb = r.ReadByte();

                    if (bb == 0)
                        break;

                    names[i][j] = bb;
                    j++;
                }

                names[i][j] = bb;
                streamnames[i] = GetStreamNames(names[i]);

                while (true)
                {
                    if (s.Position % 4 == 0)
                        break;

                    byte b = r.ReadByte();
                    if (b != 0)
                    {
                        s.Seek(-1, SeekOrigin.Current);

                        break;
                    }
                }
            }

            byte[] blob = null;
            byte[] strings = null;
            byte[] metadata = null;
            byte[] us;
            byte[] guid;

            for (i = 0; i < streams; i++)
            {
                switch (streamnames[i])
                {
                    case "#~":
                        metadata = new byte[ssize[i]];
                        s.Seek(startofmetadata + offset[i], SeekOrigin.Begin);

                        for (int k = 0; k < ssize[i]; k++)
                            metadata[k] = r.ReadByte();
                        break;
                    case "#Strings":
                        strings = new byte[ssize[i]];
                        s.Seek(startofmetadata + offset[i], SeekOrigin.Begin);

                        for (int k = 0; k < ssize[i]; k++)
                            strings[k] = r.ReadByte();
                        break;
                    case "#US":
                        us = new byte[ssize[i]];
                        s.Seek(startofmetadata + offset[i], SeekOrigin.Begin);

                        for (int k = 0; k < ssize[i]; k++)
                            us[k] = r.ReadByte();
                        break;
                    case "#GUID":
                        guid = new byte[ssize[i]];
                        s.Seek(startofmetadata + offset[i], SeekOrigin.Begin);

                        for (int k = 0; k < ssize[i]; k++)
                            guid[k] = r.ReadByte();
                        break;
                    case "#Blob":
                        blob = new byte[ssize[i]];
                        s.Seek(startofmetadata + offset[i], SeekOrigin.Begin);

                        for (int k = 0; k < ssize[i]; k++)
                            blob[k] = r.ReadByte();
                        break;
                    case "":
                        break;
                    default:
                        break;
                }
            }

            heapsizes = metadata[6];

            if ((heapsizes & 0x01) == 0x01)
            {
                offsetstring = 4;
            }

            if ((heapsizes & 0x02) == 0x02)
            {
                offsetguid = 4;
            }

            if ((heapsizes & 0x08) == 0x08)
            {
                offsetblob = 4;
            }

            valid = BitConverter.ToInt64(metadata, 8);
            tableoffset = 24;
            int[] rows;
            rows = new int[64];
            Array.Clear(rows, 0, rows.Length);

            for (int k = 0; k <= 63; k++)
            {
                int tablepresent = (int)(valid >> k) & 1;

                if (tablepresent == 1)
                {
                    rows[k] = BitConverter.ToInt32(metadata, tableoffset);
                    tableoffset += 4;
                }
            }

            FillParamsArray(metadata, strings, rows);

            DisplayMethodTable(metadata, blob, strings, rows);

            DisplayStandAloneSigTable(metadata, blob, rows);

            Console.ReadKey();
        }

        public void FillParamsArray(byte[] metadata, byte[] strings, int[] rows)
        {
            int old = tableoffset;
            bool b = tablepresent(rows, 8);
            int offs = tableoffset;
            tableoffset = old;

            if (b)
            {
                paramnames = new string[rows[8] + 1];

                for (int k = 1; k <= rows[8]; k++)
                {
                    short pattr = BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int sequence = BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int name = ReadStringIndex(metadata, offs);

                    offs += offsetstring;
                    string s = GetString(strings, name);
                    paramnames[k] = s;
                }
            }

            old = tableoffset;
            b = tablepresent(rows, 1);
            offs = tableoffset;
            tableoffset = old;

            if (b)
            {
                typerefnames = new string[rows[1] + 1];

                for (int k = 1; k <= rows[1]; k++)
                {
                    short resolutionscope = BitConverter.ToInt16(metadata, offs);

                    offs = offs + 2;
                    int name = ReadStringIndex(metadata, offs);

                    offs = offs + offsetstring;
                    int nspace = ReadStringIndex(metadata, offs);

                    offs = offs + offsetstring;

                    string s = GetString(strings, name);
                    string s1 = GetString(strings, nspace);

                    if (s1.Length != 0)
                        s1 = s1 + ".";

                    s1 = s1 + s;

                    typerefnames[k] = s1;
                }
            }

            old = tableoffset;
            b = tablepresent(rows, 2);
            offs = tableoffset;
            tableoffset = old;

            if (b)
            {
                typedefnames = new string[rows[2] + 1];

                for (int k = 1; k <= rows[2]; k++)
                {
                    TypeAttributes flags = (TypeAttributes)BitConverter.ToInt32(metadata, offs);

                    offs += 4;
                    int name = ReadStringIndex(metadata, offs);

                    offs += offsetstring;
                    int nspace = ReadStringIndex(metadata, offs);

                    offs += offsetstring;
                    short cindex = BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int findex = BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int mindex = BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    string s = GetString(strings, name);
                    string s1 = GetString(strings, nspace);

                    if (s1.Length != 0)
                        s1 = s1 + ".";

                    s1 = s1 + s;

                    typedefnames[k] = s;
                }
            }
        }

        public void DisplayMethodTable(byte[] metadata, byte[] blob, byte[] strings, int[] rows)
        {
            int old = tableoffset;
            bool b = tablepresent(rows, 6);
            int offs = tableoffset;
            tableoffset = old;

            if (b)
            {
                for (int k = 1; k <= rows[6]; k++)
                {
                    int rva = BitConverter.ToInt32(metadata, offs);

                    offs += 4;
                    MethodImplAttributes impflags = (MethodImplAttributes)BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int flags = (int)BitConverter.ToInt16(metadata, offs);

                    offs += 2;
                    int name = ReadStringIndex(metadata, offs);

                    offs += offsetstring;
                    int signature = ReadBlobIndex(metadata, offs);

                    offs += offsetblob;
                    int param = BitConverter.ToInt16(metadata, offs);

                    offs += 2;

                    Console.WriteLine("Name : {0}", GetString(strings, name));

                    string s = DisplayMethodSignature(blob, signature, param, GetString(strings, name));
                    Console.WriteLine("Signature: #Blob[{0}] {1}", signature, s);
                }
            }
        }

        public string DisplayMethodSignature(byte[] blob, int index, int param, string name)
        {
            string s = "Count=";
            int cb; int uncompressedbyte;
            int count;

            cb = CorSigUncompressData(blob, index, out uncompressedbyte);
            Console.WriteLine("Count Byte cb={0} uncompresedbyte={1}", cb, uncompressedbyte);

            count = uncompressedbyte;
            s = s + count.ToString() + " Bytes ";

            for (int l = 0; l < count; l++)
                s = s + blob[index + l + cb].ToString() + " ";

            byte[] blob1 = new byte[count];
            Array.Copy(blob, index + cb, blob1, 0, count);

            index = 0;
            cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
            Console.WriteLine("Calling Convention Byte cb={0} uncompresedbyte={1}", cb, uncompressedbyte);

            s = s + GetCallingConvention(uncompressedbyte);
            int paramcount;
            index = index + cb;
            cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
            Console.WriteLine("Parameter Count cb={0} uncompresedbyte={1}", cb, uncompressedbyte);

            paramcount = uncompressedbyte;
            s = s + "Number of Parameters " + paramcount.ToString() + "\n";
            index = index + cb;
            cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
            Console.WriteLine("Return Type cb={0} uncompresedbyte={1}", cb, uncompressedbyte);

            string s1;
            s1 = GetReturnType(blob1, index, out cb);
            s = s + "Return Type:" + s1 + "\n";
            index = index + cb;
            s = s + "Signature " + name + "(";

            for (int l = 1; l <= paramcount; l++)
            {
                cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
                int cb1;
                int bytes = uncompressedbyte;
                Console.WriteLine("Before GetElementType {0}", bytes);

                s1 = GetElementType(blob1, bytes, index, out cb1);
                index = index + cb + cb1;
                s = s + s1 + " " + paramnames[l] + " ";

                if (l != paramcount)
                    s = s + ",";
            }

            s = s + ")\n";

            return s;
        }

        public string GetCallingConvention(int uncompressedbyte)
        {
            int firstbyte = uncompressedbyte;
            byte firstfourbits = (byte)(firstbyte & 0x0f);
            string s = "\nCalling Convention ";

            if (firstfourbits == 0x00)
                s = s + " DEFAULT ";

            if (firstfourbits == 0x05)
                s = s + " VARARG ";

            if ((firstbyte & 0x20) == 0x20)
                s = s + " HASTHIS ";

            if ((firstbyte & 0x40) == 0x40)
                s = s + " EXPLICIT ";

            s = s + "\n";

            return s;
        }

        public int ReadStringIndex(byte[] a, int o)
        {
            int z = 0;

            if (offsetstring == 2)
                z = BitConverter.ToUInt16(a, o);

            if (offsetstring == 4)
                z = (int)BitConverter.ToUInt32(a, o);

            return z;
        }

        public void DisplayStandAloneSigTable(byte[] metadata, byte[] blob, int[] rows)
        {
            int old = tableoffset;
            bool b = tablepresent(rows, 17);
            int offs = tableoffset;
            tableoffset = old;

            if (b)
            {
                for (int k = 1; k <= rows[17]; k++)
                {
                    int index = ReadBlobIndex(metadata, offs);
                    offs += offsetblob;
                    Console.WriteLine("Row {0}", k);

                    byte count = blob[index];
                    string s = DisplayVariablesSignature(blob, index);
                    Console.WriteLine(s);
                }
            }
        }

        public string DisplayVariablesSignature(byte[] blob, int index)
        {
            string s = "Count=";
            int cb; int uncompressedbyte;
            int count;

            cb = CorSigUncompressData(blob, index, out uncompressedbyte);
            count = uncompressedbyte;
            s = s + count.ToString() + " Bytes ";

            for (int l = 1; l <= count; l++)
                s = s + blob[index + l + cb - 1].ToString() + " ";

            byte[] blob1 = new byte[count];
            Array.Copy(blob, index + 1 + cb - 1, blob1, 0, count);
            index = 0;
            cb = CorSigUncompressData(blob1, index, out uncompressedbyte);

            if (uncompressedbyte != 0x07)
            {
                s = "Error";

                return s;
            }

            index = index + cb;
            int paramcount;
            cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
            paramcount = uncompressedbyte;
            index = index + cb;

            for (int l = 1; l <= paramcount; l++)
            {
                cb = CorSigUncompressData(blob1, index, out uncompressedbyte);
                int cb1;
                int bytes = uncompressedbyte;
                string s1 = GetElementType(blob1, bytes, index, out cb1);
                index = index + cb + cb1;
                s = s + s1;

                if (l != paramcount)
                    s = s + " , ";
            }

            return s;
        }

        public int CorSigUncompressData(byte[] b, int index, out int answer)
        {
            int cb = 0; // 标识使用几个字节来存储
            answer = 0; // 值

            if (index >= b.Length)
            {
                return cb;
            }

            // 检查第1位来判断它是否为0，通过和0x80进行AND运算。如果答案为0，就表示最后一位为0，而不是1。
            // 这就表示这个值位于0到127的范围内，并且存储在一个单字节中。
            if ((b[index] & 0x80) == 0x00)
            {
                cb = 1;
                answer = b[index];
            }

            // 这表示接下来的2位存储了未压缩的字节，从而，变量cb被设置为2。
            if ((b[index] & 0xC0) == 0x80)
            {
                cb = 2;
                answer = ((b[index] & 0x3f) << 8) | b[index + 1];
            }

            if ((b[index] & 0xE0) == 0xC0)
            {
                cb = 2;
                answer = ((b[index] & 0x1f) << 24) | (b[index + 1] << 16) | (b[index + 2] << 8) | b[index + 3];
            }

            return cb;
        }

        public string GetStreamNames(byte[] b)
        {
            int i = 0;

            while (b[i] != 0)
            {
                i++;
            }

            System.Text.Encoding e = System.Text.Encoding.UTF8;
            string s = e.GetString(b, 0, i);

            return s;
        }

        public string GetType(int b)
        {
            switch (b)
            {
                case 0x01:
                    return "void";
                case 0x02:
                    return "boolean";
                case 0x03:
                    return "char";
                case 0x04:
                    return "byte";
                case 0x05:
                    return "ubyte";
                case 0x06:
                    return "short";
                case 0x07:
                    return "ushort";
                case 0x08:
                    return "int";
                case 0x09:
                    return "uint";
                case 0x0a:
                    return "long";
                case 0x0b:
                    return "ulong";
                case 0x0c:
                    return "float";
                case 0x0d:
                    return "double";
                case 0x0e:
                    return "string";
                default:
                    return "unknown";

            }
        }

        public string GetString(byte[] strings, int starting)
        {
            int i = starting;

            while (strings[i] != 0)
            {
                i++;
            }

            System.Text.Encoding e = System.Text.Encoding.UTF8;
            string s = e.GetString(strings, starting, i - starting);
            return s;
        }

        public string GetPinnedType(byte[] b, int bytes, int index, out int cb1)
        {
            string s = "";
            int total = 1;
            int uncompressedbyte;

            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            int cb2;

            s = "Pinned " + GetElementType(b, uncompressedbyte, index + 1, out cb2);
            total = total + cb2;
            cb1 = total;

            return s;
        }

        public string GetPointerType(byte[] b, int bytes, int index, out int cb1)
        {
            string s = "";
            int total = 1;
            int uncompressedbyte;

            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            int cb2;

            s = GetElementType(b, uncompressedbyte, index + 1, out cb2) + " *";
            total = total + cb2;
            cb1 = total;

            return s;
        }

        public string GetReferenceType(byte[] b, int bytes, int index, out int cb1)
        {
            string s = "[ByRef] ";
            int total = 1;
            int uncompressedbyte;

            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            int cb2;

            s = s + GetElementType(b, uncompressedbyte, index + 1, out cb2);
            total = total + cb2;
            cb1 = total;

            return s;
        }

        public string GetClassType(byte[] b, int bytes, int index, out int cb1)
        {
            int uncompressedbyte;
            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            Console.WriteLine("Token Count cb={0} uncompresedbyte={1}", cb, uncompressedbyte);

            byte table = (byte)(uncompressedbyte & 0x03);
            int ind = uncompressedbyte >> 2;
            Console.WriteLine("Token Table={0} index={1}", table, ind);

            string s1 = "";

            if (table == 1)
                s1 = typerefnames[ind];

            if (table == 0)
                s1 = typedefnames[ind];

            cb1 = cb;

            return s1;
        }

        public string GetValueType(byte[] b, int bytes, int index, out int cb1)
        {
            int uncompressedbyte;
            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            byte table = (byte)(uncompressedbyte & 0x03);
            int ind = uncompressedbyte >> 2;

            string s1 = "";

            if (table == 1)
                s1 = typerefnames[ind];

            if (table == 0)
                s1 = typedefnames[ind];

            cb1 = cb;

            return s1;
        }

        public string GetReturnType(byte[] b, int index, out int cb)
        {
            string s = "";
            cb = 0;

            if (b[index] <= 0x0e)
            {
                s = GetType(b[index]);
                cb = 1;
            }

            if (b[index] == 0x12)
            {
                int cb1;
                int uncompressedbyte;
                cb1 = CorSigUncompressData(b, index + 1, out uncompressedbyte);
                byte table = (byte)(uncompressedbyte & 0x03);
                int ind = uncompressedbyte >> 2;

                if (table == 1)
                    s = typerefnames[ind];

                if (table == 0)
                    s = typedefnames[ind];

                cb = cb1 + 1;
            }

            return s;
        }

        public string GetElementType(byte[] b, int bytes, int index, out int cb1)
        {
            cb1 = 0;

            string s = "";

            if (bytes <= 0x0e)
            {
                cb1 = 0;
                s = GetType(bytes);
            }

            switch (bytes)
            {
                case 0x12:
                    s = GetClassType(b, bytes, index, out cb1);
                    break;
                case 0x14:
                    s = GetArrayType20(b, bytes, index, out cb1);
                    break;
                case 0x1d:
                    s = GetArrayType29(b, bytes, index, out cb1);
                    break;
                case 0x10:
                    s = GetReferenceType(b, bytes, index, out cb1);
                    break;
                case 0x0f:
                    s = GetPointerType(b, bytes, index, out cb1);
                    break;
                case 0x11:
                    s = GetValueType(b, bytes, index, out cb1);
                    break;
                case 0x45:
                    s = GetPinnedType(b, bytes, index, out cb1);
                    break;
                case 0x1c:
                    s = "System.Object ";
                    break;
                case 0x16:
                    s = "System.TypedReference ";
                    break;
                case 0x18:
                    s = "System.IntPtr ";
                    break;
                case 0x19:
                    s = "System.UIntPtr ";
                    break;
                default:
                    break;
            }

            return s;
        }

        /// <summary>
        /// A multi-dimensional array type modifier.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="cb1"></param>
        /// <returns></returns>
        public string GetArrayType20(byte[] b, int bytes, int index, out int cb1)
        {
            string s;
            int total = 1;
            int uncompressedbyte;
            int rank;
            int numsizes;
            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);

            total = total + cb;
            s = GetElementType(b, uncompressedbyte, index + 1, out cb1);
            total = total + cb1;
            s = s + " [";
            cb = CorSigUncompressData(b, index + total, out uncompressedbyte);
            total = total + cb;
            rank = uncompressedbyte;
            Console.WriteLine("Rank {0}", rank);

            cb = CorSigUncompressData(b, index + total, out uncompressedbyte);
            total = total + cb;
            numsizes = uncompressedbyte;
            Console.WriteLine("Num Sizes {0}", numsizes);

            int[] sizearray = new int[numsizes];

            for (int l = 1; l <= numsizes; l++)
            {
                cb = CorSigUncompressData(b, index + total, out uncompressedbyte);
                total = total + cb;
                sizearray[l - 1] = uncompressedbyte;
            }

            cb = CorSigUncompressData(b, index + total, out uncompressedbyte);
            total = total + cb;

            int bounds = uncompressedbyte;
            int[] boundsarray = new int[bounds];

            for (int l = 1; l <= bounds; l++)
            {
                cb = CorSigUncompressData(b, index + total, out uncompressedbyte);
                total = total + cb;
                int ulSigned = uncompressedbyte & 0x1;
                uncompressedbyte = uncompressedbyte >> 1;
                Console.WriteLine(ulSigned);

                if (ulSigned == 1)
                {
                    if (cb == 1)
                    {
                        uncompressedbyte = (int)(uncompressedbyte | 0xffffffc0);
                    }
                    else if (cb == 2)
                    {
                        uncompressedbyte = (int)(uncompressedbyte | 0xffffe000);
                    }
                    else
                    {
                        uncompressedbyte = (int)(uncompressedbyte | 0xf0000000);
                    }
                }

                boundsarray[l - 1] = uncompressedbyte;
            }

            Console.Write("Size Array ");

            for (int l = 1; l <= numsizes; l++)
            {
                Console.Write("{0} ", sizearray[l - 1]);
            }

            Console.WriteLine();
            Console.Write("Bounds Array ");

            for (int l = 1; l <= bounds; l++)
            {
                Console.Write("{0} ", boundsarray[l - 1]);
            }

            Console.WriteLine();

            for (int l = 0; l < bounds; l++)
            {
                if (sizearray.Length >= l)
                {
                    break;
                }

                int upper = boundsarray[l] + sizearray[l] - 1;

                if (boundsarray[l] == 0 && sizearray[l] != 0)
                {
                    s = s + sizearray[l];
                    if (l != bounds - 1)
                        s = s + ",";
                }

                if (boundsarray[l] == 0 && sizearray[l] == 0)
                    s = s + ",";

                if (boundsarray[l] != 0 && sizearray[l] != 0)
                    s = s + boundsarray[l] + "..." + upper.ToString() + ",";
            }

            int leftover = rank - numsizes;

            for (int l = 1; l < leftover; l++)
                s = s + ",";

            s = s + "]";

            cb1 = total - 1;

            return s;
        }

        /// <summary>
        /// A single-dimensional, zero lower-bound array type modifier.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="cb1"></param>
        /// <returns></returns>
        public string GetArrayType29(byte[] b, int bytes, int index, out int cb1)
        {
            string s;
            int total = 1;
            int uncompressedbyte;

            int cb = CorSigUncompressData(b, index + 1, out uncompressedbyte);
            int cb2;

            s = GetElementType(b, uncompressedbyte, index + 1, out cb2);
            s = s + "[]";
            total = total + cb2;
            cb1 = total;

            return s;
        }
    }
}
