namespace Dumper
{
    partial class WinForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSn = new System.Windows.Forms.Label();
            this.lblActCode = new System.Windows.Forms.Label();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.txtActCode = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblMac = new System.Windows.Forms.Label();
            this.cbMac = new System.Windows.Forms.ComboBox();
            this.lblPws = new System.Windows.Forms.Label();
            this.txtPws = new System.Windows.Forms.TextBox();
            this.lblCountryCode = new System.Windows.Forms.Label();
            this.txtCountryCode = new System.Windows.Forms.TextBox();
            this.lblRegionCode = new System.Windows.Forms.Label();
            this.txtRegionCode = new System.Windows.Forms.TextBox();
            this.lblSiteLicense = new System.Windows.Forms.Label();
            this.txtSiteLicense = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblRequestCode = new System.Windows.Forms.Label();
            this.txtRequestCode = new System.Windows.Forms.TextBox();
            this.cbAlgorithm = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSelf = new System.Windows.Forms.TabPage();
            this.tabReqCode = new System.Windows.Forms.TabPage();
            this.lblNewRegion = new System.Windows.Forms.Label();
            this.cbNewAlgorithm = new System.Windows.Forms.CheckBox();
            this.txtNewProduct = new System.Windows.Forms.TextBox();
            this.txtNewActCode = new System.Windows.Forms.TextBox();
            this.txtNewRequestCode = new System.Windows.Forms.TextBox();
            this.txtNewSerialNumber = new System.Windows.Forms.TextBox();
            this.txtNewSite = new System.Windows.Forms.TextBox();
            this.btnNewRequestCode = new System.Windows.Forms.Button();
            this.btnNewActCode = new System.Windows.Forms.Button();
            this.btnNewSerialNumber = new System.Windows.Forms.Button();
            this.txtNewRegion = new System.Windows.Forms.TextBox();
            this.lblNewMac = new System.Windows.Forms.Label();
            this.txtNewCountryCode = new System.Windows.Forms.TextBox();
            this.cmbNewMac = new System.Windows.Forms.ComboBox();
            this.txtNewPws = new System.Windows.Forms.TextBox();
            this.lblNewPws = new System.Windows.Forms.Label();
            this.lblNewProduct = new System.Windows.Forms.Label();
            this.lblNewCountryCode = new System.Windows.Forms.Label();
            this.lblNewSite = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabSelf.SuspendLayout();
            this.tabReqCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSn
            // 
            this.lblSn.AutoSize = true;
            this.lblSn.Location = new System.Drawing.Point(8, 267);
            this.lblSn.Name = "lblSn";
            this.lblSn.Size = new System.Drawing.Size(83, 12);
            this.lblSn.TabIndex = 0;
            this.lblSn.Text = "SerialNumber:";
            // 
            // lblActCode
            // 
            this.lblActCode.AutoSize = true;
            this.lblActCode.Location = new System.Drawing.Point(8, 291);
            this.lblActCode.Name = "lblActCode";
            this.lblActCode.Size = new System.Drawing.Size(95, 12);
            this.lblActCode.TabIndex = 1;
            this.lblActCode.Text = "ActivationCode:";
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.ForeColor = System.Drawing.Color.White;
            this.txtSerialNumber.Location = new System.Drawing.Point(130, 261);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.ReadOnly = true;
            this.txtSerialNumber.Size = new System.Drawing.Size(205, 21);
            this.txtSerialNumber.TabIndex = 8;
            // 
            // txtActCode
            // 
            this.txtActCode.ForeColor = System.Drawing.Color.White;
            this.txtActCode.Location = new System.Drawing.Point(130, 288);
            this.txtActCode.Name = "txtActCode";
            this.txtActCode.ReadOnly = true;
            this.txtActCode.Size = new System.Drawing.Size(205, 21);
            this.txtActCode.TabIndex = 9;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(60, 232);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(236, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblMac
            // 
            this.lblMac.AutoSize = true;
            this.lblMac.Location = new System.Drawing.Point(8, 40);
            this.lblMac.Name = "lblMac";
            this.lblMac.Size = new System.Drawing.Size(29, 12);
            this.lblMac.TabIndex = 5;
            this.lblMac.Text = "MAC:";
            // 
            // cbMac
            // 
            this.cbMac.FormattingEnabled = true;
            this.cbMac.Location = new System.Drawing.Point(130, 37);
            this.cbMac.Name = "cbMac";
            this.cbMac.Size = new System.Drawing.Size(121, 20);
            this.cbMac.TabIndex = 1;
            // 
            // lblPws
            // 
            this.lblPws.AutoSize = true;
            this.lblPws.Location = new System.Drawing.Point(8, 66);
            this.lblPws.Name = "lblPws";
            this.lblPws.Size = new System.Drawing.Size(119, 12);
            this.lblPws.TabIndex = 7;
            this.lblPws.Text = "Number of licenses:";
            // 
            // txtPws
            // 
            this.txtPws.Location = new System.Drawing.Point(130, 66);
            this.txtPws.MaxLength = 3;
            this.txtPws.Name = "txtPws";
            this.txtPws.Size = new System.Drawing.Size(100, 21);
            this.txtPws.TabIndex = 2;
            this.txtPws.Text = "9";
            // 
            // lblCountryCode
            // 
            this.lblCountryCode.AutoSize = true;
            this.lblCountryCode.Location = new System.Drawing.Point(8, 96);
            this.lblCountryCode.Name = "lblCountryCode";
            this.lblCountryCode.Size = new System.Drawing.Size(83, 12);
            this.lblCountryCode.TabIndex = 7;
            this.lblCountryCode.Text = "Country Code:";
            // 
            // txtCountryCode
            // 
            this.txtCountryCode.Location = new System.Drawing.Point(130, 96);
            this.txtCountryCode.MaxLength = 5;
            this.txtCountryCode.Name = "txtCountryCode";
            this.txtCountryCode.Size = new System.Drawing.Size(100, 21);
            this.txtCountryCode.TabIndex = 3;
            this.txtCountryCode.Text = "1028";
            // 
            // lblRegionCode
            // 
            this.lblRegionCode.AutoSize = true;
            this.lblRegionCode.Location = new System.Drawing.Point(8, 127);
            this.lblRegionCode.Name = "lblRegionCode";
            this.lblRegionCode.Size = new System.Drawing.Size(77, 12);
            this.lblRegionCode.TabIndex = 7;
            this.lblRegionCode.Text = "Region Code:";
            // 
            // txtRegionCode
            // 
            this.txtRegionCode.Location = new System.Drawing.Point(130, 127);
            this.txtRegionCode.MaxLength = 2;
            this.txtRegionCode.Name = "txtRegionCode";
            this.txtRegionCode.Size = new System.Drawing.Size(100, 21);
            this.txtRegionCode.TabIndex = 4;
            this.txtRegionCode.Text = "1";
            // 
            // lblSiteLicense
            // 
            this.lblSiteLicense.AutoSize = true;
            this.lblSiteLicense.Location = new System.Drawing.Point(8, 160);
            this.lblSiteLicense.Name = "lblSiteLicense";
            this.lblSiteLicense.Size = new System.Drawing.Size(83, 12);
            this.lblSiteLicense.TabIndex = 7;
            this.lblSiteLicense.Text = "Site License:";
            // 
            // txtSiteLicense
            // 
            this.txtSiteLicense.Location = new System.Drawing.Point(130, 160);
            this.txtSiteLicense.MaxLength = 2;
            this.txtSiteLicense.Name = "txtSiteLicense";
            this.txtSiteLicense.Size = new System.Drawing.Size(100, 21);
            this.txtSiteLicense.TabIndex = 5;
            this.txtSiteLicense.Text = "1";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(8, 193);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(83, 12);
            this.lblProduct.TabIndex = 7;
            this.lblProduct.Text = "Product Code:";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(130, 193);
            this.txtProduct.MaxLength = 2;
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(100, 21);
            this.txtProduct.TabIndex = 6;
            this.txtProduct.Text = "1";
            // 
            // lblRequestCode
            // 
            this.lblRequestCode.AutoSize = true;
            this.lblRequestCode.Location = new System.Drawing.Point(8, 318);
            this.lblRequestCode.Name = "lblRequestCode";
            this.lblRequestCode.Size = new System.Drawing.Size(77, 12);
            this.lblRequestCode.TabIndex = 1;
            this.lblRequestCode.Text = "RequestCode:";
            // 
            // txtRequestCode
            // 
            this.txtRequestCode.ForeColor = System.Drawing.SystemColors.Window;
            this.txtRequestCode.Location = new System.Drawing.Point(130, 315);
            this.txtRequestCode.Name = "txtRequestCode";
            this.txtRequestCode.ReadOnly = true;
            this.txtRequestCode.Size = new System.Drawing.Size(205, 21);
            this.txtRequestCode.TabIndex = 9;
            // 
            // cbAlgorithm
            // 
            this.cbAlgorithm.AutoSize = true;
            this.cbAlgorithm.Checked = true;
            this.cbAlgorithm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAlgorithm.Location = new System.Drawing.Point(10, 6);
            this.cbAlgorithm.Name = "cbAlgorithm";
            this.cbAlgorithm.Size = new System.Drawing.Size(102, 16);
            this.cbAlgorithm.TabIndex = 11;
            this.cbAlgorithm.Text = "RegHelper2005";
            this.cbAlgorithm.UseVisualStyleBackColor = true;
            this.cbAlgorithm.CheckedChanged += new System.EventHandler(this.cbAlgorithm_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSelf);
            this.tabControl1.Controls.Add(this.tabReqCode);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(404, 382);
            this.tabControl1.TabIndex = 12;
            // 
            // tabSelf
            // 
            this.tabSelf.Controls.Add(this.lblRegionCode);
            this.tabSelf.Controls.Add(this.cbAlgorithm);
            this.tabSelf.Controls.Add(this.lblSn);
            this.tabSelf.Controls.Add(this.txtRequestCode);
            this.tabSelf.Controls.Add(this.lblActCode);
            this.tabSelf.Controls.Add(this.lblRequestCode);
            this.tabSelf.Controls.Add(this.txtActCode);
            this.tabSelf.Controls.Add(this.txtProduct);
            this.tabSelf.Controls.Add(this.txtSerialNumber);
            this.tabSelf.Controls.Add(this.txtSiteLicense);
            this.tabSelf.Controls.Add(this.btnGenerate);
            this.tabSelf.Controls.Add(this.txtRegionCode);
            this.tabSelf.Controls.Add(this.lblMac);
            this.tabSelf.Controls.Add(this.txtCountryCode);
            this.tabSelf.Controls.Add(this.cbMac);
            this.tabSelf.Controls.Add(this.txtPws);
            this.tabSelf.Controls.Add(this.lblPws);
            this.tabSelf.Controls.Add(this.lblProduct);
            this.tabSelf.Controls.Add(this.lblCountryCode);
            this.tabSelf.Controls.Add(this.lblSiteLicense);
            this.tabSelf.Location = new System.Drawing.Point(4, 21);
            this.tabSelf.Name = "tabSelf";
            this.tabSelf.Padding = new System.Windows.Forms.Padding(3);
            this.tabSelf.Size = new System.Drawing.Size(396, 357);
            this.tabSelf.TabIndex = 0;
            this.tabSelf.Text = "本机直接生成";
            this.tabSelf.UseVisualStyleBackColor = true;
            // 
            // tabReqCode
            // 
            this.tabReqCode.Controls.Add(this.lblNewRegion);
            this.tabReqCode.Controls.Add(this.cbNewAlgorithm);
            this.tabReqCode.Controls.Add(this.txtNewProduct);
            this.tabReqCode.Controls.Add(this.txtNewActCode);
            this.tabReqCode.Controls.Add(this.txtNewRequestCode);
            this.tabReqCode.Controls.Add(this.txtNewSerialNumber);
            this.tabReqCode.Controls.Add(this.txtNewSite);
            this.tabReqCode.Controls.Add(this.btnNewRequestCode);
            this.tabReqCode.Controls.Add(this.btnNewActCode);
            this.tabReqCode.Controls.Add(this.btnNewSerialNumber);
            this.tabReqCode.Controls.Add(this.txtNewRegion);
            this.tabReqCode.Controls.Add(this.lblNewMac);
            this.tabReqCode.Controls.Add(this.txtNewCountryCode);
            this.tabReqCode.Controls.Add(this.cmbNewMac);
            this.tabReqCode.Controls.Add(this.txtNewPws);
            this.tabReqCode.Controls.Add(this.lblNewPws);
            this.tabReqCode.Controls.Add(this.lblNewProduct);
            this.tabReqCode.Controls.Add(this.lblNewCountryCode);
            this.tabReqCode.Controls.Add(this.lblNewSite);
            this.tabReqCode.Location = new System.Drawing.Point(4, 21);
            this.tabReqCode.Name = "tabReqCode";
            this.tabReqCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabReqCode.Size = new System.Drawing.Size(396, 357);
            this.tabReqCode.TabIndex = 1;
            this.tabReqCode.Text = "根据要求码生成";
            this.tabReqCode.UseVisualStyleBackColor = true;
            // 
            // lblNewRegion
            // 
            this.lblNewRegion.AutoSize = true;
            this.lblNewRegion.Location = new System.Drawing.Point(8, 130);
            this.lblNewRegion.Name = "lblNewRegion";
            this.lblNewRegion.Size = new System.Drawing.Size(77, 12);
            this.lblNewRegion.TabIndex = 26;
            this.lblNewRegion.Text = "Region Code:";
            // 
            // cbNewAlgorithm
            // 
            this.cbNewAlgorithm.AutoSize = true;
            this.cbNewAlgorithm.Checked = true;
            this.cbNewAlgorithm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewAlgorithm.Location = new System.Drawing.Point(10, 9);
            this.cbNewAlgorithm.Name = "cbNewAlgorithm";
            this.cbNewAlgorithm.Size = new System.Drawing.Size(102, 16);
            this.cbNewAlgorithm.TabIndex = 31;
            this.cbNewAlgorithm.Text = "RegHelper2005";
            this.cbNewAlgorithm.UseVisualStyleBackColor = true;
            this.cbNewAlgorithm.CheckedChanged += new System.EventHandler(this.cbNewAlgorithm_CheckedChanged);
            // 
            // txtNewProduct
            // 
            this.txtNewProduct.Location = new System.Drawing.Point(130, 196);
            this.txtNewProduct.MaxLength = 2;
            this.txtNewProduct.Name = "txtNewProduct";
            this.txtNewProduct.Size = new System.Drawing.Size(100, 21);
            this.txtNewProduct.TabIndex = 21;
            this.txtNewProduct.Text = "1";
            // 
            // txtNewActCode
            // 
            this.txtNewActCode.ForeColor = System.Drawing.Color.White;
            this.txtNewActCode.Location = new System.Drawing.Point(130, 306);
            this.txtNewActCode.Name = "txtNewActCode";
            this.txtNewActCode.ReadOnly = true;
            this.txtNewActCode.Size = new System.Drawing.Size(205, 21);
            this.txtNewActCode.TabIndex = 28;
            // 
            // txtNewRequestCode
            // 
            this.txtNewRequestCode.ForeColor = System.Drawing.Color.White;
            this.txtNewRequestCode.Location = new System.Drawing.Point(130, 272);
            this.txtNewRequestCode.Name = "txtNewRequestCode";
            this.txtNewRequestCode.ReadOnly = true;
            this.txtNewRequestCode.Size = new System.Drawing.Size(205, 21);
            this.txtNewRequestCode.TabIndex = 28;
            // 
            // txtNewSerialNumber
            // 
            this.txtNewSerialNumber.ForeColor = System.Drawing.Color.White;
            this.txtNewSerialNumber.Location = new System.Drawing.Point(130, 237);
            this.txtNewSerialNumber.Name = "txtNewSerialNumber";
            this.txtNewSerialNumber.ReadOnly = true;
            this.txtNewSerialNumber.Size = new System.Drawing.Size(205, 21);
            this.txtNewSerialNumber.TabIndex = 28;
            // 
            // txtNewSite
            // 
            this.txtNewSite.Location = new System.Drawing.Point(130, 163);
            this.txtNewSite.MaxLength = 2;
            this.txtNewSite.Name = "txtNewSite";
            this.txtNewSite.Size = new System.Drawing.Size(100, 21);
            this.txtNewSite.TabIndex = 20;
            this.txtNewSite.Text = "1";
            // 
            // btnNewRequestCode
            // 
            this.btnNewRequestCode.Location = new System.Drawing.Point(6, 272);
            this.btnNewRequestCode.Name = "btnNewRequestCode";
            this.btnNewRequestCode.Size = new System.Drawing.Size(121, 23);
            this.btnNewRequestCode.TabIndex = 27;
            this.btnNewRequestCode.Text = "RequestCode";
            this.btnNewRequestCode.UseVisualStyleBackColor = true;
            this.btnNewRequestCode.Click += new System.EventHandler(this.btnNewRequestCode_Click);
            // 
            // btnNewActCode
            // 
            this.btnNewActCode.Location = new System.Drawing.Point(6, 306);
            this.btnNewActCode.Name = "btnNewActCode";
            this.btnNewActCode.Size = new System.Drawing.Size(121, 23);
            this.btnNewActCode.TabIndex = 27;
            this.btnNewActCode.Text = "ActivationCode";
            this.btnNewActCode.UseVisualStyleBackColor = true;
            this.btnNewActCode.Click += new System.EventHandler(this.btnNewActCode_Click);
            // 
            // btnNewSerialNumber
            // 
            this.btnNewSerialNumber.Location = new System.Drawing.Point(6, 235);
            this.btnNewSerialNumber.Name = "btnNewSerialNumber";
            this.btnNewSerialNumber.Size = new System.Drawing.Size(121, 23);
            this.btnNewSerialNumber.TabIndex = 27;
            this.btnNewSerialNumber.Text = "SerialNumber";
            this.btnNewSerialNumber.UseVisualStyleBackColor = true;
            this.btnNewSerialNumber.Click += new System.EventHandler(this.btnNewSerialNumber_Click);
            // 
            // txtNewRegion
            // 
            this.txtNewRegion.Location = new System.Drawing.Point(130, 130);
            this.txtNewRegion.MaxLength = 2;
            this.txtNewRegion.Name = "txtNewRegion";
            this.txtNewRegion.Size = new System.Drawing.Size(100, 21);
            this.txtNewRegion.TabIndex = 18;
            this.txtNewRegion.Text = "1";
            // 
            // lblNewMac
            // 
            this.lblNewMac.AutoSize = true;
            this.lblNewMac.Location = new System.Drawing.Point(8, 43);
            this.lblNewMac.Name = "lblNewMac";
            this.lblNewMac.Size = new System.Drawing.Size(29, 12);
            this.lblNewMac.TabIndex = 19;
            this.lblNewMac.Text = "MAC:";
            // 
            // txtNewCountryCode
            // 
            this.txtNewCountryCode.Location = new System.Drawing.Point(130, 99);
            this.txtNewCountryCode.MaxLength = 5;
            this.txtNewCountryCode.Name = "txtNewCountryCode";
            this.txtNewCountryCode.Size = new System.Drawing.Size(100, 21);
            this.txtNewCountryCode.TabIndex = 17;
            this.txtNewCountryCode.Text = "1028";
            // 
            // cmbNewMac
            // 
            this.cmbNewMac.FormattingEnabled = true;
            this.cmbNewMac.Location = new System.Drawing.Point(130, 40);
            this.cmbNewMac.Name = "cmbNewMac";
            this.cmbNewMac.Size = new System.Drawing.Size(121, 20);
            this.cmbNewMac.TabIndex = 15;
            // 
            // txtNewPws
            // 
            this.txtNewPws.Location = new System.Drawing.Point(130, 69);
            this.txtNewPws.MaxLength = 3;
            this.txtNewPws.Name = "txtNewPws";
            this.txtNewPws.Size = new System.Drawing.Size(100, 21);
            this.txtNewPws.TabIndex = 16;
            this.txtNewPws.Text = "9";
            // 
            // lblNewPws
            // 
            this.lblNewPws.AutoSize = true;
            this.lblNewPws.Location = new System.Drawing.Point(8, 69);
            this.lblNewPws.Name = "lblNewPws";
            this.lblNewPws.Size = new System.Drawing.Size(119, 12);
            this.lblNewPws.TabIndex = 24;
            this.lblNewPws.Text = "Number of licenses:";
            // 
            // lblNewProduct
            // 
            this.lblNewProduct.AutoSize = true;
            this.lblNewProduct.Location = new System.Drawing.Point(8, 196);
            this.lblNewProduct.Name = "lblNewProduct";
            this.lblNewProduct.Size = new System.Drawing.Size(83, 12);
            this.lblNewProduct.TabIndex = 23;
            this.lblNewProduct.Text = "Product Code:";
            // 
            // lblNewCountryCode
            // 
            this.lblNewCountryCode.AutoSize = true;
            this.lblNewCountryCode.Location = new System.Drawing.Point(8, 99);
            this.lblNewCountryCode.Name = "lblNewCountryCode";
            this.lblNewCountryCode.Size = new System.Drawing.Size(83, 12);
            this.lblNewCountryCode.TabIndex = 25;
            this.lblNewCountryCode.Text = "Country Code:";
            // 
            // lblNewSite
            // 
            this.lblNewSite.AutoSize = true;
            this.lblNewSite.Location = new System.Drawing.Point(8, 163);
            this.lblNewSite.Name = "lblNewSite";
            this.lblNewSite.Size = new System.Drawing.Size(83, 12);
            this.lblNewSite.TabIndex = 22;
            this.lblNewSite.Text = "Site License:";
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 382);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "WinForm";
            this.Text = "Cracker";
            this.tabControl1.ResumeLayout(false);
            this.tabSelf.ResumeLayout(false);
            this.tabSelf.PerformLayout();
            this.tabReqCode.ResumeLayout(false);
            this.tabReqCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSn;
        private System.Windows.Forms.Label lblActCode;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.TextBox txtActCode;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblMac;
        private System.Windows.Forms.ComboBox cbMac;
        private System.Windows.Forms.Label lblPws;
        private System.Windows.Forms.TextBox txtPws;
        private System.Windows.Forms.Label lblCountryCode;
        private System.Windows.Forms.TextBox txtCountryCode;
        private System.Windows.Forms.Label lblRegionCode;
        private System.Windows.Forms.TextBox txtRegionCode;
        private System.Windows.Forms.Label lblSiteLicense;
        private System.Windows.Forms.TextBox txtSiteLicense;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblRequestCode;
        private System.Windows.Forms.TextBox txtRequestCode;
        private System.Windows.Forms.CheckBox cbAlgorithm;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSelf;
        private System.Windows.Forms.TabPage tabReqCode;
        private System.Windows.Forms.Label lblNewRegion;
        private System.Windows.Forms.CheckBox cbNewAlgorithm;
        private System.Windows.Forms.TextBox txtNewProduct;
        private System.Windows.Forms.TextBox txtNewSerialNumber;
        private System.Windows.Forms.TextBox txtNewSite;
        private System.Windows.Forms.Button btnNewSerialNumber;
        private System.Windows.Forms.TextBox txtNewRegion;
        private System.Windows.Forms.Label lblNewMac;
        private System.Windows.Forms.TextBox txtNewCountryCode;
        private System.Windows.Forms.ComboBox cmbNewMac;
        private System.Windows.Forms.TextBox txtNewPws;
        private System.Windows.Forms.Label lblNewPws;
        private System.Windows.Forms.Label lblNewProduct;
        private System.Windows.Forms.Label lblNewCountryCode;
        private System.Windows.Forms.Label lblNewSite;
        private System.Windows.Forms.TextBox txtNewActCode;
        private System.Windows.Forms.TextBox txtNewRequestCode;
        private System.Windows.Forms.Button btnNewRequestCode;
        private System.Windows.Forms.Button btnNewActCode;

    }
}