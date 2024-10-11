using AntdUI;
using AntdUI.Theme;
using Microsoft.Win32;
using Notepad.Entities;
using Notepad.Enums;
using Notepad.Extensions;
using Notepad.Properties;
using System.Globalization;

namespace Notepad.Controls
{
    internal partial class SettingControl : UserControl
    {
        private static readonly Color _defaultLightPrimaryColor = new Light().Primary;
        private static readonly Color _defaultDarkPrimaryColor = new Dark().Primary;
        private readonly LocalConfig _localConfig = LocalConfig.Config;
        private readonly Notes _notes = Store.Cache;
        private string? _pinVal;
        private readonly HashSet<KeyModifiers> _keyModifiers = [];
        private readonly HashSet<Keys> _keys = [];

        public SettingControl()
        {
            InitializeComponent();

            ThemeLabel.Text = Resources.Theme;
            PrimaryLabel.Text = Resources.Setting_PrimaryColor;
            ResetButton.Text = Resources.Setting_PrimaryColorReset;
            LangLabel.Text = Resources.Lang;
            EncryptLabel.Text = Resources.Setting_EncryptData;
            EncryptKeyLabel.Text = Resources.Setting_EncryptKey;
            PINLabel.Text = Resources.PIN;
            PINButton.Text = Resources.Setting;
            TimeoutLabel.Text = Resources.Setting_PINTimeout;
            TimeoutInputNumber.SuffixText = Resources.Setting_PINTimeoutMinute;
            LockHotKeyLabel.Text = Resources.Setting_LockHotKey;
            OpenHotKeyLabel.Text = Resources.Setting_OpenHotKey;
            MinLabel.Text = Resources.Setting_Min;
            StartupLabel.Text = Resources.Setting_Startup;

            ThemeSelect.Items.AddRange(Resources.ThemeSelect_Items.Split(','));
            ThemeSelect.SelectedIndex = _notes.Theme;
            ThemeSelect.SelectedIndexChanged += (_, e) =>
            {
                var primaryColor = ColorPicker.Value;

                if (1 == e.Value && !Config.IsDark)
                {
                    Config.IsDark = true;
                }
                else if (0 == e.Value && !Config.IsLight)
                {
                    Config.IsLight = true;
                }

                if (primaryColor != Style.Db.Primary)
                {
                    Style.Db.SetPrimary(primaryColor);

                    EventHub.Dispatch(EventType.THEME, primaryColor);
                }
            };

            ColorPicker.Value = _notes.PrimaryColor ?? Style.Db.Primary;
            ColorPicker.ValueChanged += (_, e) =>
            {
                Style.Db.SetPrimary(e.Value);

                EventHub.Dispatch(EventType.THEME, e.Value);
            };

            LangSelect.Items.AddRange(Resources.LangSelect_Items.Split(','));
            LangSelect.SelectedIndex = _localConfig.Lang;

            EncryptSwitch.Checked = _notes.IsEncrypt;
            EncryptKeyInput.Text = _notes.IsEncrypt ? string.Empty.PadLeft(9, ' ') : string.Empty;
            EncryptKeyInput.Tag = _localConfig.Key;
            EncryptKeyInput.UseSystemPasswordChar = _notes.IsEncrypt;
            EncryptKeyInput.TextChanged += (_, _) => EncryptKeyInput.Tag = EncryptKeyInput.Text;
            EncryptKeyInput.KeyDown += (_, e) =>
            {
                if (e.KeyCode is Keys.Space)
                {
                    e.SuppressKeyPress = true;
                }
            };

            if (!_notes.IsEncrypt)
            {
                EncryptKeyInput.GotFocus += (_, _) => EncryptKeyInput.UseSystemPasswordChar = false;
                EncryptKeyInput.LostFocus += (_, _) => EncryptKeyInput.UseSystemPasswordChar = true;
            }
            else
            {
                EncryptKeyInput.KeyDown += (_, _) =>
                {
                    EncryptKeyInput.Text = string.Empty;
                    EncryptKeyInput.Tag = string.Empty;
                };
            }

            if (null != _notes.PIN)
            {
                PINButton.Text = Resources.Text_Edit;
                _pinVal = _notes.PIN;
            }

            TimeoutInputNumber.Value = _notes.PINTimeout;

            if (null != _notes.LockHotKeys)
            {
                LockHotKeyInput.Text = _notes.LockHotKeys.ToString();
                LockHotKeyInput.Tag = _notes.LockHotKeys;
            }

            if (null != _notes.OpenkHotKeys)
            {
                OpenHotKeyInput.Text = _notes.OpenkHotKeys.ToString();
                OpenHotKeyInput.Tag = _notes.OpenkHotKeys;
            }

            MinSwitch.Checked = _notes.Min;
            StartupSwitch.Checked = _localConfig.Startup && IsStartup();
        }

