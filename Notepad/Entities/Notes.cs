using Notepad.Extensions;

namespace Notepad.Entities
{
    /// <summary>
    /// Notes
    /// </summary>
    /// <param name="datas"></param>
    internal class Notes(List<Note> datas) : AppConfig
    {
        /// <summary>
        /// PIN
        /// </summary>
        public string? PIN { get; set; }

        /// <summary>
        /// PINTimeout
        /// </summary>
        public int PINTimeout { get; set; }

        /// <summary>
        /// UnLockTime
        /// </summary>
        public long UnLockTime { get; set; }

        /// <summary>
        /// ErrorRetryTimes
        /// </summary>
        public int ErrorRetryTimes { get; set; }

        /// <summary>
        /// IsEncrypt
        /// </summary>
        public bool IsEncrypt { get; set; }

        /// <summary>
        /// EncryptData
        /// </summary>
        public string? EncryptData { get; set; }

        /// <summary>
        /// LockHotKeys
        /// </summary>
        public HotKeys? LockHotKeys { get; set; }

        /// <summary>
        /// OpenkHotKeys
        /// </summary>
        public HotKeys? OpenkHotKeys { get; set; }

        /// <summary>
        /// Datas
        /// </summary>
        public List<Note> Datas { get; private set; } = datas;

        /// <summary>
        /// Version
        /// </summary>
        public UInt128 Version { get; set; }

        /// <summary>
        /// NextId
        /// </summary>
        public UInt128 NextId { get; set; }

        /// <summary>
        /// GetNextId
        /// </summary>
        /// <returns></returns>
        public UInt128 GetNextId() => NextId++;

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="note"></param>
        public void Add(Note note)
        {
            Datas.Add(note);

            Version++;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="note"></param>
        public void Update(Note note)
        {
            Remove(note, false);

            Add(note);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="datas"></param>
        public void Update(List<Note>? datas = null)
        {
            if (null != datas)
            {
                Datas = datas;
            }

            Version++;
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="note"></param>
        /// <param name="isUpdateVersion"></param>
        public void Remove(Note note, bool isUpdateVersion = true)
        {
            var data = Datas.FirstOrDefault(c => c.Id == note.Id);

            if (null != data && Datas.Remove(data) && isUpdateVersion)
            {
                Version++;
            }
        }

        /// <summary>
        /// SortNotes
        /// </summary>
        /// <returns></returns>
        public List<Note> SortNotes()
        {
            switch (Sort)
            {
                case 0:
                    Datas = [.. Datas.OrderByDescending(c => c.LastTime)];

                    break;

                case 1:
                    Datas = [.. Datas.OrderBy(c => c.LastTime)];

                    break;

                case 2:
                    Datas = [.. Datas.OrderBy(c => c.Title)];

                    break;

                case 3:
                    Datas = [.. Datas.OrderByDescending(c => c.Title)];

                    break;
            }

            return Datas;
        }

        /// <summary>
        /// UpdateConfig
        /// </summary>
        /// <param name="isCard"></param>
        /// <param name="sort"></param>
        /// <param name="isDark"></param>
        /// <returns></returns>
        public bool UpdateConfig(bool? isCard = null, int? sort = null)
        {
            var isUpdated = false;

            if (isCard.HasValue && isCard != IsCard)
            {
                IsCard = isCard.Value;

                isUpdated = true;
            }

            if (sort.HasValue && sort != Sort)
            {
                Sort = sort.Value;

                isUpdated = true;
            }

            return isUpdated;
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isClear"></param>
        public void Encrypt(string key, bool isClear = false)
        {
            EncryptData = Datas.Encrypt(key.Md5());

            if (isClear)
            {
                Datas = [];
            }
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <returns></returns>
        public Notes Encrypt()
        {
            var key = LocalConfig.Config.Key;

            if (!key.IsNullOrBlank())
            {
                Encrypt(key!, true);
            }
            else
            {
                EncryptData = null;
            }

            return this;
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isClear"></param>
        public void Decrypt(string key, bool isClear = false)
        {
            Datas = EncryptData?.Decrypt<List<Note>>(key.Md5()) ?? [];

            if (isClear)
            {
                EncryptData = null;
            }
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public Notes Clone()
        {
            return (Notes)MemberwiseClone();
        }
    }
}