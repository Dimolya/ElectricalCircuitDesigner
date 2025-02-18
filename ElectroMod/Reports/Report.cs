using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.Reports
{
    public abstract class Report
    {
        public abstract void GenerateReport(Document doc, CenterCalculation calc);

        protected void AddFormula(Document doc, string formula)
        {
            Paragraph formulaParagraph = doc.Content.Paragraphs.Add();
            formulaParagraph.Range.Text = formula;

            // Форматируем текст как OMML (формула)
            OMaths maths = formulaParagraph.Range.OMaths;
            maths.Add(formulaParagraph.Range);
            maths[1].BuildUp();

            formulaParagraph.Range.InsertParagraphAfter();
        }

        protected void AddParagraph(Document doc, string text, bool isBold = false, bool isCenterAligned = false, int fontSize = 10)
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
    }
}