using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod
{
    public class Docx
    {
        //public void CreateReportDocument(CenterCalculation calc)
        //{
        //    string filePath = "C:\\Users\\79871\\OneDrive\\Рабочий стол\\ElectroMod\\ty.docx";
        //    var currents = calc.Currents;
        //    Создаем документ
        //    using (var doc = DocX.Create(filePath))
        //    {
        //        Добавляем заголовок
        //        var title = doc.InsertParagraph("Результирующая таблица:");
        //        title.FontSize(10).Alignment = Alignment.left;

        //        Создаем таблицу с размером 3 столбца и числом строк = размер списка
        //       var table = doc.AddTable(1, 3);

        //        Заполняем таблицу
        //        for (int i = 0; i < currents.Count; i++)
        //        {
        //            var newRowParagraph = table.InsertRow();
        //            newRowParagraph.MergeCells(0, 2);
        //            newRowParagraph.InsertParagraph(0, $"Расчет токов К.З. в точке K{i + 1}", false).Bold().Alignment = Alignment.center; ;

        //            var newRowCurrentMax = table.InsertRow();
        //            newRowCurrentMax.Cells[0].Paragraphs[0].Append("Ток К.З. в максимальном режиме");
        //            newRowCurrentMax.Cells[1].Width = 150;
        //            newRowCurrentMax.Cells[1].Paragraphs[0].Append($"({currents[i].Item1})");
        //            newRowCurrentMax.Cells[2].Paragraphs[0].Append("кА");

        //            var newRowCurrentMin = table.InsertRow();
        //            newRowCurrentMin.Cells[0].Paragraphs[0].Append("Ток К.З. в минимальном режиме");
        //            newRowCurrentMin.Cells[1].Paragraphs[0].Append($"({currents[i].Item2})");
        //            newRowCurrentMin.Cells[2].Paragraphs[0].Append("кА");
        //        }

        //        Вставляем таблицу в документ
        //        doc.InsertTable(table);

        //        Сохраняем документ
        //        doc.Save();
        //    }

        //}

        public void CreateReportDocument(CenterCalculation calc)
        {
            string filePath = "C:\\Users\\79871\\OneDrive\\Рабочий стол\\ElectroMod\\ty.docx";
            var currents = calc.Currents;

            // Создаем документ Word
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Добавляем основные части документа
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                Body body = mainPart.Document.Body;

                // Добавляем заголовок
                AddParagraph(body, "Результирующая таблица:", fontSize: 24);

                // Создаем таблицу
                Table table = new Table();

                // Устанавливаем свойства таблицы (границы и размеры)
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 }
                    )
                );
                table.AppendChild(tableProperties);

                // Заполняем таблицу
                foreach (var (currentMax, currentMin) in currents)
                {
                    // Вставляем заголовок для расчета
                    TableRow headerRow = new TableRow();
                    TableCell headerCell = new TableCell(new Paragraph(new Run(new Text($"Расчет токов К.З. в точке K{currents.IndexOf((currentMax, currentMin)) + 1}"))));
                    headerCell.AppendChild(new TableCellProperties(new GridSpan { Val = 3 }, new Justification { Val = JustificationValues.Center }));
                    headerRow.AppendChild(headerCell);
                    table.AppendChild(headerRow);

                    // Добавляем строку для максимального тока
                    TableRow maxRow = new TableRow();
                    maxRow.Append(
                        CreateTableCell("Ток К.З. в максимальном режиме"),
                        CreateTableCell($"({currentMax})", width: "1500"),
                        CreateTableCell("кА")
                    );
                    table.AppendChild(maxRow);

                    // Добавляем строку для минимального тока
                    TableRow minRow = new TableRow();
                    minRow.Append(
                        CreateTableCell("Ток К.З. в минимальном режиме"),
                        CreateTableCell($"({currentMin})", width: "1500"),
                        CreateTableCell("кА")
                    );
                    table.AppendChild(minRow);
                }

                // Вставляем таблицу в документ
                body.AppendChild(table);

                // Сохраняем документ
                mainPart.Document.Save();
            }

            Console.WriteLine("Документ успешно создан.");
        }

        // Метод для добавления параграфа
        private static void AddParagraph(Body body, string text, int fontSize = 24)
        {
            Paragraph paragraph = new Paragraph(
                new ParagraphProperties(new Justification { Val = JustificationValues.Left }),
                new Run(new RunProperties(new FontSize { Val = fontSize.ToString() }), new Text(text))
            );
            body.AppendChild(paragraph);
        }

        // Метод для создания ячейки таблицы
        private static TableCell CreateTableCell(string text, string width = null)
        {
            TableCell cell = new TableCell(new Paragraph(new Run(new Text(text))));

            if (width != null)
            {
                cell.AppendChild(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = width }
                ));
            }

            return cell;
        }

    }
}
