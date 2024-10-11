using Newtonsoft.Json;
using Notepad.Extensions;

namespace Notepad.Entities
{
    /// <summary>
    /// LocalConfig
    /// </summary>
    internal class LocalConfig : AppConfig
    {
        private static AutoResetEvent? _autoResetEvent = new(false);

        /// <summary>
        /// ConfigPath
        /// </summary>
        public static string ConfigPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Notepad), "Config");

        /// <summary>
        /// ConfigFile
        /// </summary>
        public static string ConfigFile { get; } = Path.Combine(ConfigPath, $"{nameof(Notepad)}.config");

        /// <summary>
        /// UserConfigFile
        /// </summary>
        public static string UserConfigFile => Path.Combine(ConfigPath, $"{Store.User.Account}.config");

        /// <summary>
        /// LoginConfig
        /// </summary>
        public static LoginConfig LoginConfig { get; private set; } = new();

        /// <summary>
        /// Config
        /// </summary>
        public static LocalConfig Config { get; private set; } = new();

        /// <summary>
        /// Langs
        /// </summary>
        public static string[] Langs { get; } = ["", "zh", "en"];

        /// <summary>
        /// IsLoaded
        /// </summary>
        public static bool IsLoaded { get; private set; }

        /// <summary>
        /// Loaded
        /// </summary>
        public static event Action<LocalConfig>? Loaded;

        /// <summary>
        /// X
        /// </summary>
        public int? X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// IsMax
        /// </summary>
        public bool IsMax { get; set; }

        /// <summary>
        /// Lang
        /// </summary>
        public int Lang { get; set; }

        /// <summary>
        /// IsRememberPassword
        /// </summary>
        public bool IsRememberPassword { get; set; }

        /// <summary>
        /// IsAutoLogin
        /// </summary>
        public bool IsAutoLogin { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Startup
        /// </summary>
        public bool Startup { get; set; }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(bool isSaveLoginInfo = false, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Path.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(ConfigPath);
                }

                await File.WriteAllTextAsync(UserConfigFile, JsonConvert.SerializeObject(this).Encrypt(Store.User.Account.Md5()), cancellationToken);

                if (isSaveLoginInfo && LoginConfig.SetLastUser(Store.User.Account))
                {
                    await File.WriteAllTextAsync(ConfigFile, JsonConvert.SerializeObject(LoginConfig), cancellationToken);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// RemoveUserAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveUserAsync(string user, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Path.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(ConfigPath);
                }

                if (user == LoginConfig.LastUser)
                {
                    LoginConfig.LastUser = null;
                }

                if (LoginConfig.Users.Remove(user))
                {
                    await File.WriteAllTextAsync(ConfigFile, JsonConvert.SerializeObject(LoginConfig), cancellationToken);

                    File.Delete(Path.Combine(ConfigPath, $"{user}.config"));

                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// LoadLocalConfigAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<LocalConfig> LoadLocalConfigAsync(CancellationToken cancellationToken = default)
        {
            if (IsLoaded)
            {
                return Config;
            }

            try
            {
                LoginConfig = JsonConvert.DeserializeObject<LoginConfig>(await File.ReadAllTextAsync(ConfigFile, cancellationToken))!;
                Config = JsonConvert.DeserializeObject<LocalConfig>((await File.ReadAllTextAsync(Path.Combine(ConfigPath, $"{LoginConfig!.LastUser}.config"), cancellationToken)).Decrypt(LoginConfig.LastUser!.Md5()))!;
            }
            catch
            {
            }
            finally
            {
                IsLoaded = true;

                try
                {
                    Loaded?.Invoke(Config);
                }
                catch
                {
                }

                _autoResetEvent?.Set();
            }

            return Config;
        }

        /// <summary>
        /// WaitLoad
        /// </summary>
        /// <param name="timeSpan"></param>
        public static bool WaitLoad(TimeSpan? timeSpan = null)
        {
            if (null == _autoResetEvent)
            {
                return true;
            }

            var result = _autoResetEvent.WaitOne(timeSpan ?? Timeout.InfiniteTimeSpan);
            if (result)
            {
                _autoResetEvent.Dispose();
                _autoResetEvent = null;
            }

            return result;
        }

        /// <summary>
        /// LoadUserLocalConfigAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> LoadUserLocalConfigAsync(string user, CancellationToken cancellationToken = default)
        {
            try
            {
                Config = JsonConvert.DeserializeObject<LocalConfig>((await File.ReadAllTextAsync(Path.Combine(ConfigPath, $"{user}.config"), cancellationToken)).Decrypt(user.Md5()))!;

                return true;
            }
            catch
            {
                Config = new();

                return false;
            }
        }
    }
}