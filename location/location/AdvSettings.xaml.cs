using System;
using System.Collections.Generic;
using System.Windows;

namespace location
{
    /// <summary>
    /// Interaction logic for AdvSettings.xaml
    /// </summary>
    public partial class AdvSettings : Window
    {
        #region(Region - Input Getter & Setters)
        public string bUsername { get; set; }
        public string bLocale { get; set; }
        public string bProtocol { get; set; }
        public string bPort { get; set; }
        public string bIPAddress { get; set; }
        public string bTimeoutCount { get; set; }
        public string bOptionalHeaders { get; set; }
        public bool bDebugMode { get; set; }
        #endregion
        /// <summary>
        /// Initialisation of the 'Advanced Settings Window'
        /// </summary>
        public AdvSettings()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Button Logic: Method behind the submitting of the inputted information.
        /// Creates a List<>,
        /// Adds the binded text from textboxes to the List<>,
        /// Then converts List<> to an Array to use as Main() args.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //creates a new List<>
            List<string> args = new List<string>();    

            //Add username to list<>
            args.Add(bUsername);
            //Add location to list<>
            if (bLocale != null)
            {
                args.Add(bLocale);
            }
            //Add port number to list<>
            if (bPort != null)
            {
                args.Add("-p");
                args.Add(bPort);
            }
            //Add ip address to list<>
            if (bIPAddress != null)
            {
                args.Add("-h");
                args.Add(bIPAddress);
            }
            //Add timeout to list<>
            if (bTimeoutCount != null)
            {
                args.Add("-t");
                args.Add(bTimeoutCount);
            }
            //Add debugmode check to list<>
            if (bDebugMode == true)
            {
                args.Add("-d");
                foreach(string arg in args)
                {
                    Console.WriteLine(arg);
                }
            }
            //Add protocol to list<>
            args.Add(bProtocol);
            //converts List<> to array, for Main() arguments
            location.argParser(args.ToArray());

            //On sumbitting clears all textboxes 
            //(expect Username - done so user can check if change occured quickly)
            Location.Text = String.Empty;
            Port.Text = String.Empty;
            IP.Text = String.Empty;
            Timeout.Text = String.Empty;
            OptionalHeaders.Text = String.Empty;

            #region(Region - Debugging)
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
            #endregion

            //Outputs the users input on submitting useful for data checking 
            inputLabel.Content = bUsername + " " + bLocale + " " + bProtocol + " " +
                bPort + " " + bIPAddress + " " + bTimeoutCount + " " + 
                bOptionalHeaders;
        }

        /// <summary>
        /// Button Logic: Method for checking of location, works for quick checking, 
        /// before and after rest of data has been inputted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckLocButton_Click(object sender, RoutedEventArgs e)
        {
            //creates a new List<>
            List<string> args = new List<string>(); 
            //Add username to List<>
            args.Add(bUsername);
            //converts List<> to array, for Main() arguments
            location.argParser(args.ToArray());
        }

        /// <summary>
        /// Button Logic: Method for switching back to simple interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NonAdvSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            clientGUI clientGUI = new clientGUI();
            clientGUI.Title = "Client Settings";
            clientGUI.Width = 400;
            clientGUI.Height = 400;
            clientGUI.ShowDialog();           
        }

        /// <summary>
        /// Radio Buttton Logic: Determines the users selection of protocol (WhoIs selected by default)
        /// Only one can be checked at any one time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region(Region - HTTP Checkbox Methods)
        private void Checked_h9(object sender, RoutedEventArgs e)
        {
            bProtocol = "-h9";
        }

        private void Checked_h0(object sender, RoutedEventArgs e)
        {
            bProtocol = "-h0";
        }

        private void Checked_h1(object sender, RoutedEventArgs e)
        {
            bProtocol = "-h1";
        }

        private void Unchecked_h9(object sender, RoutedEventArgs e)
        {
            bProtocol = null;
        }

        private void Unchecked_h0(object sender, RoutedEventArgs e)
        {
            bProtocol = null;
        }

        private void Unchecked_h1(object sender, RoutedEventArgs e)
        {
            bProtocol = null;
        }
        #endregion

        /// <summary>
        /// Checkbox Logic: if checked debug mode is enabled (prints extra info to console)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region(Region - Debug Mode checkbox methods)
        private void Checked_dM(object sender, RoutedEventArgs e)
        {
            bDebugMode = true;
        }

        private void Unchecked_dM(object sender, RoutedEventArgs e)
        {
            bDebugMode = false;
        }
        #endregion
    }
}
