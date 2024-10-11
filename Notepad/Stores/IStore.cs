using Notepad.Entities;

namespace Notepad
{
    /// <summary>
    /// IStore
    /// </summary>
    internal interface IStore
    {
        /// <summary>
        /// Cache
        /// </summary>
        Notes Cache { get; }

        /// <summary>
        /// User
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LoginAsync(IUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetNotesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Notes> GetNotesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// SaveNotesAsync
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveNotesAsync(Notes notes, CancellationToken cancellationToken = default);
    }
}