using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FtpClient
{

    public class InfoContext
    {
        public string type;
        public string Type
        {
            get
            {
                return "Images/" + type + ".png";
            }
            set
            {
                type = value;
            }
        }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }

        public InfoContext(string size, string type, string name, string date, string address)
        {
            Size = size;
            Type = type;
            Name = name;
            Date = date;
            Address = address;
        }
    }
}
