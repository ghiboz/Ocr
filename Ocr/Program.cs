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
        [STAThread]
        static void Main(string[] args)
        {
            /*
            var testImagePath = $@"C:\Users\ghiboz\Documents\ShareX\Screenshots\2021-06\firefox_2021-06-24_09-25-17.png";
            if (args.Length > 0)
            {
                testImagePath = args[0];
            }
            */

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
                // OR 
                //client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
            }

            try
            {
                var logger = new FormattedConsoleLogger();
                var resultPrinter = new ResultPrinter(logger);
                using (var engine = new TesseractEngine(@"C:\Temp\Tesseract-OCR\tessdata", "eng", EngineMode.Default))
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
                //Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }

    class ResultPrinter
    {
        readonly FormattedConsoleLogger logger;

        public ResultPrinter(FormattedConsoleLogger logger)
        {
            this.logger = logger;
        }

        public void Print(ResultIterator iter)
        {
            logger.Log("Is beginning of block: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Block));
            logger.Log("Is beginning of para: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Para));
            logger.Log("Is beginning of text line: {0}", iter.IsAtBeginningOf(PageIteratorLevel.TextLine));
            logger.Log("Is beginning of word: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Word));
            logger.Log("Is beginning of symbol: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Symbol));

            logger.Log("Block text: \"{0}\"", iter.GetText(PageIteratorLevel.Block));
            logger.Log("Para text: \"{0}\"", iter.GetText(PageIteratorLevel.Para));
            logger.Log("TextLine text: \"{0}\"", iter.GetText(PageIteratorLevel.TextLine));
            logger.Log("Word text: \"{0}\"", iter.GetText(PageIteratorLevel.Word));
            logger.Log("Symbol text: \"{0}\"", iter.GetText(PageIteratorLevel.Symbol));
        }
    }
}
