using AntdUI;
using Notepad.Properties;

namespace Notepad.Controls
{
    /// <summary>
    /// LoadingWindow
    /// </summary>
    internal class LoadingWindow : SubWindow
    {
        private readonly int _autoCloseMills;

        /// <summary>
        /// Button
        /// </summary>
        public AntdUI.Button Button { get; }

        /// <summary>
        /// LoadingWindow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="autoCloseMills"></param>
        public LoadingWindow(Control parent, int autoCloseMills = 500)
            : base(parent, 140, 40)
        {
            Resizable = false;
            AutoWidth = false;
            Radius = 6;

            Controls.Add(Button = new AntdUI.Button
            {
                Dock = DockStyle.Fill,
                Loading = true,
                Text = Resources.Loading_Loading,
                BackActive = Color.Transparent,
                BackHover = Color.Transparent,
                WaveSize = 0,
                TabStop = false,
                Radius = Radius
            });

            _autoCloseMills = autoCloseMills;
        }

        /// <summary>
        /// SetMeasureWidth
        /// </summary>
        /// <param name="isShowLoading"></param>
        public void SetMeasureWidth(bool isShowLoading = true)
        {
            if (isShowLoading != Button.Loading)
            {
                Button.Loading = isShowLoading;
            }

            var maxWidth = (int)(ParentControl.Width * 0.85);
            if (maxWidth < Width)
            {
                maxWidth = Width;
            }

            var width = TextRenderer.MeasureText(Button.Text, Button.Font).Width + 10 + (isShowLoading ? 25 : 0) + (Controls.Count - 1) * 25;

            if (width < 100)
            {
                width = 100;
            }

            if (width > maxWidth && !Button.TextMultiLine)
            {
                Button.TextMultiLine = true;
            }

            SetDefaultSize(width > maxWidth ? maxWidth : width);
            CenterParent(null, EventArgs.Empty);
        }

        /// <summary>
        /// SetText
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isAutoWidth"></param>
        /// <param name="isShowLoading"></param>
        public void SetText(string text, bool isAutoWidth = true, bool isShowLoading = true)
        {
            Button.Text = text;

            if (isAutoWidth)
            {
                SetMeasureWidth(isShowLoading);
            }
            else if (isShowLoading != Button.Loading)
            {
                Button.Loading = isShowLoading;
            }
        }

        /// <summary>
        /// CloseAfter
        /// </summary>
        /// <param name="millsInterval"></param>
        /// <param name="isAutoCalc"></param>
        public void CloseAfter(int millsInterval, bool isAutoCalc = true)
        {
            if (isAutoCalc)
            {
                var interval = (int)(TextRenderer.MeasureText(Button.Text, Button.Font).Width / 180M * 1000);
                millsInterval = interval < millsInterval ? millsInterval : interval;
            }

            if (millsInterval > 0)
            {
                var cts = new CancellationTokenSource();
                Disposed += (_, _) => cts.Cancel();

                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(millsInterval < 100 ? 100 : millsInterval, cts.Token);

                    if (!cts.Token.IsCancellationRequested)
                    {
                        ParentControl.Invoke(() =>
                        {
                            if (!IsDisposed && !Disposing)
                            {
                                Close();
                            }
                        });
                    }
                }, cts.Token);
            }
        }

        /// <summary>
        /// SetIcon
        /// </summary>
        /// <param name="iconType"></param>
        public void SetIcon(TType iconType)
        {
            if (iconType is not TType.None)
            {
                var avatar = new Avatar
                {
                    Dock = DockStyle.Left,
                    Width = (int)(Height * 0.7),
                    Padding = new Padding(10, 10, 0, 10),
                    TabStop = false
                };

                switch (iconType)
                {
                    case TType.Success:
                        avatar.ImageSvg = SvgDb.IcoSuccess;
                        avatar.ForeColor = Style.Db.Success;

                        break;

                    case TType.Info:
                        avatar.ImageSvg = SvgDb.IcoInfo;
                        avatar.ForeColor = Style.Db.Info;

                        break;

                    case TType.Warn:
                        avatar.ImageSvg = SvgDb.IcoWarn;
                        avatar.ForeColor = Style.Db.Warning;

                        break;

                    case TType.Error:
                        avatar.ImageSvg = SvgDb.IcoError;
                        avatar.ForeColor = Style.Db.Error;

                        break;

                    default:
                        return;
                }

                Controls.Add(avatar);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            foreach (Form form in Application.OpenForms)
            {
                if (form is LoadingWindow && form != this && !form.Disposing && !form.IsDisposed)
                {
                    form.Location = new Point(form.Location.X, form.Location.Y - Height - 15);
                }
            }

            if (_autoCloseMills > 0)
            {
                CloseAfter(_autoCloseMills, false);
            }
        }
    }
}