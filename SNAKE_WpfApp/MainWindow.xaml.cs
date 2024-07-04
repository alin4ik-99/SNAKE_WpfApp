using SNAKE_WpfApp.ViewModels;
using System.Windows;

namespace SNAKE_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindVM(this);
        }
    }
}
