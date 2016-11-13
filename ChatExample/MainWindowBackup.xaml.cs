using System.Windows;

namespace ChatExample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowBackup : Window {
        public MainWindowBackup() {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();

        }
    }
}
