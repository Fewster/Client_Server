using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows;

namespace locationserver
{
    /// <summary>
    /// Class responsible for the server
    /// </summary>
    class locationserver
    {
        // Initialises a Dictionary for Users (Key = Username, Value = Location)
        static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();
        public static bool debugMode = false;
        public static int timeoutCount = 1000;
        /// <summary>
        /// Main() entry point of the program, responsible for set-up of threads, listener and creating an instance of runServer .
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {

            for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-d" || args[i] == "-t" || args[i] == "-w")
                {
                    if(args[i] == "-d")
                    {
                        debugMode = true;
                    }

                    else if (args[i] == "-t")
                    {
                        timeoutCount = int.Parse(args[i + 1]);
                    }

                    else if (args[i] == "-w")
                    {
                        serverGUI serverGUI = new serverGUI();
                        serverGUI.Title = "Server";
                        serverGUI.Width = 300;
                        serverGUI.Height = 300;
                        serverGUI.Show();
                        Thread.Sleep(500);
                        serverGUI.Close();
                    }
                }
            }
            
            //Listens for connections from TCP network clients
            TcpListener listener;
            Socket connection;
            //Creates a Listener for listening on port 43 from 'Any' IPAddress. Then Begins Listener with '.Start()'.
            listener = new TcpListener(IPAddress.Any, 43);
            listener.Start();
            Console.WriteLine("Server is Listening...");
   
