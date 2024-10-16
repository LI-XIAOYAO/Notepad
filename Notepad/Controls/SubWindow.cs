using AntdUI;
using Notepad.Entities;
using Notepad.Enums;
using Vanara.PInvoke;

namespace Notepad.Controls
{
    /// <summary>
    /// SubWindow
    /// </summary>
    internal class SubWindow : BorderlessForm, IEventListener
    {
        private readonly bool _isFormParent;
        private Size _defaultSize;

        /// <summary>
        /// ParentControl
        /// </summary>
        protected Control ParentControl { get; }

        /// <summary>
        /// ControlParentForm
        /// </summary>
        protected Form? ControlParentForm { get; }

        /// <summary>
        /// AutoWidth
        /// </summary>
        public bool AutoWidth { get; set; }

        /// <summary>
        /// SubWindowShowOptions
        /// </summary>
        public SubWindowShowOptions SubWindowShowOptions { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                return cp;
            }
        }

        /// <summary>
        /// SubWindow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="autoWidth"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SubWindow(Control parent, int width, int height, bool autoWidth = true)
        {
            ParentControl = parent ?? throw new ArgumentNullException(nameof(parent));
            AutoWidth = autoWidth;
            UseDwm = false;

            SuspendLayout();

            Size = _defaultSize = new Size(width, height);
            BackColor = Style.Db.BgBase;
            Shadow = 10;
            ShadowColor = ShadowColor.rgba(0.25F);
            Radius = 3;
            BorderWidth = 0;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            _isFormParent = parent is Form;

            parent.Move += CenterParent;
            parent.Resize += CenterParent;

            ControlParentForm = parent.FindForm();

            if (!_isFormParent && null != ControlParentForm)
            {
                ControlParentForm.Move += CenterParent;
                ControlParentForm.Resize += CenterParent;
            }

            CenterParent(this, EventArgs.Empty);

            this.AddListener();
        }

        /// <summary>
        /// SetDefaultSize
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetDefaultSize(int? width = null, int? height = null)
        {
            Size = _defaultSize = new Size(width ?? Width, height ?? Height);
        }

        /// <summary>
        /// CenterParent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CenterParent(object? sender, EventArgs e)
        {
            if (IsDisposed || Disposing)
            {
                return;
            }

            var width = Width;
            var height = Height;

            if (e is AdjustSizeEventArgs eventArgs)
            {
                width = eventArgs.Width;
                height = eventArgs.Height;
            }

            if (AutoWidth && null != ControlParentForm)
            {
                var currentMinWidth = (int)(ControlParentForm.Width * 0.85);
                currentMinWidth = currentMinWidth > _defaultSize.Width ? _defaultSize.Width : currentMinWidth;

                if (currentMinWidth != width)
                {
                    Width = currentMinWidth;
                }
            }

            if (height != Height)
            {
                Height = height;
            }

            var p = Point.Empty;
            var s = Size;
            var screenRect = Screen.GetWorkingArea(this);
            User32.GetWindowRect(ParentControl.Handle, out var rect);
            Rectangle ownerRect = rect;

            switch (SubWindowShowOptions)
            {
                case SubWindowShowOptions.Parent:
                    screenRect = ownerRect;

                    break;

                case SubWindowShowOptions.ParentForm:
                    if (null != ControlParentForm)
                    {
                        User32.GetWindowRect(ControlParentForm.Handle, out rect);
                        screenRect = rect;
                    }

                    break;

                case SubWindowShowOptions.WorkingArea:
                    RECT parentFormRect;
                    var currentRect = ownerRect;

                    if (null != ControlParentForm)
                    {
                        User32.GetWindowRect(ControlParentForm.Handle, out parentFormRect);
                        currentRect = parentFormRect;
                    }

                    if (!screenRect.IntersectsWith(currentRect))
                    {
                        Location = null != ControlParentForm ? ControlParentForm.Location : new Point(-32000);

                        return;
                    }

                    break;
            }

            p.X = (ownerRect.Left + ownerRect.Right - s.Width) / 2;
            p.Y = (ownerRect.Top + ownerRect.Bottom - s.Height) / 2;

            if (p.X < screenRect.X)
            {
                p.X = screenRect.X;
            }
            else if (p.X + s.Width > screenRect.X + screenRect.Width)
            {
                p.X = screenRect.X + screenRect.Width - s.Width;
            }

            if (p.Y < screenRect.Y)
            {
                p.Y = screenRect.Y;
            }
            else if (p.Y + s.Height > screenRect.Y + screenRect.Height)
            {
                p.Y = screenRect.Y + screenRect.Height - s.Height;
            }

            Location = p;
        }

        /// <summary>
        /// <inheritdoc cref="Control.Show()"/>
        /// </summary>
        public virtual new void Show() => Show(ParentControl);

        /// <summary>
        /// <inheritdoc cref="Form.ShowDialog()"/>
        /// </summary>
        /// <returns></returns>
        public virtual new DialogResult ShowDialog() => ShowDialog(ParentControl);

        protected override void OnClosed(EventArgs e)
        {
            ParentControl.Move -= CenterParent;
            ParentControl.Resize -= CenterParent;

            if (!_isFormParent && null != ControlParentForm)
            {
                ControlParentForm.Move -= CenterParent;
                ControlParentForm.Resize -= CenterParent;
            }

            base.OnClosed(e);

            if (null != ControlParentForm)
            {
                if (Modal)
                {
                    ControlParentForm.Activate();
                }
                else
                {
                    ControlParentForm.Focus();
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            ResumeLayout();

            if (null != ControlParentForm && TopMost != ControlParentForm.TopMost)
            {
                TopMost = ControlParentForm.TopMost;
            }

            base.OnShown(e);
        }

        public virtual void HandleEvent(EventType id, object? tag)
        {
            if (id is EventType.THEME)
            {
                BackColor = Style.Db.BgBase;
                ForeColor = Style.Db.TextBase;

                Refresh();
            }
        }
    }
}