using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Timers;
namespace httplisten
{
    class clientinfolist
    {
        public List<clientinfo> clientinfos;
        public int counter = 0;
        public clientinfolist()
        {
            clientinfos = new List<clientinfo>();
        }

    }
    class clientinfo {

public static Dictionary<IPAddress, clientinfolist> clientinfomap = new Dictionary<IPAddress, clientinfolist>();
        public string remoteclientlocalip;
        public string remoteclientlocalport;
    }
    class HttpListenerContextClass
    {
        HttpListenerContext mhttplistenercontext;
        Thread handleThread;
        public HttpListenerContextClass(HttpListenerContext p)
        {
            mhttplistenercontext = p;
            handleThread = new Thread(new ThreadStart(handlethreadfunc));
            handleThread.IsBackground = true;
            handleThread.Start();
        }
        ~HttpListenerContextClass()
        {
            Console.WriteLine("HttpListenerContextClass deconstruct");
        }
        void handlethreadfunc()
        {
            HttpListenerRequest request = mhttplistenercontext.Request;
            IPEndPoint remotendpoint = request.RemoteEndPoint;
            IPAddress Remotipaddress = remotendpoint.Address;
            System.Collections.Specialized.NameValueCollection header = request.Headers;
            string[] headerallkeys = header.AllKeys;

            string key = "";
            string exit = "";
            string content = "";
            foreach (var a in headerallkeys)
            {
                if (a == "UserName")
                {
                    key = header.GetValues(a)[0];
                }
                else if (a == "Password")
                {
                    exit = header.GetValues(a)[0];
                }
                else if (a == "Palyload")
                {
                    content = header.GetValues(a)[0];
                }

                string[] values = header.GetValues(a);
            }
            if (request.HttpMethod == "POST")
            {

                 Console.WriteLine("key : "+ key);
                 Console.WriteLine("content : " + content);
                if (key == "843A58C72161787A98C23CD33AAE66F9")
                {
                    if (exit == "exit")
                    {
                        clientinfo.clientinfomap.Remove(Remotipaddress);
                        Console.WriteLine("exit");
                    }
                    else
                    {
                        string[] info = content.Split('?');

                        bool b = clientinfo.clientinfomap.ContainsKey(Remotipaddress);
                        if (!b)
                        {
                            clientinfolist cilist = new clientinfolist();
                            clientinfo.clientinfomap.Add(Remotipaddress, cilist);
                            clientinfo ci = new clientinfo();
                            ci.remoteclientlocalip = info[0];
                            ci.remoteclientlocalport = info[1];
                            cilist.clientinfos.Clear();
                            cilist.clientinfos.Add(ci);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("pcip wan : " + Remotipaddress);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("local port : " + info[1]);
                        }
                        else
                        {
                            clientinfolist cilist = clientinfo.clientinfomap[Remotipaddress];
                            clientinfo ci = new clientinfo();
                            ci.remoteclientlocalip = info[0];
                            ci.remoteclientlocalport = info[1];
                            cilist.clientinfos.Clear();
                            cilist.clientinfos.Add(ci);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("pcip wan : " + Remotipaddress);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("local port : " + info[1]);
                        }

                    }
                }
            }
            else
            {
                if (key == "843A58C72161787A98C23CD33AAE66F9")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("iphoneip wan : " + Remotipaddress);
                    Console.ForegroundColor = ConsoleColor.White;

                    bool b = clientinfo.clientinfomap.ContainsKey(Remotipaddress);
                    Console.WriteLine("clientinfomap.ContainsKey b:  " + b); 
                    if (b)
                    {
                        
                        HttpListenerResponse response = mhttplistenercontext.Response;
                        clientinfolist cilist = clientinfo.clientinfomap[Remotipaddress];
                       // string responseString = "843A58C72161787A98C23CD33AAE66F9?" + cilist.clientinfos[cilist.counter].remoteclientlocalip + "?" + cilist.clientinfos[cilist.counter].remoteclientlocalport;
                        string responseString = "843A58C72161787A98C23CD33AAE66F9?" + cilist.clientinfos[0].remoteclientlocalip + "?" + cilist.clientinfos[0].remoteclientlocalport;
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                        if (cilist.counter < cilist.clientinfos.Count-1)
                        {
                            cilist.counter++;
                        }
                    }
                    else
                    {
                        HttpListenerResponse response = mhttplistenercontext.Response;
                        string responseString = "notready";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }


                }
            }




            System.IO.Stream input = request.InputStream;
            byte[] array = new byte[request.ContentLength64];
            input.Read(array, 0, (int)request.ContentLength64);//larg file may encounter error
            
            string utfString = Encoding.UTF8.GetString(array, 0, array.Length);
            // Construct a response.
            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            //responseString += utfString;
            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //// Get a response stream and write the response to it.
            //response.ContentLength64 = buffer.Length;
            //System.IO.Stream output = response.OutputStream;
           // output.Write(buffer, 0, buffer.Length);
            Thread.Sleep(30);
            input.Close();
        }
    }
}
