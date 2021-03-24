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
    public class VooDooAPI
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

        public VooDooAPI()
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
            Icons.Add("Do not Drink Water", "ichazarda");
            Icons.Add("Area Out of Bounds", "ichazardb");
            Icons.Add("Do not Douse Fire", "ichazardc");
            Icons.Add("No Smoking", "ichazardd");
            Icons.Add("Do Not Light Match", "ichazarde");
            Icons.Add("No Drinks", "ichazardf");
            Icons.Add("No Fork and Knife", "ichazardg");
            Icons.Add("Danger Moving Pieces", "ichazardh");
            Icons.Add("Do Not Use Elevator", "ichazardi");
            Icons.Add("Safety Harness", "ichazardj");
            Icons.Add("Lift Correctly", "ichazardkv");
            Icons.Add("Wear Eye Protection", "ichazardl");
            Icons.Add("Wear Safety Boots", "ichazardm");
            Icons.Add("Wear Safety Gloves", "ichazardn");
            Icons.Add("Wear Helmet", "ichazardo");
            Icons.Add("Wear Ear Muffs", "ichazardp");
            Icons.Add("Wear Respirator", "ichazardq");
            Icons.Add("Wear Safety Suit", "ichazardr");
            Icons.Add("Wear Safety Harness", "ichazards");
            Icons.Add("Safety Signal", "ichazardt");
            Icons.Add("Switch Off", "ichazardu");
            Icons.Add("Industial Safety and Occupational Health", "ichazardv");
            Icons.Add("Wash Hands", "ichazardw");
            Icons.Add("Face Shield", "ichazardx");
            Icons.Add("Wear Mask", "ichazardy");
            Icons.Add("Wear Welding Mask", "ichazardz");
            Icons.Add("Acid", "ichazardA");
            Icons.Add("ForkLift", "ichazardB");
            Icons.Add("Laser", "ichazardC");
            Icons.Add("Crane Lifting", "ichazardD");
            Icons.Add("Radiation", "ichazardE");
            Icons.Add("Hot Work", "ichazardF");
            Icons.Add("Danger Explosive", "ichazardG");
            Icons.Add("Flammable", "ichazardH");
            Icons.Add("Toxic Material", "ichazardI");
            Icons.Add("Slippery Road", "ichazardJ");
            Icons.Add("Danger of Death", "ichazardK");
            Icons.Add("Fire Warning", "ichazardL");
            Icons.Add("Harzard", "ichazardM");
            Icons.Add("Beware of Dog", "ichazardN");
            Icons.Add("Compressed Gas", "ichazard");
            Icons.Add("Stay Away From Food", "ichazardP");
            Icons.Add("Red Cross", "ichazardQ");
            Icons.Add("Left Arrow", "ichazardR");
            Icons.Add("Right Arrow", "ichazardS");
            Icons.Add("Up Arrow", "ichazardT");
            Icons.Add("Down Arrow", "ichazardU");
            Icons.Add("Pressing Button", "ichazardV");
            Icons.Add("Exposed Flames", "ichazardK");
            Icons.Add("Fire Extinguisher", "ichazardX");
            Icons.Add("Phone", "ichazardY");
            Icons.Add("Bridge Rising", "ichazardZ");
            Icons.Add("Forward Slash", "ichazard`");
            Icons.Add("Empty Diamond", "ichazard0");
            Icons.Add("No Symbol", "ichazard1");
            Icons.Add("Caution Stairs", "ichazard2");
            Icons.Add("Headphones", "ichazard3");
            Icons.Add("Empty Triangle", "ichazard4");
            Icons.Add("Man Smelling", "ichazard5");
            Icons.Add("Crane Lift Beam", "ichazard6");
            Icons.Add("Crane Lift Beam Simple", "ichazard7");
            Icons.Add("Radiation Circle", "ichazard8");
            Icons.Add("Radiation With Text", "ichazard9");
            Icons.Add("Line", "ichazard-");
            Icons.Add("No Junk Food", "ichazard=");
            Icons.Add("Hand Pinch", "ichazard~");
            Icons.Add("Caution", "ichazard!");
            Icons.Add("Falling Rocks", "ichazard@");
            Icons.Add("Infection Warning", "ichazard#");
            Icons.Add("C", "ichazard$");
            Icons.Add("Hazardous Material Shipping Label", "ichazard%");
            Icons.Add("Exit", "ichazard^");
            Icons.Add("Dangerous", "ichazard&");
            Icons.Add("Hazardous Area Keep Clear", "ichazard*");
            Icons.Add("Flammable Gas", "ichazard(");
            Icons.Add("Gas Fire", "ichazard)");
            Icons.Add("Nuclear", "ichazard_");
            Icons.Add("Oil Spill", "ichazard+");
            Icons.Add("Hand Injured", "ichazard{");
            Icons.Add("Slippery Slope", "ichazard}");
            Icons.Add("Do Not Park on Cliff", "ichazard[");
            Icons.Add("Emergency", "ichazard[");
            Icons.Add("Slip Hazard", "ichazard|");
            Icons.Add("Dail 999", "ichazard%47");
            Icons.Add("Not", "ichazard;");
            Icons.Add("Sharp", "ichazard:");
            Icons.Add("Quote", "ichazard’");
            Icons.Add("Double Quote", "ichazard”");
            Icons.Add("Less Than", "ichazard<");
            Icons.Add("Electricity", "ichazard,");
            Icons.Add("Hot Surface", "ichazard.");
            Icons.Add("Forward Slash Thick", "ichazard/");
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
