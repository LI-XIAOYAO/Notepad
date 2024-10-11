namespace Notepad.Entities
{
    /// <summary>
    /// EmailServer
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="user"></param>
    /// <param name="pwd"></param>
    internal readonly struct EmailUser(string host, int port, string user, string pwd) : IUser
    {
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; } = host;

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; } = port;

        /// <summary>
        /// User
        /// </summary>
        public string Account { get; } = user;

        /// <summary>
        /// Pwd
        /// </summary>
        public string Pwd { get; } = pwd;
    }
}