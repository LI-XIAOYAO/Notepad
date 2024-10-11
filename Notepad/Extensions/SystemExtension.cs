using Newtonsoft.Json;
using Notepad.Entities;
using System.Globalization;

namespace System
{
    /// <summary>
    /// SystemExtension
    /// </summary>
    internal static class SystemExtension
    {
        /// <summary>
        /// IsNullOrBlank
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrBlank(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// ReplaceNewLine
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceNewLine(this string str, string replacement = "")
        {
            return str.Replace("\r", replacement).Replace("\n", replacement);
        }

        /// <summary>
        /// Copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Copy<T>(this T value)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value))!;
        }

        /// <summary>
        /// Apply
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Apply<T>(this T obj, Action<T>? action)
        {
            action?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// DelayInvoke
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Task DelayInvoke(this Control control, Action action, int? delay = 50)
        {
            if (null == delay || delay <= 0)
            {
                delay = 50;
            }

            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(delay.Value);

                control.Invoke(action);
            });
        }

        /// <summary>
        /// DelayInvoke
        /// </summary>
        /// <param name="control"></param>
        /// <param name="func"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Task DelayInvoke(this Control control, Func<Task> func, int? delay = 50)
        {
            if (null == delay || delay <= 0)
            {
                delay = 50;
            }

            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(delay.Value);

                await control.Invoke(func);
            });
        }

        /// <summary>
        /// DelayBeginInvoke
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Task DelayBeginInvoke(this Control control, Action action, int? delay = 50)
        {
            if (null == delay || delay <= 0)
            {
                delay = 50;
            }

            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(delay.Value);

                control.BeginInvoke(action);
            });
        }

        /// <summary>
        /// IsDefaultCulture
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static bool IsDefaultCulture(this CultureInfo? cultureInfo)
        {
            return (cultureInfo ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName.Equals(LocalConfig.Langs[1], StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// IsSmallFont
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static bool IsSmallFont(this CultureInfo? cultureInfo)
        {
            return "en" == (cultureInfo ?? CultureInfo.CurrentUICulture)?.TwoLetterISOLanguageName;
        }
    }
}