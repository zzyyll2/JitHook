using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Dumper
{
    class Program
    {
        unsafe static void Main(string[] args)
        {

            //启用Windowns应用程序
            WinForm form = new WinForm();
            form.ShowDialog();
            return;

            // 启用Console控制台应用程序
            Console.ReadLine();

            try
            {

                // Enable Jit debugging
                if (IntPtr.Size == 4)
                {
                    Console.WriteLine("Enabling JIT debugging.");
                    JitLogger.JitLogger.Enabled = true;
                }

                //-839723568	PT1BR0FBQUFJQUFBQWdibUpwYW9hY25PVWI4NVVxMWZjSy90dWxyMi9nSU9CNFpoUENZdm0yd3RBS1ozM0djdmZwbzFoNERVdGRxZGlCRzc2b1NBeEVuRkoxSGJUbFUxRThONmMxc3o2YUdKYW4yNVcvM0o3MGxmc3F0MXlSTzdvbGFvMTBuYjRCcWZJRmxNdWxzOE05V0xlNXR6N0ZxU3dyRFVBZ3hRR0pvZ0M0Q0d5UVBhOXpvbEVTQ1VsTDFGTzhSR0hVWXdDRXVGR0FJWWFlck1mWlpjdjFIdjdMcEtROWdWaTZrZjJUSm9odURadFhtclBlWmV1ZjkrVjgweGtIRmRvQUZadWJMMHR0QlBWNmF3a1cyYTltS3B0dm9IQ2dpSEpKOGRVVmoxeC9LUGNXSVVQVGlBbGhMc3JXWC81czBwUjF4R0x0R1Z6Mko4OEwxMzV2S0hlMHF0MDJ1WFo1VXZ5OWs3WE5KeWNBZlZxYmhua0JDazZmdU1HQm4weEsxamo0N2lMMHB1SzVuVXQ3bStqMjRDZlAwTzczek5IRTZBMCszdWNCcmhjVEQzc3FyRGJ6bUtuN3F5RFNqaHdIbUtuTDMzMjNFdjlBbnYwd3RobVplc3NlQ1V1YWE3UmVwVURGdzJUNnBkbUNNOS9nQVdUUlZsY2dVV3d2bWVzZTRIa2pPaVNRTWQ2WUtUZmpkS3VyZDJRL3VaZnZ3ZTE3emNXQkpMaFNRcVBhQ0pmaHpVVEpVYlM0ZkJGNnJQZ002dEdSWVNjTlc3L3NxNnhhNjBrUmhuSDNoYVl0dEp6Y2xDeHc2NDV0QUlwYXpXWWNJWkdWTDJLdWdQckdOaTRXMFRSWm1kdnZrS1c0T2ZDT3ZpWkRsZ0FTSTBkeGM4cE1UYU9xanJqN2VnYzRLbG1hUEg2aUc4
                //<?xml version='1.0' encoding='utf-8'?><Document><License><CompanyName>InnoLux Display Corp.</CompanyName><UserName>古哲瑋</UserName><SerialNumber>2C741-8CB95-468C6-11BC4-16E26</SerialNumber><ActivationCode>A2004-B0267-B0E10-07D38-E6268</ActivationCode><InstallationToken></InstallationToken><MacAddress>001E0B7620B4</MacAddress><ServerName>INLCNDB05</ServerName><VirtualDir>/Analyzer</VirtualDir><Port>80</Port><RegDate>9/2/2009 10:33:03 AM</RegDate></License></Document>

                //544052896	    PT1BR0FBQUFJQUFBQUV4SWw4bXc0T25vU1AvZGprMlFjeVlETWlDb0RmSXl0WEg4WVpldXMxRlRDdzJmTVpUeDdaNU5XZytnSmh6cEhrNjFQMk5TR3JLQlNwQVh3OWZmTllzM1F1V1ppcDhQU0dETzhhcC9qK2dhRElsV1crbEhhMjhmblQ0NUdpQ1d6NlJtQUN3R1FGZEhjNURsaWlMSTN4T1NLYy9KZEFHT1J6SStXYU5RckZFeFhDcWVER3ExM2txdXpHbnNIZ1RCVzZTRHhuQ1pMWUFoQ0FXK3JDKzV2WUJyK3haN0RnMHFSTHZ2N1VURUlKRXRMbzRJS3VmcDBDNlBvV3ZmNWxGY3Y1M0FnZmRkYS83d20xRjlqa0Vrcnl6M0kvdHdqdm8ySFgxVzhiWWVRYStYOUE2SnlGVlUxaE1OZlUzWVJzM0VxVjhWbWJFcGQxTTk2dWR4VnhyV1cwNE5SNnNVV3B6L0JnbTJhcnNCNmc2QmoxYVNqbVF3ZDMwcUhvSUY3QVJYSGxMUEhXeTc4anZ1aVhVNkF2SkU5a1BSVTl2K1h0T2ZwTi9udElVSEZJZkNmT0Fxd1lRVHFUMzhBN29zL3c0RlZNb05rTWp0NDIycEJNei80TU1rK2EzUmszWmMzaGpRMWRnLy9pbU84R3VmQlZQL3BUVVJYeWRESkxtekN4YVZDN2VoQXlrcEMva0FIelNna2NjODRHUFVKQ1puWnJXZnFoUWc1Tzg3ZjRES3pxTXlkejUzd2JoeUR2UTFNSmRqeUxsbU1WWW1pcEtRL1JLOWFaeDZiaFNZekVpeW0vaXdGOFc3ZmMyZUllTmplekVFZlZBMVREMUd0WGo2bTdJN2hEYXNHelpsUWJTZXhlLyt1WUNtMlZUOWIySFpyZmVRUHBlMG85NXh2RWRo
                //<?xml version='1.0' encoding='utf-8'?><Document><License><CompanyName>InnoLux Display Corp.</CompanyName><UserName>古哲瑋</UserName><SerialNumber>D9A04-5BC51-978CA-14093-0B288</SerialNumber><ActivationCode>D1004-B0267-B0E10-02464-D2FBD</ActivationCode><InstallationToken></InstallationToken><MacAddress>001E0B7620B4</MacAddress><ServerName>INLCNDB05</ServerName><VirtualDir>/Analyzer</VirtualDir><Port>80</Port><RegDate>6/23/2008 11:28:32 AM</RegDate></License></Document>

                //1149194221	PT1BR0FBQUFJQUFBQUV5OHNxazhzR0UybEh6dXRkS3BzbytBNlR2SHBxd2tPYXRRcndZZGZpeUEvUllPYUFNYmVQcEd3K2Exa3pQR216dWZkMVFMZzQ1cVpsVi9KM1I5aDQ5cXlPSnJyRVJlTzd1ZllDZ0w0b2JhdVprNnBvV29pNlExV01NelQ5eEQ3dG1DTTJsSWd0QWpVWXpPaFRmUVdsY2RWUUdlZHZhK1hCejVFZ3BFYkROL0xQR2c3bnVlZ3grNUppOHZiazJVWWN1WmhzTkx6QUNMQW1xSVFFamNmczJHazc0b1Z3SXpUTGFPeVVZTmRFOHFvakRweUpvWFJQMXVPclRZdmpFaml3TGpYUW90ZnhSMVhJRFhya01CM1JTR1IvV2xCUXZLWE1xeE9ZSGx5MEZLRDZOZjdPL1hTSStVRnlRL1VnOFBzUHVnQmRZaDFnUWZEZk00WThwd0ZPNEt3Zno4VzlmdGk1UGEvTzl2K1BZRkdJaXMvSDBVQVRaM2w5dEI3SFRaOGVMRWxzdXJIMkVTSjVVNTk1eTFtejVmOFEzaXNrU0MrSytvTFU2WWx3ZFBNbkNtN0FWOEZWREZOdnNabm5NUHpObGQxY1Q3LzdhT1RrOUs0NEduV2I3Nyt4blQxbXNDLzkzd2Yra3dZUllCNEpuOU9wN3EzbXh5WERNTVVjekkwVnM0bFJkTzdndjNlakppSjVKYnlxVDZ3K3duUllqOWpxQS90aFpuMGkxM3pOSlZPNXhWRCtEYXpUWk9UaEplNDZ6MjlDaURucGQrUlFQZjhiaG1oa25XR1BIWmJuSlJjOThqNlNtSlNEdjVlRU1TZVRJK3M2ZWw4VWYrT2J5YmgrRHNzVGNDZllzN2lWTWZDb1pvNEdDRHg4bUp3M0MrVjlyUTYrT3BpM2JF
                //<?xml version='1.0' encoding='utf-8'?><Document><License><CompanyName>InnoLux Display Corp.</CompanyName><UserName>古哲瑋</UserName><SerialNumber>30D98-FB1D0-0B0CA-46AC1-16898</SerialNumber><ActivationCode>50004-B0267-B0E10-08E46-C4268</ActivationCode><InstallationToken></InstallationToken><MacAddress>001E0B7620B4</MacAddress><ServerName>INLCNDB05</ServerName><VirtualDir>/Analyzer</VirtualDir><Port>80</Port><RegDate>6/23/2008 11:28:10 AM</RegDate></License></Document>

                //1499490694	PT1BR0FBQUFJQUFBQUVVNU0xZHVTcnRjVERjZnJLRGp5cjZFT2lQRmd1cER1ZkIvdVZIVTVvMDlEaHlkTFR3R291bGdUUG1SbXFJL05Cc0p4emUwL2szWnBmNHA0d2tibnhMTktoQzhOZmxkQzF4WEhtQ0NjdFhxZCt2WEhYUHRmSE9zRC9Ia01lSVJPZWZMaFlHRlZEZW9xMVU5dHBFam13WFRiNXpTODRPQkZ3enlHdldYcTVIeFZnei9GcFBncjgvQXVHTXVJQzFYT3hDcmk0RmdodjU0T3RxZVUwNzA0T04yS0ZQQXJMdi9uYys0a3NTbVlaQ1UvTndCdUpqNjRNNlVMby9XQ3VpYyt1aHlObEx6Y2E5UjRkR1ZOTkNsN2ltTFpodHdmNVVGNi9iN0xuUHFvOWlmN2UwZjR4WStsUVRFRWpSMStuZ2pRbmYwZHdSZVVOalhGb3lVOU1sUDczOE4zcGpkbTBzMjFGQklCM0toVGpMeUNiRkkvTnVjOXpGSTVQdURwSm82czZXc2xNZ0NrQnpUbW9hV3lCYTZuY2N6S0twZFpOY0VGZEVaN1pPaDRBVHZydXlIRkhxcEEzTUxnNVR0Q1Fjd01PRzVZdW1GeTlNVUllNXdXSnJxTVFrM3hBL0kwY0VRZktZc2Yrb0hvSkNieHVDYnZGMTUydStmUVBGQTVkY09oWnQ3WHprUnRoNEZveHFUYkNURWdLWjZPWnpJSmRsaytlakhJalIrWnlUMGhvUkc4U2NJQWxRZzJsV1N3RHRQbHVyOEF5aUtybVN4STkwYlgzY2lWQVFteDVyWGZpVlNLcXYxUGdJOG5iQWsrbHRDLzl6RlR1SWh2OWtEdElrell3MTlkWWl2VGRZTEJidEZxSTB6ODJmeDhpd1RKMEN6bWgzREFXVUd2N1Zh
                //<?xml version='1.0' encoding='utf-8'?><Document><License><CompanyName>InnoLux Display Corp.</CompanyName><UserName>古哲瑋</UserName><SerialNumber>44E0D-F2DE8-7A0CC-A323E-0BED3</SerialNumber><ActivationCode>32005-64FCA-C6100-0495D-6EFCB</ActivationCode><InstallationToken></InstallationToken><MacAddress>00016CACF465</MacAddress><ServerName>CNPC1303</ServerName><VirtualDir>/Analyzer</VirtualDir><Port>80</Port><RegDate>2007/5/10 下午 06:24:40</RegDate></License></Document>

                //string str = CryptoService.Decrypt("PT1BR0FBQUFJQUFBQUVVNU0xZHVTcnRjVERjZnJLRGp5cjZFT2lQRmd1cER1ZkIvdVZIVTVvMDlEaHlkTFR3R291bGdUUG1SbXFJL05Cc0p4emUwL2szWnBmNHA0d2tibnhMTktoQzhOZmxkQzF4WEhtQ0NjdFhxZCt2WEhYUHRmSE9zRC9Ia01lSVJPZWZMaFlHRlZEZW9xMVU5dHBFam13WFRiNXpTODRPQkZ3enlHdldYcTVIeFZnei9GcFBncjgvQXVHTXVJQzFYT3hDcmk0RmdodjU0T3RxZVUwNzA0T04yS0ZQQXJMdi9uYys0a3NTbVlaQ1UvTndCdUpqNjRNNlVMby9XQ3VpYyt1aHlObEx6Y2E5UjRkR1ZOTkNsN2ltTFpodHdmNVVGNi9iN0xuUHFvOWlmN2UwZjR4WStsUVRFRWpSMStuZ2pRbmYwZHdSZVVOalhGb3lVOU1sUDczOE4zcGpkbTBzMjFGQklCM0toVGpMeUNiRkkvTnVjOXpGSTVQdURwSm82czZXc2xNZ0NrQnpUbW9hV3lCYTZuY2N6S0twZFpOY0VGZEVaN1pPaDRBVHZydXlIRkhxcEEzTUxnNVR0Q1Fjd01PRzVZdW1GeTlNVUllNXdXSnJxTVFrM3hBL0kwY0VRZktZc2Yrb0hvSkNieHVDYnZGMTUydStmUVBGQTVkY09oWnQ3WHprUnRoNEZveHFUYkNURWdLWjZPWnpJSmRsaytlakhJalIrWnlUMGhvUkc4U2NJQWxRZzJsV1N3RHRQbHVyOEF5aUtybVN4STkwYlgzY2lWQVFteDVyWGZpVlNLcXYxUGdJOG5iQWsrbHRDLzl6RlR1SWh2OWtEdElrell3MTlkWWl2VGRZTEJidEZxSTB6ODJmeDhpd1RKMEN6bWgzREFXVUd2N1Zh");

                //ActCode = "6200F-AF839-CF784-47A69";
                //System.Collections.Specialized.StringCollection bcd = RegHelper.GetMacAddressStrs();

                //bool isValid = new Register().ValidateSerialNumber("D9A04-5BC51-978CA-14093-0B288");


                //RegHelper helper = new RegHelper2005();

                //bool license = helper.ContainsPowerUserLicense("C5D1D-834F8-D7CCC-594EE-0B52D");

                //Register reg = new Register();
                //var licensetype = reg.IsValidLicenseType("C5D1D-834F8-D7CCC-594EE-0B52D");

                //bool code = helper.ValidateLicense("49993-D346B-72120-0677E-DDCA4", true);

                //Console.WriteLine(CryptoService.Decrypt("PT1BR0FBQUFJQUFBQThmbXo2UjlxeEI0MURobzN2MC9tRDVSOUQ2Q00zMlJHY2ZjNzQvdTNGMTZpYnR2dlRQRUd2V3RPUm01TCtpN1RjQnJqczlmZTJ3YQ=="));

                //var actCode = RegBroker.GenerateRequestCode("44E0D-F2DE8-7A0CC-A323E-0BED3");

                //var res = RegBroker.ValidateLicense(actCode, true);

                //Console.WriteLine("\uC180\u8012\u06C1\u0120\u1101\uB183\u0705\u1201\u2981");
                //InvokeLicenseManager();

                //InvokeRegHelper();

                //LocalValSign.Program.Main();

                Console.ReadLine();
            }
            finally
            {
                JitLogger.JitLogger.Enabled = false;
            }

        }

        #region CrackMethod

        private static void InvokeLicenseManager()
        {
            var sn = "AD651-1AD64-E60C4-872DA-0B631";
            var o = Activator.CreateInstance(typeof(BusinessOnline.Admins.LicenseManager), new object[] { });
            var md = typeof(BusinessOnline.Admins.LicenseManager).GetMethod("IndicateLicense", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = md.Invoke(o, new object[] { sn });
            Console.WriteLine(res);

            //var o = Activator.CreateInstance(typeof(BusinessOnline.BasePage), new object[] { });
            //var md = typeof(BusinessOnline.BasePage).GetProperty("GOC");
            //var res = md.GetValue(o, null);
            //Console.WriteLine(res);


        }

        private static void InvokeRegister()
        {
            var sn = "AD651-1AD64-E60C4-872DA-0B631";
            var o = Activator.CreateInstance(typeof(BusinessOnline.Register), new object[] { });
            var md = typeof(BusinessOnline.Register).GetMethod("UpdateLicense", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = md.Invoke(o, new object[] { sn });
            Console.WriteLine(res);

            //var o = Activator.CreateInstance(typeof(BusinessOnline.BasePage), new object[] { });
            //var md = typeof(BusinessOnline.BasePage).GetProperty("GOC");
            //var res = md.GetValue(o, null);
            //Console.WriteLine(res);


        }

        private static void InvokeRegBroker()
        {
            var sn = "30D98-FB1D0-0B0CA-46AC1-16898";
            var o = Activator.CreateInstance(typeof(BusinessOnline.Security.StdInterface.RegHelper2005), new object[] { });
            var md = typeof(BusinessOnline.Security.StdInterface.RegHelper2005).GetMethod("GetSiteLicense", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var actCode = md.Invoke(o, new object[] { sn });
            Console.WriteLine(actCode);
        }

        private static void InvokeRegHelper()
        {
            var sn = "01C6B-9149E-124CB-AEB4C";
            var actCode = "2C741-8CB95-468C6-11B84-9247D-9247D";

            var o = Activator.CreateInstance(typeof(BusinessOnline.Security.StdInterface.RegHelper), new object[] { });
            //var md = typeof(BusinessOnline.Security.StdInterface.RegHelper).GetMethods(BindingFlags.Instance | BindingFlags.Public)[10];
            var md = typeof(BusinessOnline.Security.StdInterface.RegHelper).GetMethod("Confuse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = md.Invoke(o, new object[] { sn, 2 });
            Console.WriteLine(res);
        }

        private static void InvokeRegHelper2005()
        {
            var sn = "C5D1D-834F8-D7CCC-594EE-0B52D";
            var o = Activator.CreateInstance(typeof(BusinessOnline.Security.StdInterface.RegHelper2005), new object[] { });
            var md = typeof(BusinessOnline.Security.StdInterface.RegHelper2005).GetMethod("GetSiteLicense", BindingFlags.Instance | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = md.Invoke(o, new object[] { sn });

            //var md = typeof(BusinessOnline.Security.StdInterface.RegHelper2005).GetProperty("SerialNumberLength", BindingFlags.NonPublic | BindingFlags.Instance);
            //var res = md.GetValue(o, null);


            Console.WriteLine(res);
        }

        #endregion
    }
}
