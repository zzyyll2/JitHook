using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessOnline;
using UIControls;


namespace Dumper
{
    class BasePage : Page, IBasePage
    {
        // Fields
        private List<DropdownDialog> _dialogRegistry;
        protected bool _impersonated;
        private ArrayList _objReg;
        private Hashtable _variables;

        // Methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        public OleDbConnection AllocConnection()
        {
            return null;
        }

        public BasePage()
        {
            this._objReg = new ArrayList();
            this._dialogRegistry = new List<DropdownDialog>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int AllocUniqueId()
        {
            return 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AllocXmlTextWriter(out XmlTextWriter xw, out MemoryStream ms, string rootElementName)
        {
            xw = null;
            ms = null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ApplyStyleSheet(ControlCollection controls)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private bool CheckRequest()
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void ClearCachedConnection()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void DoImpersonate()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DumpMemory()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string FinishXmlTextWriter(XmlTextWriter xw, MemoryStream ms)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetAppSetting(string key)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void LoadLanguageProfiles()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnLoad(EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void OnPreRender(EventArgs e)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected virtual void OverrideContextMenu(StringBuilder sb)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool ParseBool(string key, bool defaultValue)
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected int ParseInt(string key, int defaultValue)
        {
            return 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string ParseString(string key, string defaultValue)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override void ProcessRequest(HttpContext context)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected virtual void RedirectToExpiredPage()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void RedirectToNonAuthenticatedPage()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RegisterClientVariable(string name, string value)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void RegisterClientVariables()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void RegisterDropdownDialog(WebControl targetControl, string button, string dialogPage)
        {
        }

        // Properties
        protected string Action
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected int AdapterId
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return 0;
            }
        }

        public bool Beta
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        public ArrayList Clipboard
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        public int CTOD
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return 0;
            }
        }

        public string CurrentLanguage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
            }
        }

        public UserSession CurrentSession
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected string FromPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        public GlobalCache GOC
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (Page.Cache == null)
                {
                    return null;
                }
                object V_0 = this.Page.Cache[GlobalCache.Key];
                if (V_0 == null)
                {
                    return null;
                }
                else
                {
                    return V_0 as GlobalCache;
                }
            }
        }

        public bool HideMDXFeature
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsAdminOnly
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
            }
        }

        protected bool IsAuthenicated
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsCompanyAdmin
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsCompanyFromDirectory
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        public bool IsConfigFullControl
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsFullControl
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsITUser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsLdapAuthenticated
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsLdapAuthentication
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsLicenseValid
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsPowerUser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsReportAdmin
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsSkipCompanyFolder
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected bool IsWebPartMode
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
            }
        }

        protected bool LimitNonMasterCompany
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        public ArrayList ObjectRegistry
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected string PageName
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        public string ScriptToken
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected virtual bool SessionExpired
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }

        protected string Title
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return null;
            }
        }

        protected int Version
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return 0;
            }
        }

        public bool XMLHttp
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return false;
            }
        }
    }
}
