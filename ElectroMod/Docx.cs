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

namespace ElectroMod
{
    public class Docx
    {
        private List<(string, string)> _elementsTypes = new List<(string, string)>();
        private List<(string, string)> _elementsResistance = new List<(string, string)>();
        private int _lineCount = 1;
        private int _recloserCount = 1;

        [STAThread]
        public async Task CreateReportDocumentAsync(CenterCalculation calc, Form owner)
        {
            await Task.Run(() => CreateReportDocument(calc, owner));
        }
        private void CreateReportDocument(CenterCalculation calc, Form owner)
        {
            Application wordApp = null;
            Document doc = null;
            try
            {
                wordApp = new Application();
                wordApp.ScreenUpdating = false;
                doc = wordApp.Documents.Add();

                // Добавляем заголовок документа
                AddParagraph(doc, "Отчет по расчету токов короткого замыкания", isBold: true, fontSize: 14, isCenterAligned: true);

                // Добавляем оглавление
                Range tocRange = doc.Content.Paragraphs.Add().Range;
                doc.TablesOfContents.Add(tocRange, UseHeadingStyles: true);
                int count = 2;
                var firstListOfElements = true;
                foreach (var elementList in calc.CalculationElementList)
                {
                    foreach (var element in elementList)
                    {
                        if ((calc.CommonElements.Contains(element) && !firstListOfElements) || element is Bus)
                            continue;

                        AddParagraph(doc, $"Расчеты в точке K{count}", isBold: true, fontSize: 14);

                        var elementsNames = CreateFormulaElementName(doc, element);
                        var elementsResistanceValues = CreateFormulaElementsResistanceValues(element);
                        AddParagraph(doc, $"Ток К.З.в конце линии в макс.режиме:");
                        if (calc.IsCurrent)
                        {
                            AddFormula(doc, $"I_(к.з.max(k{count})) = " +
                                $"Sub_voltage/(√(3) + (√(({elementsNames.Item1})^2 + ({elementsNames.Item1})^2) + Z_(sub.max))) = " +
                                $"{calc.Voltage}/(√(3) + (√(({elementsResistanceValues.Item1})^2 + ({elementsResistanceValues.Item2})^2) + {calc.Zmax})) = " +
                                $"{Math.Round(calc.Currents[count - 1].Item1, 3)}");

                            AddFormula(doc, $"I_(к.з.min(k{count})) = " +
                                $"Sub_voltage/(√(3) + (√(({elementsNames.Item1})^2 + ({elementsNames.Item2})^2) + Z_(sub.min))) = " +
                                $"{calc.Voltage}/(√(3) + (√(({elementsResistanceValues.Item1})^2 + ({elementsResistanceValues.Item2})^2) + {calc.Zmin})) = " +
                                $"{Math.Round(calc.Currents[count - 1].Item2, 3)}");
                        }
                        else
                        {
                            AddFormula(doc, $"I_(к.з.max(k{count})) = " +
                                $"Sub_voltage/(√(3) + √((R_(sub.max) + {elementsNames.Item1})^2 + (X_(sub.max) + {elementsNames.Item2})^2)) = " +
                                $"{calc.Voltage}/(√(3) + √(({calc.Rmax} + {elementsResistanceValues.Item1})^2" +
                                                       $"+ ({calc.Xmax} + {elementsResistanceValues.Item2})^2)) = {Math.Round(calc.Currents[count - 1].Item1, 3)}");

                            AddFormula(doc, $"I_(к.з.min(k{count})) = " +
                                $"Sub_voltage/(√(3) + √((R_(sub.min) + {elementsNames.Item1})^2 + (X_(sub.min) + {elementsNames.Item2})^2)) = " +
                                $"{calc.Voltage}/(√(3) + √(({calc.Rmin} + {elementsResistanceValues.Item1})^2" +
                                                       $"+ ({calc.Xmin} + {elementsResistanceValues.Item2})^2)) = {Math.Round(calc.Currents[count - 1].Item1, 3)}");
                        }

                        count++;
                    }
                    firstListOfElements = false;
                }


                AddParagraph(doc, "Результаты расчетов", isBold: true);
                AddCalculationTable(doc, calc);

                AddParagraph(doc, "");
                AddParagraph(doc, "Выбор уставок защиты фидера", isBold: true);
                AddParagraph(doc, $"КТТ={calc.RecloserNtt}");
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddParagraph(doc, $"Согласно выданных ТУ \"{calc.NumberTY}\", ранее присоединённая мощность P_t.u.such {calc.PowerSuchKBT} кВт, мощность вновь присоединяемого электрооборудования P_t.u. {calc.PowerKBT} кВт");
                else
                    AddParagraph(doc, $"Согласно исходных данных мощность на фидере P_such {calc.PowerSuchKBA} кВа");

                AddParagraph(doc, "Токовая отсечка (ТО)", isBold: true, fontSize: 14);
                AddParagraph(doc, "Отстройка защиты от броска тока намагничивания");
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust:F3}");
                else
                    AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}");

