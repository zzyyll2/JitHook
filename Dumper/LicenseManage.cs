using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Web.SessionState;
using UIControls;
using System.Web.UI.HtmlControls;

namespace Dumper
{
     class LicenseManager : BasePage, IRequiresSessionState
    {
        // Fields
        protected ClientToolbar ClientToolbar1;
        protected HtmlForm Form1;
        protected LiteDataGrid grdLicenses;

        // Methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void BindLicenses()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string IndicateLicense(int p, string sn)
        {
            int V_0 = RegBroker.GetSiteLicense(sn);
            string V_1 = "\uC180\u8012\u06C1\u0120\u1101\uB183\u0705\u1201\u2981";
            if (V_0 != 1 || p == 1)
            {
                int V_2 = V_0;
                switch (V_2 - 2)
                {
                    case 1:
                        break;
                }
            }
            else
            {
                V_1 = string.Empty;
            }
            return string.Format(V_1, p);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InitializeComponent()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnInit(EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnPreRender(EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }




}
