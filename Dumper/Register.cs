using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using UIControls;
using System.Web.Profile;

namespace Dumper
{
    class Register : BasePage, IRequiresSessionState
    {
        // Fields
        protected Button btnCancel;
        protected LinkButton btnGetReqCode;
        protected Button btnOk;
        private IContainer components;
        protected HtmlForm Form1;
        protected HyperLink HyperLink1;
        protected Label Label1;
        protected Label Label2;
        protected Label Label3;
        protected Label Label4;
        protected Label Label6;
        protected Label Label7;
        protected Label Label8;
        protected LanguageProcessor languageProcessor1;
        protected RadioButton rdoInternet;
        protected RadioButton rdoOffline;
        protected TextBox txtActivationCode;
        protected TextBox txtCompanyName;
        protected TextBox txtReqCode;
        protected TextBox txtSerialNumber;
        protected TextBox txtUserName;
        protected CustomValidator vldActCode;
        protected CustomValidator vldError;
        protected CustomValidator vldSerialNum;

        // Methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void btnGetReqCode_Click(object sender, EventArgs e)
        {
            if (!this.ValidateSerialNumber(this.txtSerialNumber.Text))
            {
                this.vldSerialNum.IsValid = false;
            }
            else
            {
                this.txtReqCode.Text = RegBroker.GenerateRequestCode(this.txtSerialNumber.Text);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void btnOk_Click(object sender, EventArgs e)
        {
            this.vldSerialNum.IsValid = this.ValidateSerialNumber(this.txtSerialNumber.Text);
            this.vldActCode.IsValid = this.ValidateActCode(this.txtActivationCode.Text);

            if (!Page.IsValid)
            {
                return;
            }
            string V_0 = string.Empty;
            bool V_1 = false;
            if (this.rdoInternet.Checked)
            {
                V_1 = this.ExtendLicenseOnline(out V_0);
            }
            else
            {
                V_1 = this.ExtendLicenseOffline(out V_0);
            }
            if (V_1)
            {
                this.NotifyRecalc();
                this.GOC.SiteConfiguration.LoadRegisteredComponents(@"\u8012\u12E1\uF581\u3912\u0E0E\u120E\uF98");
                if (this.IsAdminOnly)
                {
                    Session.Remove("");
                    this.Response.Redirect(@"\u9D80\u2005\u0101\u3412\u0604\u8012\u04A1 \u081D");
                }
                if (this.IsLicenseValid)
                {
                    this.Response.Redirect("");
                }
                else
                {
                    Session.Remove("");
                    Response.Redirect("");
                }
            }

            this.vldError.IsValid = false;
            vldError.ErrorMessage = this.GetErrMsg(V_0);
            return;

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private bool ExtendLicenseOffline(out string errCode)
        {
            errCode = string.Empty;
            try
            {
                this.UpdateLicense(this.txtActivationCode.Text);
                return true;
            }
            catch (Exception err)
            {
                errCode = err.Message;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private bool ExtendLicenseOnline(out string errorCode)
        {
            errorCode = string.Empty;
            string V_0 = this.SitePath;
            if (string.IsNullOrEmpty(V_0))
            {
                errorCode = "\u6E65\u6573\u692F\u3565\u2022\u616E\u656D\u223D\u7376\u745F\u7261\u6567\u5374\u6863\u6D65\u2261\u0D3E\u090A\u3C09\u494C\u4B4E\u6820\u6572\u3D66\u2E22\u2F2E\u6564\u6166\u6C75\u2E74\u7363\u2273\u7220\u6C65\u223D\u7473\u6C79\u7365\u6568\u7465\u3E22\u0A0D\u3C09\u482F\u4145\u3E44\u0A0D\u3C09\u6F62\u7964\u0D3E\u090A\u0D09\u3C0A\u4421\u434F\u5954";
                return false;
            }
            BusinessOnline.RegistrationInfo V_1 = new BusinessOnline.RegistrationInfo();
            V_1.SerialNumber = this.txtSerialNumber.Text;
            V_1.CompanyName = txtCompanyName.Text;
            V_1.ContactName = txtUserName.Text;
            V_1.RequestCode = RegBroker.GenerateRequestCode(V_1.SerialNumber);
            //LicenseService V_2 = new LicenseService();
            //WebClientProtocol.Url(string.Format("\u2022\u616E\u656D\u223D\u7376\u745F\u7261\u6567\u5374\u6863\u6D65\u2261\u0D3E\u090A\u3C09\u494C\u4B4E\u6820\u6572\u3D66\u2E22\u2F2E\u6564\u6166\u6C75\u2E74" ,V_0);
            //string V_3 = V_2.ExtendLicense(V_1.RegDocument);
            //if(V_3.IndexOf( "\u6E65\u6573\u692F\u3565\u2022\u616E\u656D\u223D\u7376\u745F\u7261\u6567\u5374\u6863\u6D65\u2261\u0D3E\u090A\u3C09\u494C\u4B4E\u6820\u6572\u3D66\u2E22\u2F2E\u6564\u6166\u6C75\u2E74\u7363\u2273\u7220\u6C65\u223D\u7473\u6C79\u7365\u6568\u7465\u3E22\u0A0D\u3C09\u482F\u4145\u3E44\u0A0D\u3C09\u6F62\u7964\u0D3E\u090A\u0D09\u3C0A\u4421\u434F\u5954")>0)
            //{
            //    errorCode=V_3;
            //    return false;
            //}
            //if(V_3==string.Empty)
            //{
            //    errorCode=V_3;
            //    return false;
            //}
            //this.UpdateLicense(V_3);
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetErrMsg(string errCode)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void GetPhoneList()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InitializeComponent()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool IsValidLicenseType(string serialNum)
        {
            /*
03 28 3b 01 00 0a 0a 02 28 37 00 00 0a 6f 38 00 
00 0a 6f 4c 02 00 0a 2c 46 06 2c 35 06 18 2e 31  
06 1e 2e 2d 06 1f 09 2e 28 06 1f 0a 2e 23 06 1f  
0b 2e 1e 06 1f 0c 2e 19 06 1f 0d 2e 14 06 1f 0f  
2e 0f 06 1f 10 2e 0a 06 1f 12 2e 05 06 1f 13 33  
07 16 0c dd c3 00 00 00 17 0c dd bc 00 00 00 06  
1e 33 07 17 0c dd b1 00 00 00 03 28 69 02 00 0a  
18 33 0b 06 18 33 07 17 0c dd 9d 00 00 00 02 28  
37 00 00 0a 6f 38 00 00 0a 6f 2a 01 00 0a 0b 07 
17 2e 04 07 1d 33 04 06 1c 2e 14 07 1f 0f 33 05 
06 1f 13 2e 0a 07 1f 14 33 09 06 1f 15 33 04 17  
0c de 68 03 28 6a 02 00 0a 2d 04 16 0c de 5c 07  
1f 09 2e 07 07 1f 0a fe 01 2b 01 17 06 1f 09 2e  
07 06 1f 0a fe 01 2b 01 17 61 2c 04 16 0c de 3b  
07 1f 11 2e 07 07 1f 12 fe 01 2b 01 17 06 1f 11  
2e 07 06 1f 12 fe 01 2b 01 17 61 2c 04 16 0c de  
1a 07 1f 0b fe 01 06 1f 0c fe 01 61 2c 04 16 0c  
de 09 17 0c de 05 26 16 0c de 00 08 2a
             */

            /*
 03 28 9D 00 00 06 0a 02 28 62 00 00 06 6f D1 00 
 00 0a 6f D8 00 00 0a 2c 46 06 2c 35 06 18 2e 31  
 06 1e 2e 2d 06 1f 09 2e 28 06 1f 0a 2e 23 06 1f  
 0b 2e 1e 06 1f 0c 2e 19 06 1f 0d 2e 14 06 1f 0f  
 2e 0f 06 1f 10 2e 0a 06 1f 12 2e 05 06 1f 13 33  
 07 16 0c dd c3 00 00 00 17 0c dd bc 00 00 00 06  
 1e 33 07 17 0c dd b1 00 00 00 03 28 99 00 00 06  
 18 33 0b 06 18 33 07 17 0c dd 9d 00 00 00 02 28  
 62 00 00 06 6f D1 00 00 0a 6f D9 00 00 0a 0b 07 
 17 2e 04 07 1d 33 04 06 1c 2e 14 07 1f 0f 33 05 
 06 1f 13 2e 0a 07 1f 14 33 09 06 1f 15 33 04 17  
 0c de 68 03 28 9E 00 00 06 2d 04 16 0c de 5c 07  
 1f 09 2e 07 07 1f 0a fe 01 2b 01 17 06 1f 09 2e  
 07 06 1f 0a fe 01 2b 01 17 61 2c 04 16 0c de 3b  
 07 1f 11 2e 07 07 1f 12 fe 01 2b 01 17 06 1f 11  
 2e 07 06 1f 12 fe 01 2b 01 17 61 2c 04 16 0c de  
 1a 07 1f 0b fe 01 06 1f 0c fe 01 61 2c 04 16 0c  
 de 09 17 0c de 05 26 16 0c de 00 08 2a
             */

            int V_0 = RegBroker.GetSiteLicense(serialNum);
            bool V_2 = false;

            if (this.GOC.SiteConfiguration.IsEvalVersion)
            {
                if (V_0 == 0 || V_0 == 2 || V_0 == 8 || V_0 == 9 || V_0 == 10 || V_0 == 11
                    || V_0 == 12 || V_0 == 13 || V_0 == 15 || V_0 == 0x10 || V_0 == 0x12
                    )
                {
                    goto Label_1;
                }
                if (V_0 != 0x13)
                {
                    goto Label_2;
                }
            }
            else
            {
                goto Label_3;
            }

        Label_1:
            V_2 = false;
            return V_2;
        Label_2:
            V_2 = true;
            return V_2;

        Label_3:
            if (V_0 != 8)
            {
                goto Label_4;
            }
            V_2 = true;
            goto Label_23;

        Label_4:
            if (RegBroker.GetProductCode(serialNum) != 2)
            {
                goto Label_5;
            }
            if (V_0 != 2)
            {
                goto Label_5;
            }
            V_2 = true;
            goto Label_23;
        Label_5:
            int V_1 = GOC.SiteConfiguration.SL;
            if (V_1 == 1)
            {
                goto Label_6;
            }
            if (V_1 != 7)
            {
                goto Label_7;
            }
        Label_6:
            if (V_0 == 6)
            {
                goto Label_9;
            }
        Label_7:
            if (V_1 != 15)
            {
                goto Label_8;
            }
            if (V_0 == 19)
            {
                goto Label_9;
            }
        Label_8:
            if (V_1 != 20)
            {
                goto Label_10;
            }
            if (V_0 != 21)
            {
                goto Label_10;
            }
        Label_9:
            V_2 = true;
            goto Label_23;
        Label_10:
            if (RegBroker.ContainsPowerUserLicense(serialNum))
            {
                goto Label_11;
            }
            V_2 = false;
            goto Label_23;
        Label_11:
            if (V_1 == 9)
            {
                var t1 = true;
                if (V_0 == 9)
                {
                    var t2 = true;
                    bool val = t1 ^ t2;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_16;
                    }
                }
                else
                {
                    var t3 = V_0 == 10;
                    bool val = t1 ^ t3;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_16;
                    }
                }
            }
            else
            {
                var tt1 = V_1 == 10;
                if (V_0 == 9)
                {
                    var tt2 = true;
                    bool val = tt1 ^ tt2;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_16;
                    }
                }
                else
                {
                    var ttt1 = V_0 == 10;
                    bool val = tt1 ^ ttt1;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_16;
                    }
                }
            }

        Label_16:
            if (V_1 == 17)
            {
                var v1 = true;
                if (V_0 == 17)
                {
                    var v2 = true;
                    var val = v1 ^ v2;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_21;
                    }
                }
                else
                {
                    var vv2 = V_0 == 18;
                    var val = v1 ^ vv2;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_21;
                    }

                }
            }
            else
            {
                var vv1 = V_1 == 18;
                if (V_0 == 18)
                {
                    var vv2 = true;
                    var val = vv1 ^ vv2;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_21;
                    }
                }
                else
                {
                    var vv3 = V_0 == 18;
                    var val = vv1 ^ vv3;
                    if (val)
                    {
                        V_2 = false;
                        goto Label_23;
                    }
                    else
                    {
                        goto Label_21;
                    }
                }
            }
        Label_21:
            var p1 = V_1 == 11;
            var p2 = V_0 == 12;

