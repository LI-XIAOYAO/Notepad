using AntdUI;
using Notepad.Entities;
using Notepad.Enums;
using Notepad.Properties;

namespace Notepad.Controls
{
    /// <summary>
    /// DialogWindow
    /// </summary>
    internal class DialogWindow : SubWindow
    {
        private readonly DialogWindowDividerOptions _dividerOptions;
        private static readonly Dictionary<Control, DialogWindow> _parentControls = [];
        private DialogWindow? _lastDialogWindow;
        private int _width;
        private int _height;

        /// <summary>
        /// CloseClick
        /// </summary>
        public event MouseEventHandler? CloseClick;

        /// <summary>
        /// HeaderPanel
        /// </summary>
        public AntdUI.Panel HeaderPanel { get; }

        /// <summary>
        /// ContentPanel
        /// </summary>
        public AntdUI.Panel? ContentPanel { get; }

        public AntdUI.Panel? ButtonPanel { get; private set; }

        /// <summary>
        /// <inheritdoc cref="BorderlessForm.Radius"/>
        /// </summary>
        public new int Radius
        {
            get => base.Radius;
            set
            {
                base.Radius = value;
                HeaderPanel.Radius = value;

                if (null != ButtonPanel)
                {
                    ButtonPanel.Radius = value;
                }
            }
        }

        /// <summary>
        /// IsAdjustSize
        /// </summary>
        public bool IsAdjustSize { get; private set; }

        /// <summary>
        /// DialogWindow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="header"></param>
        /// <param name="content"></param>
        /// <param name="dividerOptions"></param>
        public DialogWindow(Control parent, int width, int height, object? header = null, object? content = null, DialogWindowDividerOptions dividerOptions = DialogWindowDividerOptions.TopBottom)
            : base(parent, width, height)
        {
            _dividerOptions = dividerOptions;
            SubWindowShowOptions = SubWindowShowOptions.WorkingArea;

            var minWidth = (int)(ControlParentForm?.MinimumSize.Width * 0.85 ?? 325);
            if (minWidth > width)
            {
                minWidth = width;
            }

            MinimumSize = new Size(minWidth, height);

            if (null != content)
            {
                if (content is Control control)
                {
                    control.Dock = DockStyle.Fill;
                    Controls.Add(control);
                }
                else
                {
                    Controls.Add(new AntdUI.Label
                    {
                        Text = content.ToString(),
                        Padding = new Padding(10),
                        TextAlign = ContentAlignment.TopLeft,
                        AutoEllipsis = true,
                        Dock = DockStyle.Fill,
                        TabStop = false
                    });
                }
            }
            else
            {
                Controls.Add(ContentPanel = new AntdUI.Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10, 10, 10, 1),
                    TabStop = false
                });
            }

            if ((dividerOptions & DialogWindowDividerOptions.Top) > 0)
            {
                Controls.Add(new Divider
                {
                    Name = "TopDivider",
                    Dock = DockStyle.Top,
                    Thickness = 1F,
                    Height = 1,
                    Margin = new Padding(10, 0, 10, 0),
                    TabStop = false,
                });
            }

