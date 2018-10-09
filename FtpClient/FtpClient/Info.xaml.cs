using System.Windows.Controls;

namespace FtpClient
{
    /// <summary>
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : UserControl
    {
        public Info() => InitializeComponent();

        public Info(string size, string type, string name, string date, string address)
        {
            InitializeComponent();

            DataContext = new InfoContext(size, type, name, date, address);
        }
    }
}
