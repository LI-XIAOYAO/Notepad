using Notepad.Enums;

namespace Notepad.Entities
{
    /// <summary>
    /// HotKeys
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyModifiers"></param>
    /// <param name="keys"></param>
    internal class HotKeys(int id, HashSet<KeyModifiers> keyModifiers, HashSet<Keys> keys)
    {
        /// <summary>
        /// LockHotKeyId
        /// </summary>
        public const int LockHotKeyId = 1000;

        /// <summary>
        /// OpenHotKeyId
        /// </summary>
        public const int OpenHotKeyId = 2000;

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; } = id;

        /// <summary>
        /// KeyModifiers
        /// </summary>
        public HashSet<KeyModifiers> KeyModifiers { get; set; } = new(keyModifiers);

        /// <summary>
        /// Modifiers
        /// </summary>
        public KeyModifiers Modifiers { get; } = (KeyModifiers)keyModifiers.Sum(c => (int)c);

        /// <summary>
        /// Keys
        /// </summary>
        public HashSet<Keys> Keys { get; set; } = new(keys);

        /// <summary>
        /// KeyValue
        /// </summary>
        public int KeyValue { get; } = keys.Select((_, i) => (int)Math.Pow(2, i)).Sum();

        public override string ToString()
        {
            return string.Join('+', KeyModifiers.Select(c => c.ToString()).Union(Keys.Select(c => c.ToString())));
        }
    }
}