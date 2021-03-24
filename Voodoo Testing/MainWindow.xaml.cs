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
using System.Net;
using System.IO;    // for StreamReader
using System.Threading;

namespace Voodoo_Testing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        //*******************************************************************************
        //                                  Global Variables    
        //*******************************************************************************
        VooDooAPI v = new VooDooAPI();  //create a VoodooAPI object
        public System.Collections.ObjectModel.ObservableCollection<PicClass> Images { get; set; }
        public List<Queue> Queued = new List<Queue>();

        public MainWindow()
        {
            InitializeComponent();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol;

            cbm_Line1.SelectedIndex = 0;
            cbm_Line2.SelectedIndex = 0;
            cbm_Line3.SelectedIndex = 0;
            cbm_Line4.SelectedIndex = 0;
            cbm_Line5.SelectedIndex = 0;

            foreach (string MusicName in v.Music.Keys)
            {
                cmb__Music.Items.Add(MusicName);
            }
            foreach (string Color in v.Colors.Keys)
            {
                cmb_ButtonColor.Items.Add(Color);
            }


            this.DataContext = this;
            Images = new System.Collections.ObjectModel.ObservableCollection<PicClass>();
            foreach (string name in v.Icons.Keys)
            {
                Images.Add(new PicClass
                {
                    Img = new BitmapImage(new Uri(@"/Images\" + name + ".png", UriKind.RelativeOrAbsolute)),
                    ImgName = name

                });
            }
        }

        #region "Methods"

        public string CreateURL()
        {
            string Line1 = "";
            string Line2 = "";
            string Line3 = "";
            string Line4 = "";
            string Line5 = "";
            short time = 0;
            string color = "";
            string music = "";

            if (cmb_DeviceID.SelectedItem.ToString() == "")
            {
                System.Windows.MessageBox.Show("Must Enter Device ID");
                return "";
            }


            if (cmb_MessageType.SelectedItem.ToString() == "")
            {
                System.Windows.MessageBox.Show("Must Enter Message Type");
                return "";
            }


            if (cmb__Music.SelectedItem.ToString() != "")
            {
                music = v.Music[cmb__Music.Text];
            }


            if (cmb_ButtonColor.SelectedItem.ToString() != "")
            {
                color = v.Colors[cmb_ButtonColor.Text];
            }


            if (Chk_RemainOn.IsChecked == false)
            {
                time = short.Parse(txt_ButtonColorDuration.Text);
            }
            else
            {
                txt_ButtonColorDuration.Text = "0";
            }

            switch (cbm_Line1.SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    Line1 = txt_Line1.Text;
                    break;
                case "Barcode":
                    Line1 = "%5cbc" + txt_Line1.Text;
                    break;

                case "QRCode":
                    Line1 = "%5cqr" + txt_Line1.Text;
                    break;

                case "Icon":
                    Line1 = "%5c" + v.Icons[cbm_Line1Icons.SelectedItem.ToString()];
                    break;

                default:
                    Line1 = "";
                    break;
            }

            switch (cbm_Line2.SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    Line2 = txt_Line2.Text;
                    break;
                case "Barcode":
                    Line2 = "%5cbc" + txt_Line2.Text;
                    break;

                case "QRCode":
                    Line2 = "%5cqr" + txt_Line2.Text;
                    break;

                case "Icon":
                    Line2 = "%5c" + v.Icons[cbm_Line2Icons.SelectedItem.ToString()];
                    break;

                default:
                    Line2 = "";
                    break;
            }

            switch (cbm_Line3.SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    Line3 = txt_Line3.Text;
                    break;
                case "Barcode":
                    Line3 = "%5cbc" + txt_Line3.Text;
                    break;

                case "QRCode":
                    Line3 = "%5cqr" + txt_Line3.Text;
                    break;

                case "Icon":
                    Line3 = "%5c" + v.Icons[cbm_Line3Icons.SelectedItem.ToString()];
                    break;

                default:
                    Line3 = "";
                    break;
            }

            switch (cbm_Line4.SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    Line4 = txt_Line4.Text;
                    break;
                case "Barcode":
                    Line4 = "%5cbc" + txt_Line4.Text;
                    break;

                case "QRCode":
                    Line4 = "%5cqr" + txt_Line4.Text;
                    break;

                case "Icon":
                    Line4 = "%5c" + v.Icons[cbm_Line4Icons.SelectedItem.ToString()];
                    break;

                default:
                    Line4 = "";
                    break;
            }

            switch (cbm_Line5.SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    Line5 = txt_Line5.Text;
                    break;
                case "Barcode":
                    Line5 = "%5cbc" + txt_Line5.Text;
                    break;

                case "QRCode":
                    Line5 = "%5cqr" + txt_Line5.Text;
                    break;

                case "Icon":
                    Line5 = "%5c" + v.Icons[cbm_Line5Icons.SelectedItem.ToString()];
                    break;

                default:
                    Line5 = "";
                    break;
            }

            v.setUsernameAndPassword(txt_UserName.Text, txt_Password.Text);
            v.setDeviceID(cmb_DeviceID.Text);
            v.setOperation(VooDooAPI.operationType.display);
            v.setTune(music);
            v.setDisplay(Line1 + "~" + Line2, Line3 + "~" + Line4 + "~" + Line5);
            v.setColor(color);
            v.setTime(time);
            return v.CreateURL();
        }

        #endregion

        #region "XAML Buttons"

        //*******************************************************************************
        //                                  XAML Buttons     
        //*******************************************************************************

        //--------------------------------- Send Instructions  --------------------------
        private void btn_SendInstruction_Click(object sender, RoutedEventArgs e)
        {
            foreach (Queue Q in Queued)
            {
                new Thread(() =>
                {
                    Q.TimeSent = DateTime.Now;
                    v.SendInstruction(Q.URL);
                    RemoveLine();
                    Thread.Sleep(100);
                }).Start();
            }
        }

        private void cbm_Line1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    txt_Line1.Visibility = Visibility.Visible;
                    cbm_Line1Icons.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 24;
                    break;
                case "Barcode":
                    txt_Line1.Visibility = Visibility.Visible;
                    cbm_Line1Icons.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 15;
                    break;

                case "QRCode":
                    txt_Line1.Visibility = Visibility.Visible;
                    cbm_Line1Icons.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 23;
                    break;

                case "Icon":
                    txt_Line1.Visibility = Visibility.Collapsed;
                    cbm_Line1Icons.Visibility = Visibility.Visible;
                    //foreach (string Icon in v.Icons.Keys)
                    //{
                    //    cbm_Line1Icons.Items.Add(Icon);
                    //}
                    break;

                default:
                    txt_Line1.Visibility = Visibility.Visible;
                    cbm_Line1Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    txt_Line2.Visibility = Visibility.Visible;
                    cbm_Line2Icons.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 24;
                    break;
                case "Barcode":
                    txt_Line2.Visibility = Visibility.Visible;
                    cbm_Line2Icons.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 15;
                    break;

                case "QRCode":
                    txt_Line2.Visibility = Visibility.Visible;
                    cbm_Line2Icons.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 23;
                    break;

                case "Icon":
                    txt_Line2.Visibility = Visibility.Collapsed;
                    cbm_Line2Icons.Visibility = Visibility.Visible;
                    //foreach (string Icon in v.Icons.Keys)
                    //{
                    //    cbm_Line2Icons.Items.Add(Icon);
                    //}
                    break;

                default:
                    txt_Line2.Visibility = Visibility.Visible;
                    cbm_Line2Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    txt_Line3.Visibility = Visibility.Visible;
                    cbm_Line3Icons.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 24;
                    break;
                case "Barcode":
                    txt_Line3.Visibility = Visibility.Visible;
                    cbm_Line3Icons.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 15;
                    break;

                case "QRCode":
                    txt_Line3.Visibility = Visibility.Visible;
                    cbm_Line3Icons.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 23;
                    break;

                case "Icon":
                    txt_Line3.Visibility = Visibility.Collapsed;
                    cbm_Line3Icons.Visibility = Visibility.Visible;
                    //foreach (string Icon in v.Icons.Keys)
                    //{
                    //    cbm_Line3Icons.Items.Add(Icon);
                    //}
                    break;

                default:
                    txt_Line3.Visibility = Visibility.Visible;
                    cbm_Line3Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    txt_Line4.Visibility = Visibility.Visible;
                    cbm_Line4Icons.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 24;
                    break;
                case "Barcode":
                    txt_Line4.Visibility = Visibility.Visible;
                    cbm_Line4Icons.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 15;
                    break;

                case "QRCode":
                    txt_Line4.Visibility = Visibility.Visible;
                    cbm_Line4Icons.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 23;
                    break;

                case "Icon":
                    txt_Line4.Visibility = Visibility.Collapsed;
                    cbm_Line4Icons.Visibility = Visibility.Visible;
                    //foreach (string Icon in v.Icons.Keys)
                    //{
                    //    cbm_Line4Icons.Items.Add(Icon);
                    //}
                    break;

                default:
                    txt_Line4.Visibility = Visibility.Visible;
                    cbm_Line4Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    txt_Line5.Visibility = Visibility.Visible;
                    cbm_Line5Icons.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 24;
                    break;
                case "Barcode":
                    txt_Line5.Visibility = Visibility.Visible;
                    cbm_Line5Icons.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 15;
                    break;

                case "QRCode":
                    txt_Line5.Visibility = Visibility.Visible;
                    cbm_Line5Icons.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 23;
                    break;

                case "Icon":
                    txt_Line5.Visibility = Visibility.Collapsed;
                    cbm_Line5Icons.Visibility = Visibility.Visible;
                    //foreach (string Icon in v.Icons.Keys)
                    //{
                    //    cbm_Line5Icons.Items.Add(Icon);
                    //}
                    break;

                default:
                    txt_Line5.Visibility = Visibility.Visible;
                    cbm_Line5Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        private void btn_QueueInstruction_Click(object sender, RoutedEventArgs e)
        {
            string URL = CreateURL();
            int ID;
            bool response;
            if (Chk_ResponseRequired.IsChecked == true)
            {
                response = true;
            }
            else
            {
                response = false;
            }
            if (Queued.Count == 0)
            {
                ID = 0;
            }
            else
            {
                ID = Queued[Queued.Count - 1].ID + 1;
            }
            Queued.Add(new Queue(ID, URL, response));
            if (txt_Instructions.Text == "")
            {
                txt_Instructions.Text = txt_Instructions.Text + URL + " ID:" + ID + " Response:" + response.ToString();
            }
            else
            {
                txt_Instructions.Text = txt_Instructions.Text + "\n" + URL + " ID:" + ID + " Response:" + response.ToString();
            }
        }

        private void btn_RemoveInstruction_Click(object sender, RoutedEventArgs e)
        {
            RemoveLine();
        }

        public void RemoveLine()
        {
            //TO DO - ERRORS WHEN ERASING LAST LINE
            Dispatcher.Invoke(() =>
            {
                Queued.RemoveAt(Queued.Count - 1);
                if (txt_Instructions.Text == "")
                {
                    return;
                }
                else
                {
                    txt_Instructions.Text = txt_Instructions.Text.Replace("\n" + txt_Instructions.Text.Split('\n')[txt_Instructions.Text.Split('\n').Length - 1], "");
                }
            });
        }
    }

    #region "Exceptions"

    //*******************************************************************************
    //                                  Illegal Argument Exception     
    //*******************************************************************************

    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException(string message) : base(message)
        {
        }
    }

    //*******************************************************************************
    //                                  Runtime Exception     
    //*******************************************************************************
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message)
        {
        }
    }

    #endregion

}