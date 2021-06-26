using Ghiboz.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Ocr
{
    public class ImageToText : IOperation
    {
        TesseractEngine engine;

        public ImageToText()
        {
            menuTitle = "img url => text";
            welcomeMessage = @"
read from clipboard";

            // constructor
            engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default);
        }

        public override string Operate(string input)
        {
            var ret = "";

            var imgUrl = Clipboard.GetText();
            if (string.IsNullOrEmpty(imgUrl))
            {
                ret = $"no url copied";
                return ret;
            }
            Console.WriteLine($"url: {imgUrl}");

            var testImagePath = Path.Combine(Path.GetTempPath(), "ocr.png");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(imgUrl), testImagePath);
            }

            try
            {
                using (var img = Pix.LoadFromFile(testImagePath))
                {
                    ret += "Process image\r\n";
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Clipboard.SetText(text);
                        ret += $"Text: {text}\r\n";
                        ret += $"Mean confidence: {page.GetMeanConfidence()} \r\n\n";
                    }
                }
            }
            catch (Exception e)
            {
                ret += $"Unexpected Error: {e.Message}\r\n";
                ret += $"Details:\r\n";
                ret += $"{e}";
            }
            return ret;
        }
    }
}
