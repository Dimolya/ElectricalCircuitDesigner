using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod.DataBase
{
    public static class JsonProvider
    {
        public static List<T> LoadData<T>(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var jsonData = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<List<T>>(jsonData);
                }
                else
                {
                    MessageBox.Show($"Файл не найден: {path}");
                    return new List<T>();

                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show(ex.Message);
                return new List<T>();
            }
        }

        public static void SaveData<T>(string path, List<T> data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }
    }
}