                AddFormula(doc, $"I_сз ≥ k_н*∑I_уст ≥ 1.2*{calc.Iust} ≥ {calc.IszMTO}");
                AddFormula(doc, $"I_сз ≥ 3..4 * I_уст ≥ (3..4*{calc.Iust}) ≥ ({calc.Isz2MTO.Item1}..{calc.Isz2MTO.Item2})");

                AddParagraph(doc, "Где ∑I_уст - сумма номинальных токов всех трансформаторов, которые могут одновременно включаться под напряжение по защищаемой линии. k_н- коэффициент надежности\r\n" +
                    $"Отстройка защиты от тока 3-х фазного К.З. после проектируемого трансформатора КТП {calc.PowerKBA} кВа");
                AddFormula(doc, $"I_сз > k_н * (I_(к.з.max(Klast)) * 1000) > 1.2 * ({calc.LastElementList?.IszMax} * 1000) > {calc.Isz3MTO}");
                AddParagraph(doc, "Где  k_н=1,2 для МПУ\r\n" +
                    "Проверка чувствительности при 2-х фазном К.З. в минимальном режиме в наиболее удаленной точке проектируемой отпайки::");
                AddFormula(doc, $"k_чувст=(I_(к.з.min(KsecondLast))*0,865*1000)/I_сз = ({calc.SecondLastElementList?.IszMin}*0,865*1000)/{calc.IszMTOMax}={calc.KchuvMTO}");
                if (calc.KchuvMTO > 1.2)
                    AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) ");
                else
                    AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");

