using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareToAll
{
    public class IsolatedSettingsHelper
    {
        private static System.IO.IsolatedStorage.IsolatedStorageSettings Settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;

        public static T GetValue<T>(string Key)
        {
            T returnValue;
            if (Settings.TryGetValue<T>(Key, out returnValue))// bu keyde bir değer varsa out keye deger de atar
            {
                return returnValue;
            }
            else
            {
                return default(T);
            }
            // return default(T);
        }
        // uygulamayı kapatsak bile bu değerler ısolated storage de bulunur t değeri t herşey olabilir generic string integer vb.fakat...

        public static void SetValue<T>(string Key, T Value)//generic
        {
            // add çıkar çünkü    private static System.IO.IsolatedStorage.IsolatedStorageSettings Settings; tanımlaması yaptık.
            //Settings.Add
            if (Settings.Contains(Key))
            {
                Settings[Key] = Value; //varsa bulup güncelle yoksa ekle
            }
            else
            {
                Settings.Add(Key, Value);
            }

        }
    }
}
