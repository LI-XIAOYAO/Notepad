namespace Notepad.Entities
{
    /// <summary>
    /// AdjustSizeEventArgs
    /// </summary>
    internal class AdjustSizeEventArgs : EventArgs
    {
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// AdjustSizeEventArgs
        /// </summary>
        public AdjustSizeEventArgs()
        {
        }

        /// <param name="width"></param>
        /// <param name="height"></param>
        public AdjustSizeEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}