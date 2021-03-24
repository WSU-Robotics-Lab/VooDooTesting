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
        VoodooAPI v = new VoodooAPI();  //create a VoodooAPI object

        //public static BoundProperties _BoundProperties = new BoundProperties();

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



            DataContext = new VoodooAPI();
        }

        public void SendInstruction()
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
                return;
            }


            if (cmb_MessageType.SelectedItem.ToString() == "")
            {
                System.Windows.MessageBox.Show("Must Enter Message Type");
                return;
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
            v.setOperation(VoodooAPI.operationType.display);
            v.setTune(music);
            v.setDisplay(Line1 + "~" + Line2, Line3 + "~" + Line4 + "~" + Line5);
            v.setColor(color);
            v.setTime(time);
            v.execute();
        }

        //*******************************************************************************
        //                                  XAML Buttons     
        //*******************************************************************************

        //--------------------------------- Send Instructions  --------------------------
        private void btn_SendInstruction_Click(object sender, RoutedEventArgs e)
        {
            SendInstruction();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            v.login();
        }

        private void btn_Logoff_Click(object sender, RoutedEventArgs e)
        {

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
                    foreach (string Icon in v.Icons.Keys)
                    {
                        cbm_Line1Icons.Items.Add(Icon);
                    }
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
                    foreach (string Icon in v.Icons.Keys)
                    {
                        cbm_Line2Icons.Items.Add(Icon);
                    }
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
                    foreach (string Icon in v.Icons.Keys)
                    {
                        cbm_Line3Icons.Items.Add(Icon);
                    }
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
                    foreach (string Icon in v.Icons.Keys)
                    {
                        cbm_Line4Icons.Items.Add(Icon);
                    }
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
                    foreach (string Icon in v.Icons.Keys)
                    {
                        cbm_Line5Icons.Items.Add(Icon);
                    }
                    break;

                default:
                    txt_Line5.Visibility = Visibility.Visible;
                    cbm_Line5Icons.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        //private void ColorDuration_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (_BoundProperties.ColorDuration > 5)
        //    {
        //        _BoundProperties.ColorDuration = 5;
        //    }
        //}

    }


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

    //*******************************************************************************
    //                                  Voodoo API    
    //*******************************************************************************
    public class VoodooAPI
    {

        public enum operationType
        {
            display,
            flash,
            opStatic,
            opStatic2,
            location
        };

        private string baseurl = "https://www.sku-keeper.com/";
        private string username;
        private string password;
        private string deviceID;
        private string line1;
        private string line2;
        private operationType op; //= operationType.display;
        private string tune;
        private short time = 10;
        private string color = "";
        private string txnID = "";
        private bool loggedIn = false;
        CookieContainer cookieContainer;

        public Dictionary<string, string> Icons = new Dictionary<string, string>();
        public Dictionary<string, string> Colors = new Dictionary<string, string>();
        public Dictionary<string, string> Music = new Dictionary<string, string>();
        public Dictionary<string, string> Images = new Dictionary<string, string>();
        public List<string> MessageType = new List<string>();

        public VoodooAPI()
        {
            FillColors();
            FillMusic();
            FillIcons();
            cookieContainer = new CookieContainer();
        }

        //*******************************************************************************
        //                                 User & Password
        //*******************************************************************************

        public void setUsernameAndPassword(string u, string p)
        {
            username = u;
            password = p;
        }

        //*******************************************************************************
        //                                 Login
        //*******************************************************************************

        public void login()
        {
            //make sure there is a username and password set
            if (username == "")
            {
                throw (new IllegalArgumentException("Username not set"));
            }
            if (password == "")
            {
                throw (new IllegalArgumentException("Password not set"));
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            request.CookieContainer = cookieContainer;

            string postData = "name=" + username + "&pass=" + password + "&form_id=user_login_block";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            response.Close();

            loggedIn = true;
        }

        //*******************************************************************************
        //                                 Execute
        //*******************************************************************************

        public void execute()
        {
            //make sure a deviceID has been set
            if (deviceID == "")
            {
                throw (new IllegalArgumentException("DeviceID not set"));
            }

            //set up the operation
            string opWord;
            switch (op)
            {
                case operationType.display:
                    opWord = "display";
                    break;
                case operationType.flash:
                    opWord = "flash";
                    break;
                case operationType.opStatic:
                    opWord = "static";
                    break;
                case operationType.opStatic2:
                    opWord = "static2";
                    break;
                case operationType.location:
                    opWord = "location";
                    break;
                default: throw (new IllegalArgumentException("Bad operation"));
            }

            //if there is no tune set, set it to none, which implies silence--not even a beep!
            if (tune == "")
            {
                tune = "none";
            }

            //are we already logged in?  No need to log in twice.
            if (!loggedIn)
            {
                login();
            }

            //if we get here and we're not logged in, there is some weird problem
            //I don't think this could really happen, because if there is an error, the login above will
            //throw an exception, and the code below will not execute.
            if (!loggedIn)
            {
                throw (new RuntimeException("Login failed!"));
            }

            //okay, set up the whole url for the get request
            //see:  https://voodoorobotics.com/constructing-a-url/
            string url = baseurl + "api/" + deviceID + "/" + opWord + "/" + line1 + "/" + line2;  //+ "" + "~" + "" + "~" + "";
            System.Windows.MessageBox.Show(url);
            switch (op)
            {
                case operationType.display:
                    url += "/" + tune + "/" + time.ToString() + color;
                    System.Windows.MessageBox.Show(url);
                    break;
                case operationType.flash:
                    url += "/" + tune + "/" + time.ToString() + color;
                    System.Windows.MessageBox.Show(url);
                    break;
                case operationType.opStatic:
                case operationType.opStatic2:
                case operationType.location:
                default:
                    break;
                    //do nothing!
            }

            //if there is a transaction ID for the closed loop, then add it here
            //see: https://voodoorobotics.com/closed-loop-system/
            if (txnID != "")
            {
                url += "/" + txnID;
                System.Windows.MessageBox.Show(url);
            }

            try
            {
                //testurl url = "https://www.sku-keeper.com/api/E40662:26F78E/message/line1~line2/line3~line4~line5/140,f5,2,g5,2a5,2g5,2/20r";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //response discarded in this demo!
                System.Windows.MessageBox.Show("Status" + response.StatusCode);
                response.Close();
            }
            catch (Exception e)
            {
                loggedIn = false;  //if there was a problem, make sure that I'm logged out.
                throw e;  //rethrow the error
            }

        }

        //*******************************************************************************
        //                                  Arguments for URL     
        //*******************************************************************************

        public void setDeviceID(string id)
        {
            deviceID = id;
        }

        public void setOperation(operationType ot)
        {
            op = ot;
        }

        public void setDisplay(string l1, string l2)
        {
            line1 = l1;
            line2 = l2;
        }

        public void setTune(string t)
        {
            tune = t;
        }

        public void setTime(short t)
        {
            time = t;
        }

        public void setColor(string t)
        {
            color = t;
        }

        public void setTransactionID(string txn)
        {
            txnID = txn;
        }

        public void FillIcons()
        {
            //TODO - Names need to make sense
            Icons.Add("NoDrink", "ichazarda");
            Icons.Add("NoWalk", "ichazardb");
            Icons.Add("NoWateronFire", "ichazardc");
            Icons.Add("NoSmoking", "ichazardd");
            Icons.Add("NoMatch", "ichazarde");
            Icons.Add("NoCup", "ichazardf");
            Icons.Add("NoEat", "ichazardg");
            Icons.Add("KeepFingersOut", "ichazardh");
            Icons.Add("NoElevator", "ichazardi");
            Icons.Add("IDK", "ichazardj");
            Icons.Add("NoLift", "ichazardkv");
            Icons.Add("WearEyeProtection", "ichazardl");
            Icons.Add("WearBoots", "ichazardm");
            Icons.Add("WearGloves", "ichazardn");
            Icons.Add("WearHelmet", "ichazardo");
            Icons.Add("WearEarProtection", "ichazardp");
            Icons.Add("WearMask", "ichazardq");
            Icons.Add("WearBodySuit", "ichazardr");
            Icons.Add("Wearharness", "ichazards");
            Icons.Add("DoorStop", "ichazardt");
            Icons.Add("HandSwitch", "ichazardu");
            Icons.Add("CoverSawBlade", "ichazardv");
            Icons.Add("WashHands", "ichazardw");
            Icons.Add("FaceShield", "ichazardx");
            Icons.Add("FaceMask", "ichazardy");
            Icons.Add("WeldingMask", "ichazardz");
            Icons.Add("Acid", "ichazardA");
            Icons.Add("ForkLift", "ichazardB");
            Icons.Add("Laser", "ichazardC");
            Icons.Add("CraneLift", "ichazardD");
            Icons.Add("Radioactive", "ichazardE");
            Icons.Add("Eletric", "ichazardF");
            Icons.Add("Explosion", "ichazardG");
            Icons.Add("Fire", "ichazardH");
            Icons.Add("Skull", "ichazardI");
            Icons.Add("CarSwerve", "ichazardJ");
            Icons.Add("PersonElectrocuted", "ichazardK");
            Icons.Add("BalonFire", "ichazardL");
            Icons.Add("IDK2", "ichazardM");
            Icons.Add("Doggo", "ichazardN");
            Icons.Add("CompressedGas", "ichazard");
            Icons.Add("StayAwayFromFood", "ichazardP");
            Icons.Add("Plus", "ichazardQ");
            Icons.Add("LeftArrow", "ichazardR");
            Icons.Add("RightArrow", "ichazardS");
            Icons.Add("UpArrow", "ichazardT");
            Icons.Add("DownArrow", "ichazardU");
            Icons.Add("TouchButton", "ichazardV");
            Icons.Add("SideFlame", "ichazardK");
            Icons.Add("FireExtingisher", "ichazardX");
            Icons.Add("Phone", "ichazardY");
            Icons.Add("DrawBridge", "ichazardZ");
            Icons.Add("ForwardSlash", "ichazard`");
            Icons.Add("EmptyDiamond", "ichazard0");
            Icons.Add("NO", "ichazard1");
            Icons.Add("FallDownStairs", "ichazard2");
            Icons.Add("Headphones", "ichazard3");
            Icons.Add("EmptyTriangle", "ichazard4");
            Icons.Add("Sniff", "ichazard5");
            Icons.Add("CraneLiftTriangle", "ichazard6");
            Icons.Add("CraneLiftSimple", "ichazard7");
            Icons.Add("RadioactiveCircle", "ichazard8");
            Icons.Add("RadioactiveDiamond", "ichazard9");
            Icons.Add("Line", "ichazard-");
            Icons.Add("NoFastFood", "ichazard=");
            Icons.Add("HandInGears", "ichazard~");
            Icons.Add("ExclamationPoint", "ichazard!");
            Icons.Add("RockSlide", "ichazard@");
            Icons.Add("BiochemicalHazard", "ichazard#");
            Icons.Add("CrackedC", "ichazard$");
            Icons.Add("LinedDiamond", "ichazard%");
            Icons.Add("ToExit", "ichazard^");
            Icons.Add("Dangerous", "ichazard&");
            Icons.Add("HazardousArea", "ichazard*");
            Icons.Add("FlameableGas", "ichazard(");
            Icons.Add("FlameCircle", "ichazard)");
            Icons.Add("ThreeTriangleInCircle", "ichazard_");
            Icons.Add("OilSpill", "ichazard+");
            Icons.Add("InjuredFinger", "ichazard{");
            Icons.Add("SlipperyIncline", "ichazard}");
            Icons.Add("CarFallinWater", "ichazard[");
            Icons.Add("MedicalWithArrow", "ichazard[");
            Icons.Add("ManFall", "ichazard|");
            Icons.Add("Dial999", "ichazard%47");
            Icons.Add("NegativeSign", "ichazard;");
            Icons.Add("PointCutHand", "ichazard:");
            Icons.Add("'", "ichazard’");
            Icons.Add("Quote", "ichazard”");
            Icons.Add("LessThan", "ichazard<");
            Icons.Add("Electric", "ichazard,");
            Icons.Add("HotSurface", "ichazard.");
            Icons.Add("/", "ichazard/");
        }

        public void FillColors()
        {
            Colors.Add("Red", "r");
            Colors.Add("Green", "g");
            Colors.Add("Blue", "b");
            Colors.Add("RedBlue", "rb");
            Colors.Add("RedGreen", "rg");
            Colors.Add("GreenBlue", "gb");
        }

        public void FillMusic()
        {
            Music.Add("Regular Beep", "15,c5,4");
            Music.Add("Macaroon", "250,c5,1,e5,1,g5,1,c6,1,g5,1,e5,1,c5,1");
            Music.Add("Marshmallow", "200,d5,3,d5,1,f5,2,f5,1,g5,1,f5,2");
            Music.Add("Meringue", "250,c6,1,a5,1,b5,1,g5,1,f5,1");
            Music.Add("Milkshake", "300,a5,1,g5s,1,f5s,1,d5,1");
            Music.Add("Mousse", "160,e5,3,e5,1,g5s,2,b5,2,a5,2");
            Music.Add("Muffin", "250,a5,1,c6,1,e6,1,c6,1,e6,1,c6,1,e6,1,c6,1,a5,1");
            Music.Add("Charge", "140,c5,2,f5,2,a5,2,c6,3,a5,1,c6,3");
            Music.Add("Charge2", "140,c5,2,f5,2,a5,2,g5,2,e5,2,f5,2,g5,2");
            Music.Add("Twilight", "140,g5s,2,a5,2,g5s,2,e5,2,g5s,2,a5,2,g5s,2,e5,2");
            Music.Add("Waterfall", "140,a5s,2,g5s,2,f5s,2,d5s,2,c5s,2,d5s,2,f5s,2");
            Music.Add("Skip Along", "140,f5,2,f5,2,d5,2,f5,2,e5,2,d5,2,c5,2");
            Music.Add("Yankee Doodle", "140,f5,2,f5,2,g5,2,a5,2,f5,2,a5,2,g5,2");
            Music.Add("Scale", "140,c5,2,d5,2,e5,2,f5,2,g5,2,f5,2,e5,2,d5,2,c5,2");
            Music.Add("Scale–Reverse", "140,c6,2,b5,2,a5,2,g5,2,f5,2,g5,2,a5,2,b5,2,c6,2");
            Music.Add("Day is Done", "140,c5,1,c5,1,f5,3,p,2,c5,1,f5,1,a5,3");
            Music.Add("Ta-Da", "140,e5,1,e5,3,g5,1,g5,3,c5,1,c5,3");
            Music.Add("Dreidel-Dreidel", "200,g5,1,c6,1,c6,1,d6,1,d6,1,e6,1,c6,2,e6,1,g6,1,g6,1,f6,1,e6,1,d6,3,a5,1,d6,1,d6,1,e6,1,e6,1,f6,1,d6,2,g6,1,g6,1,f6,1,e6,1,d6,1,c6,3,g5,1,g6,1,f6,1,e6,1,d6,1,c6,2");
            Music.Add("Taps", "250,g5,2,g5,1,c6,6,g5,2,c6,1,e6,6,g5,1,c6,1,e6,2,g5,1,c6,1,e6,2,g5,1,c6,1,e6,6,c6,2,e6,1,g6,5,e6,2,c6,1,g5,6,g5,2,g5,1,c6,6");
            Music.Add("Sakura", "200,a6,2,a6,2,b6,4,a6,2,a6,2,b6,4,a6,2,b6,2,c7,2,b6,2,a6,2,b6,1,a6,1,f6,4,e6,2,c6,2,e6,2,f6,2,e6,2,e6,1,c6,1,b5,4,a6,2,b6,2,c7,2,b6,2,a6,2,b6,1,a6,1,f6,4,e6,2,c6,2,e6,2,f6,2,e6,2,e6,1,c6,1,b5,4,a6,2,a6,2,b6,4,a6,2,a6,2,b6,4,e6,2,f6,2,b6,1,a6,1,f6,2,e6,4");
            Music.Add("Constant", "15,c5,400");
        }

        public void FillMessageType()
        {
            MessageType.Add("Display");
            MessageType.Add("Flash");
            MessageType.Add("OpStatic");
            MessageType.Add("OpStatic2");
            MessageType.Add("Location");
        }

        public void FillImages()
        {
            Images.Add("Test1", "/Images\test.png");
            Images.Add("Test2", "/Images\test2.png");
        }

    }
}