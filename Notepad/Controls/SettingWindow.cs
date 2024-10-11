using AntdUI;
using Notepad.Extensions;
using Notepad.Properties;

namespace Notepad.Controls
{
    /// <summary>
    /// SettingWindow
    /// </summary>
    internal class SettingWindow : DialogWindow
    {
        /// <summary>
        /// Saved
        /// </summary>
        public event Action? Saved;

        /// <summary>
        /// SettingWindow
        /// </summary>
        /// <param name="parent"></param>
        public SettingWindow(Control parent)
            : base(parent, 280, 495, Resources.Setting, new SettingControl())
        {
            Resizable = false;
            Shadow = 20;

            var isSaved = false;
            var primaryColor = Style.Db.Primary;

            FormClosed += (_, _) =>
            {
                if (!isSaved && primaryColor != Style.Db.Primary)
                {
                    Style.Db.SetPrimary(primaryColor);
                    EventHub.Dispatch(EventType.THEME, primaryColor);
                }
            };

            var settingControl = (SettingControl)Controls[nameof(SettingControl)]!;

            AddButton(Resources.Button_Save, TTypeMini.Primary, (d, b) =>
            {
                b.MouseClick += async (_, _) =>
                {
                    try
                    {
                        b.Loading = true;
                        b.Width += 15;

                        await settingControl.SaveConfigAsync();

                        parent.Loading(Resources.Loading_SaveSuccess, 1500, false, iconType: TType.Success);
                        isSaved = true;

                        Saved?.Invoke();

                        d.Close();
                    }
                    catch (Exception ex)
                    {
                        parent.Loading($"{Resources.Loading_SaveFailed}{ex.Message}", 1500, false, iconType: TType.Error);
                    }
                    finally
                    {
                        b.Loading = false;
                    }
                };
            });
        }
    }
}