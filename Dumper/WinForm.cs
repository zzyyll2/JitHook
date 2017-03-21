using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Collections;
using System.Drawing;

namespace Dumper
{
    public partial class WinForm : Form
    {
        /// <summary>
        /// 生成序列号时可用数集合
        /// </summary>
        private MapCollection mc = null;

        public WinForm()
        {
            InitializeComponent();

            InitControls();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControls()
        {
            var o = Activator.CreateInstance(typeof(RegHelper), new object[] { });
            var md = typeof(RegHelper).GetMethod("GetMacAddressStrs", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            var res = md.Invoke(o, new object[] { });

            cbMac.DataSource = res;
            cmbNewMac.DataSource = res;
        }

        /// <summary>
        /// 生成序列号、注册号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            #region 验证

            if (cbMac.SelectedValue == null || string.IsNullOrEmpty(cbMac.SelectedValue.ToString()))
            {
                MessageBox.Show("Select MAC first!");
                return;
            }
            var pws = 0;
            if (!int.TryParse(txtPws.Text, out pws))
            {
                MessageBox.Show("Wrong number of licenses!");
                return;
            }
            var countrycode = 0;
            if (!int.TryParse(txtCountryCode.Text, out countrycode))
            {
                MessageBox.Show("Wrong number of Country Code!");
                return;
            }
            var regioncode = 0;
            if (!int.TryParse(txtRegionCode.Text, out regioncode))
            {
                MessageBox.Show("Wrong number of Region Code!");
                return;
            }

            var sitelicense = 0;
            if (!int.TryParse(txtSiteLicense.Text, out sitelicense))
            {
                MessageBox.Show("Wrong number of Site License!");
                return;
            }

            var productcode = 0;
            if (!int.TryParse(txtProduct.Text, out productcode))
            {
                MessageBox.Show("Wrong number of Product Code!");
                return;
            }
            #endregion

            Type regType = null;
            if (cbAlgorithm.Checked)
            {
                regType = typeof(RegHelper2005);
                this.txtSerialNumber.Text = GenerateSerialNumber2005(txtCountryCode.Text, txtRegionCode.Text, txtPws.Text, txtSiteLicense.Text, txtProduct.Text);
            }
            else
            {
                regType = typeof(RegHelper);
                this.txtSerialNumber.Text = GenerateSerialNumber(txtCountryCode.Text, txtRegionCode.Text, txtPws.Text);
            }

            var o = Activator.CreateInstance(regType, new object[] { });
            var mdIsValidSerialNumber = regType.GetMethod("IsValidSerialNumber", BindingFlags.Instance | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(mdIsValidSerialNumber.MethodHandle);
            var isValidSN = mdIsValidSerialNumber.Invoke(o, new object[] { this.txtSerialNumber.Text }); // 验证序列号
            var checkSn = IsValidSerialNumber(regType, this.txtSerialNumber.Text);
            if (isValidSN is Boolean && (Boolean)isValidSN && checkSn)
            {
                this.txtSerialNumber.BackColor = Color.Green;
            }
            else
            {
                this.txtSerialNumber.BackColor = Color.Black;
            }

            this.txtActCode.Text = GenerateActCode(cbMac.SelectedValue.ToString(), txtPws.Text);

            var o1 = Activator.CreateInstance(typeof(RegHelper), new object[] { });
            var mdValidateLicense = typeof(RegHelper).GetMethod("ValidateLicense", new Type[] { typeof(string), typeof(bool) });
            RuntimeHelpers.PrepareMethod(mdValidateLicense.MethodHandle);
            var isValidActCode = mdValidateLicense.Invoke(o1, new object[] { this.txtActCode.Text, true }); // 验证激活号
            var checkActCode = ValidateLicense(regType, this.txtActCode.Text);
            if (isValidActCode is Boolean && (Boolean)isValidActCode && checkActCode)
            {
                this.txtActCode.BackColor = Color.Green;
            }
            else
            {
                this.txtActCode.BackColor = Color.Black;
            }

            var mdGenReqCode = regType.GetMethod("GenerateRequestCode", new Type[] { typeof(string), typeof(string) });
            RuntimeHelpers.PrepareMethod(mdGenReqCode.MethodHandle);
            var RequestCode = mdGenReqCode.Invoke(o, new object[] { this.txtSerialNumber.Text, this.cbMac.SelectedValue }); // 生成要求码
            this.txtRequestCode.Text = RequestCode.ToString();
        }

        /// <summary>
        /// DLL中的方法来验证数据的正确性
        /// </summary>
        /// <param name="actCode"></param>
        /// <returns></returns>
        private bool ValidateLicense(Type type, string actCode)
        {
            var o = Activator.CreateInstance(type, new object[] { });
            var md = type.GetMethod("ValidateLicense", new Type[] { typeof(string), typeof(Boolean) });
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = (bool)md.Invoke(o, new object[] { actCode, true });
            return res;
        }

        /// <summary>
        /// DLL中的方法来验证数据的正确性
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        private bool IsValidSerialNumber(Type type, string sn)
        {
            var o = Activator.CreateInstance(type, new object[] { });
            var md = type.GetMethod("IsValidSerialNumber", BindingFlags.Instance | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(md.MethodHandle);
            MethodBody body = md.GetMethodBody();
            byte[] bytes = body.GetILAsByteArray();
            var res = (bool)md.Invoke(o, new object[] { sn });
            return res;
        }

        /// <summary>
        /// 生成序列号(RegHelper)
        /// </summary>
        /// <returns></returns>
        private string GenerateSerialNumber(string countryCode, string regionCode, string numberOfPowerUsers)
        {
            object[] snArray = new object[2]; // 用于存储16进制的序列号

            var o = Activator.CreateInstance(typeof(RegHelper), new object[] { });

            Random random = new Random();

            // 国家码// 五位
            var countryCodeStr = GetValidCalcString(countryCode);

            // 地区码// 二位
            var regionCodeStr = GetValidCalcString(regionCode);

            // 授权数// 三位
            var numberOfPowerUsersStr = GetValidCalcString(numberOfPowerUsers);

            // 以下生成前15位序列号
            object[] snArray1 = new object[5];

            snArray1.SetValue(random.Next(9), 0); // 第0位
            snArray1.SetValue(countryCodeStr.PadLeft(6, (char)0x30), 1); // 第1、2、3、4、5、6位
            snArray1.SetValue(regionCodeStr.PadLeft(3, (char)0x30), 2); // 第7、8、9位
            System.Threading.Thread.Sleep(1);
            snArray1.SetValue(random.Next(99).ToString().PadLeft(2, (char)0x30), 3); // 第10、11位
            snArray1.SetValue(numberOfPowerUsersStr.PadLeft(4, (char)0x30), 4); // 第12、13、14、15位
            var seg1 = string.Concat(snArray1); // 16位数字
            var mdReverse = typeof(RegHelper).GetMethod("Reverse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdReverse.MethodHandle);
            seg1 = mdReverse.Invoke(o, new object[] { seg1 }).ToString(); // 反转
            seg1 = Int64.Parse(seg1).ToString("X"); // 10进制转16进制
            seg1 = seg1.PadLeft(15, (char)0x30); // 补全15位
            snArray.SetValue(seg1, 0);

            // 生成中间部分序列号（此部分序列号无关紧要）
            object[] snArray2 = new object[5];
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 0);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 1);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 2);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 3);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 4);
            snArray.SetValue(string.Concat(snArray2), 1);