            HeaderPanel = new AntdUI.Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                Radius = Radius,
                TabStop = false
            };

            HeaderPanel.Controls.Add(new AntdUI.Button
            {
                IconSvg = Resources.Svg_DialogClose,
                Dock = DockStyle.Right,
                Size = new Size(HeaderPanel.Height, HeaderPanel.Height),
                Radius = 0,
                WaveSize = 0,
                Ghost = true,
                TabStop = false
            }.Apply(b => b.MouseClick += (s, e) =>
            {
                Close();

                CloseClick?.Invoke(s, e);
            }));

            if (null != header)
            {
                if (header is Control headerControl)
                {
                    var panel = new AntdUI.Panel
                    {
                        Dock = DockStyle.Fill,
                        Height = HeaderPanel.Height,
                        Radius = Radius,
                        TabStop = false
                    };
                    panel.Controls.Add(headerControl);
                    panel.MouseDown += (_, _) => DraggableMouseDown();
                    headerControl.MouseDown += (_, _) => DraggableMouseDown();
                    HeaderPanel.Controls.Add(panel);
                }
                else
                {
                    HeaderPanel.Controls.Add(new AntdUI.Label
                    {
                        Name = "Title",
                        Text = header?.ToString(),
                        Height = HeaderPanel.Height,
                        Font = new Font(parent.Font.FontFamily, 11),
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true,
                        Dock = DockStyle.Fill,
                        TabStop = false
                    }.Apply(l => l.MouseDown += (_, _) => DraggableMouseDown()));
                }
            }

            Controls.Add(HeaderPanel);
        }

        /// <summary>
        /// AddAdjustSizeButton
        /// </summary>
        /// <param name="isAdjustSize"></param>
        /// <returns></returns>
        public DialogWindow AddAdjustSizeButton(bool isAdjustSize = false)
        {
            var adjustSizeButton = new AntdUI.Button
            {
                IconSvg = Resources.Svg_Max,
                Dock = DockStyle.Right,
                Size = new Size(HeaderPanel.Height, HeaderPanel.Height),
                Radius = 0,
                WaveSize = 0,
                Ghost = true,
                Tag = false,
                TabStop = false
            };
            adjustSizeButton.MouseClick += AdjustSizeButton_MouseDoubleClick;
            HeaderPanel.Controls.Add(adjustSizeButton);
            adjustSizeButton.BringToFront();

            if (isAdjustSize)
            {
                AdjustSizeButton_MouseDoubleClick(adjustSizeButton, null);
            }

            return this;
        }

        /// <summary>
        /// AddButton
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public DialogWindow AddButton(AntdUI.Button button)
        {
            InitButtonPanel();

            if (0 == button.Width)
            {
                button.Width = TextRenderer.MeasureText(button.Text, button.Font).Width + 30;
            }

            if (0 == button.Height || button.Height > ButtonPanel!.Height)
            {
                button.Height = ButtonPanel!.Height;
            }

            ButtonPanel!.Controls.Add(button);

            return this;
        }

        /// <summary>
        /// AddButton
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DialogWindow AddButton(string text, TTypeMini type = TTypeMini.Default, Action<DialogWindow, AntdUI.Button>? action = null)
        {
            var button = new AntdUI.Button
            {
                Text = text,
                Type = type,
                BorderWidth = type is TTypeMini.Default ? 1.5F : 0,
                Dock = DockStyle.Right,
                Radius = 3,
                WaveSize = 0,
                Padding = new Padding(0, 3, 10, 3),
                TabStop = false
            };

            action?.Invoke(this, button);

            return AddButton(button);
        }

        /// <summary>
        /// <inheritdoc cref="SubWindow.Show"/>
        /// </summary>
        /// <param name="autoHide"></param>
        public void Show(bool autoHide)
        {
            HideLast(autoHide);

            base.Show();
        }

        /// <summary>
        /// <inheritdoc cref="Form.Show(IWin32Window?)"/>
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="autoHide"></param>
        public void Show(IWin32Window? owner, bool autoHide)
        {
            HideLast(autoHide);

            Show(owner);
        }

        /// <summary>
        /// <inheritdoc cref="SubWindow.ShowDialog"/>
        /// </summary>
        /// <param name="autoHide"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(bool autoHide)
        {
            HideLast(autoHide);

            return base.ShowDialog();
        }

        /// <summary>
        /// <inheritdoc cref="Form.ShowDialog(IWin32Window?)"/>
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="autoHide"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(IWin32Window? owner, bool autoHide)
        {
            HideLast(autoHide);

            return ShowDialog(owner);
        }

        /// <summary>
        /// InitButtonPanel
        /// </summary>
        private void InitButtonPanel()
        {
            if (null == ButtonPanel)
            {
                if ((_dividerOptions & DialogWindowDividerOptions.Bottom) > 0)
                {
                    Controls.Add(new Divider
                    {
                        Name = "BottomDivider",
                        Dock = DockStyle.Bottom,
                        Thickness = 1F,
                        Height = 1,
                        Margin = new Padding(10, 0, 10, 0),
                        TabStop = false
                    });
                }

                Controls.Add(ButtonPanel = new AntdUI.Panel
                {
                    Dock = DockStyle.Bottom,
                    Size = new Size(Width, 35),
                    Padding = new Padding(1, 0, 1, 1),
                    Radius = Radius,
                    TabStop = false
                });
            }
        }

        /// <summary>
        /// HideLast
        /// </summary>
        /// <param name="autoHide"></param>
        private void HideLast(bool autoHide)
        {
            if (autoHide)
            {
                if (_parentControls.TryGetValue(ParentControl, out var dialogWindow))
                {
                    _lastDialogWindow = dialogWindow;

                    _lastDialogWindow.DelayInvoke(() =>
                    {
                        if (null != _lastDialogWindow && !_lastDialogWindow.IsDisposed && !_lastDialogWindow.Disposing)
                        {
                            _lastDialogWindow.Visible = false;
                        }
                    });
                }

                _parentControls[ParentControl] = this;
            }
        }

        /// <summary>
        /// AdjustSizeButton_MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustSizeButton_MouseDoubleClick(object? sender, MouseEventArgs? e)
        {
            var adjSizeButton = (AntdUI.Button)sender!;
            var eventArgs = new AdjustSizeEventArgs();

            if (!IsAdjustSize)
            {
                _width = Width;
                _height = Height;
                adjSizeButton.IconSvg = Resources.Svg_Restore;
                adjSizeButton.IconSize = new Size(20, 20);

                eventArgs.Width = (int)(ParentControl.Width * 0.85);
                eventArgs.Height = (int)(ParentControl.Height * 0.8);
            }
            else
            {
                adjSizeButton.IconSvg = Resources.Svg_Max;
                adjSizeButton.IconSize = new Size();

                eventArgs.Width = _width;
                eventArgs.Height = _height;
            }

            IsAdjustSize = !IsAdjustSize;

            CenterParent(sender, eventArgs);
        }

        /// <summary>
        /// <inheritdoc cref="Form.Close"/>
        /// </summary>
        public new void Close()
        {
            if (null != _lastDialogWindow)
            {
                if (!_lastDialogWindow.IsDisposed)
                {
                    _lastDialogWindow.Visible = true;
                }

                _parentControls[ParentControl] = _lastDialogWindow;
                _lastDialogWindow = null;

                this.DelayInvoke(base.Close);
            }
            else
            {
                _parentControls.Remove(ParentControl);

                base.Close();
            }
        }
    }
}