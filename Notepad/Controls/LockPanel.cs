using AntdUI;
using Notepad.Properties;

namespace Notepad.Controls
{
    /// <summary>
    /// LockPanel
    /// </summary>
    internal class LockPanel : AntdUI.Panel, IEventListener
    {
        private readonly HashSet<Form> _lockFormStates = [];
        private readonly HashSet<Form> _lockExcludeFormStates = [];
        private readonly HashSet<Type> _lockExcludeFormTypeStates = [];
        private readonly AntdUI.Label _PINLabel;
        private Form? _parent;

        /// <summary>
        /// Input
        /// </summary>
        public Input Input { get; }

        /// <summary>
        /// LockPanel
        /// </summary>
        /// <param name="contentWidth"></param>
        public LockPanel(int contentWidth)
        {
            Radius = 0;
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            Visible = false;
            TabStop = false;

            var contentPanel = new AntdUI.Panel
            {
                Radius = 0,
                Width = contentWidth,
                Height = 80,
                Location = new Point((Width - contentWidth) / 2, (int)((Height - 80) * 0.4)),
                TabStop = false
            };

            Input = new Input
            {
                Dock = DockStyle.Fill,
                MaxLength = 4,
                WaveSize = 0,
                Radius = 3,
                TextAlign = HorizontalAlignment.Center,
                BorderWidth = 0,
                ImeMode = ImeMode.Disable,
                CaretSpeed = 700,
                UseSystemPasswordChar = true
            };
            var divider = new Divider
            {
                Dock = DockStyle.Bottom,
                Thickness = 1F,
                Height = 1,
                Margin = new Padding(10, 0, 10, 0),
                TabStop = false
            };
            var tipsLabel = new AntdUI.Label
            {
                Dock = DockStyle.Bottom,
                Font = new Font(Font.FontFamily, 8),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 20,
                TabStop = false,
                AutoEllipsis = true,
                Padding = new Padding(0, 5, 0, 0)
            };

            System.Windows.Forms.Timer? timer = null;
            var notes = Store.Cache;

            void ErrorCountDown(long countdown)
            {
                Input.ReadOnly = true;
                tipsLabel.Text = Resources.Lock_CountDown.Format(countdown--);

                timer = new System.Windows.Forms.Timer
                {
                    Interval = 1000
                };
                timer.Tick += (_, _) =>
                {
                    Invoke(() =>
                    {
                        if (countdown > 0)
                        {
                            tipsLabel.Text = Resources.Lock_CountDown.Format(countdown--);
                        }
                        else
                        {
                            Input.ReadOnly = false;
                            tipsLabel.Text = string.Empty;
                            timer.Dispose();
                            timer = null;
                        }
                    });
                };
                timer.Start();
            }

            async Task UnlockApp(object? sender, EventArgs e)
            {
                if (null == Input.Text || 4 != Input.Text.Length)
                {
                    divider.ColorSplit = Color.Red;

                    return;
                }

                var lastErrorRetryTimes = notes!.ErrorRetryTimes;

                if (notes.PIN != Input.Text)
                {
                    divider.ColorSplit = Color.Red;
                    Input.Clear();

                    if (notes.ErrorRetryTimes < 29)
                    {
                        notes.ErrorRetryTimes++;
                    }

                    if (notes.ErrorRetryTimes > 2)
                    {
                        var nextCanInputSeconds = (long)TimeSpan.FromMinutes(Math.Pow(2, notes!.ErrorRetryTimes - 3)).TotalSeconds;
                        notes.UnLockTime = DateTimeOffset.Now.AddSeconds(nextCanInputSeconds).ToUnixTimeMilliseconds();

                        ErrorCountDown(nextCanInputSeconds);
                    }
                }
                else
                {
                    Visible = false;
                    notes.ErrorRetryTimes = 0;
                    notes.UnLockTime = 0;
                    Input.Clear();
                }

                if (lastErrorRetryTimes != notes.ErrorRetryTimes)
                {
                    int retryTimes = 3;

                    while (retryTimes-- > 0)
                    {
                        try
                        {
                            await Store.SaveNotesAsync(notes.Clone().Encrypt());

                            retryTimes = 0;
                        }
                        catch
                        {
                            await Task.Delay(1000);
                        }
                    }
                }
            }

            Input.KeyDown += async (_, e) =>
            {
                if (e.KeyCode is Keys.Enter)
                {
                    await UnlockApp(null, e);

                    e.SuppressKeyPress = true;
                }
                else if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
                {
                    e.SuppressKeyPress = true;
                }
            };
            Input.KeyPress += async (_, e) =>
            {
                if (4 == Input.Text.Length)
                {
                    await UnlockApp(null, e);

                    e.Handled = true;
                }
            };
            Input.GotFocus += (_, _) => divider.ColorSplit = Style.Db.Split;
            Input.TextChanged += (_, _) =>
            {
                if (Input.Text.Length > 0)
                {
                    divider.ColorSplit = Style.Db.Split;
                }
            };

            contentPanel.Controls.Add(Input);
            contentPanel.Controls.Add(_PINLabel = new AntdUI.Label
            {
                Dock = DockStyle.Top,
                Text = "PIN",
                Font = new Font(Font.FontFamily, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 25,
                ForeColor = Style.Db.Primary,
                TabStop = false
            });
            contentPanel.Controls.Add(divider);
            contentPanel.Controls.Add(tipsLabel);

            SizeChanged += (_, _) => contentPanel.Location = new Point((Width - contentWidth) / 2, (int)((Height - contentPanel.Height) * 0.4));
            VisibleChanged += (_, _) =>
            {
                if (Visible && true == Parent?.Focused)
                {
                    Input.Focus();
                }
            };
            MouseClick += (_, _) => Input.Focus();

            Controls.Add(contentPanel);

            var unLockTime = notes!.UnLockTime - DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (unLockTime > 0)
            {
                ErrorCountDown(unLockTime / 1000);
            }
        }

        /// <summary>
        /// AddExcludeForm
        /// </summary>
        /// <param name="form"></param>
        public void AddExcludeForm(Form? form)
        {
            if (null != form)
            {
                _lockExcludeFormStates.Add(form);
            }
        }

        /// <summary>
        /// AddExcludeFormType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddExcludeFormType<T>()
            where T : Form
        {
            _lockExcludeFormTypeStates.Add(typeof(T));
        }

        /// <summary>
        /// ClearExclude
        /// </summary>
        public void ClearExclude()
        {
            _lockExcludeFormStates.Clear();
            _lockExcludeFormTypeStates.Clear();
        }

        public new void HandleEvent(EventType id, object? tag)
        {
            base.HandleEvent(id, tag);

            if (id == EventType.THEME)
            {
                _PINLabel.ForeColor = Style.Db.Primary;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            _parent ??= FindForm();

            if (null != _parent)
            {
                if (Visible)
                {
                    foreach (var form in _parent.OwnedForms)
                    {
                        if (form.Visible && !form.IsDisposed && !_lockExcludeFormStates.Contains(form) && !_lockExcludeFormTypeStates.Contains(form.GetType()))
                        {
                            if (form.Modal)
                            {
                                form.Dispose();
                            }
                            else
                            {
                                form.Visible = false;

                                _lockFormStates.Add(form);
                            }
                        }
                    }
                }
                else if (_parent.Visible)
                {
                    foreach (var form in _lockFormStates)
                    {
                        if (!form.Visible && !form.IsDisposed)
                        {
                            form.Visible = true;
                        }
                    }

                    _lockFormStates.Clear();
                    ClearExclude();
                }
            }
        }
    }
}