            var sn = string.Concat(snArray);
            sn = sn.Insert(5, "-");
            sn = sn.Insert(11, "-");
            sn = sn.Insert(17, "-");
            return sn;
        }

        /// <summary>
        /// 计算激活码
        /// </summary>
        /// <returns></returns>
        private string GenerateActCode(string macCode, string pws)
        {
            object[] snArray = new object[3]; // 用于存储16进制的序列号

            var o = Activator.CreateInstance(typeof(RegHelper), new object[] { });
            Random random = new Random();

            object[] childArray = new object[5];

            //.2位随机数
            childArray.SetValue(random.Next(15).ToString("X"), 0);
            System.Threading.Thread.Sleep(1);
            childArray.SetValue(random.Next(15).ToString("X"), 1);

            // MAC地址
            childArray.SetValue(macCode, 2);

            // 授权数（三位）
            var numberOfPowerUsers = pws;
            childArray.SetValue(int.Parse(numberOfPowerUsers).ToString("X").PadLeft(3, (char)0x30), 3);

            // 1位随机数
            System.Threading.Thread.Sleep(1);
            childArray.SetValue(random.Next(15).ToString("X"), 4);

            // 连起来
            var seg1 = string.Concat(childArray);

            // 反转
            var mdReverse = typeof(RegHelper).GetMethod("Reverse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdReverse.MethodHandle);
            seg1 = mdReverse.Invoke(o, new object[] { seg1 }).ToString();

            snArray.SetValue(seg1, 0); // 序列号第一部分

            // 两位随机数
            System.Threading.Thread.Sleep(1);
            snArray.SetValue(random.Next(1, 9).ToString("X"), 1); // 第21位必须是1~9的数字
            System.Threading.Thread.Sleep(1);
            snArray.SetValue(random.Next(15).ToString("X"), 2);

            var actCode = string.Concat(snArray);
            actCode = actCode.Insert(5, "-");
            actCode = actCode.Insert(11, "-");
            actCode = actCode.Insert(17, "-");

            // 计算最后一部分
            var mdCalcSegment5 = typeof(RegHelper).GetMethod("CalcSegment5", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdCalcSegment5.MethodHandle);
            var lastSegment = mdCalcSegment5.Invoke(o, new object[] { actCode }).ToString();

            return string.Concat(actCode, "-", lastSegment);
        }

        /// <summary>
        /// 生成序列号(RegHelper2005)
        /// </summary>
        /// <returns></returns>
        private string GenerateSerialNumber2005(string countryCode, string regionCode, string numberOfPowerUsers, string siteLicense, string product)
        {
            object[] snArray = new object[3]; // 用于存储16进制的序列号

            var o = Activator.CreateInstance(typeof(RegHelper), new object[] { });

            Random random = new Random();

            // 国家码// 五位
            var countryCodeStr = GetValidCalcString(countryCode);

            // 地区码// 二位
            var regionCodeStr = GetValidCalcString(regionCode);

            // 产品码// 二位
            var productStr = GetValidCalcString(product);

            // 授权数// 三位
            // 站点码// 二位
            var pwsSiteLicense = string.Format("{0}{1}", numberOfPowerUsers.PadLeft(3, (char)0x30), siteLicense.PadLeft(2, (char)0x30)); // 连起来
            var pwsSiteLicenseStr = GetValidCalcString(pwsSiteLicense);

            // 以下生成前15位序列号
            object[] snArray1 = new object[5];

            snArray1.SetValue(random.Next(9), 0); // 第0位
            snArray1.SetValue(countryCodeStr.PadLeft(6, (char)0x30), 1); // 第1、2、3、4、5、6位
            snArray1.SetValue(regionCodeStr.PadLeft(3, (char)0x30), 2); // 第7、8、9位
            System.Threading.Thread.Sleep(1);
            snArray1.SetValue(random.Next(99).ToString().PadLeft(2, (char)0x30), 3); // 第10、11位
            snArray1.SetValue(pwsSiteLicenseStr.PadLeft(6, (char)0x30), 4); // 第12、13、14、15、16、17位
            var seg1 = string.Concat(snArray1); // 18位数字
            var mdReverse = typeof(RegHelper).GetMethod("Reverse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdReverse.MethodHandle);
            seg1 = mdReverse.Invoke(o, new object[] { seg1 }).ToString(); // 反转
            seg1 = Int64.Parse(seg1).ToString("X"); // 10进制转16进制
            seg1 = seg1.PadLeft(15, (char)0x30); // 补全15位
            snArray.SetValue(seg1, 0);

            // 生成中间部分序列号（此部分序列号无关紧要）
            object[] snArray2 = new object[5];
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 0);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 1);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 2);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 3);
            System.Threading.Thread.Sleep(1);
            snArray2.SetValue((random.Next(14) + 1).ToString("X"), 4);
            snArray.SetValue(string.Concat(snArray2), 1);

            // 生成最后一部分序列号
            object[] snArray3 = new object[5];
            var str = int.Parse(productStr).ToString("X").PadLeft(2, (char)0x30);
            snArray3.SetValue(str, 0);
            for (int i = 0; i < snArray3.Length; i++)
            {
                if (str.Length > i)
                {
                    snArray3.SetValue(str[i], i);
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                    snArray3.SetValue((random.Next(14) + 1).ToString("X"), i);
                }
            }
            snArray.SetValue(string.Concat(snArray3), 2);

            var sn = string.Concat(snArray);
            sn = sn.Insert(5, "-");
            sn = sn.Insert(11, "-");
            sn = sn.Insert(17, "-");
            sn = sn.Insert(23, "-");
            return sn;
        }

        /// <summary>
        /// 计算可用的CODE串
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetValidCalcString(string code)
        {
            if (mc == null)
            {
                var o = Activator.CreateInstance(typeof(RegHelper), new object[] { });

                mc = new MapCollection();

                var mdCheckSum = typeof(RegHelper).GetMethod("CheckSum", BindingFlags.Instance | BindingFlags.NonPublic);
                RuntimeHelpers.PrepareMethod(mdCheckSum.MethodHandle);

                Parallel.For(0, 10, i =>
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        var res = mdCheckSum.Invoke(o, new object[] { j, i });
                        if (res is Boolean && (Boolean)res)
                        {
                            mc.Add(i.ToString(), j.ToString());
                        }
                    }
                });
            }

            Random random = new Random();

            var selCode = mc.Cast<DictionaryEntry>().AsParallel()
                .Where(p => Int64.Parse(p.Value.ToString()).Equals(Int64.Parse(code)))
                .Select(p => p.Key.ToString()).ToArray();

            var codechecksum = selCode.GetValue(random.Next(selCode.Count()));

            return code + codechecksum;
        }

        private void cbAlgorithm_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbAlgorithm.Checked)
            {
                txtSiteLicense.Enabled = false;
                txtProduct.Enabled = false;
            }
            else
            {
                txtSiteLicense.Enabled = true;
                txtProduct.Enabled = true;
            }
        }

        private void cbNewAlgorithm_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbNewAlgorithm.Checked)
            {
                txtNewSite.Enabled = false;
                txtNewProduct.Enabled = false;
            }
            else
            {
                txtNewSite.Enabled = true;
                txtNewProduct.Enabled = true;
            }
        }

        private void btnNewSerialNumber_Click(object sender, EventArgs e)
        {
            #region 验证
            var pws = 0;
            if (!int.TryParse(txtNewPws.Text, out pws))
            {
                MessageBox.Show("Wrong number of licenses!");
                return;
            }
            var countrycode = 0;
            if (!int.TryParse(txtNewCountryCode.Text, out countrycode))
            {
                MessageBox.Show("Wrong number of Country Code!");
                return;
            }
            var regioncode = 0;
            if (!int.TryParse(txtNewRegion.Text, out regioncode))
            {
                MessageBox.Show("Wrong number of Region Code!");
                return;
            }

            var sitelicense = 0;
            if (!int.TryParse(txtNewSite.Text, out sitelicense))
            {
                MessageBox.Show("Wrong number of Site License!");
                return;
            }

            var productcode = 0;
            if (!int.TryParse(txtNewProduct.Text, out productcode))
            {
                MessageBox.Show("Wrong number of Product Code!");
                return;
            }
            #endregion

            Type regType = null;
            if (cbNewAlgorithm.Checked)
            {
                regType = typeof(RegHelper2005);
                this.txtNewSerialNumber.Text = GenerateSerialNumber2005(txtNewCountryCode.Text, txtNewRegion.Text, txtNewPws.Text, txtNewSite.Text, txtNewProduct.Text);
            }
            else
            {
                regType = typeof(RegHelper);
                this.txtNewSerialNumber.Text = GenerateSerialNumber(txtNewCountryCode.Text, txtNewRegion.Text, txtNewPws.Text);
            }

            var o = Activator.CreateInstance(regType, new object[] { });
            var mdIsValidSerialNumber = regType.GetMethod("IsValidSerialNumber", BindingFlags.Instance | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(mdIsValidSerialNumber.MethodHandle);
            var isValidSN = mdIsValidSerialNumber.Invoke(o, new object[] { this.txtNewSerialNumber.Text }); // 验证序列号
            var checkSn = IsValidSerialNumber(regType, this.txtNewSerialNumber.Text);
            if (isValidSN is Boolean && (Boolean)isValidSN && checkSn)
            {
                this.txtNewSerialNumber.BackColor = Color.Green;
            }
            else
            {
                this.txtNewSerialNumber.BackColor = Color.Black;
            }
        }

        private void btnNewRequestCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewSerialNumber.Text))
            {
                MessageBox.Show("Generate Serial Number first!");
                return;
            }
            if (this.cmbNewMac.SelectedValue == null || string.IsNullOrEmpty(this.cmbNewMac.SelectedValue.ToString()))
            {
                MessageBox.Show("Select MAC first!");
                return;
            }

            Type regType = null;
            if (cbNewAlgorithm.Checked)
            {
                regType = typeof(RegHelper2005);
            }
            else
            {
                regType = typeof(RegHelper);
            }
            var mdGenReqCode = regType.GetMethod("GenerateRequestCode", new Type[] { typeof(string), typeof(string) });
            RuntimeHelpers.PrepareMethod(mdGenReqCode.MethodHandle);
            var o = Activator.CreateInstance(regType, new object[] { });
            var RequestCode = mdGenReqCode.Invoke(o, new object[] { this.txtNewSerialNumber.Text, this.cmbNewMac.SelectedValue }); // 生成要求码
            this.txtNewRequestCode.Text = RequestCode.ToString();
        }

        private void btnNewActCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewSerialNumber.Text))
            {
                MessageBox.Show("Generate Serial Number first!");
                return;
            }
            if (string.IsNullOrEmpty(txtNewRequestCode.Text))
            {
                MessageBox.Show("Generate Request Code first!");
                return;
            }

            Type regType = null;
            if (cbNewAlgorithm.Checked)
            {
                regType = typeof(RegHelper2005);
            }
            else
            {
                regType = typeof(RegHelper);
            }

            var o1 = Activator.CreateInstance(regType, new object[] { });

            // 根据要求码获取MAC地址及授权数
            var mac = "";
            var numberOfPowerUsers = "";

            var V1 = txtNewRequestCode.Text.Replace("-", ""); // 20位
            var V8 = V1.Substring(0, 18); // 取前18位
            var V7 = V1.Substring(18, 1); // 第19位

            var mdConfuse = regType.GetMethod("Confuse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdConfuse.MethodHandle);
            V8 = mdConfuse.Invoke(o1, new object[] { V8, int.Parse(V7) }).ToString(); // 反混淆

            var mdReverse = regType.GetMethod("Reverse", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(mdReverse.MethodHandle);
            V8 = mdReverse.Invoke(o1, new object[] { V8 }).ToString(); // 反混淆

            // 从要求码中得到MAC和受权数
            mac = V8.Substring(0, V8.Length - 4 - 2);
            numberOfPowerUsers = V8.Substring(V8.Length - 4, 3);

            this.txtNewActCode.Text = GenerateActCode(mac, numberOfPowerUsers);

            var mdValidateLicense = regType.GetMethod("ValidateLicense", new Type[] { typeof(string), typeof(bool) });
            RuntimeHelpers.PrepareMethod(mdValidateLicense.MethodHandle);
            var isValidActCode = mdValidateLicense.Invoke(o1, new object[] { this.txtNewActCode.Text, true }); // 验证激活号
            var checkActCode = ValidateLicense(regType, this.txtNewActCode.Text);
            if (isValidActCode is Boolean && (Boolean)isValidActCode && checkActCode)
            {
                this.txtNewActCode.BackColor = Color.Green;
            }
            else
            {
                this.txtNewActCode.BackColor = Color.Black;
            }
        }
    }
}
