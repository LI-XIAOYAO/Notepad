using AntdUI;
using Notepad.Controls;
using Notepad.Entities;
using Notepad.Extensions;
using Notepad.Properties;
using System.Text;

namespace Notepad
{
    public partial class MainForm : Window, IEventListener
    {
        private static readonly string[] _srotTips = Resources.SortButton_Tips.Split(',');
        private readonly Tooltip.Config _sortTipConfig;
        private static readonly LocalConfig _localConfig = LocalConfig.Config;
        private CancellationTokenSource? _loadCts;
        private static List<Note>? _notes;
        private ControlsSelector _panelSelector;
        private Form? _settingForm;
        private Form? _panelSelectorFloatForm;
        private Size _defaultSize;
        private bool _isResize;
        private LockPanel? _lockPanel;
        private static NotifyIcon? _notifyIcon;
        private List<Color>? _colors;
        private readonly Dictionary<int, int> _hotKeyStates = [];
        private readonly HashSet<Form> _formStates = [];

        /// <summary>
        /// ThemeChanged
        /// </summary>
        private event Action? ThemeChanged;

        public MainForm()
        {
            InitializeComponent();

            _defaultSize = ClientSize;

            LoadStyle();

            LoadLang();

            this.AddListener();

            _panelSelector = LoadContentSelector();
            _sortTipConfig = new Tooltip.Config(SortButton, _srotTips[(int)SortButton.Tag!])
            {
                Radius = 3,
                ArrowSize = 5
            };
            AddButton.MouseLeave += (_, _) => AddButton.BackActive = Color.Transparent;
            VisibleChanged += MainForm_VisibleChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResizeBegin += (_, _) => _isResize = true;
            ResizeEnd += async (_, _) =>
            {
                _isResize = false;

                await SaveConfigsAsync();
            };

            Application.ThreadException += (_, e) =>
            {
                new Notification.Config(this, Resources.Notification_AppCrash, $"Error: {(e.Exception.InnerException ?? e.Exception).Message}", TType.Error, TAlignFrom.BR)
                {
                    Padding = new Size(15, 10),
                    Radius = 3,
                    ShowInWindow = true,
                    AutoClose = 0
                }.open();
            };

            LoadConfig(_localConfig);

            LoadBase();
        }

