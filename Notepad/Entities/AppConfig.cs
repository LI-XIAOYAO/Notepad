namespace Notepad.Entities
{
    /// <summary>
    /// AppConfig
    /// </summary>
    internal abstract class AppConfig
    {
        /// <summary>
        /// IsCard
        /// </summary>
        public bool IsCard { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Theme
        /// </summary>
        public int Theme { get; set; }

        /// <summary>
        /// PrimaryColor
        /// </summary>
        public Color? PrimaryColor { get; set; }

        /// <summary>
        /// IsDark
        /// </summary>
        public bool IsDark => 1 == Theme;

        /// <summary>
        /// Min
        /// </summary>
        public bool Min { get; set; }

        /// <summary>
        /// CanSync
        /// </summary>
        /// <param name="notesConfig"></param>
        /// <returns></returns>
        public bool CanSync(AppConfig notesConfig)
        {
            var canSync = false;

            if (IsCard != notesConfig.IsCard)
            {
                IsCard = notesConfig.IsCard;

                canSync = true;
            }

            if (Sort != notesConfig.Sort)
            {
                Sort = notesConfig.Sort;

                canSync = true;
            }

            if (Theme != notesConfig.Theme)
            {
                Theme = notesConfig.Theme;

                canSync = true;
            }

            if (PrimaryColor != notesConfig.PrimaryColor)
            {
                PrimaryColor = notesConfig.PrimaryColor;

                canSync = true;
            }

            if (Min != notesConfig.Min)
            {
                Min = notesConfig.Min;

                canSync = true;
            }

            return canSync;
        }
    }
}