            if (p1 ^ p2)
            {
                V_2 = false;
                goto Label_23;
            }
            else
            {
                goto Label_22;
            }
        Label_22:
            V_2 = true;
            goto Label_23;
        //pop 
        //V_2 = false;
        //goto Label_23;

        Label_23:
            return V_2;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void NotifyRecalc()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnInit(EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void UpdateLicense(string actCode)
        {
            BusinessOnline.LicenseInfoXml V_0 = new BusinessOnline.LicenseInfoXml(txtCompanyName.Text, txtUserName.Text, txtSerialNumber.Text, actCode, string.Empty);
            this.GOC.SiteConfiguration.CheckMajorVersion(this.txtSerialNumber.Text);
            if (this.GOC.SiteConfiguration.IsEvalVersion)
            {
                this.GOC.SiteConfiguration.UpgradeEvalVersion(V_0);
            }
            this.GOC.SiteConfiguration.LicensesXml.Add(V_0);
            this.GOC.SiteConfiguration.SaveSystemParameters("\u8012\u12E1\uF581\u3912\u0E0E\u120E\uF981");
            int V_1 = RegBroker.GetSiteLicense(this.txtSerialNumber.Text);
            switch (V_1)
            {
                case 6:
                case 19:
                    this.GOC.SiteConfiguration.ResetSL();
                    break;
                default:
                    BusinessOnline.LicenseInfoDb V_2 = new BusinessOnline.LicenseInfoDb();
                    V_2.CopyFrom(V_0);
                    V_2.MacAddress = RegBroker.GetValidMCode(actCode);
                    V_2.RegDate = DateTime.Now;
                    V_2.ServerName = Page.Server.MachineName;
                    V_2.VirtualDir = Page.Request.ApplicationPath;
                    V_2.Port = Page.Request.Url.Port.ToString();
                    V_2.Save(null, null, false);
                    this.GOC.RecalculatePowerUsers(true);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateActCode(string actCode)
        {
            if (this.rdoOffline.Checked)
            {
                if (!string.IsNullOrEmpty(actCode))
                {
                    if (!RegBroker.ValidateLicense(actCode, true))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidateSerialNumber(string serialNum)
        {
            bool V_0 = false;
            if (!string.IsNullOrEmpty(serialNum)
                && RegBroker.IsValidSerialNumber(serialNum))
            {
                V_0 = this.IsValidLicenseType(serialNum);
            }
            return V_0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void vldActCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void vldSerialNum_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }

        // Properties
        protected string AboutOffline
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        //protected global_asax ApplicationInstance
        //{
        //    [MethodImpl(MethodImplOptions.NoInlining)]
        //    get
        //    {
        //        return null;
        //    }
        //}

        protected string CloseHintPanel
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected DefaultProfile Profile
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        private string RegSite
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected override bool SessionExpired
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        private string SitePath
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }
    }
}
