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
//--
using System.Xml;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Linq;

using System.IO;
using Microsoft.Win32;

namespace XQfuncWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlDocument doc = null;
        static string placeholder_text = "input path in XML file here";
        public MainWindow()
        {

            InitializeComponent();
            path.Text = placeholder_text;
            //path.GotFocus += new EventHandler(HidePlaceholder);
            //path.LostFocus += new EventHandler(ShowPlaceholder);
        }
        public void HidePlaceholder(object sender, EventArgs e)
        {
            if (path.Text == placeholder_text)
            {
                path.Text = "";
            }
        }

        public void ShowPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(path.Text))
                path.Text = placeholder_text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathto = path.Text;
                if (Convert.ToChar(pathto[0]) == Convert.ToChar('\"') || Convert.ToChar(pathto[pathto.Length - 1]) == Convert.ToChar('\"'))
                {
                    pathto = pathto.Trim('\"');
                    //MessageBox.Show(pathto);
                }
                int depth = pathto.Split('/').Length;
                if (doc == null)
                    throw new Exception("please open file");
                PrintSelectedNodeText(doc.DocumentElement, pathto, depth, depth);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + ", try again!");
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

      
        private static void PrintSelectedNodeText(XmlNode x, string xp, int depth, int num)
        {
            //func to search node value 
                if (xp == placeholder_text || xp == "" || Convert.ToChar(xp[0])==Convert.ToChar('\"') || Convert.ToChar(xp[xp.Length-1]) == Convert.ToChar('\"'))
                    throw new Exception("bad or empty path");
                foreach (XmlNode i in x)
                {
                    if (i.Name == xp.Split('/')[depth - num])
                    {
                        if (num == 1)
                        {
                            ((MainWindow)Application.Current.MainWindow).ans2.Text = i.InnerText;
                            Console.WriteLine(i.InnerText);
                            break;
                        }
                        else
                        {
                            num = num - 1;
                            PrintSelectedNodeText(i, xp, depth, num);
                        }
                    }
                }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //func loading file from folder
            //doc.load("path to xml document");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true) {
                doc = new XmlDocument();
                string pt = openFileDialog.FileName;
                doc.Load(pt);
                filepath.Text = pt;
            }
        }
    }
}
