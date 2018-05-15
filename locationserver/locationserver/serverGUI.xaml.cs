using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace locationserver
{
    /// <summary>
    /// Interaction logic for serverGUI.xaml
    /// </summary>
    public partial class serverGUI : Window
    {
        string[] Input = Environment.GetCommandLineArgs();

        public serverGUI()
        {
            InitializeComponent();
            
        }

        private void changeOutput()
        {
            InputLabel.Content = Input.ToString();
        }

    }
}
