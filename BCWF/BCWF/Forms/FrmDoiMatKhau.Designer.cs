namespace BCWF.Forms
{
    partial class FrmDoiMatKhau
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
            lblUsername = new Label();
            lblTitle = new Label();
            txtUsername = new TextBox();
            lblOldPassword = new Label();
            txtOldPassword = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            lblConfirm = new Label();
            txtConfirm = new TextBox();
            btnDoi = new Button();
            btnHuy = new Button();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(30, 76);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(110, 20);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.SteelBlue;
            lblTitle.Location = new Point(125, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(194, 32);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "ĐỔI MẬT KHẨU";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(196, 76);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(230, 27);
            txtUsername.TabIndex = 9;
            // 
            // lblOldPassword
            // 
            lblOldPassword.AutoSize = true;
            lblOldPassword.Location = new Point(30, 124);
            lblOldPassword.Name = "lblOldPassword";
            lblOldPassword.Size = new Size(92, 20);
            lblOldPassword.TabIndex = 10;
            lblOldPassword.Text = "Mật khẩu cũ:";
            // 
            // txtOldPassword
            // 
            txtOldPassword.Location = new Point(196, 121);
            txtOldPassword.Name = "txtOldPassword";
            txtOldPassword.PasswordChar = '*';
            txtOldPassword.Size = new Size(230, 27);
            txtOldPassword.TabIndex = 11;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(30, 169);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(103, 20);
            lblNewPassword.TabIndex = 12;
            lblNewPassword.Text = "Mật khẩu mới:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(196, 166);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '*';
            txtNewPassword.Size = new Size(230, 27);
            txtNewPassword.TabIndex = 13;
            // 
            // lblConfirm
            // 
            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(30, 214);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(137, 20);
            lblConfirm.TabIndex = 14;
            lblConfirm.Text = "Xác nhận mật khẩu:";
            // 
            // txtConfirm
            // 
            txtConfirm.Location = new Point(196, 211);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.PasswordChar = '*';
            txtConfirm.Size = new Size(230, 27);
            txtConfirm.TabIndex = 15;
            // 
            // btnDoi
            // 
            btnDoi.BackColor = Color.CornflowerBlue;
            btnDoi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDoi.ForeColor = Color.White;
            btnDoi.Location = new Point(30, 274);
            btnDoi.Name = "btnDoi";
            btnDoi.Size = new Size(150, 62);
            btnDoi.TabIndex = 16;
            btnDoi.Text = "Đổi mật khẩu";
            btnDoi.UseVisualStyleBackColor = false;
            btnDoi.Click += btnDoi_Click;
            // 
            // btnHuy
            // 
            btnHuy.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHuy.Location = new Point(276, 274);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(150, 62);
            btnHuy.TabIndex = 17;
            btnHuy.Text = "Hủy";
            btnHuy.Click += btnHuy_Click;
            // 
            // FrmDoiMatKhau
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 373);
            Controls.Add(btnDoi);
            Controls.Add(btnHuy);
            Controls.Add(txtUsername);
            Controls.Add(lblOldPassword);
            Controls.Add(txtOldPassword);
            Controls.Add(lblNewPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(lblConfirm);
            Controls.Add(txtConfirm);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Name = "FrmDoiMatKhau";
            Text = "Đổi mật khẩu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsername;
        private Label lblTitle;
        private TextBox txtUsername;
        private Label lblOldPassword;
        private TextBox txtOldPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirm;
        private TextBox txtConfirm;
        private Button btnDoi;
        private Button btnHuy;
    }
}