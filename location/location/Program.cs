using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace location
{
    public class location
    {
        /// <summary>
        /// Main method: Entry point of client, creates new clientWindow
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null)
            {

                argParser(args);
            }
            else
            {
                //setting up of clientGUI window
                clientGUI gUI = new clientGUI();
                gUI.Title = "Client";
                gUI.Width = 400;
                gUI.Height = 400;
                gUI.ShowDialog();
            }
        }

        /// <summary>
        /// Parser for arguments passed into the program,
        /// deteremines which inputs have been entered,
        /// initialises streamers, applys connections and flushes data,
        /// also determines which reply parser is required based on input
        /// </summary>
        /// <param name="args"></param>
        public static void argParser(string[] args)
        {
            #region(Region - Local Varibles)
            string username = "";
            string locale = "";
            string protocol = "";
            string ipAddress = "127.0.0.1";//"whois.net.dcs.hull.ac.uk";
            int port = 43;
            string request = "";
            string[] optionalHeader = new string[] { };
            int timeoutCount = 1000;
            bool DebugMode = false;
            #endregion

            #region(Region - Argument Debugging)
            /*if (args != null)
            {
                foreach (string arg in args)
                {
                    Console.WriteLine("This is arg: " + arg);
                }
            }
            else
            {
                Console.WriteLine("ERROR");
            }*/
            #endregion
            //argParser main Try/Catch
            try
            {
                #region(Region - Input Parsing)
                //for each element in the args array
                for (int i = 0; i < args.Length; i++)
                {
                    //if args contains -h, the next element is the requested IP address 
                    if (args[i] == "-h")
                    {
                        try
                        {
                            ipAddress = args[i+1];
                        }
                        //if no ip address was given. ERROR
                        catch
                        {
                            Console.WriteLine("ERROR: No IP Address Given");
                        }
                    }
                    //if args contains -h9 or -h0 or -h1, the element becomes the request protocol to use
                    else if (args[i] == "-h9" || args[i] == "-h0" || args[i] == "-h1")
                    {
                        try
                        {
                            protocol = args[i];
                        }
                        //if something goes wrong in identifying command
                        catch
                        {
                            Console.WriteLine("ERROR: Incorrect Parsing of Protocol");
                        }
                    }
                    //if args contains -p, the next element is the requested port number 
                    else if (args[i] == "-p")
                    {
                        try
                        {
                            port = int.Parse(args[i+1]);
                        }
                        //if no port number was given. ERROR
                        catch
                        {
                            Console.WriteLine("ERROR: No Port Number Was Given");
                        }
                    }
                    //if args contains -h, the next element is the requested timeout amount 
                    else if (args[i] == "-t")
                    {
                        try
                        {
                            timeoutCount = int.Parse(args[i + 1]);
                        }
                        //if no timeout amount was given. ERROR
                        catch
                        {
                            Console.WriteLine("ERROR: No Timeout Value Was Given");
                        }
                    }
                    //if args contains -d, tdebug mode is enabled 
                    else if (args[i] == "-d")
                    {
                        try
                        {
                            DebugMode = true;
                        }
                        //if something goes wrong in identifying command
                        catch
                        {
                            Console.WriteLine("ERROR: Debugger Failed");
                        }
                    }
                    //if args Array is more than 0, and the element doesnt contain the commands (-h, -p, -t, -d) remaining must be username and location
                    //in that order
                    else if (i > 0 && args[i-1] != "-h" && args[i-1] != "-p" && args[i - 1] != "-t" && args[i - 1] != "-d")
                    {
                        try
                        {
                            if (username == "")
                            {
                                username = args[i];
                            }
                            else if (locale == "")
                            {
                                locale = args[i];
                            }
                        }
                        ////if something goes wrong in identifying username and location (if entered incorrectly)
                        catch
                        {
                            Console.WriteLine("ERROR: Username / Locale - Try/Catch");
                        }
                    }
                    //else if there is only one element in the args [0], this must be the username
                    else if (i == 0)
                    {
                        username = args[i];
                    }
                }
                #endregion

                #region(Region - Determining which requestBuilder)
                //'if' statement HERE - Determines if {location is not empty} or {else}
                if (locale != "")
                {
                    //request has location
                    request = requestBuilder(username, locale, protocol, port, optionalHeader, ipAddress, DebugMode);
                }
                else
                {
                    //request dosent have location
                    request = requestBuilder(username, protocol, port, ipAddress, DebugMode);
                }
                #endregion

                #region(Region - Connections, Readers and Flush)
                //creates new TCPclient
                TcpClient client = new TcpClient();
                //connects client on 'ipaddress' through 'port'
                client.Connect(ipAddress, port);                         
                //creates the stream readers and writers
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());

                //displays request and flushes the data
                Console.WriteLine("Request is: " + request);
                sw.WriteLine(request);
                sw.Flush();
                //reads reply for server
                string serverReply = sr.ReadLine();

                //client Timeout creation and setting of time
                client.ReceiveTimeout = timeoutCount;
                client.SendTimeout = timeoutCount;
                #endregion

                #region(Region - serverReply Parser Check)
                //does servers reply contains "OK"?
                bool sReply = serverReply.Contains("OK");

                //if server reply doesnt contain "OK"
                if (sReply != true)
                {
                    replyParser(serverReply, username, protocol, port, optionalHeader, ipAddress);
                    //in debug mode indicates parser
                    if (DebugMode) { Console.WriteLine("One"); }
                }
                //if server reply does contain "OK"
                if (serverReply.Contains("OK"))
                {
                    replyParser(serverReply, username, locale, protocol, port, optionalHeader, ipAddress);
                    //in debug mode indicates parser
                    if (DebugMode) { Console.WriteLine("Two"); }
                }
                #endregion
                if (DebugMode)
                {
                    Console.WriteLine(serverReply);
                    Console.WriteLine(sr.ReadLine());
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("ERROR: Exception Error Caught. In Main Method - Try/Catch. " + e.Message + " " + e.StackTrace);
            }
        }

        /// <summary>
        /// Request builder for request that are wanting the users location
        /// </summary>
        /// <param name="username"></param>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        static string requestBuilder(string username, string protocol, int port, string ipAddress, bool DebugMode)
        {
            //Non-New Location Builder
            //creates the request uses standardised method in assignement brief
            // i.e. GET , etc.
            string request = "";
            try
            {
                //Determines which (if any) protocol has been inputted
                if (protocol == "-h9")
                {
                    //HTTP/0.9 request
                    request = "GET" + " " + "/" + username + "\r\n";
                    if (DebugMode) { Console.WriteLine(request); }
                }
                else if (protocol == "-h0")
                {
                    //HTTP/1.0 request
                    request = "GET" + " " + "/?" + username + " " + "HTTP/1.0" + "\r\n";
                    if (DebugMode) { Console.WriteLine(request); }
                }
                else if (protocol == "-h1")
                {
                    //HTTP/1.1 request
                    request = "GET" + " " + "/?name=" + username + " " + "HTTP/1.1" + "\r\n" + "Host:" + " " + ipAddress + "\r\n";
                    if (DebugMode) { Console.WriteLine(request); }
                }
                else
                {
                    //WhoIs request
                    request = username;
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Something went wrong in the first 'RequestBuilder'");
            }
            return request;
        }
        
        /// <summary>
        /// Request builder for request that are wanting to set the users location
        /// </summary>
        /// <param name="username"></param>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        static string requestBuilder(string username, string locale, string protocol, int port, string[] optionalHeader, string ipAddress, bool DebugMode)
        {
            //New Location Builder
            //creates the request uses standardised method in assignement brief
            // i.e. POST , PUT , etc.
            string request = "";
            try
            {
                //Determines which (if any) protocol has been inputted
                if (protocol == "-h9")
                {
                    //HTTP/0.9 request
                    request = "PUT" + " " + "/" + username + "\r\n" + "\r\n" + locale + "\r\n";
                }
                else if (protocol == "-h0") //The <length> is the count of the number of characters in the <location>. In HTTP/1.0
                {
                    //HTTP/1.0 request
                    //determines contents length
                    int Contentlength = locale.Length;
                    //builds request
                    request = "POST" + " " + "/" + username + " " + "HTTP/1.0" + "\r\n" + "Content-Length:" + " " + Contentlength + "\r\n";
                    foreach (string header in optionalHeader)
                    {
                        //creates the optional headers
                        request = request + header + "\r\n";
                    }

                    request = request + "\r\n" + locale;
                }
                else if (protocol == "-h1") //The <length> is the count of the number of characters in the “name=<name>&location=<location>” string. In HTTP/1.1
                {
                    //HTTP/1.0 request
                    //determines contents length
                    int Contentlength = ("name=" + username + "&location=" + locale).Length;
                    //builds request
                    request = "POST" + " " + "/" + " " + "HTTP/1.1" + "\r\n" + "Host:" + " " + ipAddress + "\r\n" + "Content-Length:" + " " + Contentlength + "\r\n";
                    foreach (string header in optionalHeader)
                    {
                        //creates the optional headers
                        request = request + header + "\r\n";
                    }

                    request = request + "\r\n" + "name=" + username + "&location=" + locale;
                }
                else
                {
                    //WhoIs request
                    request = username + " " + locale;
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Something went wrong in the first 'RequestBuilder w/ Locale");
            }
            //returns request built to flush()
            return request;
        }

        /// <summary>
        ///reply parser for requests that request the users location 
        /// </summary>
        /// <param name="serverReply"></param>
        /// <param name="username"></param>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        /// <param name="optionalHeader"></param>
        /// <param name="ipAddress"></param>
        static void replyParser(string serverReply, string username, string protocol, int port, string[] optionalHeader, string ipAddress)
        {
            //server sends the location of the user as a reply
            Console.WriteLine(username + " is " + serverReply);
            MessageBox.Show(username + " is " + serverReply);
        }

        /// <summary>
        /// reply parser for requests that request the users location to be changed
        /// if locale changed respone is 'OK', Decode.
        /// </summary>
        /// <param name="serverReply"></param>
        /// <param name="username"></param>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        /// <param name="optionalHeader"></param>
        /// <param name="ipAddress"></param>
        static void replyParser(string serverReply, string username, string locale, string protocol, int port, string[] optionalHeader, string ipAddress)
        {
            //sends OK as a respone to indicate that the location has been changed
            //if response is OK, username + .Replace(\r\n) with new locale.
            Console.WriteLine(username + " location changed to be " + locale);
            MessageBox.Show(username + " location changed to be " + locale);
        }
    }
}
