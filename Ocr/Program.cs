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
    class Program
    {
        static List<IOperation> op = new List<IOperation>();

        [STAThread]
        static void Main(string[] args)
        {
            op.Add(new ImageToText() { key = 1 });

        MENU:

            op.ForEach(f => f.Disable());

            Console.Clear();
            Console.WriteLine("OCR!");
            foreach (var o in op)
            {
                Console.WriteLine(o.Menu());
            }
            Console.WriteLine("exit to close");

            var elm = Console.ReadLine();
            while (elm != "exit" && elm != "\\")
            {
                foreach (var o in op)
                {
                    Console.WriteLine(o.Welcome(elm));
                }
                var oper = Console.ReadLine();
                if (oper == "exit")
                {
                    return;
                }
                if (oper == "\\")
                {
                    goto MENU;
                }
                Console.WriteLine(op.Where(x => x.enabled).FirstOrDefault().Operate(oper));
            }

            //Console.Read(
            /*
            var imgUrl = Clipboard.GetText();
            if (string.IsNullOrEmpty(imgUrl))
            {
                Console.WriteLine($"no url copied");
                Console.Read();
                return;
            }
            Console.WriteLine($"url: {imgUrl}");

            var testImagePath = Path.Combine(Path.GetTempPath(), "ocr.png");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(imgUrl), testImagePath);
            }

            try
            {
                var logger = new FormattedConsoleLogger();
                using (var engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (logger.Begin("Process image"))
                        {
                            var i = 1;
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                Clipboard.SetText(text);
                                logger.Log("Text: {0}", text);
                                logger.Log("Mean confidence: {0}", page.GetMeanConfidence());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            */
        }
    }
}