        /// <summary>
        /// LoadContentSelector
        /// </summary>
        /// <returns></returns>
        private ControlsSelector LoadContentSelector()
        {
            var panelSelectorFloatConfig = new FloatButton.Config(this, [
                new FloatButton.ConfigBtn("Select", Resources.FloatButton_ConfigBtn_Select_All),
                new FloatButton.ConfigBtn("Delete", Resources.Svg_Delete,true)
                {
                    Fore = Color.Red,
                    IconSize = new Size(20,20)
                }
            ], b =>
            {
                switch (b.Name)
                {
                    case "Delete":
                        {
                            this.Notification(Resources.FloatButton_ConfigBtn_Delete_Notification_Title, Resources.FloatButton_ConfigBtn_Delete_Notification_Content.Format(_panelSelector.SelectedCount), buttonType: TTypeMini.Error, buttonClickAction: async (_, b) =>
                            {
                                b.Loading = true;
                                b.Width += 15;

                                if (null != _notes)
                                {
                                    var notes = _notes.ToList();
                                    foreach (var item in _panelSelector.GetSelectedControls())
                                    {
                                        notes.Remove((Note)item.Tag!);
                                    }

                                    var cache = Store.Cache;
                                    var temp = cache.Clone();

                                    temp.Update(notes);

                                    Exception? exception = null;

                                    try
                                    {
                                        await Store.SaveNotesAsync(temp.Encrypt());

                                        cache.Update(notes);
                                        _notes = cache.SortNotes();
                                    }
                                    catch (Exception ex)
                                    {
                                        exception = ex;
                                    }

                                    this.Loading(null == exception ? Resources.Loading_DeleteSuccess : $"{Resources.Loading_DeleteFailed}{exception.Message}", 0, false, iconType: null == exception ? TType.Success : TType.Error).CloseAfter(1000);
                                }

                                b.Loading = false;

                                _panelSelectorFloatForm?.Close();

                                LoadNotes();
                            });
                        }

                        break;

                    case "Select":
                        if (Resources.FloatButton_ConfigBtn_Select_All == b.Text)
                        {
                            _panelSelector?.SelectAll();
                            b.Text = Resources.FloatButton_ConfigBtn_Select_Cancel;
                        }
                        else
                        {
                            _panelSelector?.SelectAll(false);
                            b.Text = Resources.FloatButton_ConfigBtn_Select_All;
                        }

                        break;
                }
            })
            {
                Vertical = false,
                Align = TAlign.Bottom,
                MarginY = 0,
                Gap = 60
            };

            void LoadFloatForm()
            {
                var button = panelSelectorFloatConfig.Btns[0];
                button.Text = _panelSelector!.SelectedCount == _panelSelector.Controls.Count ? Resources.FloatButton_ConfigBtn_Select_Cancel : Resources.FloatButton_ConfigBtn_Select_All;
                button.Badge = _panelSelector.SelectedCount.ToString();

                if (_panelSelector.IsSelected)
                {
                    _panelSelectorFloatForm ??= FloatButton.open(panelSelectorFloatConfig);
                }
                else
                {
                    _panelSelectorFloatForm?.Close();
                    _panelSelectorFloatForm = null;
                }
            }

            _panelSelector = new ControlsSelector(FlowPanel);
            _panelSelector.Changed += (control, isSelected) =>
            {
                ((AntdUI.Panel)control).BorderWidth = isSelected ? 2 : 0;

                LoadFloatForm();
            };
            _panelSelector.Click += control =>
            {
                var note = _notes!.First(c => c == (Note)control.Tag!);
                var name = $"{nameof(AddNotesWindow)}_{note.Id}";

                if (null == Application.OpenForms[name])
                {
                    new AddNotesWindow(this, Resources.Text_View)
                        .LoadNotes(note.Copy())
                        .Apply(c =>
                        {
                            c.Name = name;
                            c.Added += (_, _) =>
                            {
                                _notes = Store.Cache.SortNotes();

                                LoadNotes();
                            };
                        })
                        .Show(true);
                }
            };
            _panelSelector.Reseted += LoadFloatForm;

            return _panelSelector;
        }

        /// <summary>
        /// LoadBase
        /// </summary>
        private void LoadBase()
        {
            this.DelayInvoke(async () =>
            {
                _settingForm = FloatButton.open(new FloatButton.Config(this, [
                    new FloatButton.ConfigBtn("Setting", Resources.Svg_Setting, true) {
                        Fore = Style.Db.Primary,
                        IconSize = new Size(20,20)
                    }.Apply(c => ThemeChanged += () => c.Fore = Style.Db.Primary)
                ], b => new SettingWindow(this).Apply(c => c.Saved += () =>
                {
                    LoadLang();

                    RegisterHotKey(true);
                }).ShowDialog())
                {
                    MarginX = 0
                }
                );

                ChangeLiteViewSvg();

                Resize += async (_, _) =>
                {
                    if (!_isResize)
                    {
                        await SaveConfigsAsync();
                    }
                };

                Opacity = 1;

                var loading = this.Loading(Resources.Loading_LoadNotes);

                Notes? notes = null;

                try
                {
                    notes = await Store.GetNotesAsync();
                }
                catch (Exception ex)
                {
                    loading.Close();

                    AntdUI.Label? contentLabel = null;

                    this.Notification(Resources.Notification_LoadNotes_Title_Failed, ex.Message, Resources.Notification_LoadNotes_Reload, TTypeMini.Primary, async (m, b) =>
                    {
                        var width = b.Width;

                        try
                        {
                            b.Loading = true;
                            b.Width += 15;

                            notes = await Store.GetNotesAsync();

                            m.Close();
                        }
                        catch (Exception ex)
                        {
                            if (null == contentLabel)
                            {
                                contentLabel = (AntdUI.Label)m.ContentPanel!.Controls["Content"]!;
                            }

                            contentLabel.Text = ex.Message;
                        }

                        b.Loading = false;
                        b.Width = width;
                    }, false);
                }

                if (null == notes)
                {
                    Application.Exit();

                    return;
                }

                LoadConfig(notes);

                if (_localConfig.CanSync(notes))
                {
                    await _localConfig.SaveAsync();
                }

                loading.Close();
                AddButton.Enabled = true;

                if (null != notes.PIN)
                {
                    LockApp();

                    Application.AddMessageFilter(new LockMessageFilter(TimeSpan.FromMinutes(notes.PINTimeout), LockApp));
                }

                if (notes.IsEncrypt)
                {
                    if (_localConfig.Key.IsNullOrBlank())
                    {
                        if (!ShowDecrypt())
                        {
                            return;
                        }

                        await _localConfig.SaveAsync();
                    }
                    else
                    {
                        try
                        {
                            notes.Decrypt(_localConfig.Key!, true);
                        }
                        catch
                        {
                            if (!ShowDecrypt())
                            {
                                return;
                            }

                            await _localConfig.SaveAsync();
                        }
                    }
                }

                _notes = notes.SortNotes();

                LoadNotes();
            }, 100);
        }

