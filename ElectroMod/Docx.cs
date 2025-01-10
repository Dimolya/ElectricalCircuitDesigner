using System;
using System.Security.AccessControl;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

namespace ElectroMod
{
    public class Docx
    {
        [STAThread]
        public void CreateReportDocument(CenterCalculation calc, Form owner)
        {
            Application wordApp = null;
            Document doc = null;

            try
            {
                wordApp = new Application();
                doc = wordApp.Documents.Add();

                // Добавляем заголовок документа
                AddParagraph(doc, "Отчет по расчету токов короткого замыкания", isBold: true, isCenterAligned: true);

                // Добавляем оглавление
                Range tocRange = doc.Content.Paragraphs.Add().Range;
                doc.TablesOfContents.Add(tocRange, UseHeadingStyles: true);

                AddParagraph(doc, "Результаты расчетов", isBold: true);
                AddCalculationTable(doc, calc);

                AddParagraph(doc, "");
                AddParagraph(doc, "Выбор уставок защиты фидера", isBold: true);
                AddParagraph(doc, "КТТ=ntt (из БД на ТТ)");
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddParagraph(doc, $"Согласно выданных ТУ \"{calc.NamberTY}\", ранее присоединённая мощность {calc.PowerSuchKBT} кВт, мощность вновь присоединяемого электрооборудования {calc.PowerKBT} кВт");
                else
                    AddParagraph(doc, $"Согласно исходных данных мощность на фидере {calc.PowerSuchKBA} кВа");

                AddParagraph(doc, "Токовая отсечка (ТО)", isBold: true, fontSize: 14);
                AddParagraph(doc, "Отстройка защиты от броска тока намагничивания");
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                    AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust}");
                else
                    AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}");

                AddFormula(doc, $"I_сз ≥ k_н*∑I_уст ≥ 1.2*{calc.Iust} ≥ {calc.IszMTO}");
                AddFormula(doc, $"I_сз ≥ 3..4 * I_уст ≥ (3..4*{calc.Iust}) ≥ ({calc.Isz2MTO.Item1}..{calc.Isz2MTO.Item2})");

                AddParagraph(doc, "Где ∑I_уст - сумма номинальных токов всех трансформаторов, которые могут одновременно включаться под напряжение по защищаемой линии. k_н- коэффициент надежности\r\n" +
                    "Отстройка защиты от тока 3-х фазного К.З. после проектируемого трансформатора КТП S кВа");
                AddFormula(doc, $"I_сз > k_н * (I_(к.з.max(к4)) * 1000) > 1.2 * ({calc.LastElementList?.IcsMax} * 1000) > {calc.Isz3MTO}");
                AddParagraph(doc, "Где  k_н=1,2 для МПУ\r\n" +
                    "Проверка чувствительности при 3-х фазном К.З. в максимальном режиме:");
                AddFormula(doc, $"k_чувст=(I_(к.з.min(к3))*0,865*1000)/I_сз = ({calc.SecondLastElementList?.IcsMin}*0,865*1000)/{calc.IszMTO}={calc.KchuvMTO}");
                if (calc.KchuvMTO > 1.2)
                    AddParagraph(doc, "k_чувст > 1,2 - условие выполняется (для зон дальнего резервирования) ");
                else
                    AddParagraph(doc, "k_чувст < 1,2 - условие  не выполняется (для зон дальнего резервирования) ");