                //Тут надо отступить на новый лист 
                AddParagraph(doc, "Максимальная токовая защита МТЗ", isBold: true, fontSize: 14, isCenterAligned: true);
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                {
                    AddParagraph(doc, $"Согласно выданных ТУ \"{calc.NumberTY}\", ранее присоединённая мощность {calc.PowerSuchKBT} кВт, мощность вновь присоединяемого электрооборудования {calc.PowerKBT} кВт");
                    AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust}");
                }
                else
                {
                    AddParagraph(doc, $"Согласно исходных данных мощность на фидере {calc.PowerSuchKBA} кВа");
                    AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}");
                }
                AddParagraph(doc, "Условие отстройки от максимального рабочего тока");
                AddFormula(doc, $"I_сз>((k_н*k_сз)/k_в)*I_уст > (({calc.RecloserKn}*{calc.RecloserKcz})/{calc.RecloserKb})*{calc.Iust} > {calc.IszMTZ}"); //ToDo: тут надо потом calc.IszMTZ округлить до целого всегда в большую сторону и потом его учитывать в формулах 
                AddParagraph(doc, $"Принимаем уставку I_сз = {calc.IszMTZCeiling}\r\n" +
                    "Проверим чувствительность к минимальному току КЗ (Кч>1,5 по ПУЭ):");
                AddFormula(doc, $"k_чувст=(I_(к.з.min(KsecondLast))*0,865*1000)/(I_(c.з.)) = ({calc.SecondLastElementList?.IszMin}*0,865*1000)/{calc.IszMTZCeiling} = {calc.KchuvMTZ}");

                //AddParagraph(doc, "Проверка чувствительности МТЗ (для схемы D/Y)", isBold: true, fontSize: 14, isCenterAligned: true);
                //AddParagraph(doc, "");
                //AddFormula(doc, $"I_р=(K_сx·I_сз)/ntt = (1*{calc.IszMTZ})/{calc.RecloserNtt} = {calc.Ip}");
                //AddParagraph(doc, "Где K_сx - коэффициент схемы принимаем 1, ntt коэффициент трансформации трансформатора тока.\r\n" +
                //    "Определяем ток в реле при КЗ за трансформатором:");
                //AddFormula(doc, $"I_р2=(0,5∙I_(к.з.max(к4))*1000)/ntt = (0,5∙{calc.LastElementList?.IcsMax}·1000)/{calc.RecloserNtt} = {calc.Ip2}");
                //AddParagraph(doc, "Определяем коэффициент чувствительности при двухфазном КЗ за трансформатором по схеме неполная звезда:");
                //AddFormula(doc, $"k_чувст=I_р2/I_р = {calc.Ip2}/{calc.Ip} = {calc.Kchuv2MTZ}");
                //AddFormula(doc, "k_чувст > 1,5");
                //AddParagraph(doc, "Рассчитываем ток однофазного К.З., за трансформатором:");
                //AddFormula(doc, $"I_кз1 = U_ф/((1/3)*Z_тр) = 220/((1/3)*{calc.TransformatorFullResistance}) = {calc.Ikz1}");
                //AddParagraph(doc, "Приведем ток однофазного КЗ на стороне 0,4 кВ к напряжению 10,5 кВ");
                //AddFormula(doc, $"I_(кз1-10кВ)=I_кз1*(U_нн/Sub_voltage) = {calc.Ikz1}*(0.4/{calc.Voltage}) = {calc.Ikz1low}");
                //AddParagraph(doc, "Определяем ток в реле при однофазном КЗ за трансформатором:");
                //AddFormula(doc, $"I_р1=I_(кз1-10кВ)/(√(3) * ntt) = {calc.Ikz1low}/(√(3) * {calc.RecloserNtt}) = {calc.Ip1}");
                //AddParagraph(doc, "Определяем коэффициент чувствительности при однофазном КЗ за трансформатором:");
                //AddFormula(doc, $"k_чувст=I_р1/I_р = {calc.Ip1}/{calc.Ip} = {calc.Kchuv3MTZ}");
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

                            // Сохраняем документ
                            doc.SaveAs2(filePath);
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                wordApp.ScreenUpdating = true;
                doc?.Close(false);
                wordApp?.Quit();
            }
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

        private void AddCalculationTable(Document doc, CenterCalculation calc)
        {
            var currents = calc.Currents;

            Table table = doc.Tables.Add(doc.Content.Paragraphs.Add().Range, currents.Count * 3 + 1, 3);
            table.Borders.Enable = 1; // Устанавливаем границы таблицы

            for (int i = 0; i < currents.Count; i++)
            {
                int rowOffset = i * 3 + 1;

                // Добавляем заголовок для точки расчета
                table.Cell(rowOffset, 1).Merge(table.Cell(rowOffset, 3));
                table.Cell(rowOffset, 1).Range.Text = $"Расчет токов К.З. в точке K{i + 1}";
                table.Cell(rowOffset, 1).Range.Bold = 1;
                table.Cell(rowOffset, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                // Добавляем данные для максимального режима
                table.Cell(rowOffset + 1, 1).Range.Text = "Ток К.З. в максимальном режиме";
                table.Cell(rowOffset + 1, 2).Range.Text = $"{Math.Round(currents[i].Item1, 3)} кА";

                // Добавляем данные для минимального режима
                table.Cell(rowOffset + 2, 1).Range.Text = "Ток К.З. в минимальном режиме";
                table.Cell(rowOffset + 2, 2).Range.Text = $"{Math.Round(currents[i].Item2, 3)} кА";
            }
        }

        private void AddFormula(Document doc, string formula)
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
                AddFormula(doc, $"R_line{_lineCount} = r*L = {line.ActiveResistanceFromDto}*{line.Length}={line.ActiveResistance}");
                AddParagraph(doc, $"Реактивное сопротивление линии {line.Name}:", isBold: false);
                AddFormula(doc, $"X_line{_lineCount} = x*L = {line.ReactiveResistanceFromDto}*{line.Length}={line.ReactiveResistance}");
                elementCount = _lineCount++;
            }
            if (element is Recloser)
                elementCount = _recloserCount++;

            //if (element is Transormator trans)
            //{
            //    AddParagraph(doc, $"Расчет сопротивления трансформатора:", isBold: false);
            //    AddFormula(doc, $"Z_trans = 10*((U_k*Sub_voltage^2)/(S_ном)) = 10*((U_k*{}^2)/(S_ном))");
            //    AddFormula(doc, $"R_trans = (P_k*Sub_voltage^2)/(S_ном^2)");
            //    AddFormula(doc, $"X_trans = √(Z_trans^2 - R_trans^2)");
            //}

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
            if (element is Recloser)
                _elementsResistance.Add(("0", "0"));
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
