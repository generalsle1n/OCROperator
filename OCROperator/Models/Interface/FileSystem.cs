using OCROperator.Factory;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tesseract;

namespace OCROperator.Models.Interface
{
    internal class FileSystem : IWatcher
    {
        public string Destination { get; set; }
        public string SuffixMetadata { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public MailFactory MailFactory { get; set; }
        public List<Task> AllItems { get; set; } = new List<Task>();
        public void Setup()
        {
            string AllPath = Path.Combine(Directory.GetCurrentDirectory(), "tesseractData");
            MailFactory = new MailFactory
            {
                SMTPServer = "smtp.wehrle-werk.internal",
                FromMail = "ocr@wehrle-werk.internal",
                Port = 25
            };
        }
        public async Task Execute()
        {
            string[] AllFiles = Directory.GetFiles(Destination, SuffixMetadata);

            foreach(string SingleItem in AllFiles)
            {
                

                //string pdfPath = Path.Combine(Item.Fields[0].Value, $"{Item.Fields[1].Value}.pdf");

                //PdfDocument OrginalPDF = PdfDocument.Load(pdfPath);
                ////File.Delete(pdfPath);
                
                //StringBuilder Content = new StringBuilder();
                //foreach (PdfPage page in OrginalPDF.Pages)
                //{
                //    //PdfBitmap Image = new PdfBitmap((int)page.Width*2, (int)page.Height*2, false);
                //    //page.Render(Image, 0, 0, (int)page.Width*2, (int)page.Height*2, PageRotate.Normal, RenderFlags.FPDF_LCD_TEXT);
                //    //string imagePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                //    //Image.Image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                //    //Pix pixelContent = Pix.LoadFromFile(imagePath);
                //    ////File.Delete(imagePath);
                //    //Page TPage = _engine.Process(pixelContent);
                //    //Content.Append(TPage.GetText());
                //    //var a = new MailAddressCollection();
                //    //a.Add(new MailAddress(Item.User.Email));
                //    //var mail = new MailMessage
                //    //{
                //    //    From = new MailAddress("ocr@wehrle-werk.internal"),
                //    //    Body = Content.ToString(),
                //    //    Subject = "OCR"
                //    //};
                //    //mail.To.Add(new MailAddress(Item.User.Email));
                //    //_smtp.Send(mail);
                //}
            }
        }

        public async Task ProcessSingleItem(string Item)
        {
            string MetadataContent = File.ReadAllText(Item);
            File.Delete(Item);

            PapercutItem SingleItem = JsonSerializer.Deserialize<PapercutItem>(MetadataContent) ?? new PapercutItem();
        }
    }
}
