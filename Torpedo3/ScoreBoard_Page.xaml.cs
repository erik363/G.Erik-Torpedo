using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ScoreBoard_Page.xaml
    /// </summary>
    public partial class ScoreBoard_Page : Page
    {
        public ScoreBoard_Page()
        {
            InitializeComponent();

            Prinnt();
        }

        private void Prinnt()
        {
            String JSONtxt = File.ReadAllText(@"d:\test.json");
            var accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Data>>(JSONtxt);
            List<Data> temp = accounts.ToList();
            foreach(var l in temp)
            {
                this.lista.Items.Add(new Data { Name1 = l.Name1, Name2 = l.Name2, Rounds = l.Rounds });
            }       
        }
    }
}
