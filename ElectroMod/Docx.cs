using Microsoft.Office.Interop.Word;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ElectroMod
{
    public class Docx
    {
        public void Generate(List<(double, double)> currents)
        {
            string filePath = "C:\\Users\\79871\\OneDrive\\Рабочий стол\\ElectroMod\\ty.docx";

            // Создаем документ
            using (var doc = DocX.Create(filePath))
            {
                // Добавляем заголовок
                var title = doc.InsertParagraph("Результирующая таблица:");
                title.FontSize(10).Alignment = Alignment.left;

                // Создаем таблицу с размером 3 столбца и числом строк = размер списка
                var table = doc.AddTable(1,3);

                // Заполняем таблицу
                for (int i = 0; i < currents.Count; i++)
                {
                    var newRowParagraph = table.InsertRow();
                    newRowParagraph.MergeCells(0, 2);
                    newRowParagraph.InsertParagraph(0, $"Расчет токов К.З. в точке K{i + 1}", false).Bold().Alignment = Alignment.center; ;

                    var newRowCurrentMax = table.InsertRow();
                    newRowCurrentMax.Cells[0].Paragraphs[0].Append("Ток К.З. в максимальном режиме");
                    newRowCurrentMax.Cells[1].Width = 150;
                    newRowCurrentMax.Cells[1].Paragraphs[0].Append($"({currents[i].Item1})");
                    newRowCurrentMax.Cells[2].Paragraphs[0].Append("кА");

                    var newRowCurrentMin = table.InsertRow();
                    newRowCurrentMin.Cells[0].Paragraphs[0].Append("Ток К.З. в минимальном режиме");
                    newRowCurrentMin.Cells[1].Paragraphs[0].Append($"({currents[i].Item2})");
                    newRowCurrentMin.Cells[2].Paragraphs[0].Append("кА");
                }

                // Вставляем таблицу в документ
                doc.InsertTable(table);

                // Сохраняем документ
                doc.Save();
            }
        }
    }
}
