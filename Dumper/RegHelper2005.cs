using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Dumper
{
    class RegHelper2005 : RegHelper
    {
        // Methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override bool ContainsSiteLicense(string serialNumber)
        {
            SerialNumberInfo2005 V_0 = this.DecodeSerialNumber(serialNumber) as SerialNumberInfo2005;
            return V_0.SiteLicense == 1;
        }

        /// <summary>
        /// 运行成功，则序列号是正确的（29位序列号(44E0D-F2DE8-7A0CC-A323E-0BED3)）
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override RegHelper.SerialNumberInfo DecodeSerialNumber(string serialNumber)
        {
            string V_0 = serialNumber.Replace("-", ""); // 29位变25位

            #region 第一部分,处理序列号的后五位

            string V_1 = V_0.Substring(20, 5); // 取后五位

            int V_2 = Convert.ToInt32(V_1.Substring(0, 2), 0x10); // 后五位的前两位,16进制转10进制

            string V_3 = V_2.ToString(); // 转换成字符串

            V_2 = int.Parse(V_3.Substring(0, V_3.Length - 1)); // 去掉最后一位,并转换成整数（ProductCode）

            char V_13 = V_3[V_3.Length - 1]; // 取最后一位

            int V_4 = int.Parse(V_13.ToString()); // 转换成整数

            if (!this.CheckSum(V_2, V_4)) // 进行比较（ProductCode）
            {
                throw new Exception();
            }

            #endregion

            #region 第一部分,处理序列号的前十五位

            string V_5 = V_0.Substring(0, 15); // 取前15位

            Int64 V_14 = Convert.ToInt64(V_5, 0x10); // 16进制转换成10进制

            string V_6 = V_14.ToString(); // 转换成字符串

            V_6 = V_6.PadLeft(0x12, (char)0x30); // 用0补全18位

            string V_7 = this.Reverse(V_6); // 前后反转

            int V_8 = int.Parse(V_7.Substring(1, 5));  // countryCode, 从1开始,取五位 1、2、3、4、5

            if (!this.CheckSum(V_8, int.Parse(V_7.Substring(6, 1)))) // 6、
            {
                throw new Exception();
            }

            int V_9 = int.Parse(V_7.Substring(7, 2)); // regionCode, 从7开始,取二位 7、8

            if (!this.CheckSum(V_9, int.Parse(V_7.Substring(9, 1)))) // 9、
            {
                throw new Exception();
            }

            int V_10 = int.Parse(V_7.Substring(12, 3)); // 授权人数,从12开始,取3位 12、13、14

            int V_11 = int.Parse(V_7.Substring(15, 2)); // siteLicense 从15开始,取2位 15、16

            int V_12 = int.Parse(string.Format("{0}{1}", V_10, V_11)); // 连起来

            if (!this.CheckSum(V_12, int.Parse(V_7.Substring(0x11, 1)))) // 17、最后一位。
            {
                throw new Exception();
            }

            #endregion

            return new RegHelper2005.SerialNumberInfo2005(V_8, V_9, V_10, V_11, V_2);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override int GetCountryCode(string serialNumber)
        {
            SerialNumberInfo2005 V_0 = this.DecodeSerialNumber(serialNumber) as SerialNumberInfo2005;
            return V_0.CountryCode;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override int GetProductCode(string serialNumber)
        {
            SerialNumberInfo2005 V_0 = this.DecodeSerialNumber(serialNumber) as SerialNumberInfo2005;
            return V_0.Product;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override int GetSiteLicense(string serialNumber)
        {
            SerialNumberInfo2005 V_0 = this.DecodeSerialNumber(serialNumber) as SerialNumberInfo2005;
            return V_0.SiteLicense;
        }

        /// <summary>
        /// 此类的序列号长度为29
        /// </summary>
        internal override int SerialNumberLength
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return 0x1d; // 29
            }
        }

        // Nested Types
        protected class SerialNumberInfo2005 : RegHelper.SerialNumberInfo
        {
            // Fields
            public int Product;
            public int SiteLicense;

            // Methods
            [MethodImpl(MethodImplOptions.NoInlining)]
            public SerialNumberInfo2005(int countryCode, int regionCode, int numberOfPowerUsers, int siteLicense, int product)
                : base(countryCode, regionCode, numberOfPowerUsers)
            {
                this.SiteLicense = siteLicense;
                this.Product = product;
            }
        }

    }
}
