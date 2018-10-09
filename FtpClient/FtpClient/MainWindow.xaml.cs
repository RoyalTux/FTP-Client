using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace FtpClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект подключения по FTP
                client = new Client(txtboxAddress.Text, txtboxLogin.Text, txtboxPassword.Password);

                // Регулярное выражение, которое ищет информацию о папках и файлах 
                // в строке ответа от сервера
                Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                // Получаем список корневых файлов и папок
                // Используется LINQ to Objects и регулярные выражения
                Info item;
                List<Info> list = client.ListDirectoryDetails().ToList().Select(s =>
                {
                    Match match = regex.Match(s);
                    if (match.Length > 5)
                    {
                        // Устанавливаем тип, чтобы отличить файл от папки (используется также для установки рисунка)
                        string type = match.Groups[1].Value == "d" ? "DIR" : "FILE";

                        // Размер задаем только для файлов, т.к. для папок возвращается
                        // размер ярлыка 4кб, а не самой папки
                        string size = "";
                        if (type == "FILE")
                        {
                            size = (Int64.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";
                        }

                        string address = txtboxAddress.Text;
                        if (address.Last() != '/')
                        {
                            address += '/';
                        }
                        item = new Info(size, type, match.Groups[6].Value, match.Groups[4].Value, address);
                    }
                    else
                    {
                        item = new Info();
                    }
                    item.MouseDoubleClick += Info_MouseDoubleClick;
                    return item;
                }).ToList();

                // Добавить поле, которое будет возвращать пользователя на директорию выше
                item = new Info("", "PREV", "...", "", txtboxAddress.Text);
                item.MouseDoubleClick += Info_MouseDoubleClick;
                list.Add(item);

                list.Reverse();

                // Отобразить список в ListView
                listbox.Items.Clear();
                foreach (Info info in list)
                {
                    listbox.Items.Add(info);
                }
            }
            catch (Exception ex)
            {
                listbox.Items.Clear();
                MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
            }
        }

        private void Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InfoContext context = (InfoContext)(sender as Info).DataContext;

            switch (context.type)
            {
                case "FILE":
                    return;

                case "DIR":
                    txtboxAddress.Text = context.Address + context.Name + "/";
                    btnConnect_Click(null, null);
                    break;

                case "PREV":
                    context.Address = context.Address.TrimEnd('/');
                    txtboxAddress.Text = context.Address.Remove(context.Address.LastIndexOf('/') + 1);
                    break;
            }
            btnConnect_Click(null, null);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //bool ctrlPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

            //// Save
            //if (e.Key == Key.D)
            //{
            //    DownloadFile();
            //}

            //if (e.Key == Key.U)
            //{
            //    UploadFile();
            //}
        }

        private void DownloadFile()
        {
            Info info = (Info)listbox.SelectedItem;
            if (info == null)
            {
                return;
            }
            
            InfoContext context = (InfoContext)info.DataContext;
            if (context.type != "FILE")
            {
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = context.Name;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    client.DownloadFile(context.Address + context.Name, dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
                }
            }
        }

        private void UploadFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    client.UploadFile(dlg.FileName, txtboxAddress.Text + System.IO.Path.GetFileName(dlg.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
                }
                btnConnect_Click(null, null);
            }
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            DownloadFile();
        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            UploadFile();
        }
    }
}
