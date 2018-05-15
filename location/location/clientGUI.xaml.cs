using System.Collections.Generic;
using System.Windows;

namespace location
{
    /// <summary>
    /// Interaction logic for clientGUI.xaml
    /// </summary>
    public partial class clientGUI : Window
    {
        #region(Region - Input Getter & Setters)
        public string bUsername { get; set; }
        public string bLocale { get; set; }
        #endregion
        /// <summary>
        /// Initialisation of the 'Client GUI'
        /// </summary>
        public clientGUI()
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
            //converts List<> to array, for Main() arguments
            location.argParser(args.ToArray());            
        }

        /// <summary>
        /// Button Logic: Method for switching to advanced interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvSetting_Click(object sender, RoutedEventArgs e)
        {
            //closes this window
            this.Close();
            //Setting up of the new window
            AdvSettings AdvSettings = new AdvSettings();            
            AdvSettings.Title = "Client Advanced Settings";
            AdvSettings.Width = 500;
            AdvSettings.Height = 500;
            AdvSettings.ShowDialog();
        }
    }
}
