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
using System.IO;    // for StreamReader
using System.Threading;
using System.Threading.Tasks;

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


            //VooDoo
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
            foreach (string MType in v.MessageType)
            {
                cmb_MessageType.Items.Add(MType);
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

            //END OF VOODOO






        }

        #region "VooDoo"
        #region "Methods"

        public (string, int) CreateURL()
        {
            string url = "";
            int ID = 0;
            string DeviceID = "";
            string Line1 = "";
            string Line2 = "";
            string Line3 = "";
            string Line4 = "";
            string Line5 = "";
            short time = 0;
            string color = "";
            string music = "";
            bool abort = false;
            bool response = false;
            string opWord = "";
            VooDooAPI.operationType OpType = VooDooAPI.operationType.display;

            //Checking Device ID is selected
            if (cmb_DeviceID.SelectedItem != null)
            {
                if (cmb_DeviceID.SelectedItem.ToString() == "")
                {
                    brd_cmb_DeviceID.BorderBrush = new SolidColorBrush(Colors.Red);
                    abort = true;
                }
                else
                {
                    DeviceID = cmb_DeviceID.Text;
                    brd_cmb_DeviceID.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
            }
            else
            {
                brd_cmb_DeviceID.BorderBrush = new SolidColorBrush(Colors.Red);
                abort = true;
            }

            //Checking Message Type is Selected
            if (cmb_MessageType.SelectedItem != null)
            {
                if (cmb_MessageType.SelectedItem.ToString() == "")
                {
                    brd_cmb_MessageType.BorderBrush = new SolidColorBrush(Colors.Red);
                    abort = true;
                }
                else
                {
                    opWord = cmb_MessageType.Text;
                    switch (opWord)
                    {
                        case "display":
                            OpType = VooDooAPI.operationType.display;
                            break;
                        case "flash":
                            OpType = VooDooAPI.operationType.flash;
                            break;
                        case "static":
                            OpType = VooDooAPI.operationType.opStatic;
                            break;
                        case "static2":
                            OpType = VooDooAPI.operationType.opStatic2;
                            break;
                        case "location":
                            OpType = VooDooAPI.operationType.location;
                            break;
                        default:
                            break;
                    }
                    brd_cmb_MessageType.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
            }
            else
            {
                brd_cmb_MessageType.BorderBrush = new SolidColorBrush(Colors.Red);
                abort = true;
            }

            //Checking Music is Selected
            if (cmb__Music.SelectedItem != null)
            {
                if (cmb__Music.SelectedItem.ToString() != "")
                {
                    music = v.Music[cmb__Music.Text];
                    brd_cmb__Music.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    brd_cmb__Music.BorderBrush = new SolidColorBrush(Colors.Red);
                    abort = true;
                }
            }
            else
            {
                brd_cmb__Music.BorderBrush = new SolidColorBrush(Colors.Red);
                abort = true;
            }

            //Checking Button Colors is Selected
            if (cmb_ButtonColor.SelectedItem != null)
            {
                if (cmb_ButtonColor.SelectedItem.ToString() != "")
                {
                    color = v.Colors[cmb_ButtonColor.Text];
                    brd_cmb_ButtonColor.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    brd_cmb_ButtonColor.BorderBrush = new SolidColorBrush(Colors.Red);
                    abort = true;
                }
            }
            else
            {
                brd_cmb_ButtonColor.BorderBrush = new SolidColorBrush(Colors.Red);
                abort = true;
            }

            //If remain on is checked set color duration to 0
            if (Chk_RemainOn.IsChecked == false)
            {
                if (txt_ButtonColorDuration.Text != "")
                {
                    time = short.Parse(txt_ButtonColorDuration.Text);
                    txt_ButtonColorDuration.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    txt_ButtonColorDuration.BorderBrush = new SolidColorBrush(Colors.Red);
                    abort = true;
                }
            }
            else
            {
                txt_ButtonColorDuration.Text = "0";
                txt_ButtonColorDuration.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }

            //Checking Response Required is Checked
            if (Chk_ResponseRequired.IsChecked == true)
            {
                response = true;
            }
            else
            {
                response = false;
            }


            //Set lines based on entered text - blank is ok
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
                    //object test = cbm_Line1Icons.SelectedValue;
                    //test.
                    //PicClass t = new PicClass();
                    Line1 = "%5c" + v.Icons[((PicClass)cbm_Line1Icons.SelectedValue).ImgName];
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

            //Set transaction ID
            if (Queued.Count == 0)
            {
                ID = 0;
            }
            else
            {
                ID = Queued[Queued.Count - 1].ID + 1;
            }

            //Abort if any data was missing
            if (abort == true)
            {
                return ("", 0);
            }

            //okay, set up the whole url for the get request
            //see:  https://voodoorobotics.com/constructing-a-url/
            url = v.baseurl + "api/" + DeviceID + "/" + opWord + "/" + Line1 + "~" + Line2 + "/" + Line3 + "~" + Line4 + "~" + Line5;
            switch (OpType)
            {
                case VooDooAPI.operationType.display:
                    url += "/" + music + "/" + time.ToString() + color;
                    break;
                case VooDooAPI.operationType.flash:
                    url += "/" + music + "/" + time.ToString() + color;
                    break;
                case VooDooAPI.operationType.opStatic:
                case VooDooAPI.operationType.opStatic2:
                case VooDooAPI.operationType.location:
                default:
                    break;
                    //do nothing!
            }

            //If response is requested then add transaction ID
            if (response)
            {
                url += "/TransactionID" + ID;
            }

            return (url, ID);
        }

        public void RemoveLine()
        {
            Dispatcher.Invoke(() =>
            {
                if (Queued.Count != 0)
                {
                    Queued.RemoveAt(Queued.Count - 1);
                    FillText();
                }
            });
        }

        public void FillText()
        {
            txt_Instructions.Text = "";
            Queued = Queued.OrderBy(o => o.ID).ToList();
            foreach (Queue q in Queued)
            {
                if (q.ID == 0)
                {
                    txt_Instructions.Text = txt_Instructions.Text + q.ID + ":" + q.URL;
                }
                else
                {
                    txt_Instructions.Text = txt_Instructions.Text + "\n" + q.ID + ":" + q.URL;
                }

            }
        }

        #endregion

        #region "XAML Buttons"

        //*******************************************************************************
        //                                  XAML Buttons     
        //*******************************************************************************

        //--------------------------------- Send Instructions  --------------------------
        private void btn_SendInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (Queued.Count != 0)
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
            else
            {
                MessageBox.Show("Queue messages before sending");
                return;
            }

        }

        private void cbm_Line1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    vbx_Line1Txt.Visibility = Visibility.Visible;
                    vbx_Line1Cbb.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 24;
                    break;
                case "Barcode":
                    vbx_Line1Txt.Visibility = Visibility.Visible;
                    vbx_Line1Cbb.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 15;
                    break;

                case "QRCode":
                    vbx_Line1Txt.Visibility = Visibility.Visible;
                    vbx_Line1Cbb.Visibility = Visibility.Collapsed;
                    txt_Line1.MaxLength = 23;
                    break;

                case "Icon":
                    vbx_Line1Txt.Visibility = Visibility.Collapsed;
                    vbx_Line1Cbb.Visibility = Visibility.Visible;
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
                    vbx_Line2Txt.Visibility = Visibility.Visible;
                    vbx_Line2Cbb.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 24;
                    break;
                case "Barcode":
                    vbx_Line2Txt.Visibility = Visibility.Visible;
                    vbx_Line2Cbb.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 15;
                    break;

                case "QRCode":
                    vbx_Line2Txt.Visibility = Visibility.Visible;
                    vbx_Line2Cbb.Visibility = Visibility.Collapsed;
                    txt_Line2.MaxLength = 23;
                    break;

                case "Icon":
                    vbx_Line2Txt.Visibility = Visibility.Collapsed;
                    vbx_Line2Cbb.Visibility = Visibility.Visible;
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
                    vbx_Line3Txt.Visibility = Visibility.Visible;
                    vbx_Line3Cbb.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 24;
                    break;
                case "Barcode":
                    vbx_Line3Txt.Visibility = Visibility.Visible;
                    vbx_Line3Cbb.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 15;
                    break;

                case "QRCode":
                    vbx_Line3Txt.Visibility = Visibility.Visible;
                    vbx_Line3Cbb.Visibility = Visibility.Collapsed;
                    txt_Line3.MaxLength = 23;
                    break;

                case "Icon":
                    vbx_Line3Txt.Visibility = Visibility.Collapsed;
                    vbx_Line3Cbb.Visibility = Visibility.Visible;
                    break;

                default:
                    txt_Line3.Visibility = Visibility.Visible;
                    vbx_Line3Cbb.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    vbx_Line4Txt.Visibility = Visibility.Visible;
                    vbx_Line4Cbb.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 24;
                    break;
                case "Barcode":
                    vbx_Line4Txt.Visibility = Visibility.Visible;
                    vbx_Line4Cbb.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 15;
                    break;

                case "QRCode":
                    vbx_Line4Txt.Visibility = Visibility.Visible;
                    vbx_Line4Cbb.Visibility = Visibility.Collapsed;
                    txt_Line4.MaxLength = 23;
                    break;

                case "Icon":
                    vbx_Line4Txt.Visibility = Visibility.Collapsed;
                    vbx_Line4Cbb.Visibility = Visibility.Visible;
                    break;

                default:
                    vbx_Line4Txt.Visibility = Visibility.Visible;
                    vbx_Line4Cbb.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbm_Line5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.ComboBox).SelectedItem.ToString().Split(' ').Last())
            {
                case "Text":
                    vbx_Line5Txt.Visibility = Visibility.Visible;
                    vbx_Line5Cbb.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 24;
                    break;
                case "Barcode":
                    vbx_Line5Txt.Visibility = Visibility.Visible;
                    vbx_Line5Cbb.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 15;
                    break;

                case "QRCode":
                    vbx_Line5Txt.Visibility = Visibility.Visible;
                    vbx_Line5Cbb.Visibility = Visibility.Collapsed;
                    txt_Line5.MaxLength = 23;
                    break;

                case "Icon":
                    vbx_Line5Txt.Visibility = Visibility.Collapsed;
                    vbx_Line5Cbb.Visibility = Visibility.Visible;
                    break;

                default:
                    vbx_Line5Txt.Visibility = Visibility.Visible;
                    vbx_Line5Cbb.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void btn_QueueInstruction_Click(object sender, RoutedEventArgs e)
        {
            string url;
            int ID;
            (url, ID) = CreateURL();
            if (url == "")
            {
                return;
            }
            Queued.Add(new Queue(ID, url));
            FillText();
        }

        private void btn_InsertInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (txtInsertID.Text == "" || int.Parse(txtInsertID.Text.ToString()) >= Queued.Count())
            {
                txtInsertID.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            else
            {
                txtInsertID.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            string url;
            int ID;
            (url, ID) = CreateURL();
            if (url == "")
            {
                return;
            }
            int loop = 1;

            Queued.RemoveAt(int.Parse(txtInsertID.Text.ToString()));
            Queued.Add(new Queue(int.Parse(txtInsertID.Text.ToString()), url));
            foreach (Queue q in Queued) if (q.ID > int.Parse(txtInsertID.Text.ToString()))
                {
                    q.ID = int.Parse(txtInsertID.Text.ToString()) + loop;
                    loop += 1;
                }


            FillText();
        }

        private void btn_RemoveInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (txtDeleteID.Text == "")
            {
                RemoveLine();
            }

            else
            {
                if (Queued.Count >= int.Parse(txtDeleteID.Text.ToString()))
                {
                    int loop = 0;

                    Queued.RemoveAt(int.Parse(txtDeleteID.Text.ToString()));
                    foreach (Queue q in Queued) if (q.ID > int.Parse(txtDeleteID.Text.ToString()))
                        {
                            q.ID = int.Parse(txtDeleteID.Text.ToString()) + loop;
                            loop += 1;
                        }

                    FillText();
                }
            }
        }

        private void Chk_RemainOn_Unchecked(object sender, RoutedEventArgs e)
        {
            txt_ButtonColorDuration.Text = "";
            txt_ButtonColorDuration.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void Chk_RemainOn_Checked(object sender, RoutedEventArgs e)
        {
            txt_ButtonColorDuration.Text = "0";
            txt_ButtonColorDuration.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            Queued.Clear();
            System.Windows.Forms.OpenFileDialog choofdlog = new System.Windows.Forms.OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            choofdlog.ShowDialog();
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(choofdlog.FileName);
            while ((line = file.ReadLine()) != null)
            {
                if (line != "")
                {
                    Queue q = new Queue(int.Parse(line.Split(':').First()), line.Split(':')[1] + line.Split(':')[2] + line.Split(':')[3]);
                    Queued.Add(q);
                }
            }

            file.Close();

            FillText();
        }

        private void Btn_Export_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog choofdlog = new System.Windows.Forms.SaveFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;

            choofdlog.ShowDialog();
            File.WriteAllText(choofdlog.FileName + ".txt", txt_Instructions.Text);

        }

        private void txt_UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            v.setUsername(txt_UserName.Text);
        }

        private void txt_Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            v.setPassword(txt_Password.Text);
        }



        #endregion

        #endregion

        Lightning_Pick PTL = new Lightning_Pick();
        Boolean Status;

        
        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void boxID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LTP_CONNECT_Click(object sender, RoutedEventArgs e)
        {
            Status = PTL.Connect();
            if (Status == true)
            {
                listStatus.Items.Add("Connection Succesful");
            }
            else
            {
                listStatus.Items.Add("Connection Failed");
            }
        }

        private void btnSEND_Click(object sender, RoutedEventArgs e)
        {
            String[] ModuleArray = { boxID.Text };
            String[] MessageArray = { txtMes.Text };
            String ButtonColor = boxColor.Text;
            String[] ButtonBehavior = { "", "" };
            String DisplayBehavior;

            //sets Button Behavior
            if (chkEnableBut.IsChecked == true)
            {
                ButtonBehavior[0] = "Enabled";
            }
            else
            {
                ButtonBehavior[0] = "Disabled";
            }
            if (btnOnBut.IsChecked == true)
            {
                ButtonBehavior[1] = "On";
            }
            else if (btnOffBut.IsChecked == true)
            {
                ButtonBehavior[1] = "Off";
            } 
            else
            {
                ButtonBehavior[1] = "Flash";
            }


            //sets Display Behavior
            if (btnOnDis.IsChecked == true)
            {
                DisplayBehavior = "On";
            }
            else if (btnOffDis.IsChecked == true)
            {
                DisplayBehavior = "Off";
            }
            else
            {
                DisplayBehavior = "Flash";
            }


            Status = PTL.ActivateModule(ModuleArray, MessageArray, ButtonColor, ButtonBehavior, DisplayBehavior);

            
            if (Status == true)
            {
                listStatus.Items.Add("Command Succesfully Sent");
            }
            else
            {
                listStatus.Items.Add("Command Failed");
            }

        }

        private void btnMaint_Click(object sender, RoutedEventArgs e)
        {
            PTL.MaintanenanceModules();
        }
    }
}