using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Engine
{


    /// <summary>
    /// Класс окружения, создан для получения внешних данных, мышь, клавиатура и тд.
    /// </summary>
    public static class Env
    {
        /// <summary>
        /// Массив нажатых кнопок
        /// </summary>
        public static List<Key> keys = new List<Key>();

        /// <summary>
        /// Получить все нажате клавиши (столько сколько поддерживает клавиатура)
        /// </summary>
        /// <returns></returns>
        public static List<Key> GetInput()
        {
            return GetDownKeys().ToList();
        }
        /// <summary>
        /// Проверить есть ли кнопка в нажатых клавишах
        /// </summary>
        /// <param name="key">Клавиша для проверки</param>
        /// <returns></returns>
        public static bool GetKey(Key key) //
        {
            return GetDownKeys().ToList().Contains(key);
        }

        private static readonly byte[] DistinctVirtualKeys = Enumerable //Ищим кнопки и преабразуем
                .Range(0, 256)
                .Select(KeyInterop.KeyFromVirtualKey)
                .Where(item => item != Key.None)
                .Distinct()
                .Select(item => (byte)KeyInterop.VirtualKeyFromKey(item))
                .ToArray();

        /// <summary>
        /// Возвращаем список из кнопок
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Key> GetDownKeys() //
        {
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var downKeys = new List<Key>();
            for (var index = 0; index < DistinctVirtualKeys.Length; index++)
            {
                var virtualKey = DistinctVirtualKeys[index];
                if ((keyboardState[virtualKey] & 0x80) != 0)
                {
                    downKeys.Add(KeyInterop.KeyFromVirtualKey(virtualKey));
                }
            }

            return downKeys;
        }
        [DllImport("user32.dll")] //Работа с ОС
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetKeyboardState(byte[] keyState);
    }
}
