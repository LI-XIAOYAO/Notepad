namespace Notepad.Entities
{
    /// <summary>
    /// Note
    /// </summary>
    /// <param name="title"></param>
    /// <param name="notes"></param>
    internal class Note(string title, string? value = null, List<Note>? notes = null)
    {
        /// <summary>
        /// Id
        /// </summary>
        public UInt128 Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; } = title;

        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; } = value;

        /// <summary>
        /// LastTime
        /// </summary>
        public DateTimeOffset LastTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Notes
        /// </summary>
        public List<Note>? Notes { get; set; } = notes;

        /// <summary>
        /// NextId
        /// </summary>
        public UInt128 NextId { get; set; }

        /// <summary>
        /// HasNotes
        /// </summary>
        public bool HasNotes => null != Notes && Notes.Count > 0;

        /// <summary>
        /// GetNextId
        /// </summary>
        /// <returns></returns>
        public UInt128 GetNextId() => NextId++;
    }
}