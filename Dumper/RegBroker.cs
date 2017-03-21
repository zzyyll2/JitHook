using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dumper
{
    class RegBroker
    {
        #region 属性

        /// <summary>
        /// 修改过，并非原程序
        /// </summary>
        internal static RegHelper Helper
        {
            get
            {
                var helper = new RegHelper();
                return helper;
            }
        }

        /// <summary>
        /// 修改过，并非原程序
        /// </summary>
        internal static RegHelper2005 Helper2005
        {
            get
            {
                var helper2005 = new RegHelper2005();
                return helper2005;
            }
        }

        #endregion

        public static bool IsValidSerialNumber(string serialNumber)
        {
            RegHelper helper = GetHelperBySerialNumber(serialNumber);
            if (helper != null)
            {
                return helper.IsValidSerialNumber(serialNumber);
            }
            return false;
        }

        private static RegHelper GetHelperBySerialNumber(string serialNumber)
        {
            if (serialNumber.Length == Helper.SerialNumberLength)
            {
                return Helper;
            }
            if (serialNumber.Length == Helper2005.SerialNumberLength)
            {
                return Helper2005;
            }
            throw new Exception("Invalid serial number.");
        }

        public static int GetProductCode(string serialNumber)
        {
            RegHelper loc0 = GetHelperBySerialNumber(serialNumber);
            return loc0.GetProductCode(serialNumber);
        }

        private static RegHelper GetHelperByActCode(string actCode)
        {
            return Helper;
        }

        public static string GenerateRequestCode(string serialNumber)
        {
            RegHelper helper = GetHelperBySerialNumber(serialNumber);
            return helper.GenerateRequestCode(serialNumber);
        }

        public static bool ContainsSiteLicense(string serialNumber)
        {
            RegHelper V_0 = GetHelperBySerialNumber(serialNumber);
            return V_0.ContainsSiteLicense(serialNumber);
        }

        public static int GetSiteLicense(string serialNumber)
        {
            RegHelper V_0 = GetHelperBySerialNumber(serialNumber);
            return V_0.GetSiteLicense(serialNumber);
        }

        public static bool ContainsPowerUserLicense(string serialNumber)
        {
            RegHelper V_0 = GetHelperBySerialNumber(serialNumber);
            return V_0.ContainsPowerUserLicense(serialNumber);
        }

        public static string GetValidMCode(string actCode)
        {
            RegHelper V_0 = GetHelperByActCode(actCode);
            return V_0.GetValidMCode(actCode);
        }

        public static bool ValidateLicense(string actCode)
        {
            RegHelper V_0 = GetHelperByActCode(actCode);
            return V_0.ValidateLicense(actCode);
        }

        public static bool ValidateLicense(string actCode, out int p)
        {
            RegHelper V_0 = GetHelperByActCode(actCode);
            return V_0.ValidateLicense(actCode, out p);
        }

        public static bool ValidateLicense(string actCode, bool onRegister)
        {
            RegHelper V_0 = GetHelperByActCode(actCode);
            return V_0.ValidateLicense(actCode, onRegister);
        }

        public static bool ValidateLicense(string actCode, string mCode, out int p)
        {
            RegHelper V_0 = GetHelperByActCode(actCode);
            return V_0.ValidateLicense(actCode, mCode, out p);
        }
    }
}
