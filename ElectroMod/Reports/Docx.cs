using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Drawing.Text;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.ExtendedProperties;
using Xceed.Words.NET;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Linq;

namespace ElectroMod.Reports
{
    public class Docx
    {
        private List<(string, string)> _elementsTypes = new List<(string, string)>();
        private List<(string, string)> _elementsResistance = new List<(string, string)>();
        private int _lineCount = 1;
        private int _recloserCount = 1;
        private int _transformatorCount = 1;
        private double _voltage;

        [STAThread]
        public async Task CreateReportDocumentAsync(CenterCalculation calc, Form owner, IProgress<int> progress)
        {
            await Task.Run(() => CreateReportDocument(calc, owner, progress));
        }
        private void CreateReportDocument(CenterCalculation calc, Form owner, IProgress<int> progress)
        {
            Application wordApp = null;
            Document doc = null;
            try
            {
                wordApp = new Application();
                wordApp.ScreenUpdating = false;
                doc = wordApp.Documents.Add();
                _voltage = calc.Voltage;
                //Настройка полей для всех разделов документа
                foreach (Microsoft.Office.Interop.Word.Section section in doc.Sections)
                {
                    // Устанавливаем ширину полей (в пунктах)
                    section.PageSetup.LeftMargin = wordApp.CentimetersToPoints(2.0f);   // Левое поле: 2 см
                    section.PageSetup.RightMargin = wordApp.CentimetersToPoints(2.0f);  // Правое поле: 2 см
                    section.PageSetup.TopMargin = wordApp.CentimetersToPoints(2.0f);    // Верхнее поле: 2 см
                    section.PageSetup.BottomMargin = wordApp.CentimetersToPoints(2.0f); // Нижнее поле: 2 см
                }

                // Добавляем заголовок документа
                AddParagraph(doc, "Расчету токов короткого замыкания", isBold: true, fontSize: 14, isCenterAligned: true);

                // Добавляем оглавление
                Range tocRange = doc.Content.Paragraphs.Add().Range;
                doc.TablesOfContents.Add(tocRange, UseHeadingStyles: true);
                int countK = 2;
                progress.Report(10);
                foreach (var element in calc.UnionElements)
                {
                    if (element is Bus)
                        continue;
                    if(element is Recloser)
                    {
                        countK++;
                        continue;
                    }

                    AddParagraph(doc, $"Расчеты в точке K{countK}", isBold: true, fontSize: 14);

                    var elementsNames = CreateFormulaElementName(doc, element);
                    var elementsResistanceValues = CreateFormulaElementsResistanceValues(element);
                    AddParagraph(doc, $"Ток К.З.в конце линии в макс.режиме:");
                    if (calc.IsCurrent)
                    {
                        AddFormula(doc, $"I_(к.з.max(k{countK})) = " +
                            $"Sub_voltage/(√(3) * (√(({elementsNames.Item1})^2 + ({elementsNames.Item2})^2) + Z_(sub.max))) = " +
                            $"{calc.Voltage}/(√(3) * (√(({elementsResistanceValues.Item1})^2 + ({elementsResistanceValues.Item2})^2) + {calc.Zmax})) = " +
                            $"{Math.Round(element.IkzMax, 3)} кА");

                        AddFormula(doc, $"I_(к.з.min(k{countK})) = " +
                            $"Sub_voltage/(√(3) * (√(({elementsNames.Item1})^2 + ({elementsNames.Item2})^2) + Z_(sub.min))) = " +
                            $"{calc.Voltage}/(√(3) * (√(({elementsResistanceValues.Item1})^2 + ({elementsResistanceValues.Item2})^2) + {calc.Zmin})) = " +
                            $"{Math.Round(element.IkzMin, 3)} кА");
                    }
                    else
                    {
                        AddFormula(doc, $"I_(к.з.max(k{countK})) = " +
                            $"Sub_voltage/(√(3) * √((R_(sub.max) + {elementsNames.Item1})^2 + (X_(sub.max) + {elementsNames.Item2})^2)) = " +
                            $"{calc.Voltage}/(√(3) * √(({calc.Rmax} + {elementsResistanceValues.Item1})^2" +
                                                   $"+ ({calc.Xmax} + {elementsResistanceValues.Item2})^2)) = {Math.Round(element.IkzMax, 3)} кА");

                        AddFormula(doc, $"I_(к.з.min(k{countK})) = " +
                            $"Sub_voltage/(√(3) * √((R_(sub.min) + {elementsNames.Item1})^2 + (X_(sub.min) + {elementsNames.Item2})^2)) = " +
                            $"{calc.Voltage}/(√(3) * √(({calc.Rmin} + {elementsResistanceValues.Item1})^2" +
                                                   $"+ ({calc.Xmin} + {elementsResistanceValues.Item2})^2)) = {Math.Round(element.IkzMin, 3)} кА");
                    }
                    countK++;
                }

                progress.Report(40);
                AddParagraph(doc, "");
                AddParagraph(doc, "Результаты расчетов токов К.З.", isBold: true);
                AddCalculationTableCurrentsKZ(doc, calc);

                AddParagraph(doc, "");
                AddParagraph(doc, "Расчет уставок защит МТО, МТЗ", isBold: true, fontSize: 14, isCenterAligned: true);
                AddParagraph(doc, "");
                AddParagraph(doc, $"Коэффициент трансформации установленного трансформатора тока {calc.TypeTTBus} Ктт={calc.NttBus}\r\n");

                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddParagraph(doc, $"Согласно выданных ТУ \"{calc.NumberTY}\", ранее присоединённая мощность P_t.u.such {calc.PowerSuchKBT} кВт, мощность вновь присоединяемого электрооборудования P_t.u. {calc.PowerKBT} кВт\r\n");
                else
                    AddParagraph(doc, $"Согласно исходных данных мощность на фидере P_such {calc.PowerSuchKBA} кВа\r\n");

                foreach (var report in calc.Reports)
                {
                    report.GenerateReport(doc, calc);
                }
                progress.Report(80);

                AddTableResoultsMTOMTZ(doc, calc);
                progress.Report(100);
                owner.Invoke(new Action(() =>
                {
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Документы Word (*.docx)|*.docx";
                        saveFileDialog.Title = "Сохранить документ Word";
                        owner?.Activate();
                        owner?.Focus();
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string filePath = saveFileDialog.FileName;
                            try
                            {
                                doc.SaveAs2(filePath);
                                MessageBox.Show("Документ успешно сохранен!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Документ не сформирован по причине: {ex.Message}");
            }
            finally
            {
                //wordApp.ScreenUpdating = true;
                doc?.Close(false);
                wordApp?.Quit();
            }
        }

        private void AddTableResoultsMTOMTZ(Document doc, CenterCalculation calc)
        {
            var countRows = calc.UnionElements.OfType<Recloser>().Count(); 
            Table table = doc.Tables.Add(doc.Content.Paragraphs.Add().Range, countRows + 2, 3); // + 1 потому что надо заголовок
            table.Borders.Enable = 1; // Устанавливаем границы таблицы
            table.Cell(1, 1).Range.Text = "Наименование";
            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Text = "МТО";
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Text = "МТЗ";
            table.Cell(1, 3).Range.Bold = 1;

            table.Cell(2, 1).Range.Text = $"{calc.Bus.Name}";
            table.Cell(2, 2).Range.Text = $"{calc.Bus.TableMTO}";
            table.Cell(2, 3).Range.Text = $"{calc.Bus.TableMTZ}";

            for (int i = 0; i < countRows; i++)
            {
                table.Cell(i + 3, 1).Range.Text = $"{calc.Reclosers[i].Name}";
                table.Cell(i + 3, 2).Range.Text = $"{calc.Reclosers[i].TableMTO}";
                table.Cell(i + 3, 3).Range.Text = $"{calc.Reclosers[i].TableMTZ}";
            }

            // Форматирование заголовков
            //Microsoft.Office.Interop.Word.Range headerRange = table.Rows[1].Range;
            //headerRange.Bold = 1;
            //headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //for (int i = 0; i < reports.Count; i++)
            //{
            //    table.Cell(i + 2, 1).Range.Text = reports[i].Element.Name;
            //    if (reports[i].Element is Bus bus)
            //        table.Cell(i + 2, 2).Range.Text = bus.MTO.ToString();
            //    if (reports[i].Element is Recloser recloser)
            //        table.Cell(i + 2, 2).Range.Text = recloser.MTO.ToString();
            //}
        }

        private void AddParagraph(Document doc, string text, bool isBold = false, bool isCenterAligned = false, int fontSize = 10)
        {
            Paragraph paragraph = doc.Content.Paragraphs.Add();
            paragraph.Range.Text = text;
            paragraph.Range.Font.Size = fontSize;
            paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

            if (isBold)
                paragraph.Range.Font.Bold = 1;
            else
                paragraph.Range.Font.Bold = 0;

            if (isCenterAligned)
                paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            paragraph.Range.InsertParagraphAfter();
        }

        public void AddCalculationTableCurrentsKZ(Document doc, CenterCalculation calc)
        {
            var unionElements = calc.UnionElements.ToArray();
            Table table = doc.Tables.Add(doc.Content.Paragraphs.Add().Range, unionElements.Count() * 3 + 1, 3);
            table.Borders.Enable = 1; // Устанавливаем границы таблицы

            for (int i = 0; i < unionElements.Count(); i++)
            {
                int rowOffset = i * 3 + 1;

                // Добавляем заголовок для точки расчета
                table.Cell(rowOffset, 1).Merge(table.Cell(rowOffset, 3));
                table.Cell(rowOffset, 1).Range.Text = $"Расчет токов К.З. в точке K{i + 1}";
                table.Cell(rowOffset, 1).Range.Bold = 1;
                table.Cell(rowOffset, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                // Добавляем данные для максимального режима
                table.Cell(rowOffset + 1, 1).Range.Text = "Ток К.З. в максимальном режиме";
                table.Cell(rowOffset + 1, 2).Range.Text = $"{Math.Round(unionElements[i].IkzMax, 3)}";
                table.Cell(rowOffset + 1, 2).Range.Bold = 0;
                table.Cell(rowOffset + 1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(rowOffset + 1, 3).Range.Text = "кА";
                table.Cell(rowOffset + 1, 3).Range.Bold = 0;

                // Добавляем данные для минимального режима
                table.Cell(rowOffset + 2, 1).Range.Text = "Ток К.З. в минимальном режиме";
                table.Cell(rowOffset + 2, 2).Range.Text = $"{Math.Round(unionElements[i].IkzMin, 3)}";
                table.Cell(rowOffset + 2, 2).Range.Bold = 0;
                table.Cell(rowOffset + 2, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(rowOffset + 2, 3).Range.Text = "кА";
                table.Cell(rowOffset + 2, 3).Range.Bold = 0;
            }
        }

        public void AddFormula(Document doc, string formula)
        {
            Paragraph formulaParagraph = doc.Content.Paragraphs.Add();
            formulaParagraph.Range.Text = formula;

            // Форматируем текст как OMML (формула)
            OMaths maths = formulaParagraph.Range.OMaths;
            maths.Add(formulaParagraph.Range);
            maths[1].BuildUp();

            formulaParagraph.Range.InsertParagraphAfter();
        }

        private (string, string) CreateFormulaElementName(Document doc, Element element)
        {
            int elementCount = 0;
            if (element is Line line)
            {
                AddParagraph(doc, $"Активное сопротивление линии {line.Name}:", isBold: false);
                AddFormula(doc, $"R_line{_lineCount} = r*L = {line.ActiveResistanceFromDto}*{line.Length}={line.ActiveResistance} Ом");
                AddParagraph(doc, $"Реактивное сопротивление линии {line.Name}:", isBold: false);
                AddFormula(doc, $"X_line{_lineCount} = x*L = {line.ReactiveResistanceFromDto}*{line.Length}={line.ReactiveResistance} Ом");
                elementCount = _lineCount++;
            }

            if (element is Transormator trans)
            {
                AddParagraph(doc, $"Расчет сопротивления трансформатора:", isBold: false);
                AddFormula(doc, $"Z_trans = 10*((U_k*Sub_voltage^2)/(S_ном)) = 10*(({trans.Uk}*{_voltage}^2)/({trans.S})) = {trans.FullResistance} Ом");
                AddFormula(doc, $"R_trans = (P_k*Sub_voltage^2)/(S_ном^2) = ({trans.Pk}*{_voltage}^2)/({trans.S}^2) = {trans.ActiveResistance} Ом");
                AddFormula(doc, $"X_trans = √(Z_trans^2 - R_trans^2) = √({trans.FullResistance}^2 - {trans.ActiveResistance}^2) = {trans.ReactiveResistance} Ом");
                elementCount = _transformatorCount++;
            }

            var elementTypeR = $"R_{element.GetType().Name}{elementCount}";
            var elementTypeX = $"X_{element.GetType().Name}{elementCount}";
            var elementTypesR = default(string);
            var elementTypesX = default(string);
            _elementsTypes.Add((elementTypeR, elementTypeX));

            for (int i = 0; i < _elementsTypes.Count; i++)
            {
                if (i == _elementsTypes.Count - 1)
                {
                    elementTypesR += _elementsTypes[i].Item1;
                    elementTypesX += _elementsTypes[i].Item2;
                    break;
                }
                elementTypesR += _elementsTypes[i].Item1 + " + ";
                elementTypesX += _elementsTypes[i].Item2 + " + ";
            }

            return (elementTypesR, elementTypesX);
        }

        private (string, string) CreateFormulaElementsResistanceValues(Element element)
        {
            var elementActiveRes = default(string);
            var elementReactiveRes = default(string);

            if (element is Line line)
                _elementsResistance.Add((line.ActiveResistance.ToString(), line.ReactiveResistance.ToString()));
            if (element is Transormator transormator)
                _elementsResistance.Add((transormator.ActiveResistance.ToString(), transormator.ReactiveResistance.ToString()));
            for (int i = 0; i < _elementsResistance.Count; i++)
            {
                if (i == _elementsResistance.Count - 1)
                {
                    elementActiveRes += _elementsResistance[i].Item1;
                    elementReactiveRes += _elementsResistance[i].Item2;
                    break;
                }
                elementActiveRes += _elementsResistance[i].Item1 + " + ";
                elementReactiveRes += _elementsResistance[i].Item2 + " + ";
            }
            return (elementActiveRes, elementReactiveRes);
        }

    }
}
