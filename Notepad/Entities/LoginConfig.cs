namespace Notepad.Entities
{
    /// <summary>
    /// LoginConfig
    /// </summary>
    internal class LoginConfig
    {
        /// <summary>
        /// LastUser
        /// </summary>
        public string? LastUser { get; set; }

        /// <summary>
        /// Users
        /// </summary>
        public HashSet<string> Users { get; set; } = [];

        /// <summary>
        /// SetLastUser
        /// </summary>
        /// <param name="lastUser"></param>
        /// <returns></returns>
        public bool SetLastUser(string lastUser)
        {
            if (LastUser != lastUser)
            {
                LastUser = lastUser;

                Users.Add(lastUser);

                return true;
            }

            return false;
        }
    }
}