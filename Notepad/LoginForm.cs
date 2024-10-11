using AntdUI;
using AntdUI.Theme;
using Notepad.Controls;
using Notepad.Entities;
using Notepad.Extensions;
using Notepad.Properties;
using System.Globalization;

namespace Notepad
{
    public partial class LoginForm : Window
    {
        private CancellationTokenSource? _autoLoginCts;
        private bool _isInputPassword;
        private bool _canClickLogin;
        private LocalConfig _localConfig = LocalConfig.Config;
        private readonly Color _defaultPrimaryColor = new Light().Primary;

        /// <summary>
        /// MouseRaised
        /// </summary>
        private event EventHandler? MouseRaised;

        public LoginForm()
        {
            InitializeComponent();

            LoadLocalConfig();

            AccountInput.LostFocus += (_, _) =>
            {
                if (!AccountInput.Text.IsNullOrEmpty() && !AccountInput.Text.IsEmail())
                {
                    if (AccountInput.BorderColor != Color.Red)
                    {
                        SetValidInputBorderColor(AccountInput, false);
                    }
                }
                else
                {
                    SetValidInputBorderColor(AccountInput);
                }
            };
            ShowPasswordButton.Click += (_, _) =>
            {
                if (_isInputPassword)
                {
                    PasswordInput.UseSystemPasswordChar = false;
                }
            };
            ShowPasswordButton.MouseLeave += (_, _) => PasswordInput.UseSystemPasswordChar = true;
            PasswordInput.GotFocus += (_, _) => SetValidInputBorderColor(PasswordInput);
            PasswordInput.TextChanged += (_, _) => PasswordInput.Tag = PasswordInput.Text;
            RememberLabel.Click += OnCheckBoxLabelClick;
            AutoLoginLabel.Click += OnCheckBoxLabelClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var rectangle = Screen.GetBounds(this);
            Location = new Point((rectangle.Width - Width) / 2, (int)((rectangle.Height - Height) * 0.35));
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(100);

                LoadingWindow? loading = null;

                if (!LocalConfig.WaitLoad(TimeSpan.FromMilliseconds(10)))
                {
                    Invoke(() =>
                    {
                        Opacity = 1;

                        loading = this.Loading(Resources.Loading_LoadingSettings, 0);
                    });

                    LocalConfig.WaitLoad();
                }

                await Invoke(async () =>
                {
                    _canClickLogin = true;

                    if (!_localConfig.Account.IsNullOrBlank() && !_localConfig.Password.IsNullOrBlank())
                    {
                        LoginButton.Focus();
                    }

                    if (null != loading)
                    {
                        loading.Close();
                    }
                    else
                    {
                        Opacity = 1;
                    }

                    if (!await AutoLoginAsync())
                    {
                        ShowAccounts(LocalConfig.LoginConfig.Users);
                    }
                });
            });
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (!_canClickLogin || null != _autoLoginCts)
            {
                return;
            }

            if (AccountInput.Text.IsNullOrEmpty() || !AccountInput.Text.IsEmail())
            {
                AccountInput.Focus();

                if (AccountInput.BorderColor != Color.Red)
                {
                    SetValidInputBorderColor(AccountInput, false);
                }

                return;
            }

            var password = PasswordInput.Tag?.ToString();
            if (password.IsNullOrBlank())
            {
                PasswordInput.Focus();

                SetValidInputBorderColor(PasswordInput, false);

                return;
            }

            var user = AccountInput.Text.GetEmailServer(password!);
            if (!user.HasValue)
            {
                this.Loading(Resources.Loading_EMallNotSupport, 0, isShowLoading: false).CloseAfter(1500);

                return;
            }

