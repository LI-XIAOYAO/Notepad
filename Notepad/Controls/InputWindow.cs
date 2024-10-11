using AntdUI;
using Notepad.Enums;

namespace Notepad.Controls
{
    /// <summary>
    /// InputWindow
    /// </summary>
    internal class InputWindow : DialogWindow
    {
        /// <summary>
        /// Valid
        /// </summary>
        public event Func<Input, string?>? Valid;

        /// <summary>
        /// Tips
        /// </summary>
        public AntdUI.Label Tips { get; }

        /// <summary>
        /// Input
        /// </summary>
        public Input Input { get; }

        /// <summary>
        /// Divider
        /// </summary>
        public Divider Divider { get; }

        /// <summary>
        /// IsNumeric
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// IsRequired
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// InputWindow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="header"></param>
        public InputWindow(Control parent, int width, int height, object? header = null)
            : base(parent, width, height, header, null, DialogWindowDividerOptions.None)
        {
            Resizable = false;
            Shadow = 20;

            Tips = new AntdUI.Label
            {
                Font = new Font(Font.FontFamily, 7),
                ForeColor = Color.Red,
                Dock = DockStyle.Top,
                Height = 10,
                TextAlign = ContentAlignment.MiddleCenter,
                TabStop = false,
                Visible = false
            };
            Input = new Input
            {
                Dock = DockStyle.Fill,
                WaveSize = 0,
                Radius = 3,
                TextAlign = HorizontalAlignment.Center,
                BorderWidth = 0,
                ImeMode = ImeMode.Disable
            };

            ContentPanel!.Padding = new Padding();
            ContentPanel.Radius = 0;
            ContentPanel.Controls.Add(Tips);
            ContentPanel.Controls.Add(Input);

            Divider = new Divider
            {
                Dock = DockStyle.Bottom,
                Thickness = 1F,
                Height = 1,
                Margin = new Padding(10, 0, 10, 0),
                TabStop = false
            };
            Controls.Add(Divider);
            Controls.Add(new AntdUI.Panel
            {
                Dock = DockStyle.Bottom,
                Height = 10,
                Radius = 3,
                TabStop = false
            });

            Shown += (_, _) => Input.Focus();

            void ValidInput()
            {
                if (IsRequired && Input.Text.IsNullOrBlank())
                {
                    SetDividerColor();
                }

                if (null != Valid)
                {
                    var tips = Valid(Input);

                    if (!tips.IsNullOrBlank())
                    {
                        Tips.Text = tips;
                        Tips.Visible = true;
                    }
                }
            }

            Input.KeyDown += (_, e) =>
            {
                if (e.KeyCode is Keys.Enter)
                {
                    ValidInput();

                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode is Keys.Space || IsNumeric && (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
                {
                    e.SuppressKeyPress = true;
                }
            };
            Input.KeyPress += (_, e) =>
            {
                if (Input.MaxLength == Input.Text.Length)
                {
                    ValidInput();

                    e.Handled = true;
                }
            };
            Input.GotFocus += (_, _) => Divider.ColorSplit = Style.Db.Split;
            Input.TextChanged += (_, _) =>
            {
                Tips.Visible = false;
                Tips.Text = string.Empty;
                Divider.ColorSplit = Style.Db.Split;
            };
        }

        /// <summary>
        /// SetDividerColor
        /// </summary>
        /// <param name="color"></param>
        public void SetDividerColor(Color? color = null)
        {
            Divider.ColorSplit = color ?? Color.Red;
        }

        /// <summary>
        /// SetTips
        /// </summary>
        /// <param name="tips"></param>
        public void SetTips(string? tips = null)
        {
            Tips.Text = tips ?? string.Empty;
            Tips.Visible = null != tips;
        }
    }
}