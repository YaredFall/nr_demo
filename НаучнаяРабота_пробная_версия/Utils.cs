using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace НаучнаяРабота_пробная_версия
{
    public static class Utils
    {
        public static string ProjectDirectory()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        }

        public static string ToString<T>(this List<T> list)
        {
            string result = "";
            foreach (T item in list)
            {
                result += item.ToString() + ", ";
            }

            return result.Substring(0, result.Length - 2);
        }
    }
}
