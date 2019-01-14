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

namespace PrintSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Printer printer = new Printer();

        public MainWindow()
        {
            InitializeComponent();

            printer.Job = new GSharp.Job();
            printer.Job.Load(@"C:\Users\cadik\Desktop\CFFFP_Warp_Reaction_Chamber.gcode");

            printer.Port = "COM3";

            printer.Print();
        }
    }
}