        /// <summary>
        /// LoadNotes
        /// </summary>
        /// <returns></returns>
        private void LoadNotes()
        {
            _loadCts?.Cancel();
            FlowPanel.Controls.Clear();
            _panelSelector.ResetSelector();
            _loadCts = new CancellationTokenSource();
            var cancellationToken = _loadCts.Token;

            if (null == _notes || 0 == _notes.Count)
            {
                return;
            }

            var dpi = Config.Dpi;
            FlowPanel.PauseLayout = true;

            if ((bool)ShowButton.Tag!)
            {
                var panelSize = new Size((int)(300 * dpi), (int)(146 * dpi));

                foreach (var item in _notes)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var panel = new AntdUI.Panel
                    {
                        Shadow = 10,
                        ShadowOpacityAnimation = true,
                        ShadowOpacity = 0.15F,
                        Size = panelSize,
                        Radius = 3,
                        Padding = new Padding(1),
                        Cursor = Cursors.Hand,
                        BorderColor = Style.Db.Primary,
                        Tag = item,
                        TabStop = false
                    };

                    if (item.HasNotes)
                    {
                        for (int i = 0; i < item.Notes!.Count; i++)
                        {
                            if (i > 2)
                            {
                                break;
                            }

                            var content = item.Notes[i].Value;
                            var notesPanel = new AntdUI.Panel
                            {
                                Dock = DockStyle.Top,
                                Height = 29,
                                Radius = 0,
                                TabStop = false
                            };
                            var contentLabel = new AntdUI.Label
                            {
                                Dock = DockStyle.Fill,
                                Text = content?.ReplaceNewLine(" "),
                                AutoEllipsis = true,
                                TextMultiLine = false,
                                ShowTooltip = false,
                                Padding = new Padding(5, 0, 0, 0),
                                TabStop = false
                            };
                            var titleLabel = new AntdUI.Label
                            {
                                Dock = DockStyle.Left,
                                Text = item.Notes[i].Title,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Width = 50,
                                AutoEllipsis = true,
                                TextMultiLine = false,
                                ShowTooltip = false,
                                Padding = new Padding(3, 0, 0, 0),
                                TabStop = false
                            };
                            notesPanel.Controls.Add(contentLabel);
                            notesPanel.Controls.Add(new Divider
                            {
                                Dock = DockStyle.Left,
                                Vertical = true,
                                Thickness = 1F,
                                Width = 1,
                                Margin = new Padding(5),
                                TabStop = false
                            });
                            notesPanel.Controls.Add(titleLabel);

                            panel.Controls.Add(notesPanel);
                            notesPanel.BringToFront();

                            titleLabel.LinePopover();
                            contentLabel.LinePopover(content);

                            if (i < 2)
                            {
                                var divider = new Divider
                                {
                                    Dock = DockStyle.Top,
                                    Thickness = 1F,
                                    Height = 1,
                                    //Margin = new Padding(10, 0, 10, 0),
                                    TabStop = false
                                };
                                panel.Controls.Add(divider);
                                divider.BringToFront();
                            }
                        }
                    }

                    panel.Controls.Add(new Divider
                    {
                        Dock = DockStyle.Top,
                        Thickness = 1F,
                        Height = 1,
                        TabStop = false
                    });
                    var headerLabel = new AntdUI.Label
                    {
                        Dock = DockStyle.Top,
                        Text = item.Title,
                        Font = new Font(DefaultFont.FontFamily, 12, FontStyle.Bold),
                        Height = 31,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true,
                        TextMultiLine = false,
                        ShowTooltip = false,
                        TabStop = false
                    };
                    panel.Controls.Add(headerLabel);

                    headerLabel.LinePopover();

                    _panelSelector.AddControl(panel);
                    FlowPanel.Controls.Add(panel);
                    panel.BringToFront();
                }
            }
            else
            {
                var avatarColor = _colors![4];
                var panelSize = new Size((int)(300 * dpi), (int)(70 * dpi));

                foreach (var item in _notes)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var panel = new AntdUI.Panel
                    {
                        Shadow = 10,
                        ShadowOpacityAnimation = true,
                        ShadowOpacity = 0.15F,
                        Size = panelSize,
                        Radius = 3,
                        Padding = new Padding(1),
                        Cursor = Cursors.Hand,
                        BorderColor = Style.Db.Primary,
                        Tag = item,
                        TabStop = false
                    };

                    var titleLabel = new AntdUI.Label
                    {
                        Dock = DockStyle.Fill,
                        Text = item.Title,
                        Font = new Font(DefaultFont.FontFamily, 12, FontStyle.Bold),
                        AutoEllipsis = true,
                        TextMultiLine = false,
                        ShowTooltip = false,
                        TabStop = false
                    };
                    panel.Controls.Add(titleLabel);
                    panel.Controls.Add(new Divider
                    {
                        Dock = DockStyle.Left,
                        Vertical = true,
                        Thickness = 1F,
                        Width = 10,
                        Margin = new Padding((int)(panelSize.Height * 0.2)),
                        TabStop = false
                    });
                    panel.Controls.Add(new Avatar
                    {
                        Dock = DockStyle.Left,
                        Text = item.Title[0].ToString(),
                        Width = panel.DisplayRectangle.Height,
                        Font = new Font(DefaultFont.FontFamily, 12, FontStyle.Bold),
                        BackColor = avatarColor,
                        Radius = panel.Radius,
                        Padding = new Padding(5),
                        ForeColor = Style.Db.PrimaryColor,
                        TabStop = false
                    }.Apply(c => ThemeChanged += () => c.BackColor = _colors[4]));

                    titleLabel.LinePopover();

                    _panelSelector.AddControl(panel);
                    FlowPanel.Controls.Add(panel);
                    panel.BringToFront();
                }
            }

