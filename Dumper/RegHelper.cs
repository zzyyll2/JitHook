using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Dumper
{
    class RegHelper
    {
        // Fields
        private const int ERROR_BUFFER_OVERFLOW = 0x6f;
        private const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        private const int MAX_ADAPTER_DESCRIPTION_LENGTH = 0x80;
        private const int MAX_ADAPTER_NAME_LENGTH = 0x100;

        [DllImport("iphlpapi.dll", CharSet = CharSet.Ansi)]
        public static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref long pBufOutLen);

        #region 结构

        [StructLayout(LayoutKind.Sequential)]
        public struct IP_ADAPTER_INFO
        {
            public IntPtr Next;
            public int ComboIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string AdapterName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x84)]
            public string AdapterDescription;
            public uint AddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Address;
            public int Index;
            public uint Type;
            public uint DhcpEnabled;
            public IntPtr CurrentIpAddress;
            public RegHelper.IP_ADDR_STRING IpAddressList;
            public RegHelper.IP_ADDR_STRING GatewayList;
            public RegHelper.IP_ADDR_STRING DhcpServer;
            public bool HaveWins;
            public RegHelper.IP_ADDR_STRING PrimaryWinsServer;
            public RegHelper.IP_ADDR_STRING SecondaryWinsServer;
            public int LeaseObtained;
            public int LeaseExpires;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IP_ADDR_STRING
        {
            public IntPtr Next;
            public RegHelper.IP_ADDRESS_STRING IpAddress;
            public RegHelper.IP_ADDRESS_STRING Mask;
            public int Context;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IP_ADDRESS_STRING
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
            public string Address;
        }

        #endregion

        public RegHelper()
        {
        }

        /// <summary>
        /// 激活码计算
        /// </summary>
        /// <param name="code">激活码</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private string CalcSegment5(string code)
        {
            char[] V_5 = new char[1];

            V_5[0] = (char)0x2d; // '-'

            string[] V_0 = code.Split(V_5);

            long V_1 = Convert.ToInt64(V_0[0], 0x10); // 第一个元素转换成10进制

            long V_2 = Convert.ToInt64(V_0[1], 0x10); // 第二个元素转换成10进制

            char V_6 = code[0x15]; // 第21位字符

            int V_3 = int.Parse(V_6.ToString()); // 转换成数字

            long V_4 = (V_1 / V_3 + V_2) % 0x100000; // 取余

            return V_4.ToString("X5"); // 转换成16进制，五位精度
        }

        /// <summary>
        /// num位数值相加的合去掉最后一位于checksum相等,比如num=1028,各位相加为11,去掉最后一位为1
        /// 然后与checksum比较,如果相等,返回true,否则,返回false
        /// </summary>
        /// <param name="num"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        protected bool CheckSum(int num, int checksum)
        {
            int V_0 = 0;
            string V_1 = num.ToString();

            // 每位相加取合
            for (int V_2 = 0; V_2 < V_1.Length; V_2++)
            {
                char V_5 = V_1[V_2];
                V_0 += int.Parse(V_5.ToString());
            }

            string V_3 = V_0.ToString();

            char V_6 = V_3[V_3.Length - 1];

            int V_4 = int.Parse(V_6.ToString());

            return V_4 == checksum;
        }

        /// <summary>
        /// 混淆
        /// </summary>
        /// <param name="originalStr"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string Confuse(string originalStr, int factor)
        {
            int V_0 = originalStr.Length;
            for (int V_1 = 0; V_1 < V_0; V_1++)
            {
                if ((V_1 + factor) < V_0)
                {
                    char V_2 = originalStr[V_1];
                    char V_3 = originalStr[V_1 + factor];
                    originalStr.Remove(V_1, 1);
                    originalStr.Insert(V_1, V_3.ToString());
                    originalStr.Remove(V_1 + factor, 1);
                    originalStr.Insert(V_1 + factor, V_2.ToString());
                }
            }
            return originalStr;
        }

        /// <summary>
        /// 是否包含有效的授权人数
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ContainsPowerUserLicense(string serialNumber)
        {
            SerialNumberInfo V_0 = this.DecodeSerialNumber(serialNumber);
            return V_0.NumberOfPowerUsers != 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual bool ContainsSiteLicense(string serialNumber)
        {
            return false;
        }

        /// <summary>
        /// 解密序列号（23位序列号）
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        protected virtual SerialNumberInfo DecodeSerialNumber(string serialNumber)
        {
            string V_0 = serialNumber.Replace("-", "").Substring(0, 15); // 23位序列号去掉"-"后变成20位，然后取前15位。

            long V_6 = Convert.ToInt64(V_0, 0x10); // 16进制转换成10进制（没错）

            string V_1 = V_6.ToString(); // 转换成字符串

            string V_2 = this.Reverse(V_1); // 前后反转

            int V_3 = int.Parse(V_2.Substring(1, 5)); // 从1开始,取5位：1、2、3、4、5

            int V_4 = int.Parse(V_2.Substring(7, 2)); // 从7开始，取2位：7、8

            int V_5 = int.Parse(V_2.Substring(12, 3)); // 从12开始，取3位：12、13、14

            if (!this.CheckSum(V_3, int.Parse(V_2.Substring(6, 1)))) // 与6、位比较 （CountryCode）
            {
                throw new Exception();
            }
            if (!this.CheckSum(V_4, int.Parse(V_2.Substring(9, 1)))) // 与9、位比较 （RegionCode）
            {
                throw new Exception();
            }
            if (!this.CheckSum(V_5, int.Parse(V_2.Substring(15, 1)))) // 与15、位比较 （NumberOfPowerUsers）
            {
                throw new Exception();
            }

            return new SerialNumberInfo(V_3, V_4, V_5);
        }

        /// <summary>
        /// 提取激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private ActivationCodeInfo ExtractActivationCode(string actCode)
        {
            string V_0 = actCode.Replace("-", ""); // 去掉连字符

            char V_4 = V_0[0x15]; // 取第21位字符

            int V_1 = Convert.ToInt16(V_4.ToString(), 0x10); // 转换成10进制数字

            V_0 = V_0.Substring(0, 0x15); // 取前21位

            V_0 = this.Confuse(V_0, V_1); // 混淆

            char V_5 = V_0[20]; // 取第20位

            V_1 = Convert.ToInt16(V_5.ToString(), 0x10); // 转换成10进制数字

            V_0 = V_0.Substring(0, 20); // 取前20位

            V_0 = this.Confuse(V_0, V_1); // 混淆

            char V_6 = V_0[0x12]; // 最第18位

            V_1 = Convert.ToInt16(V_6.ToString(), 0x10); // 转换成数字

            V_0 = V_0.Substring(0, 0x12); // 取前18位

            V_0 = this.Confuse(V_0, V_1); // 混淆

            V_0 = this.Reverse(V_0); // 反转

            string V_2 = V_0.Substring(2, 12); // 从第2位开始取12位

            string V_3 = V_0.Substring(14, 3); // 从第14开始取3位

            return new ActivationCodeInfo(V_2, Convert.ToInt16(V_3, 0x10)); // 生成激活码信息类
        }

        /// <summary>
        /// 生成要求码
        /// </summary>
        /// <param name="serialNumber">序列号</param>
        /// <param name="mCode">MAC地址</param>
        /// <returns></returns>
        public string GenerateRequestCode(string serialNumber, string mCode)
        {
            string V_1 = mCode;

            if (string.IsNullOrEmpty(V_1))
            {
                return string.Empty;
            } 
            
            SerialNumberInfo V_2 = this.DecodeSerialNumber(serialNumber); // 解密序列号

            Random V_3 = new Random();
            int V_10 = V_3.Next(0xff); // 随机数,最大值255(0~FF)
            int V_11 = V_3.Next(14) + 1; // 随机数,最大值15(1~F)

            string V_4 = V_10.ToString("X").PadLeft(2, V_11.ToString("X")[0]); // V_11生成的值填充V_10（二位）

            V_1 += V_4; // 与MAC地址连起来

            string V_5 = V_2.NumberOfPowerUsers.ToString("X").PadLeft(3, (char)0x30); // 将授权数转换成16进制,用0填充到3位(最大FFF个授权码).

            System.Threading.Thread.Sleep(1);

            int V_12 = V_3.Next(14) + 1; // 随机数,最大值15(1~F)
            string V_6 = V_12.ToString("X"); // 转换成16进制

            System.Threading.Thread.Sleep(1);

            int V_7 = V_3.Next(5) + 2; // 随机数,最大值5(2~7)

            string V_8 = V_1 + V_5 + V_6; // 与MAC地址连起来
            V_8 = this.Reverse(V_8); // 反转
            V_8 = this.Confuse(V_8, V_7); // 混淆

            System.Threading.Thread.Sleep(1);

            int V_13 = V_3.Next(14) + 1; // 随机数,最大值15(1~F)

            string V_9 = V_8 + V_7.ToString("X") + V_13.ToString("X"); // 与MAC地址连起来

            object[] V_14 = new object[7];

            V_14[0] = V_9.Substring(0, 5);

            V_14[1] = (char)0x2d; // '-'

            V_14[2] = V_9.Substring(5, 5);

            V_14[3] = (char)0x2d; // '-'

            V_14[4] = V_9.Substring(10, 5);

            V_14[5] = (char)0x2d; // '-'

            V_14[6] = V_9.Substring(15, 5);

            return string.Concat(V_14);
        }

        /// <summary>
        /// 生成要求码（用于在线注册）
        /// </summary>
        /// <param name="serialNumber">序列号</param>
        /// <returns></returns>
        public string GenerateRequestCode(string serialNumber)
        {
            StringCollection V_0 = GetMacAddressStrs(); // 获取所有网卡的MAC地址

            string V_1;

            // 取第一个网卡的MAC地址
            if (V_0.Count > 0)
            {
                V_1 = V_0[0];
            }
            else
            {
                V_1 = string.Empty;
            }

            return GenerateRequestCode(serialNumber, V_1);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int GetCountryCode(string serialNumber)
        {
            return -1;
        }

        /// <summary>
        /// 计算网卡MAC地址
        /// </summary>
        /// <returns></returns>
        private static StringCollection GetMacAddressStrs()
        {
            StringCollection V_0 = new StringCollection();

            long V_1 = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));

            IntPtr V_2 = Marshal.AllocHGlobal(new IntPtr(V_1));

            int V_3 = GetAdaptersInfo(V_2, ref V_1);

            if (V_3.Equals(0x6f))
            {
                V_2 = Marshal.ReAllocHGlobal(V_2, new IntPtr(V_1));

                V_3 = GetAdaptersInfo(V_2, ref V_1);
            }

            if (V_3.Equals(0))
            {
                IntPtr V_4 = V_2;
                do
                {
                    IP_ADAPTER_INFO V_5 = (IP_ADAPTER_INFO)Marshal.PtrToStructure(V_4, typeof(IP_ADAPTER_INFO));
                    string V_6 = string.Empty;
                    for (int i = 0; i < V_5.AddressLength - 1; i++)
                    {
                        V_6 += string.Format("{0:X2}", V_5.Address[i]);
                    }
                    V_0.Add(string.Format("{0}{1:X2}", V_6, V_5.Address[V_5.AddressLength - 1]));
                    V_4 = V_5.Next;
                } while (V_4 != IntPtr.Zero);
            }

            Marshal.FreeHGlobal(V_2);

            return V_0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int GetProductCode(string serialNumber)
        {
            return -1;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int GetSiteLicense(string serialNumber)
        {
            return -1;
        }

        /// <summary>
        /// 通过激活码来找出本机MAC是否合法
        /// </summary>
        /// <param name="actCode"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetValidMCode(string actCode)
        {
            ActivationCodeInfo V_0 = this.ExtractActivationCode(actCode); // 提取激活码

            StringCollection V_1 = GetMacAddressStrs(); // 获取本机网卡MAC地址

            StringEnumerator V_4 = V_1.GetEnumerator();
            try
            {
                while (V_4.MoveNext())
                {
                    string V_2 = V_4.Current;
                    if (V_2.Equals(V_0.Mac))
                    {
                        string V_3 = V_0.Mac;
                        return V_3;
                    }
                }
            }
            finally
            {
                if (V_4 is IDisposable)
                {
                    ((IDisposable)V_4).Dispose();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 序列号是否合法
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool IsValidSerialNumber(string serialNumber)
        {
            if (serialNumber.Length != this.SerialNumberLength)
            {
                return false;
            }
            bool V_0 = false;
            try
            {
                this.DecodeSerialNumber(serialNumber);
                V_0 = true;
            }
            catch
            {
                V_0 = false;
            }
            return V_0;
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string Reverse(string str)
        {
            char[] loc0 = str.ToCharArray();
            Array.Reverse(loc0);
            string loc1 = string.Empty;
            foreach (char loc2 in loc0)
            {
                loc1 += loc2;
            }
            return loc1;
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateLicense(string actCode)
        {
            int V_0 = 0;
            return ValidateLicense(actCode, out V_0);
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <param name="onRegister">是否注册</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateLicense(string actCode, bool onRegister)
        {
            int V_0 = 0;
            return this.ValidateLicense(actCode, out V_0, onRegister);
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <param name="pws">授权人数</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateLicense(string actCode, out int pws)
        {
            return this.ValidateLicense(actCode, out pws, false);
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <param name="mCode">MAC地址</param>
        /// <param name="pws">授权人数</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateLicense(string actCode, string mCode, out int pws)
        {
            pws = 0;

            // 校验激活码
            if (!this.ValidateSegment5(actCode, false))
            {
                return false;
            }

            // 提取激活码
            ActivationCodeInfo V_0 = this.ExtractActivationCode(actCode);

            // 授权人数
            pws = V_0.NumberOfPowerUsers;

            // 激活码的MAC地址是否与参数MAC地址相同
            return mCode.Equals(V_0.Mac);
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <param name="pws">授权数</param>
        /// <param name="onRegister">是否是注册</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateLicense(string actCode, out int pws, bool onRegister)
        {
            pws = 0;

            // 校验激活码
            if (!this.ValidateSegment5(actCode, onRegister))
            {
                return false;
            }

            // 提取激活码
            ActivationCodeInfo V_0 = this.ExtractActivationCode(actCode);

            // 授权人数
            pws = V_0.NumberOfPowerUsers;

            // 本机网卡MAC地址
            StringCollection V_1 = GetMacAddressStrs(); // 0B

            StringEnumerator V_4 = V_1.GetEnumerator();

            // 如果激活码中的MAC地址包含本机MAC地址,则验证通过
            try
            {
                while (V_4.MoveNext())
                {
                    string V_2 = V_4.Current;
                    if (V_2.Equals(V_0.Mac))
                    {
                        bool V_3 = true;
                        return V_3;
                    }
                }
            }
            finally
            {
                if (V_4 is IDisposable)
                {
                    ((IDisposable)V_4).Dispose();
                }
            }
            return false;
        }

        /// <summary>
        /// 校验激活码
        /// </summary>
        /// <param name="actCode">激活码</param>
        /// <param name="onRegister">是否注册</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private bool ValidateSegment5(string actCode, bool onRegister)
        {
            string V_0 = actCode.Substring(0x18); // 从第24位开始取

            bool V_1 = V_0.Equals(this.CalcSegment5(actCode));

            if (onRegister)
            {
                return V_1;
            }

            char V_2 = V_0[0]; // 取第一个字符

            char V_3 = V_0[1]; // 取第二个字符

            char V_4 = V_0[2]; // 取第三个字符

            bool V_5 = (V_2 >= '2') && (V_2 <= '6') && (V_3 >= '2') && (V_3 <= '6') && (V_4 != '0');

            if (!V_1)
            {
                return V_5;
            }

            return true;
        }

        /// <summary>
        /// 此类的序列号长度为23
        /// </summary>
        internal virtual int SerialNumberLength
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return 0x17; // 23
            }
        }

        #region 信息类

        /// <summary>
        /// 激活码信息类
        /// </summary>
        protected class ActivationCodeInfo
        {
            /// <summary>
            /// MAC地址
            /// </summary>
            public string Mac;

            /// <summary>
            /// 授权人数
            /// </summary>
            public int NumberOfPowerUsers;

            // Methods
            [MethodImpl(MethodImplOptions.NoInlining)]
            public ActivationCodeInfo(string mac, int numberOfPowerUsers)
            {
                this.Mac = mac;
                this.NumberOfPowerUsers = numberOfPowerUsers;
            }
        }

        /// <summary>
        /// 序列号信息类
        /// </summary>
        protected class SerialNumberInfo
        {
            /// <summary>
            /// 国家码
            /// </summary>
            public int CountryCode;

            /// <summary>
            /// 授权人数
            /// </summary>
            public int NumberOfPowerUsers;

            /// <summary>
            /// 地区码
            /// </summary>
            public int RegionCode;

            // Methods
            [MethodImpl(MethodImplOptions.NoInlining)]
            public SerialNumberInfo(int countryCode, int regionCode, int numberOfPowerUsers)
            {
                this.CountryCode = countryCode;
                this.RegionCode = regionCode;
                this.NumberOfPowerUsers = numberOfPowerUsers;
            }
        }

        #endregion

    }
}
