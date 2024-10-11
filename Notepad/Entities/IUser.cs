namespace Notepad.Entities
{
    /// <summary>
    /// IUser
    /// </summary>
    internal interface IUser
    {
        /// <summary>
        /// Account
        /// </summary>
        string Account { get; }

        /// <summary>
        /// Pwd
        /// </summary>
        string Pwd { get; }
    }
}