            FlowPanel.PauseLayout = false;
        }

        /// <summary>
        /// LoadConfig
        /// </summary>
        /// <param name="config"></param>
        private void LoadConfig(AppConfig config)
        {
            if (null != config)
            {
                if (config is LocalConfig localConfig && localConfig.X.HasValue)
                {
                    var point = new Point(localConfig.X.Value, localConfig.Y);

                    if (Screen.AllScreens.Any(c => c.WorkingArea.IntersectsWith(new Rectangle(point, Size))))
                    {
                        Location = point;
                    }

                    Size = new Size(localConfig.Width, localConfig.Height);

                    if (localConfig.IsMax)
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                }

                SortButton.Tag = config.Sort;
                ShowButton.Tag = config.IsCard;

                _sortTipConfig.Text = _srotTips[config.Sort];

                if (config.IsCard)
                {
                    ShowButton.IconSvg = Resources.Svg_ShowCard;
                }

                if (config.IsDark)
                {
                    Dark = Config.IsDark = true;
                }

                if (config.PrimaryColor.HasValue)
                {
                    Style.Db.SetPrimary(config.PrimaryColor.Value);

                    EventHub.Dispatch(EventType.THEME, config.PrimaryColor.Value);
                }

                if (config.Min)
                {
                    ShowNotifyIcon(false);
                }

                if (config is Notes)
                {
                    RegisterHotKey();
                }
            }
        }

        /// <summary>
        /// SaveNotes
        /// </summary>
        private void SaveNotes()
        {
            Invoke(async () =>
            {
                await Store.SaveNotesAsync(Store.Cache.Clone().Encrypt());

                await SaveConfigsAsync();
            });
        }

        /// <summary>
        /// SaveConfigsAsync
        /// </summary>
        /// <returns></returns>
        private async Task SaveConfigsAsync()
        {
            ChangeLiteViewSvg();

            _localConfig.IsMax = WindowState is FormWindowState.Maximized;

            if (WindowState is FormWindowState.Normal)
            {
                _localConfig.X = Location.X;
                _localConfig.Y = Location.Y;
                _localConfig.Width = Width;
                _localConfig.Height = Height;
            }

            await _localConfig.SaveAsync();
        }

        /// <summary>
        /// ChangeLiteViewSvg
        /// </summary>
        private void ChangeLiteViewSvg()
        {
            var isLite = MinimumSize.Width == Width && MinimumSize.Height == Height;

            LiteButton.IconSvg = isLite ? Resources.Svg_Restore : Resources.Svg_Lite;
            LiteButton.Tag = isLite;
            FlowPanel.Align = isLite ? TAlignFlow.Center : TAlignFlow.LeftCenter;

            if (null != _settingForm && true != _lockPanel?.Visible)
            {
                _settingForm.Visible = !isLite;
            }
        }

        /// <summary>
        /// LockApp
        /// </summary>
        private void LockApp()
        {
            if (true == _lockPanel?.Visible || null == Store.Cache.PIN)
            {
                return;
            }

            if (null == _lockPanel)
            {
                _lockPanel = new LockPanel((int)(MinimumSize.Width * 0.8))
                {
                    Location = new Point(0, WindowBar.ReadRectangle.Height),
                    Size = new Size(Width, Height - WindowBar.ReadRectangle.Height),
                };
                _lockPanel.VisibleChanged += (_, _) =>
                {
                    var visible = _lockPanel.Visible;

                    ChangeFormVisible(_settingForm, !visible && !(bool)LiteButton.Tag!);
                };

                Controls.Add(_lockPanel);
                _lockPanel.BringToFront();
            }

            if (Visible)
            {
                _lockPanel.ClearExclude();
                _lockPanel.AddExcludeForm(_settingForm);
                _lockPanel.AddExcludeFormType<LoadingWindow>();

                _lockPanel.Show();
            }
            else
            {
                VisibleChanged -= OnTryLockApp;
                VisibleChanged += OnTryLockApp;
            }
        }

        /// <summary>
        /// OnTryLockApp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTryLockApp(object? sender, EventArgs e)
        {
            if (Visible)
            {
                VisibleChanged -= OnTryLockApp;

                LockApp();
            }
        }

        /// <summary>
        /// ShowDecrypt
        /// </summary>
        /// <returns></returns>
        private bool ShowDecrypt()
        {
            var isValid = false;
            var decryptWindow = new InputWindow(this, 250, 80, Resources.Decrypt);
            decryptWindow.Input.UseSystemPasswordChar = true;
            decryptWindow.Valid += input =>
            {
                try
                {
                    Store.Cache.Decrypt(input.Text, true);

                    _localConfig.Key = input.Text;
                }
                catch (Exception)
                {
                    input.Text = string.Empty;

                    return Resources.Decrypt_KeyIncorrect;
                }

                isValid = true;
                decryptWindow.Close();

                return null;
            };
            decryptWindow.FormClosed += (_, _) =>
            {
                if (!isValid)
                {
                    Application.Exit();
                }
            };
            decryptWindow.ShowDialog();

            return isValid;
        }

        /// <summary>
        /// ChangeFormVisible
        /// </summary>
        /// <param name="form"></param>
        /// <param name="visible"></param>
        private void ChangeFormVisible(Form? form, bool visible)
        {
            if (null != form && !form.IsDisposed && !form.Disposing)
            {
                if (visible && !Visible)
                {
                    return;
                }

                form.Visible = visible;
            }
        }

        /// <summary>
        /// ShowNotifyIcon
        /// </summary>
        /// <param name="isChangeVisible"></param>
        public void ShowNotifyIcon(bool isChangeVisible = true)
        {
            if (isChangeVisible && Visible)
            {
                Visible = false;
            }

            if (null != _notifyIcon)
            {
                _notifyIcon.Visible = true;

                return;
            }

            _notifyIcon = new NotifyIcon
            {
                Icon = Icon,
                Text = Store.User.Account,
                Visible = true
            };
            _notifyIcon.MouseClick += (_, e) =>
            {
                if (e.Button is MouseButtons.Left)
                {
                    ShowMain(!Visible || WindowState is FormWindowState.Minimized);
                }
                else if (e.Button is MouseButtons.Right)
                {
                    AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(this, c =>
                    {
                        switch (c.Tag)
                        {
                            case 0:
                            case 1:
                                ShowMain();

                                break;

                            case 2:
                                LockApp();

                                break;

                            case 3:
                                Application.Exit();

                                break;
                        }
                    }, [
                        new ContextMenuStripItem(Store.User.Account) {
                            IconSvg = "UserOutlined",
                            Tag = 0
                        },
                        new ContextMenuStripItem(Resources.NotifyIcon_Open) {
                            IconSvg = Resources.Svg_Icon,
                            Tag = 1
                        },
                        new ContextMenuStripItem(Resources.NotifyIcon_Lock) {
                            IconSvg = "LockOutlined",
                            Tag = 2
                        },
                        new ContextMenuStripItem(Resources.NotifyIcon_Exit) {
                            IconSvg = Resources.Svg_Exit,
                            Tag = 3
                        },
                    ])
                    {
                        Align = TAlign.TL,
                        TopMost = true,
                        Radius = 3
                    })
                    .Apply(c =>
                    {
                        c!.Activate();
                    });
                }
            };
        }

        /// <summary>
        /// CloseNotifyIcon
        /// </summary>
        public static void CloseNotifyIcon()
        {
            if (null != _notifyIcon)
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        /// <summary>
        /// ShowMain
        /// </summary>
        /// <param name="visible"></param>
        private void ShowMain(bool visible = true)
        {
            //_notifyIcon!.Visible = false;

            if (Visible = visible)
            {
                if (WindowState is FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }

                Activate();

                if (true == _lockPanel?.Visible)
                {
                    _lockPanel.Input.Focus();
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchButton.Loading = true;
            SearchInput.Enabled = false;

            Invoke(() =>
            {
                _notes = Store.Cache.SortNotes().Where(c => c.Title.Contains(SearchInput.Text)).ToList();

                LoadNotes();

                SearchButton.Loading = false;
                SearchInput.Enabled = true;
            });
        }

        private void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Enter)
            {
                SearchButton_Click(sender, e);

                SearchInput.Focus();
            }
        }

        private void SortButton_MouseEnter(object sender, EventArgs e)
        {
            Tooltip.open(_sortTipConfig);
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            var next = (int)SortButton.Tag! + 1;
            SortButton.Tag = _localConfig.Sort = next = next >= _srotTips.Length ? 0 : next;

            _sortTipConfig.Text = _srotTips[next];

            if (Store.Cache.UpdateConfig(sort: next))
            {
                _notes = Store.Cache.SortNotes();

                LoadNotes();
                SaveNotes();
            }

            Tooltip.open(_sortTipConfig);
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            if ((bool)ShowButton.Tag!)
            {
                ShowButton.Tag = _localConfig.IsCard = false;
                ShowButton.IconSvg = Resources.Svg_ShowList;
            }
            else
            {
                ShowButton.Tag = _localConfig.IsCard = true;
                ShowButton.IconSvg = Resources.Svg_ShowCard;
            }

            if (Store.Cache.UpdateConfig(isCard: _localConfig.IsCard))
            {
                LoadNotes();
                SaveNotes();
            }
        }

        private async void LiteButton_Click(object sender, EventArgs e)
        {
            if (!(bool)LiteButton.Tag!)
            {
                if (WindowState is FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }

                Size = new Size(MinimumSize.Width, MinimumSize.Height);
            }
            else
            {
                Size = new Size(_defaultSize.Width, _defaultSize.Height);
            }

            await SaveConfigsAsync();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (null == Application.OpenForms[nameof(AddNotesWindow)])
            {
                AddButton.Enabled = false;

                new AddNotesWindow(this, Resources.Text_New)
                    .Apply(c =>
                    {
                        c.Name = nameof(AddNotesWindow);
                        c.FormClosed += (_, _) => AddButton.Enabled = true;
                        c.Added += (_, _) =>
                        {
                            _notes = Store.Cache.SortNotes();

                            LoadNotes();
                        };
                    })
                    .Show(true);
            }
        }

        private void MainForm_VisibleChanged(object? sender, EventArgs e)
        {
            var visible = Visible;

            ChangeFormVisible(_settingForm, visible && !(bool)LiteButton.Tag!);

            if (visible)
            {
                foreach (var form in _formStates)
                {
                    if (!form.Visible && !form.IsDisposed)
                    {
                        form.Visible = true;
                    }
                }

                _formStates.Clear();
            }
            else
            {
                foreach (var form in OwnedForms)
                {
                    if (form.Visible && !form.IsDisposed && form != _settingForm)
                    {
                        if (form.Modal)
                        {
                            form.Dispose();
                        }
                        else
                        {
                            form.Visible = false;

                            _formStates.Add(form);
                        }
                    }
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Store.Cache.LockHotKeys.UnregisterHotKey(Handle);
            Store.Cache.OpenkHotKeys.UnregisterHotKey(Handle);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x10:
                    if (_localConfig.Min)
                    {
                        ShowNotifyIcon();

                        return;
                    }

                    break;

                case 0x0312:
                    if (HotKeysHandler(Store.Cache.LockHotKeys, m, () =>
                    {
                        if (null != ActiveForm)
                        {
                            LockApp();
                        }
                    }))
                    {
                        return;
                    }

                    if (HotKeysHandler(Store.Cache.OpenkHotKeys, m, () =>
                    {
                        ShowMain(!Visible || null == ActiveForm);
                    }))
                    {
                        return;
                    }

                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// RegisterHotKey
        /// </summary>
        /// <param name="isUnRegister"></param>
        private void RegisterHotKey(bool isUnRegister = false)
        {
            var notes = Store.Cache;

            if (isUnRegister)
            {
                notes.LockHotKeys?.UnregisterHotKey(Handle);
                notes.OpenkHotKeys?.UnregisterHotKey(Handle);
            }

            var errorMsgSb = new StringBuilder();
            if (null != notes.LockHotKeys && !notes.LockHotKeys.RegisterHotKey(Handle))
            {
                errorMsgSb.AppendLine(Resources.Loading_RegisterHotKeyFailed.Format(Resources.Setting_LockHotKey));
            }

            if (null != notes.OpenkHotKeys && !notes.OpenkHotKeys.RegisterHotKey(Handle))
            {
                errorMsgSb.AppendLine(Resources.Loading_RegisterHotKeyFailed.Format(Resources.Setting_OpenHotKey));
            }

            if (errorMsgSb.Length > 0)
            {
                this.Loading(errorMsgSb.ToString(), 1500, false);
            }
        }

        /// <summary>
        /// HotKeysHandler
        /// </summary>
        /// <param name="hotKeys"></param>
        /// <param name="m"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool HotKeysHandler(HotKeys? hotKeys, System.Windows.Forms.Message m, Action action)
        {
            if (null != hotKeys)
            {
                if (hotKeys.Keys.Count > 1)
                {
                    for (int i = 0; i < hotKeys.Keys.Count; i++)
                    {
                        if (hotKeys.Id + i == m.WParam)
                        {
                            var key = (int)Math.Pow(2, i);

                            if (!_hotKeyStates.ContainsKey(hotKeys.Id))
                            {
                                _hotKeyStates[hotKeys.Id] = key;
                            }
                            else
                            {
                                _hotKeyStates[hotKeys.Id] |= key;
                            }

                            if (hotKeys.KeyValue == _hotKeyStates[hotKeys.Id])
                            {
                                action();

                                _hotKeyStates.Remove(hotKeys.Id);

                                return true;
                            }
                        }
                    }
                }
                else if (hotKeys.Id == m.WParam)
                {
                    action();
                }
            }

            return false;
        }

        /// <summary>
        /// LoadStyle
        /// </summary>
        private void LoadStyle()
        {
            BackColor = Style.Db.BgBase;
            ForeColor = Style.Db.TextBase;
            SearchInput.BorderActive = Style.Db.BorderColor;
            SearchInput.BorderHover = Style.Db.BorderColor;
            ShowButton.ForeColor = Style.Db.Primary;
            SortButton.ForeColor = Style.Db.Primary;
            AddButton.ForeColor = Style.Db.Primary;

            _colors = Style.GenerateColors(Style.Db.Primary);

            ThemeChanged?.Invoke();

            Refresh();
        }

        /// <summary>
        /// LoadLang
        /// </summary>
        private void LoadLang()
        {
            SearchInput.PlaceholderText = Resources.Text_Search;
            TooltipComponent.SetTip(ShowButton, Resources.Text_ShowStyle);
            TooltipComponent.SetTip(AddButton, Resources.Text_New);
        }

        public void HandleEvent(EventType id, object? tag)
        {
            if (id is EventType.THEME)
            {
                LoadStyle();
            }
        }
    }
}