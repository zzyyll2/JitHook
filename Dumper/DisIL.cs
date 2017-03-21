using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Dumper
{
    class Disassembler
    {
        StringBuilder m_Output = null;
        Module m_Module = null;
        byte[] m_aIL = null;
        int m_nPos = 0;

        /// <summary>
        /// Disassembles the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public static string Disassemble(MethodBase method)
        {
            return new Disassembler(method).Dis();
        }

        static Dictionary<short, OpCode> staticOpcodes = new Dictionary<short, OpCode>();

        /// <summary>
        /// Initializes the <see cref="Disassembler"/> class.
        /// </summary>
        static Disassembler()
        {
            foreach (FieldInfo fieldInfo in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {

                if (typeof(OpCode).IsAssignableFrom(fieldInfo.FieldType))
                {
                    OpCode opCode = (OpCode)fieldInfo.GetValue(null);
                    if (opCode.OpCodeType != OpCodeType.Nternal)
                    {
                        staticOpcodes.Add(opCode.Value, opCode);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disassembler"/> class.
        /// </summary>
        /// <param name="_methodBase">The _method base.</param>
        Disassembler(MethodBase _methodBase)
        {
            this.m_Module = _methodBase.DeclaringType.Module;
            this.m_aIL = _methodBase.GetMethodBody().GetILAsByteArray();
        }

        /// <summary>
        /// Dises this instance.
        /// </summary>
        /// <returns></returns>
        string Dis()
        {
            this.m_Output = new StringBuilder();
            while (this.m_nPos < m_aIL.Length)
            {
                DisassembleNextInstruction();
            }
            return this.m_Output.ToString();
        }

        /// <summary>
        /// Disassembles the next instruction.
        /// </summary>
        void DisassembleNextInstruction()
        {
            int opStart = this.m_nPos;

            OpCode code = ReadOpCode();
            string sOperand = ReadOperand(code);

            this.m_Output.AppendFormat("IL_{0:X4}:  {1,-12} {2}", opStart, code.Name, sOperand);
            this.m_Output.AppendLine();
        }

        /// <summary>
        /// Reads the operand.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        string ReadOperand(OpCode _opCode)
        {
            int operandLength =
                _opCode.OperandType == OperandType.InlineNone
                ? 0 :
                _opCode.OperandType == OperandType.ShortInlineBrTarget
                || _opCode.OperandType == OperandType.ShortInlineI
                || _opCode.OperandType == OperandType.ShortInlineVar
                ? 1 :
                _opCode.OperandType == OperandType.InlineVar
                ? 2 :
                _opCode.OperandType == OperandType.InlineI8
                || _opCode.OperandType == OperandType.InlineR
                ? 8 :
                _opCode.OperandType == OperandType.InlineSwitch
                ? 4 * (BitConverter.ToInt32(this.m_aIL, this.m_nPos) + 1)
                : 4;

            if (this.m_nPos + operandLength > this.m_aIL.Length)
                throw new Exception("Unexpected end of IL");

            string sResult = FormatOperand(_opCode, operandLength);
            if (sResult == null)
            {
                sResult = "";
                for (int i = 0; i < operandLength; i++)
                {
                    sResult += this.m_aIL[this.m_nPos + i].ToString("X2") + " ";
                }
            }
            this.m_nPos += operandLength;
            return sResult;
        }

        /// <summary>
        /// Reads the op code.
        /// </summary>
        /// <returns></returns>
        OpCode ReadOpCode()
        {
            byte byteCode = this.m_aIL[this.m_nPos++];
            if (staticOpcodes.ContainsKey(byteCode)) return staticOpcodes[byteCode];

            if (this.m_nPos == m_aIL.Length)
                throw new Exception("Cannot find opcode " + byteCode);

            short shortCode = (short)(byteCode * 256 + this.m_aIL[this.m_nPos++]);

            if (!staticOpcodes.ContainsKey(shortCode))
                throw new Exception("Cannot find opcode " + shortCode);

            return staticOpcodes[shortCode];
        }

        string FormatOperand(OpCode _opCode, int _nOperandLength)
        {
            if (_nOperandLength == 0) return "";

            if (_nOperandLength == 4)
                return Get4ByteOperand(_opCode);
            else if (_opCode.OperandType == OperandType.ShortInlineBrTarget)
                return GetShortRelativeTarget();
            else if (_opCode.OperandType == OperandType.InlineSwitch)
                return GetSwitchTarget(_nOperandLength);
            else
                return null;
        }

        /// <summary>
        /// Get4s the byte operand.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        string Get4ByteOperand(OpCode _opCode)
        {
            int intOp = BitConverter.ToInt32(m_aIL, this.m_nPos);

            switch (_opCode.OperandType)
            {
                case OperandType.InlineTok:
                case OperandType.InlineMethod:
                case OperandType.InlineField:
                case OperandType.InlineType:
                    MemberInfo memberInfo = null;
                    try
                    {
                        memberInfo = m_Module.ResolveMember(intOp);
                    }
                    catch
                    {
                        return null;
                    }
                    if (memberInfo == null) return null;

                    if (memberInfo.ReflectedType != null)
                        return memberInfo.ReflectedType.FullName + "." + memberInfo.Name;
                    else if (memberInfo is Type)
                        return ((Type)memberInfo).FullName;
                    else
                        return memberInfo.Name;

                case OperandType.InlineString:
                    string s = m_Module.ResolveString(intOp);
                    if (s != null) s = "\"" + s + "\"";
                    return s;

                case OperandType.InlineBrTarget:
                    return "IL_" + (this.m_nPos + intOp + 4).ToString("X4");

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the short relative target.
        /// </summary>
        /// <returns></returns>
        string GetShortRelativeTarget()
        {
            return "IL_" + (this.m_nPos + (sbyte)this.m_aIL[this.m_nPos] + 1).ToString("X4");
        }

        string GetSwitchTarget(int _nOperandLength)
        {
            int nTargetCount = BitConverter.ToInt32(this.m_aIL, this.m_nPos);
            string[] asTarget = new string[nTargetCount];
            for (int i = 0; i < nTargetCount; i++)
            {
                int nILlTarget = BitConverter.ToInt32(this.m_aIL, this.m_nPos + (i + 1) * 4);
                asTarget[i] = "IL_" + (this.m_nPos + nILlTarget + _nOperandLength).ToString("X4");
            }
            return "(" + string.Join(", ", asTarget) + ")";
        }
    }
}