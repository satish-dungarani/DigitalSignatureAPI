using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DIGISIGN_API.Models;
using Syncfusion.Pdf;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace DIGISIGN_API.Controllers
{
    public class DIGISIGNController : ApiController
    {
        // POST api/DIGISIGN
        [HttpPost]
        public ResponseModel Post(InfoModel model)
        {
            try
            {
                //Register Syncfusion license
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(model.licenseKey);

                //Commanc Base Path to manage files
                string path = ConfigurationManager.AppSettings["Path"].ToString();

                //Convert Base64 To Byte
                byte[] byt = Convert.FromBase64String(model.encodedstring);
                System.IO.File.WriteAllBytes(path + "Invoice.pdf", byt);

                //Load existing PDF document.
                PdfLoadedDocument document = new PdfLoadedDocument(path + "Invoice.pdf");
                //Get Graphics object for put digital signature
                PdfGraphics graphics = document.Pages[0].Graphics;
                //Load digital ID with password.
                PdfCertificate certificate = new PdfCertificate(new X509Certificate2(path + "Test DS.pfx", "123"));

                //Create a signature with loaded digital ID.
                PdfSignature signature = new PdfSignature(document, document.Pages[0], certificate, "DigitalSignature");

                #region For Add Image as Signature
                //Set bounds to the signature.
                //signature.Bounds = new System.Drawing.RectangleF(0, 0, 350, 100);

                //Load image from file.
                //PdfImage image = PdfImage.FromFile(@"C:\Darshan\PDF Digital Signature\DIGISIGN\DIGISIGN_API\Files\imgtick.png");
                ////Create a font to draw text.
                //PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);

                ////Drawing text, shape, and image into the signature appearance.
                //signature.Appearance.Normal.Graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, new System.Drawing.RectangleF(50, 0, 300, 100));
                //signature.Appearance.Normal.Graphics.DrawImage(image, 0, 0, 350, 100);
                //signature.Appearance.Normal.Graphics.DrawString("Digitally Signed by Syncfusion", font, PdfBrushes.Black, 120, 17);
                //signature.Appearance.Normal.Graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 120, 39);
                //signature.Appearance.Normal.Graphics.DrawString("Location: USA", font, PdfBrushes.Black, 120, 60);
                #endregion 

                //350, 700, 120, 30
                int x = Convert.ToInt32(model.cordinatex1);
                int y = Convert.ToInt32(model.cordinatey1); 
                int width = Convert.ToInt32(model.cordinatex2);
                int height = Convert.ToInt32(model.cordinatey2);
                signature.Bounds = new RectangleF(x,y,width,height);
                //Draws the signature 
                PdfPen pen = new PdfPen(Color.Black);
                graphics.DrawRectangle(pen, PdfBrushes.White, signature.Bounds);
                PdfFont font = new PdfTrueTypeFont(new Font("Sans Serif", 7));
                graphics.DrawString("Digitally Signed By\n" + certificate.SubjectName + "\nDate:" + System.DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss") + " IST", font, PdfBrushes.Black, (width + 10), (height + 4));

                //Save the PDF document.
                document.Save(path + "Invoice_WithSign.pdf");

                //Close the document.
                document.Close(true);

                //Convert Output PDF into Base64String
                byte[] bytes = System.IO.File.ReadAllBytes(path + "Invoice_WithSign.pdf");
                string base64 = Convert.ToBase64String(bytes);

                return new ResponseModel()
                {
                    Status = "S",
                    StatusCode = 200,
                    Message = "Documnet Signed Successfully.",
                    Data = base64
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = "E",
                    StatusCode = 500,
                    Message = "Documnet Signed fail.",
                    Data = ex.Message
                };
            }
        }

    }
}