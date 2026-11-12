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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblUsername = new Label();
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
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.SteelBlue;
            lblTitle.Location = new Point(136, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(194, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ĐỔI MẬT KHẨU";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(30, 70);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(110, 20);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(196, 67);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(230, 27);
            txtUsername.TabIndex = 2;
            // 
            // lblOldPassword
            // 
            lblOldPassword.AutoSize = true;
            lblOldPassword.Location = new Point(30, 115);
            lblOldPassword.Name = "lblOldPassword";
            lblOldPassword.Size = new Size(92, 20);
            lblOldPassword.TabIndex = 3;
            lblOldPassword.Text = "Mật khẩu cũ:";
            // 
            // txtOldPassword
            // 
            txtOldPassword.Location = new Point(196, 112);
            txtOldPassword.Name = "txtOldPassword";
            txtOldPassword.PasswordChar = '*';
            txtOldPassword.Size = new Size(230, 27);
            txtOldPassword.TabIndex = 4;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(30, 160);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(103, 20);
            lblNewPassword.TabIndex = 5;
            lblNewPassword.Text = "Mật khẩu mới:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(196, 157);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '*';
            txtNewPassword.Size = new Size(230, 27);
            txtNewPassword.TabIndex = 6;
            // 
            // lblConfirm
            // 
            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(30, 205);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(137, 20);
            lblConfirm.TabIndex = 7;
            lblConfirm.Text = "Xác nhận mật khẩu:";
            // 
            // txtConfirm
            // 
            txtConfirm.Location = new Point(196, 202);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.PasswordChar = '*';
            txtConfirm.Size = new Size(230, 27);
            txtConfirm.TabIndex = 8;
            // 
            // btnDoi
            // 
            btnDoi.BackColor = Color.CornflowerBlue;
            btnDoi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDoi.ForeColor = Color.White;
            btnDoi.Location = new Point(30, 279);
            btnDoi.Name = "btnDoi";
            btnDoi.Size = new Size(150, 62);
            btnDoi.TabIndex = 9;
            btnDoi.Text = "Đổi mật khẩu";
            btnDoi.UseVisualStyleBackColor = false;
            btnDoi.Click += btnDoi_Click;
            // 
            // btnHuy
            // 
            btnHuy.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHuy.Location = new Point(276, 270);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(150, 62);
            btnHuy.TabIndex = 10;
            btnHuy.Text = "Hủy";
            btnHuy.Click += btnHuy_Click;
            // 
            // FrmDoiMatKhau
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 373);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblOldPassword);
            Controls.Add(txtOldPassword);
            Controls.Add(lblNewPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(lblConfirm);
            Controls.Add(txtConfirm);
            Controls.Add(btnDoi);
            Controls.Add(btnHuy);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDoiMatKhau";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đổi Mật Khẩu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblUsername;
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