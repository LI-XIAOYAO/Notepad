using AntdUI;

namespace Notepad
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            AccountInput = new Input();
            LoginWindowBar = new PageHeader();
            PasswordInput = new Input();
            LoginButton = new AntdUI.Button();
            RememberCheckbox = new Checkbox();
            AutoLoginCheckbox = new Checkbox();
            RememberLabel = new AntdUI.Label();
            AutoLoginLabel = new AntdUI.Label();
            LoginPanel = new AntdUI.Panel();
            ShowPasswordButton = new AntdUI.Button();
            CancelLoginButton = new AntdUI.Button();
            CancelButtonPanel = new AntdUI.Panel();
            Avatar = new Avatar();
            CancelButtonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // AccountInput
            // 
            AccountInput.BorderWidth = 1.5F;
            AccountInput.CaretSpeed = 700;
            AccountInput.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            AccountInput.ImeMode = ImeMode.Disable;
            AccountInput.Location = new Point(20, 159);
            AccountInput.Name = "AccountInput";
            AccountInput.PlaceholderText = "请输入邮箱账号";
            AccountInput.PrefixText = "账号";
            AccountInput.Radius = 3;
            AccountInput.Size = new Size(270, 40);
            AccountInput.TabIndex = 0;
            AccountInput.WaveSize = 0;
            AccountInput.TextChanged += Input_TextChanged;
            AccountInput.KeyDown += Input_KeyDown;
            // 
            // LoginWindowBar
            // 
            LoginWindowBar.Dock = DockStyle.Top;
            LoginWindowBar.Location = new Point(0, 0);
            LoginWindowBar.MaximizeBox = false;
            LoginWindowBar.Name = "LoginWindowBar";
            LoginWindowBar.ShowButton = true;
            LoginWindowBar.Size = new Size(310, 35);
            LoginWindowBar.TabIndex = 1;
            LoginWindowBar.TabStop = false;
            // 
            // PasswordInput
            // 
            PasswordInput.BorderWidth = 1.5F;
            PasswordInput.CaretSpeed = 700;
            PasswordInput.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            PasswordInput.ImeMode = ImeMode.Close;
            PasswordInput.Location = new Point(20, 226);
            PasswordInput.Name = "PasswordInput";
            PasswordInput.PrefixText = "密码";
            PasswordInput.Radius = 3;
            PasswordInput.Size = new Size(270, 40);
            PasswordInput.SuffixSvg = "EyeOutlined";
            PasswordInput.TabIndex = 1;
            PasswordInput.UseSystemPasswordChar = true;
            PasswordInput.WaveSize = 0;
            PasswordInput.TextChanged += Input_TextChanged;
            PasswordInput.KeyDown += Input_KeyDown;
            // 
            // LoginButton
            // 
            LoginButton.BackExtend = "135, #6253E1, #04BEFE";
            LoginButton.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            LoginButton.IconSize = new Size(24, 24);
            LoginButton.Location = new Point(70, 375);
            LoginButton.Name = "LoginButton";
            LoginButton.Radius = 3;
            LoginButton.Size = new Size(170, 40);
            LoginButton.TabIndex = 4;
            LoginButton.TabStop = false;
            LoginButton.Text = "登录";
            LoginButton.TextMultiLine = true;
            LoginButton.Type = TTypeMini.Primary;
            LoginButton.WaveSize = 0;
            LoginButton.Click += LoginButton_Click;
            // 
            // RememberCheckbox
            // 
            RememberCheckbox.AutoCheck = true;
            RememberCheckbox.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RememberCheckbox.Location = new Point(37, 285);
            RememberCheckbox.Name = "RememberCheckbox";
            RememberCheckbox.Size = new Size(35, 35);
            RememberCheckbox.TabIndex = 2;
            RememberCheckbox.TabStop = false;
            // 
            // AutoLoginCheckbox
            // 
            AutoLoginCheckbox.AutoCheck = true;
            AutoLoginCheckbox.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            AutoLoginCheckbox.Location = new Point(189, 285);
            AutoLoginCheckbox.Name = "AutoLoginCheckbox";
            AutoLoginCheckbox.Size = new Size(35, 35);
            AutoLoginCheckbox.TabIndex = 3;
            AutoLoginCheckbox.TabStop = false;
            // 
            // RememberLabel
            // 
            RememberLabel.AutoSizeMode = TAutoSize.Width;
            RememberLabel.Cursor = Cursors.Hand;
            RememberLabel.Location = new Point(70, 295);
            RememberLabel.Name = "RememberLabel";
            RememberLabel.Size = new Size(54, 17);
            RememberLabel.TabIndex = 0;
            RememberLabel.TabStop = false;
            RememberLabel.Text = "记住密码";
            RememberLabel.TooltipConfig = null;
            // 
            // AutoLoginLabel
            // 
            AutoLoginLabel.AutoSizeMode = TAutoSize.Width;
            AutoLoginLabel.Cursor = Cursors.Hand;
            AutoLoginLabel.Location = new Point(221, 295);
            AutoLoginLabel.Name = "AutoLoginLabel";
            AutoLoginLabel.Size = new Size(54, 17);
            AutoLoginLabel.TabIndex = 0;
            AutoLoginLabel.TabStop = false;
            AutoLoginLabel.Text = "自动登录";
            AutoLoginLabel.TooltipConfig = null;
            // 
            // LoginPanel
            // 
            LoginPanel.Location = new Point(50, 355);
            LoginPanel.Name = "LoginPanel";
            LoginPanel.Radius = 3;
            LoginPanel.Shadow = 20;
            LoginPanel.ShadowOpacity = 0.3F;
            LoginPanel.ShadowOpacityAnimation = true;
            LoginPanel.ShadowOpacityHover = 0.4F;
            LoginPanel.Size = new Size(210, 80);
            LoginPanel.TabIndex = 5;
            LoginPanel.TabStop = false;
            // 
            // ShowPasswordButton
            // 
            ShowPasswordButton.Enabled = false;
            ShowPasswordButton.ForeColor = Color.Gray;
            ShowPasswordButton.IconSvg = "EyeOutlined";
            ShowPasswordButton.Location = new Point(265, 237);
            ShowPasswordButton.Name = "ShowPasswordButton";
            ShowPasswordButton.Size = new Size(20, 18);
            ShowPasswordButton.TabIndex = 6;
            ShowPasswordButton.TabStop = false;
            ShowPasswordButton.WaveSize = 0;
            // 
            // CancelLoginButton
            // 
            CancelLoginButton.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            CancelLoginButton.Location = new Point(10, 10);
            CancelLoginButton.Name = "CancelLoginButton";
            CancelLoginButton.Radius = 3;
            CancelLoginButton.Size = new Size(90, 40);
            CancelLoginButton.TabIndex = 8;
            CancelLoginButton.TabStop = false;
            CancelLoginButton.Text = "取消";
            CancelLoginButton.WaveSize = 0;
            // 
            // CancelButtonPanel
            // 
            CancelButtonPanel.Controls.Add(CancelLoginButton);
            CancelButtonPanel.Location = new Point(100, 432);
            CancelButtonPanel.Name = "CancelButtonPanel";
            CancelButtonPanel.Radius = 3;
            CancelButtonPanel.Shadow = 10;
            CancelButtonPanel.ShadowOpacityAnimation = true;
            CancelButtonPanel.ShadowOpacityHover = 0.4F;
            CancelButtonPanel.Size = new Size(110, 60);
            CancelButtonPanel.TabIndex = 9;
            CancelButtonPanel.TabStop = false;
            CancelButtonPanel.Visible = false;
            // 
            // Avatar
            // 
            Avatar.ForeColor = Color.FromArgb(22, 119, 255);
            Avatar.ImageSvg = resources.GetString("Avatar.ImageSvg");
            Avatar.Location = new Point(79, 79);
            Avatar.Name = "Avatar";
            Avatar.Size = new Size(152, 33);
            Avatar.TabIndex = 10;
            Avatar.TabStop = false;
            Avatar.Text = "";
            // 
            // LoginForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(310, 500);
            Controls.Add(Avatar);
            Controls.Add(CancelButtonPanel);
            Controls.Add(LoginButton);
            Controls.Add(ShowPasswordButton);
            Controls.Add(AutoLoginLabel);
            Controls.Add(RememberLabel);
            Controls.Add(AutoLoginCheckbox);
            Controls.Add(RememberCheckbox);
            Controls.Add(LoginWindowBar);
            Controls.Add(PasswordInput);
            Controls.Add(AccountInput);
            Controls.Add(LoginPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(310, 500);
            MinimizeBox = false;
            MinimumSize = new Size(310, 500);
            Name = "LoginForm";
            Opacity = 0D;
            Resizable = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Notepad";
            TopMost = true;
            CancelButtonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Input AccountInput;
        private AntdUI.PageHeader LoginWindowBar;
        private Input PasswordInput;
        private AntdUI.Button LoginButton;
        private Checkbox RememberCheckbox;
        private Checkbox AutoLoginCheckbox;
        private AntdUI.Label RememberLabel;
        private AntdUI.Label AutoLoginLabel;
        private AntdUI.Panel LoginPanel;
        private AntdUI.Button ShowPasswordButton;
        private AntdUI.Button CancelLoginButton;
        private AntdUI.Panel CancelButtonPanel;
        private Avatar Avatar;
    }
}