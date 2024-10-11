using AntdUI;
using Notepad.Extensions;
using Notepad.Properties;

namespace Notepad.Controls
{
    /// <summary>
    /// CrashWindow
    /// </summary>
    internal class CrashWindow : Window
    {
        /// <summary>
        /// CrashWindow
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public CrashWindow(string title, object content, int width = 500, int height = 250)
        {
            TopMost = true;

            string text;

            if (content is Exception exception)
            {
                exception = exception.InnerException ?? exception;

                text = $"Error: {exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}";
            }
            else
            {
                text = $"Error: {content}";
            }

            SuspendLayout();

            Size = new Size(width, height);
            Resizable = false;
            BackColor = Style.Db.BgBase;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Divider
            {
                Dock = DockStyle.Top,
                Thickness = 1F,
                Height = 1,
                Margin = new Padding(10, 0, 10, 0),
                TabStop = false,
            });

            var headerPanel = new AntdUI.Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                Radius = 3,
                TabStop = false
            };

            headerPanel.Controls.Add(new Avatar
            {
                Dock = DockStyle.Left,
                Width = headerPanel.Height,
                Padding = new Padding(8),
                ImageSvg = Resources.Svg_Icon
            });
            headerPanel.Controls.Add(new AntdUI.Button
            {
                IconSvg = Resources.Svg_DialogClose,
                Dock = DockStyle.Right,
                Size = new Size(headerPanel.Height, headerPanel.Height),
                Radius = 0,
                WaveSize = 0,
                Ghost = true,
                TabStop = false
            }.Apply(b => b.MouseClick += (_, _) =>
            {
                MainForm.CloseNotifyIcon();
                Application.Exit();
            }));
            headerPanel.Controls.Add(new AntdUI.Label
            {
                Text = title,
                PrefixSvg = SvgDb.IcoError,
                PrefixColor = Style.Db.Error,
                Height = headerPanel.Height,
                Font = new Font(Font.FontFamily, 11),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                TabStop = false
            }.Apply(l => l.MouseDown += (_, _) => DraggableMouseDown()));

            Controls.Add(headerPanel);

            var panel = new StackPanel
            {
                Dock = DockStyle.Fill,
                Vertical = true,
                AutoScroll = true,
                TabStop = false
            };
            var label = new AntdUI.Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10, 0, 10, 0),
                TabStop = false
            };
            label.MouseDoubleClick += (_, e) =>
            {
                string? tips = null;

                try
                {
                    Clipboard.SetText(label.Text);
                }
                catch
                {
                    tips = Resources.CopyFailed;
                }

                label.Popover(tips ?? Resources.CopySuccess, 1, offset: new Rectangle(e.Location, new()));
            };
            panel.Controls.Add(label);

            Controls.Add(panel);
            panel.BringToFront();

            ResumeLayout();

            Shown += (_, _) =>
            {
                var rectangle = Screen.GetBounds(this);
                Location = new Point((rectangle.Width - Width) / 2, (int)((rectangle.Height - Height) * 0.35));

                using var graphics = panel.CreateGraphics();
                var size = graphics.MeasureString(text, label.Font, label.ReadRectangle.Width - 20, new(StringFormatFlags.LineLimit)).ToSize();

                if (size.Height > label.Font.Height)
                {
                    label.TextAlign = ContentAlignment.MiddleLeft;
                }

                var minHeight = panel.ReadRectangle.Height - panel.Margin.Vertical;

                if (size.Height < minHeight)
                {
                    size.Height = minHeight;
                }

                label.Height = size.Height;
            };
        }
    }
}