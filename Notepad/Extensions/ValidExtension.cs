using Notepad.Entities;
using System.Text.RegularExpressions;

namespace Notepad.Extensions
{
    /// <summary>
    /// ValidExtension
    /// </summary>
    internal static partial class ValidExtension
    {
        /// <summary>
        /// IsEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            return EmailRegex().IsMatch(email);
        }

        /// <summary>
        /// GetEmailServer
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static EmailUser? GetEmailServer(this string user, string pwd)
        {
            var match = EmailRegex().Match(user);
            if (!match.Success)
            {
                return default;
            }

            return match.Groups[1].Value.ToLower() switch
            {
                "qq.com" or
                "vip.qq.com" or
                "foxmail.com" => new("imap.qq.com", 993, user, pwd),
                "163.com" => new("imap.163.com", 993, user, pwd),
                "126.com" => new("imap.126.com", 993, user, pwd),
                "189.com" => new("imap.189.com", 993, user, pwd),
                "aliyun.com" => new("imap.aliyun.com", 993, user, pwd),
                "sina.com" => new("imap.sina.com", 993, user, pwd),
                "sohu.com" => new("imap.sohu.com", 993, user, pwd),
                "gmail.com" => new("imap.gmail.com", 993, user, pwd),
                "outlook.com" => new("outlook.office365.com", 993, user, pwd),
                "yahoo.com" => new("imap.mail.yahoo.com", 993, user, pwd),
                "icloud.com" => new("imap.mail.me.com", 993, user, pwd),
                _ => default,
            };
        }

        /// <summary>
        /// MatchUrls
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> MatchUrls(this string value)
        {
            return UrlRegex().Matches(value).Select(c => c.Value).ToList();
        }

        [GeneratedRegex(@"^\w+(?:[-+.]\w+)*@(\w+(?:[-.]\w+)*\.\w+(?:[-.]\w+)*$)")]
        private static partial Regex EmailRegex();

        [GeneratedRegex(@"https?://\S+\.\S+", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
        private static partial Regex UrlRegex();
    }
}