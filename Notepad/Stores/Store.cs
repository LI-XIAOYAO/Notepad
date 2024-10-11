using Notepad.Entities;
using Notepad.Stores;

namespace Notepad
{
    /// <summary>
    /// Store
    /// </summary>
    internal static class Store
    {
        /// <summary>
        /// Provider
        /// </summary>
        public static IStore Provider { get; private set; } = new Mail();

        /// <summary>
        /// Cache
        /// </summary>
        public static Notes Cache => Provider.Cache;

        /// <summary>
        /// User
        /// </summary>
        public static IUser User => Provider.User;

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static IStore Add(IStore store)
        {
            return Provider = store;
        }

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task LoginAsync(IUser user, CancellationToken cancellationToken = default) => Provider.LoginAsync(user, cancellationToken);

        /// <summary>
        /// GetNotesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Notes> GetNotesAsync(CancellationToken cancellationToken = default) => Provider.GetNotesAsync(cancellationToken);

        /// <summary>
        /// SaveNotesAsync
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task SaveNotesAsync(Notes notes, CancellationToken cancellationToken = default) => Provider.SaveNotesAsync(notes, cancellationToken);
    }
}