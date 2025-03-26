using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                //string appPath = @"C:\Users\79871\OneDrive\Рабочий стол\ElectroMod\ElectricalCircuitDesigner\ElectricalCircuitDesigner\ElectroMod\";
                //string hashPath = Path.Combine(Path.GetDirectoryName(appPath), "hash.txt");

                //if (!File.Exists(hashPath))
                //{
                //    MessageBox.Show("Файл проверки hash.txt не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //string expectedHash = File.ReadAllText(hashPath).Trim();

                //if (expectedHash != "7df3c162455e7471d3ba1f7fd9c434c4883b9b85e72504dbc18c057c0cce12ac")
                //{
                //    MessageBox.Show("Нарушена целостность программы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                Application.Run(new MainCalculatForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке целостности: {ex.Message}");
            }
        }
    } 
}
