using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TrickyUnits;

namespace StarStory2_Foe_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            GetFoeList();
        }

        void GetFoeList() {
            Debug.WriteLine("Renewing Foe List");
            FoeList.Items.Clear();
            var L = FileList.GetTree(Data.Dir);
            foreach(var f in L) {
                if (f.ToUpper() != "README.MD") FoeList.Items.Add(f);
            }
        }
    }
}
