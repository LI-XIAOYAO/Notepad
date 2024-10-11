using AntdUI;
using Notepad.Properties;
using System.Globalization;

namespace Notepad.Extensions
{
    /// <summary>
    /// LangExtension
    /// </summary>
    internal static class LangExtension
    {
        /// <summary>
        /// Load
        /// </summary>
        public static void Load(this CultureInfo cultureInfo)
        {
            Localization.Provider = new LangLocalization();
        }

        /// <summary>
        /// LangLocalization
        /// </summary>
        private class LangLocalization : ILocalization
        {
            public string GetLocalizedString(string key) => Resources.ResourceManager.GetString($"DEFAULT_{key}", Resources.Culture)!;
        }
    }
}