            try
            {
                LoginButton.Text = Resources.LoginButton_Text_Logging;
                LoginButton.Loading = true;

                await Store.LoginAsync(user.Value);

                var hasChange = false;
                if (_localConfig.IsRememberPassword != RememberCheckbox.Checked)
                {
                    _localConfig.IsRememberPassword = RememberCheckbox.Checked;

                    hasChange = true;
                }

                if (_localConfig.IsAutoLogin != AutoLoginCheckbox.Checked)
                {
                    _localConfig.IsAutoLogin = AutoLoginCheckbox.Checked;

                    hasChange = true;
                }

                if (_localConfig.Account != AccountInput.Text)
                {
                    _localConfig.Account = AccountInput.Text;

                    hasChange = true;
                }

                if (RememberCheckbox.Checked)
                {
                    password = password!.Encrypt(_localConfig.Account.Md5());

                    if (_localConfig.Password != password)
                    {
                        _localConfig.Password = password;

                        hasChange = true;
                    }
                }
                else if (!_localConfig.Password.IsNullOrBlank())
                {
                    _localConfig.Password = null;

                    hasChange = true;
                }

                if (!hasChange && user.Value.Account != LocalConfig.LoginConfig.LastUser)
                {
                    hasChange = true;
                }

                if (hasChange)
                {
                    await _localConfig.SaveAsync(true);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.Loading($"{Resources.Loading_LoginFailed}{ex.Message}", 0, false, iconType: TType.Error).CloseAfter(2000);
            }
            finally
            {
                LoginButton.Text = Resources.LoginButton_Text;
                LoginButton.Loading = false;
            }
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {
            if (sender is Input input && !input.Text.IsNullOrEmpty() && input.BorderColor == Color.Red && (input != AccountInput || AccountInput.Text.IsEmail()))
            {
                SetValidInputBorderColor(input);
            }

            if (sender == PasswordInput && _isInputPassword)
            {
                ShowPasswordButton.Enabled = PasswordInput.Text.Length > 0;
            }

            if (sender == AccountInput)
            {
                PasswordInput.Text = string.Empty;
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (null != _autoLoginCts)
            {
                e.SuppressKeyPress = true;

                return;
            }

            if (e.KeyCode is Keys.Space)
            {
                e.SuppressKeyPress = true;

                return;
            }

            if (e.KeyCode is Keys.Enter)
            {
                e.SuppressKeyPress = true;

                LoginButton_Click(sender, e);

                return;
            }

            if (sender == PasswordInput && !_isInputPassword)
            {
                PasswordInput.Text = string.Empty;
                PasswordInput.Tag = string.Empty;
                _isInputPassword = true;
            }
        }

        /// <summary>
        /// LoadLocalConfig
        /// </summary>
        private void LoadLocalConfig()
        {
            LocalConfig.Loaded += c =>
            {
                _localConfig = c;

                Invoke(LoadUserConfig);
            };

            Task.Factory.StartNew(() => LocalConfig.LoadLocalConfigAsync());
        }

        /// <summary>
        /// LoadUserConfig
        /// </summary>
        public void LoadUserConfig()
        {
            LoadStyle();

            RememberCheckbox.Checked = _localConfig.IsRememberPassword;
            AutoLoginCheckbox.Checked = _localConfig.IsAutoLogin;

            if (_localConfig.IsRememberPassword)
            {
                if (!_localConfig.Account.IsNullOrBlank())
                {
                    AccountInput.Text = _localConfig.Account!;
                }

                if (!_localConfig.Password.IsNullOrBlank())
                {
                    try
                    {
                        var password = _localConfig.Password!.Decrypt(_localConfig.Account!.Md5());
                        PasswordInput.Text = string.Empty.PadLeft(password.Length, ' ');
                        PasswordInput.Tag = password;
                    }
                    catch
                    {
                        PasswordInput.Text = string.Empty;
                    }
                }
            }
            else
            {
                PasswordInput.Text = string.Empty;
            }

            LoadLang();
        }

        /// <summary>
        /// AutoLoginAsync
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AutoLoginAsync()
        {
            if (_localConfig.IsAutoLogin && !_localConfig.Account.IsNullOrBlank())
            {
                var password = PasswordInput.Tag?.ToString();
                if (!password.IsNullOrBlank())
                {
                    var user = _localConfig.Account!.GetEmailServer(password!);
                    if (user.HasValue)
                    {
                        LoginButton.Click += OnLoginButtonCancel;

                        var isLogined = false;
                        var countdown = 3;
                        _autoLoginCts = new CancellationTokenSource();

                        var isSmallFont = Resources.Culture.IsSmallFont();
                        if (isSmallFont)
                        {
                            LoginButton.Font = new Font(Font.FontFamily, 10);
                        }

                        try
                        {
                            for (int i = 0; i < countdown; i++)
                            {
                                if (_autoLoginCts.IsCancellationRequested)
                                {
                                    return false;
                                }

                                LoginButton.Text = Resources.LoginButton_Text_AutoLogin.Format(countdown - i);

                                try
                                {
                                    await Task.Delay(1000, _autoLoginCts.Token);
                                }
                                catch
                                {
                                    return false;
                                }
                            }

                            LoginButton.Loading = true;
                            LoginButton.Text = Resources.LoginButton_Text_Logging;

                            await Store.LoginAsync(user.Value);

                            DialogResult = DialogResult.OK;
                            isLogined = true;

                            return true;
                        }
                        catch (Exception ex)
                        {
                            this.Loading($"{Resources.Loading_LoginFailed}{ex.Message}", 0, isShowLoading: false).CloseAfter(1500);

                            return false;
                        }
                        finally
                        {
                            LoginButton.Click -= OnLoginButtonCancel;

                            if (isSmallFont)
                            {
                                LoginButton.Font = new Font(Font.FontFamily, 10.5F);
                            }

                            LoginButton.Text = Resources.LoginButton_Text;

                            if (!isLogined && _localConfig.IsRememberPassword)
                            {
                                PasswordInput.Text = password ?? string.Empty;
                            }

                            LoginButton.Loading = false;
                            _autoLoginCts = null;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// OnCheckBoxLabelClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckBoxLabelClick(object? sender, EventArgs e)
        {
            if (sender == RememberLabel)
            {
                RememberCheckbox.Checked = !RememberCheckbox.Checked;
            }
            else if (sender == AutoLoginLabel)
            {
                AutoLoginCheckbox.Checked = !AutoLoginCheckbox.Checked;
                RememberCheckbox.Checked = AutoLoginCheckbox.Checked;
            }
        }

        /// <summary>
        /// SetValidInputBorderColor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoginButtonCancel(object? sender, EventArgs e)
        {
            if (sender == LoginButton && null != _autoLoginCts)
            {
                _autoLoginCts.Cancel();
            }
        }

        /// <summary>
        /// SetValidInputBorderColor
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isValid"></param>
        private void SetValidInputBorderColor(Input input, bool isValid = true)
        {
            if (isValid)
            {
                input.BorderColor = Style.Db.BorderColor;
                input.BorderHover = Style.Db.BorderColor;
                input.BorderActive = Style.Db.BorderColor;
            }
            else
            {
                input.BorderColor = Color.Red;
                input.BorderHover = Color.Red;
                input.BorderActive = Color.Red;

                input.Popover(input == AccountInput ? input.Text.IsNullOrBlank() ? Resources.Valid_AccountRequired : Resources.Valid_Account : Resources.Valid_PasswordRequired, 1, TAlign.TL, Rectangle.Empty, font: DefaultFont);
            }
        }

        /// <summary>
        /// ShowAccounts
        /// </summary>
        /// <param name="accounts"></param>
        private void ShowAccounts(HashSet<string> accounts)
        {
            if (0 == accounts.Count)
            {
                return;
            }

            AccountInput.SuffixSvg = Resources.Svg_Down;

            var switchButton = new AntdUI.Button
            {
                Ghost = true,
                WaveSize = 0,
                ForeColor = Style.Db.BorderColor,
                IconSvg = Resources.Svg_Down,
                BackHover = Color.Transparent,
                BackActive = Color.Transparent,
                Size = new Size(20, 16),
                Location = new Point(265, 170),
                TabStop = false
            };

            var max = Math.Min(4, accounts.Count);
            var accountPanel = new AntdUI.Panel
            {
                Shadow = 10,
                Size = new Size(AccountInput.Width + 18, 30 * max + 20 + (max - 1) * 3 + 10),
                Location = new Point(AccountInput.Location.X - 9, AccountInput.Location.Y + AccountInput.Height),
                Padding = new Padding(5),
                TabStop = false,
                Visible = false
            };
            var panel = new StackPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Vertical = true,
                Gap = 3,
                TabStop = false
            };

            panel.ScrollBar!.Back = false;
            panel.ScrollBar.SIZE = 8;

            void OnHideAccountPanel(object? s, EventArgs e) => accountPanel!.Visible = false;

            foreach (var account in accounts)
            {
                var rowPanel = new AntdUI.Panel
                {
                    Dock = DockStyle.Top,
                    Margin = new Padding(1, 0, 1, 0),
                    Height = 30,
                    Radius = 3,
                    TabStop = false
                };
                var label = new AntdUI.Label
                {
                    Dock = DockStyle.Top,
                    Padding = new Padding(5, 0, 5, 0),
                    Text = account,
                    Height = rowPanel.Height,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                    TabStop = false
                };
                var button = new AntdUI.Button
                {
                    Dock = DockStyle.Right,
                    Width = rowPanel.Height,
                    Ghost = true,
                    IconSize = new Size(18, 18),
                    IconSvg = Resources.Svg_Delete,
                    WaveSize = 0,
                    ForeColor = Style.Db.Error,
                    Radius = 3,
                    BorderWidth = 0,
                    TabStop = false
                };

                label.MouseClick += async (_, _) =>
                {
                    if (account != AccountInput.Text)
                    {
                        await LocalConfig.LoadUserLocalConfigAsync(account);

                        _localConfig = LocalConfig.Config;

                        AccountInput.Text = account;

                        LoadUserConfig();
                    }

                    accountPanel.Visible = false;
                };
                button.MouseClick += async (_, _) =>
                {
                    if (await LocalConfig.RemoveUserAsync(account))
                    {
                        panel.Controls.Remove(rowPanel);

                        var count = LocalConfig.LoginConfig.Users.Count;

                        if (0 == count)
                        {
                            AccountInput.MouseClick -= OnHideAccountPanel;
                            AccountInput.SuffixSvg = null;
                            MouseRaised -= OnHideAccountPanel;

                            accountPanel.Dispose();
                            switchButton.Dispose();
                        }
                        else if (count < 4)
                        {
                            accountPanel.Height -= 30 + (count > 1 ? 3 : 0);
                        }
                    }
                };

                rowPanel.Controls.Add(label);
                rowPanel.Controls.Add(button);

                label.MouseEnter += (_, _) => rowPanel.Back = Style.Db.PrimaryBg;
                label.MouseLeave += (_, _) => rowPanel.Back = null;

                panel.Controls.Add(rowPanel);
            }

            accountPanel.Controls.Add(panel);
            Controls.Add(switchButton);
            Controls.Add(accountPanel);
            switchButton.BringToFront();
            accountPanel.BringToFront();

            AccountInput.MouseClick += OnHideAccountPanel;
            accountPanel.LostFocus += OnHideAccountPanel;
            switchButton.MouseClick += (_, _) => accountPanel.Visible = !accountPanel.Visible;
            accountPanel.VisibleChanged += (_, _) => switchButton.IconSvg = accountPanel.Visible ? Resources.Svg_Up : Resources.Svg_Down;
            MouseRaised += OnHideAccountPanel;
        }

        /// <summary>
        /// LoadStyle
        /// </summary>
        private void LoadStyle()
        {
            if (_localConfig.PrimaryColor.HasValue)
            {
                Style.Db.SetPrimary(_localConfig.PrimaryColor.Value);

                var colors = Style.Db.Primary.GenerateColors();
                LoginButton.BackExtend = $"135, #{colors[6].ToHex()}, #{colors[4].ToHex()}";
            }
            else if (Style.Db.Primary != _defaultPrimaryColor)
            {
                Style.Db.SetPrimary(_defaultPrimaryColor);

                LoginButton.BackExtend = "135, #6253E1, #04BEFE";
            }

            BackColor = Style.Db.BgBase;
            ForeColor = Style.Db.TextBase;
            Avatar.ForeColor = Style.Db.Primary;
            AccountInput.BorderActive = Style.Db.BorderColor;
            AccountInput.BorderHover = Style.Db.BorderColor;
            PasswordInput.BorderActive = Style.Db.BorderColor;
            PasswordInput.BorderHover = Style.Db.BorderColor;
            ShowPasswordButton.ForeColor = Style.Db.BorderColor;
            ShowPasswordButton.BackHover = Color.Transparent;
            ShowPasswordButton.BackActive = Color.Transparent;
        }

        /// <summary>
        /// LoadLang
        /// </summary>
        private void LoadLang()
        {
            Resources.Culture.Load();

            if (!Resources.Culture.IsDefaultCulture())
            {
                Resources.Culture = 0 == _localConfig.Lang ? null : CultureInfo.GetCultureInfo(LocalConfig.Langs[_localConfig.Lang]);

                var isSmallFont = Resources.Culture.IsSmallFont();

                AccountInput.PrefixText = Resources.AccountInput_PrefixText;
                AccountInput.PlaceholderText = Resources.AccountInput_PlaceholderText;
                PasswordInput.PrefixText = Resources.PasswordInput_PrefixText;
                RememberLabel.Text = Resources.RememberLabel_Text;
                RememberLabel.Font = isSmallFont ? new Font(Font.FontFamily, 8) : DefaultFont;
                AutoLoginLabel.Text = Resources.AutoLoginLabel_Text;
                AutoLoginLabel.Font = isSmallFont ? new Font(Font.FontFamily, 8) : DefaultFont;
                LoginButton.Text = Resources.LoginButton_Text;
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg is >= 0x0201 and <= 0x0209)
            {
                MouseRaised?.Invoke(this, EventArgs.Empty);
            }

            base.WndProc(ref m);
        }
    }
}