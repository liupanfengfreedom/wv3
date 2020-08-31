using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace httplisten
{
    class Program
    {
        static readonly Int32 CHUNCK = 1024;
        static Byte[] array;
        static Byte[] array1;
        public static Config config = new Config();
        static void Main(string[] args)
        {
            int int1 = 1024 * 1024 * 1024 ;
           // string[] prefixes = { config.configinfor.ipaddress,string.Format("http://+:{0}/", config.configinfor.tccipport)};
            string[] prefixes = {string.Format("http://+:{0}/", config.configinfor.tccipport) };
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8000/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    new HttpListenerContextClass(context);
                }
                catch
                {
                    listener.Stop();
                }

            }

           // SimpleListenerExample(prefixes);
        }
        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8000/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            System.IO.Stream input =  request.InputStream;
            Int64 int64num = 1024l * 1024l * 1024l*1024l;
            Int64 contentlen = request.ContentLength64;
            array = new Byte[contentlen];
            // array1 = new Byte[request.ContentLength64];

            //input.Seek(1024,System.IO.SeekOrigin.Begin);
            //int counter = 0;
            //do {
            //    int bytestoread = contentlen > CHUNCK ? CHUNCK : (int)contentlen;
            //    input.Read(array, CHUNCK * counter, bytestoread);
            //    contentlen -= CHUNCK;
            //    counter++;
            //} while (contentlen>0);
            input.Read(array, 0, (int)contentlen);
            String pathfile = "G:\\x1.exe";
            File.WriteAllBytes(pathfile, array);
            string utfString = Encoding.UTF8.GetString(array, 0, array.Length);
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // You must close the output stream.
            output.Close();
            listener.Stop();
        }
    }
}