                //Тут надо отступить на новый лист 
                AddParagraph(doc, "Максимальная токовая защита МТЗ", isBold: true, fontSize: 14, isCenterAligned: true);
                if (calc.ReconnectName == "Расчет по мощности ТУ")
                {
                    AddParagraph(doc, $"Согласно выданных ТУ \"{calc.NamberTY}\", ранее присоединённая мощность {calc.PowerSuchKBT} кВт, мощность вновь присоединяемого электрооборудования {calc.PowerKBT} кВт");
                    AddFormula(doc, $"I_уст = (P_(t.u.such)+P_(t.u))/(√(3) * Sub_voltage * 0.95) = ({calc.PowerSuchKBT} + {calc.PowerKBT})/(√(3) * {calc.Voltage} * 0.95) = {calc.Iust}");
                }
                else
                {
                    AddParagraph(doc, $"Согласно исходных данных мощность на фидере {calc.PowerSuchKBA} кВа");
                    AddFormula(doc, $"I_уст = (P_such+S)/(√(3) * Sub_voltage) = ({calc.PowerSuchKBA} + {calc.PowerKBA})/(√(3) * {calc.Voltage}) = {calc.Iust}");
                }
                AddParagraph(doc, "Условие отстройки от максимального рабочего тока");
                AddFormula(doc, $"I_сз>((k_н*k_сз)/k_в)*I_уст > (({calc.RecloserKn}*{calc.RecloserKcz})/{calc.RecloserKb})*{calc.Iust} > {calc.IszMTZ}");
                AddParagraph(doc, "Принимаем уставку I_сз\r\n" +
                    "Проверим чувствительность к минимальному току КЗ (Кч>1,5 по ПУЭ):");
                AddFormula(doc, $"k_чувст=(I_(к.з.min(к3))*0,865)/(I_(c.з.)) = ({calc.SecondLastElementList?.IcsMin}*0,865)/{calc.IszMTZ} = {calc.KchuvMTZ}");

                AddParagraph(doc, "Проверка чувствительности МТЗ (для схемы D/Y)", isBold: true, fontSize: 14, isCenterAligned: true);
                AddParagraph(doc, "");
                AddFormula(doc, $"I_р=(K_сx·I_сз)/ntt = (1*{calc.IszMTZ})/{calc.RecloserNtt} = {calc.Ip}");
                AddParagraph(doc, "Где K_сx - коэффициент схемы принимаем 1, ntt коэффициент трансформации трансформатора тока.\r\n" +
                    "Определяем ток в реле при КЗ за трансформатором:");
                AddFormula(doc, $"I_р2=(0,5∙I_(к.з.max(к4))*1000)/ntt = (0,5∙{calc.LastElementList?.IcsMax}·1000)/{calc.RecloserNtt} = {calc.Ip2}");
                AddParagraph(doc, "Определяем коэффициент чувствительности при двухфазном КЗ за трансформатором по схеме неполная звезда:");
                AddFormula(doc, $"k_чувст=I_р2/I_р = {calc.Ip2}/{calc.Ip} = {calc.Kchuv2MTZ}");
                AddFormula(doc, "k_чувст > 1,5");
                AddParagraph(doc, "Рассчитываем ток однофазного К.З., за трансформатором:");
                AddFormula(doc, $"I_кз1 = U_ф/((1/3)*Z_тр) = 220/((1/3)*{calc.TransformatorFullResistance}) = {calc.Ikz1}");
                AddParagraph(doc, "Приведем ток однофазного КЗ на стороне 0,4 кВ к напряжению 10,5 кВ");
                AddFormula(doc, $"I_(кз1-10кВ)=I_кз1*(U_нн/Sub_voltage) = {calc.Ikz1}*(0.4/{calc.Voltage}) = {calc.Ikz1low}");
                AddParagraph(doc, "Определяем ток в реле при однофазном КЗ за трансформатором:");
                AddFormula(doc, $"I_р1=I_(кз1-10кВ)/(√(3) * ntt) = {calc.Ikz1low}/(√(3) * {calc.RecloserNtt}) = {calc.Ip1}");
                AddParagraph(doc, "Определяем коэффициент чувствительности при однофазном КЗ за трансформатором:");
                AddFormula(doc, $"k_чувст=I_р1/I_р = {calc.Ip1}/{calc.Ip} = {calc.Kchuv3MTZ}");

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Документы Word (*.docx)|*.docx";
                    saveFileDialog.Title = "Сохранить документ Word";
                    //saveFileDialog.Owner = owner;
                    owner?.Activate();
                    owner?.Focus();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Сохраняем документ
                        doc.SaveAs2(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                doc?.Close(false);
                wordApp?.Quit();
            }
        }

        private void AddParagraph(Document doc,
                                  string text,
                                  bool isBold = false,
                                  bool isCenterAligned = false,
                                  int fontSize = 10)
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
    }
}