            //Continuos Loop
            for (;;)
            {
                try
                {
                    //if there is a pending requet on the listener
                    if (listener.Pending())
                    {
                        //Accepts sockets, instantiates runServer and creates new threads
                        connection = listener.AcceptSocket();
                        runServer server = new runServer(connection, users);
                        Thread thread = new Thread(new ThreadStart(server.handleReq));
                        thread.Start();
                    }
                }
                catch
                {
                    Console.WriteLine("ERROR: Problem with multi-threading");
                }
            }
        }
    }
    /// <summary>
    /// runServer() method creates and handles all listening, network streams and sockets. Then sets-up the server to
    /// be ready to recieve data.
    /// </summary>
    class runServer// Running of the Server Method
    {
        static ConcurrentDictionary<string, string> users;
        //Implements a socket interface
        Socket connection;
        //Provides a stream of data for network access
        NetworkStream socketStream;

        public runServer(Socket connection, ConcurrentDictionary<string,string> _users)
        {
            this.connection = connection;
            users = _users;
        }

        public void handleReq()
        {
            socketStream = new NetworkStream(connection);

            doRequest(socketStream);
            //Closes the data stream and the connection to the client
            socketStream.Close();
            connection.Close();
        }
        
        /// <summary>
        /// doRequest(NetworkStream socketStream) responsible for reading in of the data stream, 
        /// setting a Timeout and determining wheather the data is a HTTP or Whois format.
        /// Prepares the data for parsing.
        /// </summary>
        /// <param name="socketStream"></param>
        public static void doRequest(NetworkStream socketStream)
        {
            //Sets Timeout count
            socketStream.ReadTimeout = 1000;
            socketStream.WriteTimeout = 1000;

            //Initialises the StreamWriter and Reader
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);
            string dataIn = ""; //Empty string for the streamed data to be held.

            try //Try/Catch for Reading of the data inputted.
            {
                Thread.Sleep(4);
                //'Peeks' to find next avaiable character if there is reads the line.
                if (sr.Peek() > 0)
                {
                    dataIn = sr.ReadLine();
                }
                //while there is a next available character, the loop will continue to read lines. (Allows for \r\n to be read past)
                while (sr.Peek() > 0)
                {
                    dataIn = (dataIn + "\r\n") + sr.ReadLine();
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Exception Error Caught. doRequest Method 1st Try/Catch....\r\n  ");
            }
            //Console.WriteLine(dataIn); //Outputs the data stream held in dataIn (Used for Debugging).

            try //Try/Catch for determining the format of the request, HTTP or Whois.
            {
                //Attempts to find accepted string within the data streamed in, if the data does contain vaild string its in HTTP format.
                if (dataIn.Contains("GET /") || dataIn.Contains("PUT /") || dataIn.Contains("POST /"))
                {
                    //if data contains valid string its format is HTTP 1.0 or HTTP 1.1 else its HTTP 0.9.
                    if (dataIn.Contains("HTTP/1.0"))
                    {
                        Console.WriteLine("httpProtocol == -h0");
                        //Calls httpProtocol method and passes the protocol command to aid in parsing.
                        httpProtocol(sw, dataIn, "-h0");
                    }
                    else if (dataIn.Contains("HTTP/1.1"))
                    {
                        Console.WriteLine("httpProtocol == -h1");
                        httpProtocol(sw, dataIn, "-h1");
                    }
                    else
                    {
                        Console.WriteLine("httpProtocol == -h9");
                        httpProtocol(sw, dataIn, "-h9");
                    }
                }
                //Else if no valid string found, data is in a Whois format.
                else
                {
                    whoIsProtocol(sw, dataIn); //Calls whoIsProtocol method
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("ERROR: Exception Error Caught. doRequest Method....\r\n  " + e.StackTrace + "\r\n" + e.Message);
            }
        }

        /// <summary>
        /// httpProtocol responsible for the parsing of the data stream that is in a HTTP format, providing relevant output as a result.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="dataIn"></param>
        /// <param name="protocol"></param>
        public static void httpProtocol(StreamWriter sw, string dataIn, string protocol)
        {
            //Splits the data by \r\n or on new lines, creaing an array.
            string[] splitData = dataIn.Split(new[] { "\r\n" }, StringSplitOptions.None);

            //Empty string used in parsing.
            string sReply = "";
            string username = "";
            string locale = "";
            string optionalHeaders = "";

            try //Try/Catch for HTTP parsing fails if no vaild data is found in the splitData array.
            {
                //If first array entry contains the string (GET /)
                //User is request the current location of the this user.
                if (splitData[0].Contains("GET /"))
                {
                    // If the protocol variable passed from doRequest == -h9, parse data accordingly.
                    if (protocol == "-h9")
                    {
                        //Replaces the prefix (GET /) to be an empty space, removing it. String Remaining is the username.
                        username = splitData[0].Replace("GET /", "");

                        //if the dictionary contains a user with the same username.
                        if (users.ContainsKey(username))
                        {
                            //users locale in dictionary is located and outputted.
                            locale = users[username];
                            sReply = "HTTP/0.9" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + "\r\n" + locale + "\r\n";
                        }
                        //if user isnt found in dictionary.
                        else
                        {
                            sReply = "HTTP/0.9" + " " + "404" + " " + "Not" + " " + "Found" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                        }
                    }
                    // If the protocol variable passed from doRequest == -h0, parse data accordingly.
                    else if (protocol == "-h0")
                    {
                        //provides a string for the optionalheaders
                        optionalHeaders = splitData[1];

                        //Replaces the prefix (GET /) and suffex (HTTP/1.0) to be an empty spaces, removing them. String Remaining is the username.
                        string temp = splitData[0].Replace("GET /?", "");
                        username = temp.Replace(" HTTP/1.0", "");

                        //if the dictionary contains a user with the same username.
                        if (users.ContainsKey(username))
                        {
                            //users locale in dictionary is located and outputted.
                            locale = users[username];
                            sReply = "HTTP/1.0" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + "\r\n" + locale + "\r\n";
                        }
                        //if user isnt found in dictionary.
                        else
                        {
                            sReply = "HTTP/1.0" + " " + "404" + " " + "Not" + " " + "Found" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                        }
                    }
                    // If the protocol variable passed from doRequest == -h1, parse data accorddingly.
                    else if (protocol == "-h1")
                    {
                        //provides a string for the optionalheaders
                        optionalHeaders = splitData[2];

                        //Replaces the prefix (GET /?name=) and suffex (HTTP/1.1) to be an empty spaces, removing them. String Remaining is the username.
                        string temp = splitData[0].Replace("GET /?name=", "");
                        username = temp.Replace(" HTTP/1.1", "");

                        //if the dictionary contains a user with the same username.
                        if (users.ContainsKey(username))
                        {
                            //users locale in dictionary is located and outputted.
                            locale = users[username];
                            sReply = "HTTP/1.1" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + optionalHeaders + "\r\n" + locale + "\r\n";
                        }
                        //if user isnt found in dictionary.
                        else
                        {
                            sReply = "HTTP/1.1" + " " + "404" + " " + "Not" + " " + "Found" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + optionalHeaders;
                        }
                    }
                }

                //If first array entry contains the string (PUT /)
                //User requesting a locale change on this user.
                else if (splitData[0].Contains("PUT /"))
                {
                    //Replaces the prefix (GET /) to be an empty space, removing it. String Remaining is the username.
                    username = splitData[0].Replace("PUT /", "");
                    //users new locale is the 3rd array entry.
                    locale = splitData[2];

                    //if the dictionary contains a user with the same username.
                    if (users.ContainsKey(username))
                    {
                        //users locale in dictionary is located and changed.
                        locale = users[username];
                        sReply = "HTTP/0.9" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                    }
                    //if user isnt found in dictionary. Username and Locale is added as a new user.
                    else
                    {
                        //Adds user to dictionary database
                        users.TryAdd(username, locale);
                        sReply = "HTTP/0.9" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                    }
                }

                //If first array entry contains the string (POST /)
                //User requesting a locale change on this user.
                else if (splitData[0].Contains("POST /"))
                {
                    // If the protocol variable passed from doRequest == -h0, parse data accordingly.
                    if (protocol == "-h0")
                    {
                        //Replaces the prefix (GET /) and suffex (HTTP/1.0) to be an empty spaces, removing them. String Remaining is the username.
                        string temp = splitData[0].Replace("POST /", "");
                        username = temp.Replace(" HTTP/1.0", "");
                        //users new locale is the 4th array entry.
                        locale = splitData[3];

                        //if the dictionary contains a user with the same username.
                        if (users.ContainsKey(username))
                        {
                            //users locale in dictionary is located and changed.
                            users[username] = locale;
                            sReply = "HTTP/1.0" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                        }
                        //if user isnt found in dictionary. Username and Locale is added as a new user.
                        else
                        {
                            //Adds user to dictionary database
                            users.TryAdd(username, locale);
                            sReply = "HTTP/1.0" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n";
                        }
                    }
                    // If the protocol variable passed from doRequest == -h1, parse data accordingly.
                    else if (protocol == "-h1")
                    {
                        //provides a string for the optionalheaders
                        optionalHeaders = splitData[3];

                        //Replaces the prefixes of the locale and username (name=) and (&location=) with an empty space.
                        string temp = splitData[4].Replace("name=", "");
                        string temp2 = temp.Replace("&location=", " ");

                        //splits the remaining data which contains (username & locale)
                        string[] usernameLocation = temp2.Split(new char[] { ' ' }, 2);

                        //username is first array entry
                        username = usernameLocation[0];
                        Console.WriteLine("Username is: " + username);

                        //locale is second array entry
                        locale = usernameLocation[1];
                        Console.WriteLine("Location is: " + locale);

                        //if the dictionary contains a user with the same username.
                        if (users.ContainsKey(username))
                        {
                            //users locale in dictionary is located and changed.
                            users[username] = locale;
                            sReply = "HTTP/1.1" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + optionalHeaders;
                        }
                        //if user isnt found in dictionary. Username and Locale is added as a new user.
                        else
                        {
                            //Adds user to dictionary database
                            users.TryAdd(username, locale);
                            sReply = "HTTP/1.1" + " " + "200" + " " + "OK" + "\r\n" + "Content-Type:" + " " + "text/plain" + "\r\n" + optionalHeaders;
                        }
                    }
                }
                //Outputs, writes and flushes the data parsed and resulting reply.
                Console.WriteLine(sReply);
                sw.WriteLine(sReply);
                sw.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Something went wrong in the parseData function.\r\n  " + e.Message);
            }
        }

        /// <summary>
        /// whoIsProtocol responsible for the parsing of the data stream that is in a Whois format, providing relevant output as a result.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="dataIn"></param>
        public static void whoIsProtocol(StreamWriter sw, string dataIn)
        {
            //Variable initialing for Whois parsing
            string sReply = "";
            //Replaces new lines with empty spaces, removing syntax.
            dataIn = dataIn.Replace("\r\n", "");
            //Input data splitting, by white space
            string[] Information = dataIn.Split(new char[] { ' ' }, 2);
            string username = Information[0];
            string locale = "";

            #region(Debugging Region)
            /*
                Console.WriteLine("Data: " + dataIn);
                Console.WriteLine("dataIn.Length: " + dataIn.Length);
                Console.WriteLine("Information.Length: " + Information.Length);
                Console.WriteLine("Username: " + Information[0].ToString());
                Console.WriteLine("Location: " + Information[1].ToString());
                for (int i = 0; i < Information.Length; i++)
                {
                    Console.WriteLine(Information[i]);
                }  */
            #endregion

            try //Try/Catch 
            {
                //If the length of the split data array is 1
                if (Information.Length == 1)
                {
                    //if the dictionary contains a user with the same username.
                    if (users.ContainsKey(username))
                    {
                        //Reply is the locale of user
                        sReply = users[username];
                    }
                    //if user isnt found in dictionary.
                    else
                    {
                        sReply = "ERROR: no entries found";
                    }
                }
                //If the length of the split data array is greater than 1
                else if (Information.Length > 1)
                {
                    //Removes the username and the white space, remaining is the locale
                    locale = dataIn.Replace(username + " ", "");
                    //Changes the locale of user in dicitionary
                    users[username] = locale;
                    sReply = "OK";
                }
                //Outputs, writes and flushes the data parsed and resulting reply.
                Console.WriteLine(sReply);
                sw.WriteLine(sReply);
                sw.Flush();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: whoIsProtocol(): Exception Error Caught. " + e.StackTrace);
            }
        }
    }
}


