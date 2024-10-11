using Notepad.Entities;
using Notepad.Enums;
using System.Runtime.InteropServices;

namespace Notepad.Extensions
{
    internal static partial class U32Extension
    {
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// RegisterHotKey
        /// </summary>
        /// <param name="hotkeys"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool RegisterHotKey(this HotKeys? hotkeys, IntPtr hWnd)
        {
            if (null != hotkeys)
            {
                var isRegister = true;

                if (0 == hotkeys.Keys.Count)
                {
                    return RegisterHotKey(hWnd, hotkeys.Id, hotkeys.Modifiers, Keys.None);
                }

                var i = 0;

                foreach (var keys in hotkeys.Keys)
                {
                    isRegister &= RegisterHotKey(hWnd, hotkeys.Id + i++, hotkeys.Modifiers, keys);
                }

                if (!isRegister)
                {
                    hotkeys.UnregisterHotKey(hWnd);
                }

                return isRegister;
            }

            return false;
        }

        /// <summary>
        /// UnregisterHotKey
        /// </summary>
        /// <param name="hotkeys"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool UnregisterHotKey(this HotKeys? hotkeys, IntPtr hWnd)
        {
            if (null != hotkeys)
            {
                if (0 == hotkeys.Keys.Count)
                {
                    UnregisterHotKey(hWnd, hotkeys.Id);
                }

                var i = 0;

                foreach (var keys in hotkeys.Keys)
                {
                    UnregisterHotKey(hWnd, hotkeys.Id + i++);
                }

                return true;
            }

            return false;
        }
    }
}