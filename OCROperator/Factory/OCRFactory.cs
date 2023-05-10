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

        internal List<Bitmap> ConvertPDFToBitmap(byte[] PDFByte)
        {
            List<Bitmap> Result = new List<Bitmap>();

            using (MemoryStream ms = new MemoryStream(PDFByte))
            {
                using (PdfReader pdfReader = new PdfReader(ms))
                {
                    Logger.LogInformation($"File read {pdfReader}");

                    using (PdfDocument MainDocument = new PdfDocument(pdfReader))
                    {
                        int AllPages = MainDocument.GetNumberOfPages();
                        int Count = 1;
                        PdfToImageConverter Converter = new PdfToImageConverter();
                        while (Count <= AllPages)
                        {
                            PdfPage SinglePage = MainDocument.GetPage(Count);
                            Bitmap BitMap = Converter.ConvertToBitmap(SinglePage);

                            Result.Add(BitMap);

                            Logger.LogInformation($"Processed {Count} from {AllPages}");
                            Count++;
                        }
                    }
                }
            }



            return Result;
        }
        internal List<byte[]> ConvertBitmapListToByte(List<Bitmap> Pages)
        {
            List<byte[]> Result = new List<byte[]>();

            foreach (Bitmap Page in Pages)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Page.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    Result.Add(ms.ToArray());
                }
            }
            return Result;
        }

        internal string GetTextFromPDF(byte[] PDFContent)
        {
            StringBuilder Content = new StringBuilder();
            List<Bitmap> AllPages = ConvertPDFToBitmap(PDFContent);
            foreach (Bitmap Page in AllPages)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Page.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    using (Pix picture = Pix.LoadFromMemory(ms.ToArray()))
                    {
                        using (Page TPage = _engine.Process(picture))
                        {
                            Content.Append(TPage.GetText());
                        }
                    }
                }
            }
            return Content.ToString();
        }
    }
}
