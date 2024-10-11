namespace Notepad.Controls
{
    partial class SettingControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            SettingStackPanel = new AntdUI.StackPanel();
            StartupPanel = new AntdUI.Panel();
            StartupSwitch = new AntdUI.Switch();
            StartupLabel = new AntdUI.Label();
            MinPanel = new AntdUI.Panel();
            MinSwitch = new AntdUI.Switch();
            MinLabel = new AntdUI.Label();
            OpenHotKeyPanel = new AntdUI.Panel();
            OpenHotKeyInput = new AntdUI.Input();
            OpenHotKeyLabel = new AntdUI.Label();
            LockHotKeyPanel = new AntdUI.Panel();
            LockHotKeyInput = new AntdUI.Input();
            LockHotKeyLabel = new AntdUI.Label();
            OtherDivider = new AntdUI.Divider();
            TimeoutPanel = new AntdUI.Panel();
            TimeoutInputNumber = new AntdUI.InputNumber();
            TimeoutLabel = new AntdUI.Label();
            PINPanel = new AntdUI.Panel();
            PINButton = new AntdUI.Button();
            PINLabel = new AntdUI.Label();
            EncryptKeyPanel = new AntdUI.Panel();
            EncryptKeyInput = new AntdUI.Input();
            EncryptKeyLabel = new AntdUI.Label();
            EncryptPanel = new AntdUI.Panel();
            EncryptSwitch = new AntdUI.Switch();
            EncryptLabel = new AntdUI.Label();
            SecurityDivider = new AntdUI.Divider();
            LangPanel = new AntdUI.Panel();
            LangSelect = new AntdUI.Select();
            LangLabel = new AntdUI.Label();
            PrimaryColorPanel = new AntdUI.Panel();
            ResetButton = new AntdUI.Button();
            ColorPicker = new AntdUI.ColorPicker();
            PrimaryLabel = new AntdUI.Label();
            ThemePanel = new AntdUI.Panel();
            ThemeSelect = new AntdUI.Select();
            ThemeLabel = new AntdUI.Label();
            SettingStackPanel.SuspendLayout();
            StartupPanel.SuspendLayout();
            MinPanel.SuspendLayout();
            OpenHotKeyPanel.SuspendLayout();
            LockHotKeyPanel.SuspendLayout();
            TimeoutPanel.SuspendLayout();
            PINPanel.SuspendLayout();
            EncryptKeyPanel.SuspendLayout();
            EncryptPanel.SuspendLayout();
            LangPanel.SuspendLayout();
            PrimaryColorPanel.SuspendLayout();
            ThemePanel.SuspendLayout();
            SuspendLayout();
            // 
            // SettingStackPanel
            // 
            SettingStackPanel.AutoScroll = true;
            SettingStackPanel.Controls.Add(StartupPanel);
            SettingStackPanel.Controls.Add(MinPanel);
            SettingStackPanel.Controls.Add(OpenHotKeyPanel);
            SettingStackPanel.Controls.Add(LockHotKeyPanel);
            SettingStackPanel.Controls.Add(OtherDivider);
            SettingStackPanel.Controls.Add(TimeoutPanel);
            SettingStackPanel.Controls.Add(PINPanel);
            SettingStackPanel.Controls.Add(EncryptKeyPanel);
            SettingStackPanel.Controls.Add(EncryptPanel);
            SettingStackPanel.Controls.Add(SecurityDivider);
            SettingStackPanel.Controls.Add(LangPanel);
            SettingStackPanel.Controls.Add(PrimaryColorPanel);
            SettingStackPanel.Controls.Add(ThemePanel);
            SettingStackPanel.Dock = DockStyle.Fill;
            SettingStackPanel.Gap = 5;
            SettingStackPanel.Location = new Point(0, 0);
            SettingStackPanel.Name = "SettingStackPanel";
            SettingStackPanel.Padding = new Padding(5);
            SettingStackPanel.Size = new Size(250, 597);
            SettingStackPanel.TabIndex = 0;
            SettingStackPanel.TabStop = false;
            SettingStackPanel.Vertical = true;
            // 
            // StartupPanel
            // 
            StartupPanel.Controls.Add(StartupSwitch);
            StartupPanel.Controls.Add(StartupLabel);
            StartupPanel.Location = new Point(8, 392);
            StartupPanel.Name = "StartupPanel";
            StartupPanel.Radius = 3;
            StartupPanel.Size = new Size(234, 25);
            StartupPanel.TabIndex = 0;
            StartupPanel.TabStop = false;
            // 
            // StartupSwitch
            // 
            StartupSwitch.AutoCheck = true;
            StartupSwitch.Dock = DockStyle.Right;
            StartupSwitch.Location = new Point(204, 0);
            StartupSwitch.Name = "StartupSwitch";
            StartupSwitch.Padding = new Padding(0, 5, 0, 5);
            StartupSwitch.Size = new Size(30, 25);
            StartupSwitch.TabIndex = 1;
            StartupSwitch.TabStop = false;
            StartupSwitch.WaveSize = 0;
            // 
            // StartupLabel
            // 
            StartupLabel.Dock = DockStyle.Left;
            StartupLabel.Location = new Point(0, 0);
            StartupLabel.Name = "StartupLabel";
            StartupLabel.Size = new Size(193, 25);
            StartupLabel.TabIndex = 0;
            StartupLabel.TabStop = false;
            StartupLabel.Text = "开机自启动";
            StartupLabel.TooltipConfig = null;
            // 
            // MinPanel
            // 
            MinPanel.Controls.Add(MinSwitch);
            MinPanel.Controls.Add(MinLabel);
            MinPanel.Location = new Point(8, 356);
            MinPanel.Name = "MinPanel";
            MinPanel.Radius = 3;
            MinPanel.Size = new Size(234, 25);
            MinPanel.TabIndex = 0;
            MinPanel.TabStop = false;
            // 
            // MinSwitch
            // 
            MinSwitch.AutoCheck = true;
            MinSwitch.Dock = DockStyle.Right;
            MinSwitch.Location = new Point(204, 0);
            MinSwitch.Name = "MinSwitch";
            MinSwitch.Padding = new Padding(0, 5, 0, 5);
            MinSwitch.Size = new Size(30, 25);
            MinSwitch.TabIndex = 1;
            MinSwitch.TabStop = false;
            MinSwitch.WaveSize = 0;
            // 
            // MinLabel
            // 
            MinLabel.Dock = DockStyle.Left;
            MinLabel.Location = new Point(0, 0);
            MinLabel.Name = "MinLabel";
            MinLabel.Size = new Size(193, 25);
            MinLabel.TabIndex = 0;
            MinLabel.TabStop = false;
            MinLabel.Text = "关闭时最小化至托盘";
            MinLabel.TooltipConfig = null;
            // 
            // OpenHotKeyPanel
            // 
            OpenHotKeyPanel.Controls.Add(OpenHotKeyInput);
            OpenHotKeyPanel.Controls.Add(OpenHotKeyLabel);
            OpenHotKeyPanel.Location = new Point(8, 320);
            OpenHotKeyPanel.Name = "OpenHotKeyPanel";
            OpenHotKeyPanel.Radius = 3;
            OpenHotKeyPanel.Size = new Size(234, 25);
            OpenHotKeyPanel.TabIndex = 0;
            OpenHotKeyPanel.TabStop = false;
            // 
            // OpenHotKeyInput
            // 
            OpenHotKeyInput.AllowClear = true;
            OpenHotKeyInput.CaretColor = Color.Transparent;
            OpenHotKeyInput.Dock = DockStyle.Right;
            OpenHotKeyInput.ImeMode = ImeMode.Disable;
            OpenHotKeyInput.Location = new Point(118, 0);
            OpenHotKeyInput.Name = "OpenHotKeyInput";
            OpenHotKeyInput.Radius = 3;
            OpenHotKeyInput.Size = new Size(116, 25);
            OpenHotKeyInput.TabIndex = 1;
            OpenHotKeyInput.TabStop = false;
            OpenHotKeyInput.WaveSize = 0;
            OpenHotKeyInput.TextChanged += HotKeyInput_TextChanged;
            OpenHotKeyInput.KeyDown += HotKeyInput_KeyDown;
            OpenHotKeyInput.KeyUp += HotKeyInput_KeyUp;
            // 
            // OpenHotKeyLabel
            // 
            OpenHotKeyLabel.Dock = DockStyle.Left;
            OpenHotKeyLabel.Location = new Point(0, 0);
            OpenHotKeyLabel.Name = "OpenHotKeyLabel";
            OpenHotKeyLabel.Size = new Size(120, 25);
            OpenHotKeyLabel.TabIndex = 0;
            OpenHotKeyLabel.TabStop = false;
            OpenHotKeyLabel.Text = "面板快捷键";
            OpenHotKeyLabel.TooltipConfig = null;
            // 
            // LockHotKeyPanel
            // 
            LockHotKeyPanel.Controls.Add(LockHotKeyInput);
            LockHotKeyPanel.Controls.Add(LockHotKeyLabel);
            LockHotKeyPanel.Location = new Point(8, 284);
            LockHotKeyPanel.Name = "LockHotKeyPanel";
            LockHotKeyPanel.Radius = 3;
            LockHotKeyPanel.Size = new Size(234, 25);
            LockHotKeyPanel.TabIndex = 0;
            LockHotKeyPanel.TabStop = false;
            // 
            // LockHotKeyInput
            // 
            LockHotKeyInput.AllowClear = true;
            LockHotKeyInput.CaretColor = Color.Transparent;
            LockHotKeyInput.Dock = DockStyle.Right;
            LockHotKeyInput.ImeMode = ImeMode.Disable;
            LockHotKeyInput.Location = new Point(118, 0);
            LockHotKeyInput.Name = "LockHotKeyInput";
            LockHotKeyInput.Radius = 3;
            LockHotKeyInput.Size = new Size(116, 25);
            LockHotKeyInput.TabIndex = 1;
            LockHotKeyInput.TabStop = false;
            LockHotKeyInput.WaveSize = 0;
            LockHotKeyInput.TextChanged += HotKeyInput_TextChanged;
            LockHotKeyInput.KeyDown += HotKeyInput_KeyDown;
            LockHotKeyInput.KeyUp += HotKeyInput_KeyUp;
            // 
            // LockHotKeyLabel
            // 
            LockHotKeyLabel.Dock = DockStyle.Left;
            LockHotKeyLabel.Location = new Point(0, 0);
            LockHotKeyLabel.Name = "LockHotKeyLabel";
            LockHotKeyLabel.Size = new Size(120, 25);
            LockHotKeyLabel.TabIndex = 0;
            LockHotKeyLabel.TabStop = false;
            LockHotKeyLabel.Text = "锁快捷键";
            LockHotKeyLabel.TooltipConfig = null;
            // 
            // OtherDivider
            // 
            OtherDivider.Location = new Point(8, 272);
            OtherDivider.Name = "OtherDivider";
            OtherDivider.Size = new Size(234, 1);
            OtherDivider.TabIndex = 1;
            OtherDivider.TabStop = false;
            OtherDivider.Thickness = 1F;
            // 
            // TimeoutPanel
            // 
            TimeoutPanel.Controls.Add(TimeoutInputNumber);
            TimeoutPanel.Controls.Add(TimeoutLabel);
            TimeoutPanel.Location = new Point(8, 236);
            TimeoutPanel.Name = "TimeoutPanel";
            TimeoutPanel.Radius = 3;
            TimeoutPanel.Size = new Size(234, 25);
            TimeoutPanel.TabIndex = 0;
            TimeoutPanel.TabStop = false;
            // 
            // TimeoutInputNumber
            // 
            TimeoutInputNumber.Dock = DockStyle.Right;
            TimeoutInputNumber.Location = new Point(134, 0);
            TimeoutInputNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            TimeoutInputNumber.Name = "TimeoutInputNumber";
            TimeoutInputNumber.Radius = 3;
            TimeoutInputNumber.ShowControl = false;
            TimeoutInputNumber.Size = new Size(100, 25);
            TimeoutInputNumber.SuffixText = "分钟";
            TimeoutInputNumber.TabIndex = 1;
            TimeoutInputNumber.TabStop = false;
            TimeoutInputNumber.Text = "1";
            TimeoutInputNumber.TextAlign = HorizontalAlignment.Center;
            TimeoutInputNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
            TimeoutInputNumber.WaveSize = 0;
            // 
            // TimeoutLabel
            // 
            TimeoutLabel.Dock = DockStyle.Left;
            TimeoutLabel.Location = new Point(0, 0);
            TimeoutLabel.Name = "TimeoutLabel";
            TimeoutLabel.Size = new Size(120, 25);
            TimeoutLabel.TabIndex = 0;
            TimeoutLabel.TabStop = false;
            TimeoutLabel.Text = "超时时间";
            TimeoutLabel.TooltipConfig = null;
            // 
            // PINPanel
            // 
            PINPanel.Controls.Add(PINButton);
            PINPanel.Controls.Add(PINLabel);
            PINPanel.Location = new Point(8, 200);
            PINPanel.Name = "PINPanel";
            PINPanel.Radius = 3;
            PINPanel.Size = new Size(234, 25);
            PINPanel.TabIndex = 0;
            PINPanel.TabStop = false;
            // 
            // PINButton
            // 
            PINButton.Dock = DockStyle.Right;
            PINButton.Ghost = true;
            PINButton.Location = new Point(187, 0);
            PINButton.Name = "PINButton";
            PINButton.Padding = new Padding(1);
            PINButton.Radius = 3;
            PINButton.Size = new Size(47, 25);
            PINButton.TabIndex = 1;
            PINButton.TabStop = false;
            PINButton.Text = "设置";
            PINButton.WaveSize = 0;
            PINButton.Click += PINButton_Click;
            // 
            // PINLabel
            // 
            PINLabel.Dock = DockStyle.Left;
            PINLabel.Location = new Point(0, 0);
            PINLabel.Name = "PINLabel";
            PINLabel.Size = new Size(120, 25);
            PINLabel.TabIndex = 0;
            PINLabel.TabStop = false;
            PINLabel.Text = "PIN";
            PINLabel.TooltipConfig = null;
            // 
            // EncryptKeyPanel
            // 
            EncryptKeyPanel.Controls.Add(EncryptKeyInput);
            EncryptKeyPanel.Controls.Add(EncryptKeyLabel);
            EncryptKeyPanel.Location = new Point(8, 164);
            EncryptKeyPanel.Name = "EncryptKeyPanel";
            EncryptKeyPanel.Radius = 3;
            EncryptKeyPanel.Size = new Size(234, 25);
            EncryptKeyPanel.TabIndex = 0;
            EncryptKeyPanel.TabStop = false;
            // 
            // EncryptKeyInput
            // 
            EncryptKeyInput.CaretSpeed = 700;
            EncryptKeyInput.Dock = DockStyle.Right;
            EncryptKeyInput.Location = new Point(118, 0);
            EncryptKeyInput.Name = "EncryptKeyInput";
            EncryptKeyInput.Radius = 3;
            EncryptKeyInput.Size = new Size(116, 25);
            EncryptKeyInput.TabIndex = 1;
            EncryptKeyInput.TabStop = false;
            EncryptKeyInput.UseSystemPasswordChar = true;
            EncryptKeyInput.WaveSize = 0;
            // 
            // EncryptKeyLabel
            // 
            EncryptKeyLabel.Dock = DockStyle.Left;
            EncryptKeyLabel.Location = new Point(0, 0);
            EncryptKeyLabel.Name = "EncryptKeyLabel";
            EncryptKeyLabel.Size = new Size(120, 25);
            EncryptKeyLabel.TabIndex = 0;
            EncryptKeyLabel.TabStop = false;
            EncryptKeyLabel.Text = "加密密钥";
            EncryptKeyLabel.TooltipConfig = null;
            // 
            // EncryptPanel
            // 
            EncryptPanel.Controls.Add(EncryptSwitch);
            EncryptPanel.Controls.Add(EncryptLabel);
            EncryptPanel.Location = new Point(8, 128);
            EncryptPanel.Name = "EncryptPanel";
            EncryptPanel.Radius = 3;
            EncryptPanel.Size = new Size(234, 25);
            EncryptPanel.TabIndex = 0;
            EncryptPanel.TabStop = false;
            // 
            // EncryptSwitch
            // 
            EncryptSwitch.AutoCheck = true;
            EncryptSwitch.Dock = DockStyle.Right;
            EncryptSwitch.Location = new Point(204, 0);
            EncryptSwitch.Name = "EncryptSwitch";
            EncryptSwitch.Padding = new Padding(0, 5, 0, 5);
            EncryptSwitch.Size = new Size(30, 25);
            EncryptSwitch.TabIndex = 1;
            EncryptSwitch.TabStop = false;
            EncryptSwitch.WaveSize = 0;
            // 
            // EncryptLabel
            // 
            EncryptLabel.Dock = DockStyle.Left;
            EncryptLabel.Location = new Point(0, 0);
            EncryptLabel.Name = "EncryptLabel";
            EncryptLabel.Size = new Size(120, 25);
            EncryptLabel.TabIndex = 0;
            EncryptLabel.TabStop = false;
            EncryptLabel.Text = "加密数据";
            EncryptLabel.TooltipConfig = null;
            // 
            // SecurityDivider
            // 
            SecurityDivider.Location = new Point(8, 116);
            SecurityDivider.Name = "SecurityDivider";
            SecurityDivider.Size = new Size(234, 1);
            SecurityDivider.TabIndex = 1;
            SecurityDivider.TabStop = false;
            SecurityDivider.Thickness = 1F;
            // 
            // LangPanel
            // 
            LangPanel.Controls.Add(LangSelect);
            LangPanel.Controls.Add(LangLabel);
            LangPanel.Location = new Point(8, 80);
            LangPanel.Name = "LangPanel";
            LangPanel.Radius = 3;
            LangPanel.Size = new Size(234, 25);
            LangPanel.TabIndex = 0;
            LangPanel.TabStop = false;
            // 
            // LangSelect
            // 
            LangSelect.Dock = DockStyle.Right;
            LangSelect.List = true;
            LangSelect.Location = new Point(118, 0);
            LangSelect.MaxCount = 10;
            LangSelect.Name = "LangSelect";
            LangSelect.Radius = 3;
            LangSelect.Size = new Size(116, 25);
            LangSelect.TabIndex = 1;
            LangSelect.TabStop = false;
            LangSelect.WaveSize = 0;
            // 
            // LangLabel
            // 
            LangLabel.Dock = DockStyle.Left;
            LangLabel.Location = new Point(0, 0);
            LangLabel.Name = "LangLabel";
            LangLabel.Size = new Size(120, 25);
            LangLabel.TabIndex = 0;
            LangLabel.TabStop = false;
            LangLabel.Text = "语言";
            LangLabel.TooltipConfig = null;
            // 
            // PrimaryColorPanel
            // 
            PrimaryColorPanel.Controls.Add(ResetButton);
            PrimaryColorPanel.Controls.Add(ColorPicker);
            PrimaryColorPanel.Controls.Add(PrimaryLabel);
            PrimaryColorPanel.Location = new Point(8, 44);
            PrimaryColorPanel.Name = "PrimaryColorPanel";
            PrimaryColorPanel.Radius = 3;
            PrimaryColorPanel.Size = new Size(234, 25);
            PrimaryColorPanel.TabIndex = 0;
            PrimaryColorPanel.TabStop = false;
            // 
            // ResetButton
            // 
            ResetButton.Dock = DockStyle.Right;
            ResetButton.Ghost = true;
            ResetButton.Location = new Point(166, 0);
            ResetButton.Name = "ResetButton";
            ResetButton.Radius = 3;
            ResetButton.Size = new Size(43, 25);
            ResetButton.TabIndex = 2;
            ResetButton.TabStop = false;
            ResetButton.Text = "重置";
            ResetButton.WaveSize = 0;
            ResetButton.Click += ResetButton_Click;
            // 
            // ColorPicker
            // 
            ColorPicker.Dock = DockStyle.Right;
            ColorPicker.Location = new Point(209, 0);
            ColorPicker.Name = "ColorPicker";
            ColorPicker.Padding = new Padding(1);
            ColorPicker.Radius = 3;
            ColorPicker.Round = true;
            ColorPicker.Size = new Size(25, 25);
            ColorPicker.TabIndex = 1;
            ColorPicker.TabStop = false;
            ColorPicker.Value = Color.FromArgb(22, 119, 255);
            ColorPicker.WaveSize = 0;
            // 
            // PrimaryLabel
            // 
            PrimaryLabel.Dock = DockStyle.Left;
            PrimaryLabel.Location = new Point(0, 0);
            PrimaryLabel.Name = "PrimaryLabel";
            PrimaryLabel.Size = new Size(120, 25);
            PrimaryLabel.TabIndex = 0;
            PrimaryLabel.TabStop = false;
            PrimaryLabel.Text = "主色调";
            PrimaryLabel.TooltipConfig = null;
            // 
            // ThemePanel
            // 
            ThemePanel.Controls.Add(ThemeSelect);
            ThemePanel.Controls.Add(ThemeLabel);
            ThemePanel.Location = new Point(8, 8);
            ThemePanel.Name = "ThemePanel";
            ThemePanel.Radius = 3;
            ThemePanel.Size = new Size(234, 25);
            ThemePanel.TabIndex = 0;
            ThemePanel.TabStop = false;
            ThemePanel.Text = "panel1";
            // 
            // ThemeSelect
            // 
            ThemeSelect.Dock = DockStyle.Right;
            ThemeSelect.List = true;
            ThemeSelect.Location = new Point(118, 0);
            ThemeSelect.Name = "ThemeSelect";
            ThemeSelect.Radius = 3;
            ThemeSelect.Size = new Size(116, 25);
            ThemeSelect.TabIndex = 1;
            ThemeSelect.TabStop = false;
            ThemeSelect.WaveSize = 0;
            // 
            // ThemeLabel
            // 
            ThemeLabel.Dock = DockStyle.Left;
            ThemeLabel.Location = new Point(0, 0);
            ThemeLabel.Name = "ThemeLabel";
            ThemeLabel.Size = new Size(120, 25);
            ThemeLabel.TabIndex = 0;
            ThemeLabel.TabStop = false;
            ThemeLabel.Text = "主题";
            ThemeLabel.TooltipConfig = null;
            // 
            // SettingControl
            // 
            Controls.Add(SettingStackPanel);
            DoubleBuffered = true;
            MinimumSize = new Size(250, 280);
            Name = "SettingControl";
            Padding = new Padding(0, 0, 0, 5);
            Size = new Size(250, 602);
            SettingStackPanel.ResumeLayout(false);
            StartupPanel.ResumeLayout(false);
            MinPanel.ResumeLayout(false);
            OpenHotKeyPanel.ResumeLayout(false);
            LockHotKeyPanel.ResumeLayout(false);
            TimeoutPanel.ResumeLayout(false);
            PINPanel.ResumeLayout(false);
            EncryptKeyPanel.ResumeLayout(false);
            EncryptPanel.ResumeLayout(false);
            LangPanel.ResumeLayout(false);
            PrimaryColorPanel.ResumeLayout(false);
            ThemePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.StackPanel SettingStackPanel;
        private AntdUI.Panel ThemePanel;
        private AntdUI.Label ThemeLabel;
        private AntdUI.Select ThemeSelect;
        private AntdUI.Panel LangPanel;
        private AntdUI.Select LangSelect;
        private AntdUI.Label LangLabel;
        private AntdUI.Label MinLabel;
        private AntdUI.Panel MinPanel;
        private AntdUI.Switch MinSwitch;
        private AntdUI.Panel EncryptPanel;
        private AntdUI.Switch EncryptSwitch;
        private AntdUI.Label EncryptLabel;
        private AntdUI.Panel TimeoutPanel;
        private AntdUI.Label TimeoutLabel;
        private AntdUI.Panel PINPanel;
        private AntdUI.Label PINLabel;
        private AntdUI.InputNumber TimeoutInputNumber;
        private AntdUI.Panel EncryptKeyPanel;
        private AntdUI.Label EncryptKeyLabel;
        private AntdUI.Input EncryptKeyInput;
        private AntdUI.Button PINButton;
        private AntdUI.Divider SecurityDivider;
        private AntdUI.Divider OtherDivider;
        private AntdUI.Panel PrimaryColorPanel;
        private AntdUI.Label PrimaryLabel;
        private AntdUI.ColorPicker ColorPicker;
        private AntdUI.Button ResetButton;
        private AntdUI.Panel OpenHotKeyPanel;
        private AntdUI.Input OpenHotKeyInput;
        private AntdUI.Label OpenHotKeyLabel;
        private AntdUI.Panel LockHotKeyPanel;
        private AntdUI.Input LockHotKeyInput;
        private AntdUI.Label LockHotKeyLabel;
        private AntdUI.Panel StartupPanel;
        private AntdUI.Switch StartupSwitch;
        private AntdUI.Label StartupLabel;
    }
}
