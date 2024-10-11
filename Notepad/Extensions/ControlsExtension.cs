using AntdUI;
using Notepad.Controls;
using Notepad.Enums;
using Notepad.Properties;

namespace Notepad.Extensions
{
    /// <summary>
    /// ControlsExtension
    /// </summary>
    internal static class ControlsExtension
    {
        /// <summary>
        /// Loading
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <param name="autoCloseMills"></param>
        /// <param name="isShowLoading"></param>
        /// <param name="isAutoWidth"></param>
        /// <param name="iconType"></param>
        /// <returns></returns>
        public static LoadingWindow Loading(this Control control, string? text = null, int autoCloseMills = 500, bool isShowLoading = true, bool isAutoWidth = true, TType iconType = TType.None)
        {
            var loading = new LoadingWindow(control, autoCloseMills);
            loading.SetIcon(iconType);

            if (!string.IsNullOrEmpty(text))
            {
                loading.Button.Text = text;
            }

            if (!isShowLoading)
            {
                loading.Button.Loading = false;
            }

            if (isAutoWidth)
            {
                loading.SetMeasureWidth(isShowLoading);
            }

            loading.Show();

            return loading;
        }

        /// <summary>
        /// Loading
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="text"></param>
        /// <param name="autoCloseMills"></param>
        /// <param name="isShowLoading"></param>
        /// <param name="isAutoWidth"></param>
        /// <param name="iconType"></param>
        public static void Loading(this Control control, Action action, string? text = null, int autoCloseMills = 500, bool isShowLoading = true, bool isAutoWidth = true, TType iconType = TType.None)
        {
            var loading = control.Loading(text, autoCloseMills, isShowLoading, isAutoWidth, iconType);
            action();

            loading.Close();
        }

        /// <summary>
        /// LoadingAsync
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="text"></param>
        /// <param name="autoCloseMills"></param>
        /// <param name="isShowLoading"></param>
        /// <param name="isAutoWidth"></param>
        /// <param name="iconType"></param>
        /// <returns></returns>
        public static async Task LoadingAsync(this Control control, Func<LoadingWindow, Task> action, string? text = null, int autoCloseMills = 500, bool isShowLoading = true, bool isAutoWidth = true, TType iconType = TType.None)
        {
            var loading = control.Loading(text, autoCloseMills, isShowLoading, isAutoWidth, iconType);
            await action(loading);

            loading.Close();
        }

        /// <summary>
        /// Notification
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="header"></param>
        /// <param name="content"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonType"></param>
        /// <param name="buttonClickAction"></param>
        /// <param name="isCloseOnButtonClick"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="dividerOptions"></param>
        public static void Notification(this Control parent, object header, object content, string? buttonText = null, TTypeMini buttonType = TTypeMini.Primary, Func<DialogWindow, AntdUI.Button, Task>? buttonClickAction = null, bool isCloseOnButtonClick = true, int width = 300, int height = 150, DialogWindowDividerOptions dividerOptions = DialogWindowDividerOptions.TopBottom)
        {
            var window = new DialogWindow(parent, width, height, header, null, dividerOptions)
                .AddButton(buttonText ?? Resources.Notification_Confirm, buttonType, (d, b) =>
                {
                    b.MouseClick += async (_, _) =>
                    {
                        if (null != buttonClickAction)
                        {
                            await buttonClickAction(d, b);
                        }

                        if (isCloseOnButtonClick)
                        {
                            d.Close();
                        }
                    };
                });

            if (content is Control control)
            {
                window.ContentPanel!.Controls.Add(control);
            }
            else
            {
                var panel = new StackPanel
                {
                    Dock = DockStyle.Fill,
                    Vertical = true,
                    AutoScroll = true,
                    TabStop = false
                };

                var text = content.ToString();

                var label = new AntdUI.Label
                {
                    Name = "Content",
                    Text = text,
                    TextAlign = ContentAlignment.MiddleCenter,
                    TabStop = false
                };
                panel.Controls.Add(label);

                window.ContentPanel!.Controls.Add(panel);
                window.Shown += (_, _) =>
                {
                    using var graphics = window.ContentPanel.CreateGraphics();
                    var size = graphics.MeasureString(text, label.Font, label.ReadRectangle.Width - 20, new(StringFormatFlags.LineLimit)).ToSize();

                    if (size.Height > window.Font.Height)
                    {
                        label.TextAlign = ContentAlignment.MiddleLeft;
                    }

                    var minHeight = window.ContentPanel.ClientRectangle.PaddingRect(window.ContentPanel.Padding).Height - window.ContentPanel.Margin.Vertical;

                    if (size.Height < minHeight)
                    {
                        size.Height = minHeight;
                    }

                    label.Height = size.Height;
                };
            }

            window.ShowDialog();
        }

        /// <summary>
        /// Popover
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <param name="autoClose"></param>
        /// <param name="arrowAlign"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Form Popover(this Control control, string text, int autoClose = 0, TAlign arrowAlign = TAlign.Top, Rectangle? offset = null, Size? size = null, Font? font = null)
        {
            return new Popover.Config(control, null != size ? new AntdUI.Label
            {
                Text = text,
                TextMultiLine = true,
                Size = size.Value,
                Font = font,
                TabStop = false,
            } : text)
            {
                Radius = 3,
                ArrowSize = 5,
                AutoClose = autoClose,
                ArrowAlign = arrowAlign,
                Font = font,
                Offset = offset ?? new Rectangle(control.PointToClient(Control.MousePosition), new())
            }.open()!;
        }

        /// <summary>
        /// LinePopover
        /// </summary>
        /// <param name="control"></param>
        /// <param name="content"></param>
        /// <param name="font"></param>
        /// <param name="minWidth"></param>
        public static void LinePopover(this Control control, string? content = null, Font? font = null, int minWidth = 150)
        {
            if (control.Text!.Length > 0)
            {
                var rectangle = control.ClientRectangle.PaddingRect(control.Padding);
                using var graphics = control.CreateGraphics();

                if (graphics.MeasureString(control.Text, control.Font).Width > rectangle.Width)
                {
                    Form? popover = null;
                    content ??= control.Text;

                    var size = graphics.MeasureString(content, control.Font, (rectangle.Width < minWidth ? minWidth : rectangle.Width) - 20, new(StringFormatFlags.LineLimit)).ToSize();

                    control.MouseHover += (_, _) => popover = control.Popover(content, size: size, font: font);
                    control.MouseLeave += (_, _) => popover?.Dispose();
                }
            }
        }

        /// <summary>
        /// HoverLinePopover
        /// </summary>
        /// <param name="control"></param>
        /// <param name="isReplaceNewLine"></param>
        /// <param name="font"></param>
        /// <param name="minWidth"></param>
        public static void HoverLinePopover(this Control control, bool isReplaceNewLine = false, Font? font = null, int minWidth = 150)
        {
            Form? popover = null;

            control.MouseHover += (_, _) =>
            {
                if (control.Text!.Length > 0)
                {
                    var rectangle = control.ClientRectangle.PaddingRect(control.Padding);
                    using var graphics = control.CreateGraphics();

                    if (graphics.MeasureString(control.Text, control.Font).Width > rectangle.Width)
                    {
                        popover = control.Popover(control.Text, size: graphics.MeasureString(isReplaceNewLine ? control.Text.ReplaceNewLine(" ") : control.Text, control.Font, (rectangle.Width < minWidth ? minWidth : rectangle.Width) - 20, new(StringFormatFlags.LineLimit)).ToSize(), font: font);
                    }
                }
            };
            control.MouseLeave += (_, _) => popover?.Dispose();
        }
    }
}