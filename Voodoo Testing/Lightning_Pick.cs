using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPT;

namespace Voodoo_Testing
{


    class Lightning_Pick
    {

        System.Net.IPAddress IpAddress = System.Net.IPAddress.Parse("192.168.1.254");
        Int32 iPort = 5003;
        Boolean bReturnValue = false;
        private LPT.EthernetController oEthernetController;


        public Boolean Connect()
        {
            oEthernetController = new LPT.EthernetController();

         //   oEthernetController.OnConnect -= new LPT.EthernetController.OnConnectEventHandler(oEthernetController_OnConnect);

            oEthernetController.AcceptFirstSequenceNumber = true;
            bReturnValue = oEthernetController.Connect(IpAddress.ToString(), iPort);
            bReturnValue = oEthernetController.InitializeAllModules();

            return bReturnValue;

        }

        public Boolean ActivateModule(string[] ModAddressArray, string[] MessageArray, string Color, string[] ButtonBehavior, string DisplayBehavior)
        {

            bReturnValue = oEthernetController.InitializeAllModules();

            char[] Comma = { ',' };
            Int32[] iAddressArray = { };
            Array.Resize(ref iAddressArray, ModAddressArray.Length);

            for(int x = 0; x <= ModAddressArray.Length - 1; x++)
            {
                iAddressArray[x] = Convert.ToInt32(ModAddressArray[x]);
            }

            LPT.EthernetController.ModuleButtonColor oColor;
            switch (Color)
            {
                case "Red":
                    oColor = EthernetController.ModuleButtonColor.Red;
                    break;
                case "Yellow":
                    oColor = EthernetController.ModuleButtonColor.Yellow;
                    break;
                case "Green":
                    oColor = EthernetController.ModuleButtonColor.Green;
                    break;
                case "Cyan":
                    oColor = EthernetController.ModuleButtonColor.Cyan;
                    break;
                case "Blue":
                    oColor = EthernetController.ModuleButtonColor.Blue;
                    break;
                case "Purple":
                    oColor = EthernetController.ModuleButtonColor.Purple;
                    break;
                case "White":
                    oColor = EthernetController.ModuleButtonColor.White;
                    break;
                default:
                    oColor = EthernetController.ModuleButtonColor.Red;
                    break;

            }

            LPT.EthernetController.ModuleBehavior oButtonBehavior;

            if(ButtonBehavior[0] == "Enabled")
            {
                switch (ButtonBehavior[1])
                {
                    case "Flash":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeFlash;
                        break;
                    case "On":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOn;
                        break;
                    case "Off":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOffWithButtonEnabled;
                        break;
                    default:
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOn;
                        break;
                }


            }
            else
            {
                switch (ButtonBehavior[1])
                {
                    case "Flash":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeFlashWithButtonDisabled;
                        break;
                    case "On":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOnWithButtonDisabled;
                        break;
                    case "Off":
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOff;
                        break;
                    default:
                        oButtonBehavior = EthernetController.ModuleBehavior.ModeOff;
                        break;
                }
            }

            LPT.EthernetController.ModuleBehavior oDisplayBehavior;

            switch (DisplayBehavior)
            {
                case "On":
                    oDisplayBehavior = EthernetController.ModuleBehavior.ModeOn;
                    break;
                case "Off":
                    oDisplayBehavior = EthernetController.ModuleBehavior.ModeOff;
                    break;
                case "Flash":
                    oDisplayBehavior = EthernetController.ModuleBehavior.ModeFlash;
                    break;
                default:
                    oDisplayBehavior = EthernetController.ModuleBehavior.ModeOff;
                    break;
            }

            bReturnValue = oEthernetController.ActivateModule(iAddressArray, MessageArray, oColor, oButtonBehavior, oDisplayBehavior, LPT.EthernetController.ModuleModelType.MWU2040, LPT.EthernetController.ActivateCommandType.PP5);

            return bReturnValue;

        }

        public void MaintanenanceModules()
        {
            oEthernetController.InitializeAllModules();
            oEthernetController.MaintenanceAllModules();
        }

    }
}