        private void PINButton_Click(object sender, EventArgs e)
        {
            var inputWindow = new InputWindow(this, 250, 70, null != _notes.PIN ? Resources.PIN : Resources.PIN_Setting)
            {
                IsNumeric = true,
                IsRequired = false
            };

            inputWindow.Input.MaxLength = 4;
            inputWindow.Input.UseSystemPasswordChar = true;

            int PINStep = 0;
            string? tempPINVal = null;
            var title = (AntdUI.Label)inputWindow.HeaderPanel.Controls["Title"]!;

            inputWindow.Valid += input =>
            {
                if (null != input.Text && input.Text.Length > 0 && input.Text.Length < input.MaxLength)
                {
                    inputWindow.SetDividerColor();

                    return null;
                }

                var current = input.MaxLength == input.Text?.Length ? input.Text : null;

                if (0 == PINStep)
                {
                    if (null != _notes.PIN)
                    {
                        if (_notes.PIN != input.Text)
                        {
                            input.Text = string.Empty;

                            return Resources.PIN_Incorrect;
                        }
                        else
                        {
                            input.Text = string.Empty;
                            title.Text = Resources.PIN_New;
                            PINStep++;
                        }

                        return null;
                    }
                    else
                    {
                        PINStep++;
                    }
                }

                if (1 == PINStep)
                {
                    input.Text = string.Empty;
                    title.Text = Resources.PIN_Confirm;
                    PINStep++;
                    tempPINVal = current;

                    return null;
                }
                else if (2 == PINStep)
                {
                    if (tempPINVal != current)
                    {
                        input.Text = string.Empty;

                        return Resources.PIN_Difference;
                    }
                }

                _pinVal = current;

                if (null != _pinVal)
                {
                    PINButton.Text = Resources.Text_Edit;
                }

                inputWindow.Close();

                return null;
            };
            inputWindow.ShowDialog();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ColorPicker.Value = Config.IsDark ? _defaultDarkPrimaryColor : _defaultLightPrimaryColor;

            EventHub.Dispatch(EventType.THEME, ColorPicker.Value);
        }

