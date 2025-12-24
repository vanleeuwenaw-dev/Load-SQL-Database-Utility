namespace LoadSQLDatabase
{
    partial class EditConnectionForm
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
            this.DataSourceLbl = new System.Windows.Forms.Label();
            this.ConnectionStringLbl = new System.Windows.Forms.Label();
            this.DataSourceTxtBx = new System.Windows.Forms.TextBox();
            this.ConnectionStringTxtBx = new System.Windows.Forms.TextBox();
            this.GenerateConnectionStringBtn = new System.Windows.Forms.Button();
            this.SaveConnectionStringBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.DbNameLbl = new System.Windows.Forms.Label();
            this.DatabaseNameTxtBx = new System.Windows.Forms.TextBox();
            this.IntegratedSecurityCBx = new System.Windows.Forms.CheckBox();
            this.EncryptDataCBx = new System.Windows.Forms.CheckBox();
            this.TrustServerCerticateCBx = new System.Windows.Forms.CheckBox();
            this.UseWindowsAuthenticationCBx = new System.Windows.Forms.CheckBox();
            this.UserIdLbl = new System.Windows.Forms.Label();
            this.MyPasswordLbl = new System.Windows.Forms.Label();
            this.UserIDTxtBx = new System.Windows.Forms.TextBox();
            this.PasswordTxtBx = new System.Windows.Forms.TextBox();
            this.TestConnectionBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DataSourceLbl
            // 
            this.DataSourceLbl.AutoSize = true;
            this.DataSourceLbl.Location = new System.Drawing.Point(36, 41);
            this.DataSourceLbl.Name = "DataSourceLbl";
            this.DataSourceLbl.Size = new System.Drawing.Size(70, 13);
            this.DataSourceLbl.TabIndex = 0;
            this.DataSourceLbl.Text = "Data Source:";
            // 
            // ConnectionStringLbl
            // 
            this.ConnectionStringLbl.AutoSize = true;
            this.ConnectionStringLbl.Location = new System.Drawing.Point(36, 210);
            this.ConnectionStringLbl.Name = "ConnectionStringLbl";
            this.ConnectionStringLbl.Size = new System.Drawing.Size(94, 13);
            this.ConnectionStringLbl.TabIndex = 1;
            this.ConnectionStringLbl.Text = "Connection String:";
            // 
            // DataSourceTxtBx
            // 
            this.DataSourceTxtBx.Location = new System.Drawing.Point(127, 38);
            this.DataSourceTxtBx.Name = "DataSourceTxtBx";
            this.DataSourceTxtBx.Size = new System.Drawing.Size(197, 20);
            this.DataSourceTxtBx.TabIndex = 2;
            this.DataSourceTxtBx.TextChanged += new System.EventHandler(this.DataSourceTxtBx_TextChanged);
            // 
            // ConnectionStringTxtBx
            // 
            this.ConnectionStringTxtBx.Location = new System.Drawing.Point(39, 230);
            this.ConnectionStringTxtBx.Multiline = true;
            this.ConnectionStringTxtBx.Name = "ConnectionStringTxtBx";
            this.ConnectionStringTxtBx.Size = new System.Drawing.Size(441, 48);
            this.ConnectionStringTxtBx.TabIndex = 3;
            this.ConnectionStringTxtBx.TextChanged += new System.EventHandler(this.ConnectionStringTxtBx_TextChanged);
            // 
            // GenerateConnectionStringBtn
            // 
            this.GenerateConnectionStringBtn.Location = new System.Drawing.Point(39, 295);
            this.GenerateConnectionStringBtn.Name = "GenerateConnectionStringBtn";
            this.GenerateConnectionStringBtn.Size = new System.Drawing.Size(99, 39);
            this.GenerateConnectionStringBtn.TabIndex = 4;
            this.GenerateConnectionStringBtn.Text = "Generate Connection String";
            this.GenerateConnectionStringBtn.UseVisualStyleBackColor = true;
            this.GenerateConnectionStringBtn.Click += new System.EventHandler(this.GenerateConnectionStringBtn_Click);
            // 
            // SaveConnectionStringBtn
            // 
            this.SaveConnectionStringBtn.Location = new System.Drawing.Point(157, 295);
            this.SaveConnectionStringBtn.Name = "SaveConnectionStringBtn";
            this.SaveConnectionStringBtn.Size = new System.Drawing.Size(99, 39);
            this.SaveConnectionStringBtn.TabIndex = 5;
            this.SaveConnectionStringBtn.Text = "Save Connection String";
            this.SaveConnectionStringBtn.UseVisualStyleBackColor = true;
            this.SaveConnectionStringBtn.Click += new System.EventHandler(this.SaveConnectionStringBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(381, 295);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(99, 39);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // DbNameLbl
            // 
            this.DbNameLbl.AutoSize = true;
            this.DbNameLbl.Location = new System.Drawing.Point(36, 70);
            this.DbNameLbl.Name = "DbNameLbl";
            this.DbNameLbl.Size = new System.Drawing.Size(87, 13);
            this.DbNameLbl.TabIndex = 7;
            this.DbNameLbl.Text = "Database Name:";
            // 
            // DatabaseNameTxtBx
            // 
            this.DatabaseNameTxtBx.Location = new System.Drawing.Point(127, 70);
            this.DatabaseNameTxtBx.Name = "DatabaseNameTxtBx";
            this.DatabaseNameTxtBx.Size = new System.Drawing.Size(197, 20);
            this.DatabaseNameTxtBx.TabIndex = 8;
            this.DatabaseNameTxtBx.TextChanged += new System.EventHandler(this.DatabaseNameTxtBx_TextChanged);
            // 
            // IntegratedSecurityCBx
            // 
            this.IntegratedSecurityCBx.AutoSize = true;
            this.IntegratedSecurityCBx.Location = new System.Drawing.Point(209, 110);
            this.IntegratedSecurityCBx.Name = "IntegratedSecurityCBx";
            this.IntegratedSecurityCBx.Size = new System.Drawing.Size(115, 17);
            this.IntegratedSecurityCBx.TabIndex = 9;
            this.IntegratedSecurityCBx.Text = "Integrated Security";
            this.IntegratedSecurityCBx.UseVisualStyleBackColor = true;
            this.IntegratedSecurityCBx.CheckedChanged += new System.EventHandler(this.IntegratedSecurityCBx_CheckedChanged);
            // 
            // EncryptDataCBx
            // 
            this.EncryptDataCBx.AutoSize = true;
            this.EncryptDataCBx.Location = new System.Drawing.Point(39, 179);
            this.EncryptDataCBx.Name = "EncryptDataCBx";
            this.EncryptDataCBx.Size = new System.Drawing.Size(88, 17);
            this.EncryptDataCBx.TabIndex = 10;
            this.EncryptDataCBx.Text = "Encrypt Data";
            this.EncryptDataCBx.UseVisualStyleBackColor = true;
            this.EncryptDataCBx.CheckedChanged += new System.EventHandler(this.EncryptDataCBx_CheckedChanged);
            // 
            // TrustServerCerticateCBx
            // 
            this.TrustServerCerticateCBx.AutoSize = true;
            this.TrustServerCerticateCBx.Location = new System.Drawing.Point(330, 110);
            this.TrustServerCerticateCBx.Name = "TrustServerCerticateCBx";
            this.TrustServerCerticateCBx.Size = new System.Drawing.Size(134, 17);
            this.TrustServerCerticateCBx.TabIndex = 11;
            this.TrustServerCerticateCBx.Text = "Trust Server Certificate";
            this.TrustServerCerticateCBx.UseVisualStyleBackColor = true;
            this.TrustServerCerticateCBx.CheckedChanged += new System.EventHandler(this.TrustServerCerticateCBx_CheckedChanged);
            // 
            // UseWindowsAuthenticationCBx
            // 
            this.UseWindowsAuthenticationCBx.AutoSize = true;
            this.UseWindowsAuthenticationCBx.Location = new System.Drawing.Point(36, 110);
            this.UseWindowsAuthenticationCBx.Name = "UseWindowsAuthenticationCBx";
            this.UseWindowsAuthenticationCBx.Size = new System.Drawing.Size(163, 17);
            this.UseWindowsAuthenticationCBx.TabIndex = 12;
            this.UseWindowsAuthenticationCBx.Text = "Use Windows Authentication";
            this.UseWindowsAuthenticationCBx.UseVisualStyleBackColor = true;
            this.UseWindowsAuthenticationCBx.CheckedChanged += new System.EventHandler(this.UseWindowsAuthenticationCBx_CheckedChanged);
            // 
            // UserIdLbl
            // 
            this.UserIdLbl.AutoSize = true;
            this.UserIdLbl.Location = new System.Drawing.Point(36, 146);
            this.UserIdLbl.Name = "UserIdLbl";
            this.UserIdLbl.Size = new System.Drawing.Size(46, 13);
            this.UserIdLbl.TabIndex = 13;
            this.UserIdLbl.Text = "User ID:";
            // 
            // MyPasswordLbl
            // 
            this.MyPasswordLbl.AutoSize = true;
            this.MyPasswordLbl.Location = new System.Drawing.Point(223, 146);
            this.MyPasswordLbl.Name = "MyPasswordLbl";
            this.MyPasswordLbl.Size = new System.Drawing.Size(56, 13);
            this.MyPasswordLbl.TabIndex = 14;
            this.MyPasswordLbl.Text = "Password:";
            // 
            // UserIDTxtBx
            // 
            this.UserIDTxtBx.Location = new System.Drawing.Point(88, 143);
            this.UserIDTxtBx.Name = "UserIDTxtBx";
            this.UserIDTxtBx.Size = new System.Drawing.Size(111, 20);
            this.UserIDTxtBx.TabIndex = 15;
            this.UserIDTxtBx.TextChanged += new System.EventHandler(this.UserIDTxtBx_TextChanged);
            // 
            // PasswordTxtBx
            // 
            this.PasswordTxtBx.Location = new System.Drawing.Point(294, 143);
            this.PasswordTxtBx.Name = "PasswordTxtBx";
            this.PasswordTxtBx.PasswordChar = '*';
            this.PasswordTxtBx.Size = new System.Drawing.Size(111, 20);
            this.PasswordTxtBx.TabIndex = 16;
            this.PasswordTxtBx.UseSystemPasswordChar = true;
            this.PasswordTxtBx.TextChanged += new System.EventHandler(this.PasswordTxtBx_TextChanged);
            // 
            // TestConnectionBtn
            // 
            this.TestConnectionBtn.Location = new System.Drawing.Point(275, 295);
            this.TestConnectionBtn.Name = "TestConnectionBtn";
            this.TestConnectionBtn.Size = new System.Drawing.Size(99, 39);
            this.TestConnectionBtn.TabIndex = 17;
            this.TestConnectionBtn.Text = "Test Connection";
            this.TestConnectionBtn.UseVisualStyleBackColor = true;
            this.TestConnectionBtn.Click += new System.EventHandler(this.TestConnectionBtn_Click);
            // 
            // EditConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 363);
            this.Controls.Add(this.TestConnectionBtn);
            this.Controls.Add(this.PasswordTxtBx);
            this.Controls.Add(this.UserIDTxtBx);
            this.Controls.Add(this.MyPasswordLbl);
            this.Controls.Add(this.UserIdLbl);
            this.Controls.Add(this.UseWindowsAuthenticationCBx);
            this.Controls.Add(this.TrustServerCerticateCBx);
            this.Controls.Add(this.EncryptDataCBx);
            this.Controls.Add(this.IntegratedSecurityCBx);
            this.Controls.Add(this.DatabaseNameTxtBx);
            this.Controls.Add(this.DbNameLbl);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SaveConnectionStringBtn);
            this.Controls.Add(this.GenerateConnectionStringBtn);
            this.Controls.Add(this.ConnectionStringTxtBx);
            this.Controls.Add(this.DataSourceTxtBx);
            this.Controls.Add(this.ConnectionStringLbl);
            this.Controls.Add(this.DataSourceLbl);
            this.Name = "EditConnectionForm";
            this.Text = "Edit Connection Form";
            this.Load += new System.EventHandler(this.EditConnectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DataSourceLbl;
        private System.Windows.Forms.Label ConnectionStringLbl;
        private System.Windows.Forms.TextBox DataSourceTxtBx;
        private System.Windows.Forms.TextBox ConnectionStringTxtBx;
        private System.Windows.Forms.Button GenerateConnectionStringBtn;
        private System.Windows.Forms.Button SaveConnectionStringBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Label DbNameLbl;
        private System.Windows.Forms.TextBox DatabaseNameTxtBx;
        private System.Windows.Forms.CheckBox IntegratedSecurityCBx;
        private System.Windows.Forms.CheckBox EncryptDataCBx;
        private System.Windows.Forms.CheckBox TrustServerCerticateCBx;
        private System.Windows.Forms.CheckBox UseWindowsAuthenticationCBx;
        private System.Windows.Forms.Label UserIdLbl;
        private System.Windows.Forms.Label MyPasswordLbl;
        private System.Windows.Forms.TextBox UserIDTxtBx;
        private System.Windows.Forms.TextBox PasswordTxtBx;
        private System.Windows.Forms.Button TestConnectionBtn;
    }
}