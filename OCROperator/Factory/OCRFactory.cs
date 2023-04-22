using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCROperator.Factory
{
    internal class OCRFactory
    {
        internal string TesseractDataPath { get; init; }
        internal string Language { get; init; }
        internal ILogger Logger { get; init; }
        private TesseractEngine _engine;
        internal void Setup()
        {
            _engine = new TesseractEngine(TesseractDataPath, Language);
        }

        internal string GetTextFromPDF(string PDFPath)
        {
            StringBuilder Content = new StringBuilder();
            byte[] pdfContent = File.ReadAllBytes(PDFPath);
            File.Delete(PDFPath);
            Logger.LogInformation($"File {PDFPath} read and deleted");

            using(PdfDocument pdf = PdfDocument.Load(pdfContent))
            {
                int Count = 1;
                foreach(PdfPage page in pdf.Pages)
                {
                    //string tempFileName = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                    using(PdfBitmap Image = new PdfBitmap((int)page.Width * 2, (int)page.Height * 2, false))
                    {
                        page.Render(Image, 0, 0, (int)page.Width * 2, (int)page.Height * 2, PageRotate.Normal, RenderFlags.FPDF_LCD_TEXT);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            using(Pix picture = Pix.LoadFromMemory(ms.ToArray()))
                            {
                                using(Page TPage = _engine.Process(picture))
                                {
                                    Content.Append(TPage.GetText());
                                }
                            }
                        }
                    }
                    Logger.LogInformation($"Process {Count} from {pdf.Pages.Count}");
                    Count++;
                }
            }
            return Content.ToString();
        }
    }
}
