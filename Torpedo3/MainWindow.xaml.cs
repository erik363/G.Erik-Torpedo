using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Torpedo3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new System.Uri("PvAI_Page.xaml", UriKind.Relative));
        }

        private void AIPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("PvAI_Page.xaml", UriKind.Relative));
        }

        private void PvP_Page(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("PvP_Page.xaml", UriKind.Relative));
        }

        private void ScorePage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("ScoreBoard_Page.xaml", UriKind.Relative));
        }
    }
}
