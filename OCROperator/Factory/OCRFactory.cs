using itext.pdfimage;
using iText.Kernel.Pdf;
using System.Drawing;
using System.Text;
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

        internal string GetTextFromPDF(string PDFPath, bool DetelePDF = false)
        {
            StringBuilder Content = new StringBuilder();
            using (PdfReader pdfReader = new PdfReader(PDFPath))
            {
                Logger.LogInformation($"File read {PDFPath}");
                
                using(PdfDocument MainDocument = new PdfDocument(pdfReader))
            {
                    int AllPages = MainDocument.GetNumberOfPages();
                int Count = 1;
                    PdfToImageConverter Converter = new PdfToImageConverter();
                    while(Count <= AllPages)
                {
                        PdfPage SinglePage = MainDocument.GetPage(Count);
                        Bitmap BitMap = Converter.ConvertToBitmap(SinglePage);

                        using(MemoryStream ms = new MemoryStream())
                    {
                            BitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            using (Pix picture = Pix.LoadFromMemory(ms.ToArray()))
                        {
                                using(Page TPage = _engine.Process(picture))
                                {
                                    Content.Append(TPage.GetText());
                                }
                            }
                        }

                        Logger.LogInformation($"Processed {Count} from {AllPages}");
                        Count++;
                    }
                }
                }
            if (DetelePDF)
            {
                File.Delete(PDFPath);
                Logger.LogInformation($"{PDFPath} deleted");
            }
            
            return Content.ToString();
        }
    }
}