        /// <summary>
        /// SaveConfigAsync
        /// </summary>
        /// <returns></returns>
        public async Task SaveConfigAsync()
        {
            var copy = _notes.Copy();
            var key = EncryptKeyInput.Tag?.ToString();

            if (copy.IsEncrypt = EncryptSwitch.Checked && !key.IsNullOrBlank())
            {
                copy.Encrypt(key!, true);
            }
            else
            {
                copy.EncryptData = null;
            }

            copy.Theme = ThemeSelect.SelectedIndex;
            copy.PrimaryColor = (Config.IsDark ? _defaultDarkPrimaryColor == ColorPicker.Value : _defaultLightPrimaryColor == ColorPicker.Value) ? null : ColorPicker.Value;
            copy.PIN = _pinVal;
            copy.PINTimeout = (int)TimeoutInputNumber.Value;
            copy.LockHotKeys = (HotKeys?)LockHotKeyInput.Tag;
            copy.OpenkHotKeys = (HotKeys?)OpenHotKeyInput.Tag;
            copy.Min = MinSwitch.Checked;

            await Store.SaveNotesAsync(copy);

            _localConfig.Lang = LangSelect.SelectedIndex;
            _localConfig.Theme = copy.Theme;
            _localConfig.PrimaryColor = copy.PrimaryColor;
            _localConfig.Key = key;
            _localConfig.Min = copy.Min;
            _localConfig.Startup = StartupSwitch.Checked;

            await _localConfig.SaveAsync();

            _notes.Theme = copy.Theme;
            _notes.PrimaryColor = copy.PrimaryColor;
            _notes.IsEncrypt = copy.IsEncrypt;
            _notes.PIN = copy.PIN;
            _notes.PINTimeout = copy.PINTimeout;
            _notes.LockHotKeys = copy.LockHotKeys;
            _notes.OpenkHotKeys = copy.OpenkHotKeys;
            _notes.Min = copy.Min;

            if (null == _pinVal)
            {
                LockMessageFilter.Stop();
            }
            else
            {
                LockMessageFilter.Start((int)TimeSpan.FromMinutes(_notes.PINTimeout).TotalMilliseconds);
            }

            if (!MinSwitch.Checked)
            {
                MainForm.CloseNotifyIcon();
            }

            RegisterStartup(StartupSwitch.Checked);

            Resources.Culture = 0 == _localConfig.Lang ? null : CultureInfo.GetCultureInfo(LocalConfig.Langs[_localConfig.Lang]);
        }

        private void HotKeyInput_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (e.Control)
            {
                _keyModifiers.Add(KeyModifiers.Ctrl);
            }

            if (e.Alt)
            {
                _keyModifiers.Add(KeyModifiers.Alt);
            }

            if (e.Shift)
            {
                _keyModifiers.Add(KeyModifiers.Shift);
            }

            if (e.KeyCode is Keys.LWin || e.KeyCode is Keys.RWin)
            {
                _keyModifiers.Add(KeyModifiers.Win);
            }

            if (!(e.KeyCode is Keys.ControlKey || e.KeyCode is Keys.Menu || e.KeyCode is Keys.ShiftKey))
            {
                _keys.Add(e.KeyCode);
            }

            var input = (Input)sender;
            var hotKeys = new HotKeys(input == LockHotKeyInput ? HotKeys.LockHotKeyId : HotKeys.OpenHotKeyId, _keyModifiers, _keys);

            input.Text = hotKeys.ToString();
            input.Tag = hotKeys;
            input.SelectionStart = input.Text.Length;
        }

        private void HotKeyInput_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _keyModifiers.Remove(KeyModifiers.Ctrl);

                    break;

                case Keys.Menu:
                    _keyModifiers.Remove(KeyModifiers.Alt);

                    break;

                case Keys.ShiftKey:
                    _keyModifiers.Remove(KeyModifiers.Shift);

                    break;

                case Keys.LWin:
                case Keys.RWin:
                    _keyModifiers.Remove(KeyModifiers.Win);

                    break;
            }

            if (!(e.KeyCode is Keys.ControlKey || e.KeyCode is Keys.Menu || e.KeyCode is Keys.ShiftKey))
            {
                _keys.Remove(e.KeyCode);
            }
        }

        private void HotKeyInput_TextChanged(object sender, EventArgs e)
        {
            var input = (Input)sender;

            if (0 == input.Text.Length)
            {
                _keyModifiers.Clear();
                _keys.Clear();
            }
        }

        /// <summary>
        /// RegisterStartup
        /// </summary>
        /// <param name="isRegister"></param>
        private static void RegisterStartup(bool isRegister)
        {
            using var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (null != registryKey)
            {
                var isRegistered = null != registryKey.GetValue(Application.ProductName);

                if (!isRegister)
                {
                    if (isRegistered)
                    {
                        registryKey.DeleteValue(Application.ProductName!);
                    }
                }
                else if (!isRegistered)
                {
                    registryKey.SetValue(Application.ProductName, Application.ExecutablePath);
                }
            }
        }

        /// <summary>
        /// IsStartup
        /// </summary>
        /// <returns></returns>
        private static bool IsStartup()
        {
            using var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (null != registryKey && null != registryKey.GetValue(Application.ProductName))
            {
                return true;
            }

            return false;
        }
    }
}