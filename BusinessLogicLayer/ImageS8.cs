using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Transactions;

using Global.Class.Library;
using DataTransferObject;

using DataAccessLayer;
using DataAccessLayer.DataDetailTableAdapters;
using DataAccessLayer.DataMessageDTSTableAdapters;


namespace BusinessLogicLayer.BarcodeScanner
{
    public class ImageS8 : CommonThreadProperty
    {
        private PrinterFaultStatus[] printerFaultStatus;

        private DateTime cartonDateFrom;
        public DateTime CartonDateFrom
        {
            get { return cartonDateFrom; }
            set { cartonDateFrom = value; }
        }

        private DateTime cartonDateTo;
        public DateTime CartonDateTo
        {
            get { return cartonDateTo; }
            set { cartonDateTo = value; }
        }



        private bool warningSetMessage;
        public bool WarningSetMessage
        {
            get { return warningSetMessage; }
            set { warningSetMessage = value; }
        }

        private DataMessageMaster dataMessageMaster;
        public DataMessageMaster DataMessageMaster
        {
            get { return this.dataMessageMaster; }
            set { this.dataMessageMaster = value; this.WarningSetMessage = true; }
        }



        #region ------------A--------------

        private bool printerReady;
        public bool PrinterReady
        {
            get
            {
                return this.printerReady;
            }

            protected set
            {
                this.printerReady = value;
                this.NotifyPropertyChanged("PrinterReady");
            }
        }

        DataDetailCartonTableAdapter cartonTableAdapter;
        protected DataDetailCartonTableAdapter CartonTableAdapter
        {
            get
            {
                if (cartonTableAdapter == null) cartonTableAdapter = new DataDetailCartonTableAdapter();
                return cartonTableAdapter;
            }
        }




        double counterValue;
        public double CounterValue
        {
            get
            {
                return counterValue;
            }

            protected set
            {
                if (counterValue != value)
                {
                    counterValue = Math.Round(value, 0);
                    this.NotifyPropertyChanged("CounterValue");

                    if (this.CounterValue > this.DataMessageMaster.CounterValue)
                    {
                        this.UpdateCounter(this.CounterValue, -1);
                        this.AddDataMessageCounterLog();
                    }

                }
            }
        }

        public bool WarningCounterValue
        { get { return this.CounterValue < this.DataMessageMaster.CounterValue; } }

        double counterAutonics;
        public double CounterAutonics
        {
            get
            {
                return counterAutonics;
            }

            protected set
            {
                if (counterAutonics != value)
                {
                    if (value == 0)
                        value = 0;
                    counterAutonics = Math.Round(value, 0);
                    this.NotifyPropertyChanged("CounterAutonics");

                    if (counterAutonics == this.StartCounterValue && this.DataMessageMaster.CounterAutonics > 100) this.SetCDChanged(); // LEMINHHIEP04NOV2017: CAU LENH NAY BO SUNG NGAY 11/NOV/2017: COUNTER VE 0 XEM NHU CO TIN HIEU DAO CAT

                    if (this.CounterAutonics > this.DataMessageMaster.CounterAutonics)
                    {
                        this.UpdateCounter(-1, this.CounterAutonics);
                        this.AddDataMessageCounterLog();
                    }

                }
            }
        }

        public bool WarningCounterAutonics
        { get { return this.CounterAutonics < this.DataMessageMaster.CounterAutonics; } }


        private double autonicsPreScaleValue = 1;

        public double UserInputCounterValue = -1;

        DataMessageMasterTableAdapter dataMessageMasterTableAdapter;
        protected DataMessageMasterTableAdapter DataMessageMasterTableAdapter
        {
            get
            {
                if (dataMessageMasterTableAdapter == null) dataMessageMasterTableAdapter = new DataMessageMasterTableAdapter();
                return dataMessageMasterTableAdapter;
            }
        }

        public bool UpdateCounter(double counterValue, double counterAutonics)
        {
            try
            {
                if (this.DataMessageMaster.DataMessageID <= 0) return false;

                if (this.DataMessageMasterTableAdapter.DataMessageMasterUpdateCounter(this.DataMessageMaster.DataMessageID, GlobalVariables.GlobalUserInformation.UserID, counterValue, counterAutonics) == 1)
                {
                    if (counterValue >= 0) this.NotifyPropertyChanged("UpdateCounterValue");
                    if (counterAutonics >= 0) this.NotifyPropertyChanged("UpdateCounterAutonics");
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }

        #endregion ------------A--------------

        private TcpClient inkJetTcpClient;
        private NetworkStream inkJetNetworkStream;

        private IPAddress ipAddress = IPAddress.Parse("192.168.1.19");
        private int portNumber = 2101;

        IOPortRS232 ioPortRS232ImageS8;
        IOPortRS232 ioPortRS232Autonis;

        #region Contructor

        public ImageS8()
        {
            try
            {
                this.dataMessageMaster = new DataMessageMaster();

                this.ioPortRS232ImageS8 = new IOPortRS232(GlobalVariables.ImageS8PortName, 38400, Parity.None, 8, StopBits.One, true);
                this.ioPortRS232Autonis = new IOPortRS232(GlobalVariables.AutonicsPortName, 9600, Parity.None, 8, StopBits.Two, true);

                this.printerFaultStatus = new PrinterFaultStatus[9];
                foreach (GlobalVariables.PrinterFaultCategory printerFaultCategoryID in (GlobalVariables.PrinterFaultCategory[])Enum.GetValues(typeof(GlobalVariables.PrinterFaultCategory)))
                    this.printerFaultStatus[(int)printerFaultCategoryID] = new PrinterFaultStatus(printerFaultCategoryID);

                this.ioPortRS232ImageS8.PropertyChanged += new PropertyChangedEventHandler(ioPortRS232_PropertyChanged);
                this.ioPortRS232Autonis.PropertyChanged += new PropertyChangedEventHandler(ioPortRS232_PropertyChanged);
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        void ioPortRS232_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                PropertyInfo prop = this.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                    prop.SetValue(this, sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null), null);
                else
                    this.MainStatus = e.PropertyName + ": " + sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null).ToString();
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }


        public void Initialize()
        {
            try
            {
                this.PrinterReady = false;
                this.CounterValue = 0;
                this.CounterAutonics = 0;

                this.CartonDateFrom = DateTime.Today;
                this.CartonDateTo = DateTime.Today.AddHours(24).AddSeconds(-1);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        #endregion Contructor


        #region Public Properties

        public bool LedPrinterFaultState(GlobalVariables.PrinterFaultCategory printerFaultCategoryID)
        {
            return this.printerFaultStatus[(int)printerFaultCategoryID].FaultState;
        }

        public string LedPrinterFaultStatus(GlobalVariables.PrinterFaultCategory printerFaultCategoryID)
        {
            return this.printerFaultStatus[(int)printerFaultCategoryID].FaultStatus;
        }

        public void SetPrinterFaultStatus(GlobalVariables.PrinterFaultCategory printerFaultCategoryID, bool faultState)
        {
            if (this.printerFaultStatus[(int)printerFaultCategoryID].FaultState != faultState)
            {
                this.printerFaultStatus[(int)printerFaultCategoryID].FaultState = faultState;
                this.NotifyPropertyChanged("LedPrinterFaultState");
            }
        }

        public void SetPrinterFaultStatus(GlobalVariables.PrinterFaultCategory printerFaultCategoryID, string faultStatus)
        {
            if (this.printerFaultStatus[(int)printerFaultCategoryID].FaultStatus != faultStatus)
            {
                this.printerFaultStatus[(int)printerFaultCategoryID].FaultStatus = faultStatus;
                //this.MainStatus = printerFaultCategoryID.ToString() + ": " + faultStatus;
                this.NotifyPropertyChanged("LedPrinterFaultStatus");
            }
        }

        #endregion Public Properties

        #region Public Method


        public bool Connect()
        {
            try
            {
                //                return true; //TEST ONLY


                if (GlobalVariables.AX350)
                {
                    this.inkJetTcpClient = new TcpClient();

                    if (!this.inkJetTcpClient.Connected)
                    {
                        this.inkJetTcpClient.Connect(this.ipAddress, this.portNumber);
                        this.inkJetNetworkStream = inkJetTcpClient.GetStream();
                    }
                }

                this.ioPortRS232ImageS8.Connect();
                this.ioPortRS232Autonis.Connect();


                this.LedGreenOn = true;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");

                this.PrinterReady = true;
                this.MainStatus = "Kết nối thành công";

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }

        }

        private bool Disconnect()
        {
            try
            {
                if (GlobalVariables.AX350)
                {
                    if (this.inkJetNetworkStream != null) { this.inkJetNetworkStream.Close(); this.inkJetNetworkStream.Dispose(); }

                    if (this.inkJetTcpClient != null) this.inkJetTcpClient.Close();
                }


                this.ioPortRS232ImageS8.Disconnect();
                this.ioPortRS232Autonis.Disconnect();


                this.LedGreenOn = false;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");

                foreach (GlobalVariables.PrinterFaultCategory printerFaultCategoryID in (GlobalVariables.PrinterFaultCategory[])Enum.GetValues(typeof(GlobalVariables.PrinterFaultCategory)))
                    this.SetPrinterFaultStatus(printerFaultCategoryID, false);

                this.PrinterReady = false;
                this.MainStatus = "Đã ngắt kết nối.";

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }

        }

        public void SetIOPortRS232AutonisDSR(bool enabledOrDisabled)
        {
            try
            {
                this.ioPortRS232Autonis.SetPinRTS(enabledOrDisabled);
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }




        private void WriteToStream(string stringWriteTo)
        {
            try
            {
                //this.MainStatus = ""; this.MainStatus = stringWriteTo;
                GlobalNetSockets.WriteToStream(inkJetNetworkStream, stringWriteTo);
            }
            catch (Exception exception)
            { throw exception; }
        }

        //**********************************************


        public bool ReadFromStream(bool responseACK, ref string stringReadFrom)
        {
            try
            {
                stringReadFrom = GlobalNetSockets.ReadFromStream(inkJetTcpClient, inkJetNetworkStream);

                //this.MainStatus = ""; this.MainStatus = stringReadFrom;

                if (stringReadFrom.Length > 0)
                {
                    if (responseACK)
                        return stringReadFrom.ElementAt(0) == GlobalVariables.charACK;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch
            {
                stringReadFrom = "";
                return false;
            }
        }


        public bool ReadFromStream(bool responseACK)
        {
            string stringReadFrom = "";
            return this.ReadFromStream(responseACK, ref stringReadFrom);
        }

        //**********************************************




        public void SendToDOMINO(string stringToWrite)
        {
            if (GlobalVariables.AX350)
                this.WriteToStream(stringToWrite);
            else
                this.SendToDOMINO(stringToWrite);
        }

        public bool ReadFromDOMINO(bool waitForACK)
        {
            string stringReadFrom = "";
            return this.ReadFromDOMINO(waitForACK, ref stringReadFrom);
        }

        private bool ReadFromDOMINO(bool waitForACK, ref string stringReadFrom)
        {
            if (GlobalVariables.AX350)
                return this.ReadFromStream(waitForACK, ref stringReadFrom);
            else
                return this.ioPortRS232ImageS8.ReadFromSerial(waitForACK, ref stringReadFrom);
        }

        #endregion Public Method





        #region Public Thread


        private bool onPrinting;
        private bool resetMessage;

        private bool OnPrinting
        {
            get { return this.onPrinting; }
            set { this.onPrinting = value; this.resetMessage = true; }
        }


        public void StartPrint() { this.OnPrinting = true; }
        public void StopPrint() { this.OnPrinting = false; }


        private bool setMessage;
        public void SetMessage() { this.setMessage = true; }

        private bool setCounter;
        public void SetCounter() { this.setCounter = true; }



        private int StartCounterValue = 4;
        // LEMINHHIEP04NOV2017: METHOD SetCDChanged NAY BO SUNG NGAY 11/NOV/2017: DUNG DE SET CDChanged. TRUOC DO CAU LENH NAY NAM 2014 DUOC DUNG DE TEST ONLY
        public void SetCDChanged() { this.ioPortRS232Autonis.CDChanged = true; this.CounterAutonics = this.StartCounterValue; } //TEST ONLY  -- INIT this.CounterAutonics = this.StartCounterValue


        public void ClearCDChanged() { this.ioPortRS232Autonis.CDChanged = false; }

        public void ThreadRoutine()
        {// TU KHOA UPDATE PHAN MEM 2017: LEMINHHIEP04NOV2017
            string stringHexCommand = ""; bool setupCounterSuccessfull = false;
            byte[] returnResultBuffer = new byte[0];
            CRCModbus crcModbus = new CRCModbus();

            int loopCount = 0; int loopReset = 1000; int loopCheckPrinter = 100; int loopCheckFault = 100; int loopCheckCounter = 10; int repeatedTimes = 0;

            this.LoopRoutine = true; this.StopPrint();

            try
            {
                //LAY HEX COMMAND DE TEST HERCULES this.ReadCounterValue(true);//ImageS8

                if (!this.Connect()) throw new System.InvalidOperationException("NMVN: Can not connect to printer and/ or counter.");
                else this.WarningSetMessage = true;

                

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    loopCount++;

                    if (setupCounterSuccessfull)
                    {




                        if (loopCount % loopCheckPrinter == 0) //Check printer
                        {
                            stringHexCommand = GlobalStaticFunction.IntToHexString((int)GlobalEnum.ImageS8Command.CheckPrinterReady);
                            this.SendToDOMINO(stringHexCommand);
                            this.PrinterReady = this.ReadFromDOMINO(true);  //??????this.ReadFromDOMINO(true, out returnResultBuffer);

                            Thread.Sleep(10);
                        }



                        if (loopCount % loopCheckFault == 0) //Check printer
                        {
                            this.ReadPrinterFault();
                            Thread.Sleep(10);
                        }



                        if (loopCount % loopCheckCounter == 0 || this.setCounter) //Check counter (Enforce to read Autonics Counter when this.setCounter)
                        {


                            //BEGIN.Autonis
                            byte[] arrayByteCommand;
                            byte[] array2BytesCRC;


                            //DO NOT NEED TO GET Prescale decimal point position. (This autonics counter at SONG THAN is fixed with: x.xxxxx: 5 decimals)
                            ////BEGIN: FUNCTION: 03    ADDRESS 00,3B (Prescale decimal point position)
                            //stringHexCommand = "01,03,00,3B,00,01";
                            //arrayByteCommand = GlobalStaticFunction.StringToByteArrayFastest(stringHexCommand.Replace(",", ""));
                            //array2BytesCRC = BitConverter.GetBytes(crcModbus.crc16(arrayByteCommand, arrayByteCommand.Length));

                            //this.ioPortRS232Autonis.SendToSerial(stringHexCommand + "," + array2BytesCRC[1].ToString("X2") + "," + array2BytesCRC[0].ToString("X2"));
                            //if (this.ioPortRS232Autonis.ReadFromSerial(false, out returnResultBuffer)) ????;
                            ////END: FUNCTION: 03    ADDRESS 00,3B



                            ////////BEGIN: FUNCTION: 03    ADDRESS 00,3C (Prescale value)
                            //////stringHexCommand = "01,03,00,3C,00,01";
                            //////arrayByteCommand = GlobalStaticFunction.StringToByteArrayFastest(stringHexCommand.Replace(",", ""));
                            //////array2BytesCRC = BitConverter.GetBytes(crcModbus.crc16(arrayByteCommand, arrayByteCommand.Length));

                            //////this.ioPortRS232Autonis.SendToSerial(stringHexCommand + "," + array2BytesCRC[1].ToString("X2") + "," + array2BytesCRC[0].ToString("X2"));
                            //////if (this.ioPortRS232Autonis.ReadFromSerial(false, out returnResultBuffer)) this.TryPickupAutonisPreScaleValue(returnResultBuffer);
                            ////////END: FUNCTION: 03    ADDRESS 00,3C


                            ////////this.MainStatus = ""; this.MainStatus = "TRY TO READ AUTONICS"; 
                            //BEGIN: FUNCTION: 04    ADDRESS 03,EB (Present value of counter)
                            stringHexCommand = "01,04,03,EB,00,02"; //XEM SAMPLE TRONG AUTONIS DOCUMENT, LUU Y: NO.POINT: DOI VOI 4DIGIT COUNTER: 01, 6DIGIT: 02 => VI VAY: COMMAND 4DIGIT: "01,04,03,EB,00,01", COMMAND 6DIGIT: "01,04,03,EB,00,02"
                            arrayByteCommand = GlobalStaticFunction.StringToByteArrayFastest(stringHexCommand.Replace(",", ""));
                            array2BytesCRC = BitConverter.GetBytes(crcModbus.crc16(arrayByteCommand, arrayByteCommand.Length));

                            this.ioPortRS232Autonis.SendToSerial(stringHexCommand + "," + array2BytesCRC[1].ToString("X2") + "," + array2BytesCRC[0].ToString("X2"));
                            if (this.ioPortRS232Autonis.ReadFromSerial(false, out returnResultBuffer)) this.TryPickupAutonisCounter(returnResultBuffer);

                            //////////this.MainStatus = "AUTONICS - VALUE: "; returnResultBuffer.ToString();
                            //END: FUNCTION: 04    ADDRESS 03,EB

                            //END.Autonis


                            Thread.Sleep(10);



                            if (!this.setCounter) //Ignore to read ImageS8.CounterValue when this.setCounter
                            {
                                this.ReadCounterValue(true);//ImageS8
                                Thread.Sleep(10);
                            }
                        }
                        else
                            Thread.Sleep(10);





                        if (!this.ioPortRS232Autonis.CDChanged && this.setCounter) //Remember: setCounter should place before setMessage in loop, because after setMessage, it will call setCounter => it should wait for the next loop to read CounterAutonics in order to setCounter
                        {
                            if (this.CounterAutonics % 2 == 0 && this.SetImageS8Counter())
                                this.setCounter = false;

                            Thread.Sleep(10);
                        }






                        if (!this.ioPortRS232Autonis.CDChanged && this.setMessage)
                        {
                            if (this.SendMessageToImageS8())
                            {
                                this.SetCounter(); //Force to reset counter

                                this.setMessage = false;
                            }
                            Thread.Sleep(10);
                        }
                        else
                            if (loopCount % loopCheckPrinter != 0 && loopCount % loopCheckCounter != 0) Thread.Sleep(30);






                        // TU KHOA UPDATE PHAN MEM 2017: LEMINHHIEP04NOV2017
                        if (this.ioPortRS232Autonis.CDChanged)
                        {
                            if (this.DataMessageMaster.DataMessageID > 0 && this.DataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.OnPrinting)
                            {
                                if (this.CounterAutonics == this.StartCounterValue)
                                {
                                    repeatedTimes = 0;
                                    this.NotifyPropertyChanged("CDChanged");
                                }
                                else
                                {//Wait for this.CounterAutonics == this.StartCounterValue Within 5 times * 1000ms (5 Second) before cancel
                                    if (repeatedTimes++ < 100)
                                        Thread.Sleep(50);
                                    else
                                    {
                                        repeatedTimes = 0;
                                        this.ioPortRS232Autonis.CDChanged = false;
                                    }
                                }
                            }
                            else
                                this.ioPortRS232Autonis.CDChanged = false;
                        }






                        if (loopCount % loopReset == 0) loopCount = 0;
                    }
                    else
                    {
                        setupCounterSuccessfull = this.SetupImageS8Counter();
                        if (!setupCounterSuccessfull)
                        {
                            Thread.Sleep(100);
                            setupCounterSuccessfull = true; //test 11.FEB.14
                        }
                        //else //Force to reset counter????: KHI CHANGE COUNTER SEETING => COUNTER RESET = 0, BUT: SETUP WITHOUT CHANGE SEETING => DON'T RESET COUNTER => NO NEED TO Force to reset counter                        
                    }

                } //End while this.LoopRoutine

            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message; // ToString();

                this.LedRedOn = true;
                this.NotifyPropertyChanged("LedStatus");
            }
            finally
            {
                this.Disconnect();
            }



        }



        private bool SendMessageToImageS8()
        {
            try
            {
                if (this.DataMessageMaster.DataMessageID <= 0) { this.MainStatus = "Vui lòng xác định bản in"; return false; }


                string stringHexCommand = ""; int repeatedTimes = 0;

                stringHexCommand = "01,0A,"; //Jet 1, Start 1 line 0A

                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS81(true);
                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS82(true);

                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS83(true);
                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS84(true);

                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS85(true);

                stringHexCommand = stringHexCommand + this.DataMessageMaster.DisplayImageS86(true);

                stringHexCommand = stringHexCommand + "0D"; //Delimiter of end of message 

                stringHexCommand = stringHexCommand.Replace(" ", "");

                string[] arrayHexCommand = stringHexCommand.Split(new Char[] { (char)44 }); //For calculate command lenght
                stringHexCommand = "0A,00," + GlobalStaticFunction.NumericTextToHEX(arrayHexCommand.Length.ToString()).ToUpper() + "," + stringHexCommand;

                stringHexCommand = stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand));

                do
                {
                    this.SendToDOMINO(stringHexCommand);
                    if (this.ReadFromDOMINO(true))
                    {
                        this.WarningSetMessage = false;
                        this.MainStatus = "";
                        this.MainStatus = "Cài đặt bản in thành công";
                        this.AddDataMessageDetail("Cài đặt bản in", this.DataMessageMaster.Display1 + " " + this.DataMessageMaster.Display2 + " " + this.DataMessageMaster.Display3 + " " + this.DataMessageMaster.Display4 + " " + this.DataMessageMaster.Display5 + " " + this.DataMessageMaster.Display6, this.CounterAutonics, "", "");
                        return true;
                    }
                    else
                        if (repeatedTimes < 5)
                            Thread.Sleep(50);
                        else
                        {
                            this.MainStatus = "Lỗi cài đặt bản in.";
                            return false;
                        }
                }
                while (repeatedTimes++ < 5);
                return false;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }
        }



        private bool SetupImageS8Counter()
        {
            try
            {
                string stringHexCommand = "38,00,18,01,   30,30,30,30,30,30,30,30,30,   39,39,39,39,39,39,39,39,39,   30,32,   00,00,01"; //SET counter step TO 2
                stringHexCommand = stringHexCommand.Replace(" ", ""); int repeatedTimes = 0;
                stringHexCommand = stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand));
                do
                {
                    this.SendToDOMINO(stringHexCommand);

                    if (this.ReadFromDOMINO(true))
                    {
                        this.MainStatus = "";
                        this.MainStatus = "Định dạng số mét ok";
                        return true;
                    }
                    else
                        if (repeatedTimes < 5)
                            Thread.Sleep(50);
                        else
                        {
                            this.MainStatus = "Lỗi định dạng số mét";
                            return false;
                        }
                }
                while (repeatedTimes++ < 5);
                return false;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }
        }



        private bool SetImageS8Counter()
        {
            try
            {
                string stringHexCommand = ""; int repeatedTimes = 0;
                do
                {
                    double counterValueSendToUpdate = this.UserInputCounterValue == -1 ? this.CounterAutonics : this.UserInputCounterValue;

                    stringHexCommand = GlobalStaticFunction.IntToHexString((int)GlobalEnum.ImageS8Command.SetCounterValue) + ",00,0A,01," + GlobalStaticFunction.TextToHEX(counterValueSendToUpdate.ToString("000000000"));
                    this.SendToDOMINO(stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand)));

                    if (this.ReadFromDOMINO(true))
                    {
                        Thread.Sleep(3000);
                        double counterValueAfterUpdate = this.ReadCounterValue(false);

                        if (counterValueAfterUpdate == counterValueSendToUpdate || counterValueAfterUpdate <= counterValueSendToUpdate + 14)
                        {
                            if (this.UserInputCounterValue != -1) { this.UpdateCounter(counterValueAfterUpdate, -1); this.UserInputCounterValue = -1; } //Ep phai: UpdateCounter. Boi vi: T/h thong thuong: chi UpdateCounter khi va chi khi so CounterValue > last CounterValue (Xem Set CounterValue property for more information)
                            this.MainStatus = ""; this.MainStatus = "Cài đặt số mét thành công";
                            this.AddDataMessageDetail("Cài đặt số mét", "", counterValueAfterUpdate, "", "");
                            return true;
                        }
                        else
                        {
                            this.MainStatus = "Gửi số mét thành công \r\nTuy nhiên cài đặt thất bại";
                            return false;
                        }
                    }
                    else
                        if (repeatedTimes < 3)
                            Thread.Sleep(50);
                        else
                        {
                            this.MainStatus = "Lỗi cài đặt số mét";
                            return false;
                        }
                }
                while (repeatedTimes++ < 3);
                return false;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }
        }


        private double ReadCounterValue(bool updateCounterValue)
        {
            try
            {
                string stringHexCommand = "";
                string returnResultBuffer = "";

                stringHexCommand = GlobalStaticFunction.IntToHexString((int)GlobalEnum.ImageS8Command.RequestCounterValue) + ",00,01,01";

                this.SendToDOMINO(stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand)));
                if (this.ReadFromDOMINO(true, ref returnResultBuffer))
                    return this.TryPickupCounterValue(returnResultBuffer, updateCounterValue);
                else
                    return 0;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return 0;
            }
        }


        private double TryPickupCounterValue(string counterStringReturned, bool updateCounterValue)
        {
            try
            {
                string counterString = ""; double counterDouble = 0;
                if (counterStringReturned.Length >= 14)//OLD VERSION FOR MAJE S8: 17, NEW VERSION 9040: 14 
                {
                    for (int i = 3; i <= 11; i++) //OLD VERSION FOR MAJE S8: for (int i = 4; i <= 13; i++), NEW VERSION 9040: for (int i = 3; i <= 11; i++)
                    {
                        counterString = counterString + ((char)(counterStringReturned.ElementAt(i))).ToString();
                        int x = (int)(char)(counterStringReturned.ElementAt(i));
                    }
                }
                if (double.TryParse(counterString, out counterDouble) && updateCounterValue)
                    this.CounterValue = counterDouble;

                return counterDouble;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return 0;
            }
        }


        private void TryPickupAutonisCounter(byte[] counterStringReturned) //FUNCTION: 04    ADDRESS 03,EB
        {
            try
            {
                double counterDouble = 0;
                if (counterStringReturned.Length >= 7)
                    counterDouble = Convert.ToInt32(GlobalStaticFunction.ByteArrayToHexString(counterStringReturned).Replace(" ", "").Substring(6, 4), 16);

                if (counterDouble > 0) this.CounterAutonics = counterDouble;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }


        private void TryPickupAutonisPreScaleValue(byte[] preScaleValueStringReturned) //FUNCTION: 03    ADDRESS 00,3C
        {
            try
            {
                string preScaleValueString = ""; double preScaleValue;
                if (preScaleValueStringReturned.Length >= 7)
                {
                    preScaleValueString = GlobalStaticFunction.ByteArrayToHexString(preScaleValueStringReturned).Replace(" ", "").Substring(6, 4);
                    preScaleValueString = (Convert.ToInt32(preScaleValueString, 16) - Convert.ToInt32("86A0", 16)).ToString(); //Because the return hex value start at:  1.00000 = "86A0" (ROOT VALUE), any returned value should MINUS (-) this ROOT VALUE

                    if (preScaleValueString.Length <= 5)
                    {
                        preScaleValueString = "1." + new string('0', 5 - preScaleValueString.Length) + preScaleValueString;

                        if (double.TryParse(preScaleValueString, out preScaleValue))
                            this.autonicsPreScaleValue = preScaleValue;
                    }
                }
            }
            catch (Exception exception)
            {
                this.autonicsPreScaleValue = 1;
                this.MainStatus = exception.Message;
            }
        }



        private void ReadPrinterFault()
        {
            try
            {
                string stringHexCommand = "";
                string returnResultBuffer = ""; 

                stringHexCommand = GlobalStaticFunction.IntToHexString((int)GlobalEnum.ImageS8Command.CheckPrinterFault) + ",00,00";

                this.SendToDOMINO(stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand)));
                if (this.ReadFromDOMINO(true, ref returnResultBuffer))
                {
                    if (returnResultBuffer.Length == 22) //17 bytes of data, from [4 to 20), zero base
                    {
                        foreach (GlobalVariables.PrinterFaultCategory printerFaultCategoryID in (GlobalVariables.PrinterFaultCategory[])Enum.GetValues(typeof(GlobalVariables.PrinterFaultCategory)))
                        {
                            if (printerFaultCategoryID != GlobalVariables.PrinterFaultCategory.Head1Unserviceable && printerFaultCategoryID != GlobalVariables.PrinterFaultCategory.PrintingSpeed) //PrintingSpeed = 8: never check this!!! Very important
                                this.SetPrinterFaultStatus(printerFaultCategoryID, IsBitSet(returnResultBuffer.ElementAt(4), (int)printerFaultCategoryID));
                        }

                        this.SetPrinterFaultStatus(GlobalVariables.PrinterFaultCategory.PrintingSpeed, IsBitSet(returnResultBuffer.ElementAt(9), 2));


                        #region GlobalVariables.PrinterFaultCategory.Head1Unserviceable
                        bool faultState = false; string faultStatus = "Đầu in";
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(7), 0); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(7), 0) ? "\n\rHard printing" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(7), 5); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(7), 5) ? "\n\rRaster generator" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(7), 6); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(7), 6) ? "\n\rCharactor generator" : "");

                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(8), 4); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(8), 4) ? "\n\rNắp đầu in" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(8), 5); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(8), 5) ? "\n\rTHT" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(8), 6); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(8), 6) ? "\n\rRecuperation" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(8), 7); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(8), 7) ? "\n\rPhase detection" : "");

                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 0); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 0) ? "\n\rKhông có đầu in" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 1); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 1) ? "\n\rLỗi giao tiếp giữa CPU và IMP" : "");
                        //faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 2); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 2) ? "\n\rLỗi tốc độ in" : ""); //Tach rieng ra thanh 1 loi
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 3); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 3) ? "\n\rLỗi lọc DTOP" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 4); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 4) ? "\n\rChưa cài bản in" : "");
                        faultState = faultState | IsBitSet(returnResultBuffer.ElementAt(9), 5); faultStatus = faultStatus + (IsBitSet(returnResultBuffer.ElementAt(9), 5) ? "\n\rLỗi bản in" : "");

                        this.SetPrinterFaultStatus(GlobalVariables.PrinterFaultCategory.Head1Unserviceable, faultState);
                        this.SetPrinterFaultStatus(GlobalVariables.PrinterFaultCategory.Head1Unserviceable, faultStatus);
                        #endregion GlobalVariables.PrinterFaultCategory.Head1Unserviceable



                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        private bool IsBitSet(char charChecked, int positionChecked)//Zero base: pos 0 is least significant bit, pos 7 is most.
        {
            return this.IsBitSet(Convert.ToByte(charChecked), positionChecked);
        }

        private bool IsBitSet(byte byteChecked, int positionChecked)//Zero base: pos 0 is least significant bit, pos 7 is most.
        {
            return (byteChecked & (1 << positionChecked)) != 0;
        }


        private DataMessageDetailTableAdapter detailTableAdapter;
        protected DataMessageDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new DataMessageDetailTableAdapter();
                return detailTableAdapter;
            }
        }

        public bool AddDataMessageDetail(string sendType, string sendMessage, double counterValue, string description, string remarks)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    DataMessageDTS.DataMessageDetailDataTable dataMessageDetailDataTable = new DataMessageDTS.DataMessageDetailDataTable();
                    DataMessageDTS.DataMessageDetailRow dataMessageDetailRow = dataMessageDetailDataTable.NewDataMessageDetailRow();

                    dataMessageDetailRow.DataMessageID = this.DataMessageMaster.DataMessageID;

                    dataMessageDetailRow.SendDate = DateTime.Now;
                    dataMessageDetailRow.SendType = sendType;
                    dataMessageDetailRow.SendMessage = sendMessage;
                    dataMessageDetailRow.CounterValueBefore = this.CounterValue;
                    dataMessageDetailRow.CounterValueAfter = counterValue;
                    dataMessageDetailRow.CounterAutonicsBefore = this.CounterAutonics;
                    dataMessageDetailRow.CounterAutonicsAfter = this.CounterAutonics;
                    dataMessageDetailRow.Description = description;
                    dataMessageDetailRow.Remarks = remarks;

                    dataMessageDetailDataTable.AddDataMessageDetailRow(dataMessageDetailRow);

                    if (this.DetailTableAdapter.Update(dataMessageDetailDataTable) != 1) throw new System.ArgumentException("Lỗi lưu nhật ký", "Save detail");

                    transactionScope.Complete();
                }

                this.NotifyPropertyChanged("AddDataMessageDetail");
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        private DataMessageCounterLogTableAdapter counterLogTableAdapter;
        protected DataMessageCounterLogTableAdapter CounterLogTableAdapter
        {
            get
            {
                if (counterLogTableAdapter == null) counterLogTableAdapter = new DataMessageCounterLogTableAdapter();
                return counterLogTableAdapter;
            }
        }

        public bool AddDataMessageCounterLog()
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    DataMessageDTS.DataMessageCounterLogDataTable DataMessageCounterLogDataTable = new DataMessageDTS.DataMessageCounterLogDataTable();
                    DataMessageDTS.DataMessageCounterLogRow DataMessageCounterLogRow = DataMessageCounterLogDataTable.NewDataMessageCounterLogRow();

                    DataMessageCounterLogRow.DataMessageID = this.DataMessageMaster.DataMessageID;

                    DataMessageCounterLogRow.EntryDate = DateTime.Now;

                    DataMessageCounterLogRow.CounterValue = this.CounterValue;
                    DataMessageCounterLogRow.CounterAutonics = this.CounterAutonics;

                    DataMessageCounterLogDataTable.AddDataMessageCounterLogRow(DataMessageCounterLogRow);

                    if (this.CounterLogTableAdapter.Update(DataMessageCounterLogDataTable) != 1) throw new System.ArgumentException("Lỗi lưu nhật ký counter", "Save detail");

                    transactionScope.Complete();
                }

                this.NotifyPropertyChanged("AddDataMessageCounterLog");
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        #region Public Thread test



        private void toolStripButtonCompose_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GlobalStaticFunction.TextToHEX("THEP SONG THAN" ));

            ////////////byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,13,01,0A,02,38,49,4D,41,4A,45,20,02,54,46,52,41,4E,43,45,0D");

            ////////////byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,01,53,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,54,46,52,41,4E,43,45,1E,1E,1E,0D");
            //byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,1C,01,53,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,54,46,52,41,4E,43,45,1E,1E,1E,0D");

            //OK-2 ROW, BUT THEM SAME FONT => 1 ROW:      0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,01,38,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,38,46,52,41,4E,43,45,1E,1E,1E,0D,3D


            string stringCommand = "";
            //ok-----------thep st -- stringCommand = "0A,00,29,01,0A,02,38,54,48,45,50,20,53,54,20,01,38,30,37,2F,30,39,2F,31,33,20,0A,02,38,42,30,30,31,38,2D,31,38,30,31,20,1E,1E,1E,0D";






            stringCommand = "01,0A,    02,38,54,48,45,50,20,53,54,20,        02,34,42,30,30,31,38,2D,31,38,30,31,20,      0A,    02,35,30,37,2F,30,39,2F,31,33,20,      02,38,1C,     02,38,20,4D,45,54,      1E,1E,1E,0D"; //ok 2 row, 1 counter






            //OK!stringCommand = "01,0A,    01,FF,21,     02,38,54,48,45,50,20,53,54,20,        02,34,42,30,30,31,38,2D,31,38,30,31,20,      0A,    02,35,30,37,2F,30,39,2F,31,33,20,      02,38,1C,     02,38,20,4D,45,54,      1E,1E,1E,0D"; //dang test logo 21h(LOGO) - SYMBOL 255 -- Page 99

            //OK!stringCommand = "01,0A,    01,FF,21,     02,38,54,48,45,50,20,53,54,20,        01,FE,21,     01,FD,21,     01,FC,21,     01,FB,21,     01,FA,21,     01,F9,21,     02,34,42,30,30,31,38,2D,31,38,30,31,20,      0A,    02,35,30,37,2F,30,39,2F,31,33,20,      02,38,1C,     02,38,20,4D,45,54,      1E,1E,1E,0D"; //dang test logo 21h(LOGO) - SYMBOL 255 -- Page 99



            stringCommand = "01,0A,    01,FF,21,     02,38,     54,48,45,50,20,53,54,20,        01,FE,21,     01,FD,21,     01,FC,21,     01,FB,21,     01,FA,21,     01,F9,21,                         02,34,    42,30,30,31,38,2D,31,38,30,31,20,      0A,    02,35,30,37,2F,30,39,2F,31,33,20,      02,38,1C,     02,38,20,4D,45,54,      1E,1E,1E,0D"; //dang test logo 21h(LOGO) - SYMBOL 255 -- Page 99





            //          stringCommand = "01,0A,02,38,54,48,45,50,20,53,54,20,01,38,30,37,2F,30,39,2F,31,33,20,0A,02,38,42,30,30,31,38,2D,31,38,30,31,20,02,38,1C,02,38,20,4D,45,54,02,38,1C,1E,1E,1E,0D"; //2 counter 

            ////////stringCommand = "01,0A,02,34,54,48,45,50,20,53,54,20,01,34,30,37,2F,30,39,2F,31,33,20,02,34,42,30,30,31,38,2D,31,38,30,31,20,1E,1E,1E,0D"; //2 counter 

            //stringCommand = "01,0A,02,38,54,48,45,50,20,53,54,20,01,38,30,37,2F,30,39,2F,31,33,20,0A,02,38,42,30,30,31,38,2D,31,38,30,31,20,1E,1E,1E,0D";




            stringCommand = stringCommand.Replace(" ", "");
            string[] arrayBarcode = stringCommand.Split(new Char[] { (char)44 });

            stringCommand = "0A,00," + GlobalStaticFunction.NumericTextToHEX(arrayBarcode.Length.ToString()).ToUpper() + "," + stringCommand;

            //stringCommand = "0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,01,38,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,38,46,52,41,4E,43,45,1E,1E,1E,0D";

            //OK!stringCommand = "39,00,01,01"; //read counter
            //OK!stringCommand = "3A,00,01,01"; //reset counter





            //OK!stringCommand = "38,00,18,01,   30,30,30,30,30,30,30,30,30,   39,39,39,39,39,39,39,39,39,   30,32,   00,00,01"; //SET counter step TO 2

            //OK!stringCommand = "51,00,0A,01,   30,30,30,30,30,30,30,30,36"; //SET counter VALUE TO 06


            stringCommand = stringCommand.Replace(" ", "");

            byte[] a = GlobalStaticFunction.CheckSumHEXString(stringCommand);

            //1row: byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,13,01,0A,02,38,49,4D,41,4A,45,20,02,38,46,52,41,4E,43,45,0D");

            //MessageBox.Show(BitConverter.ToString(a));



            //this.toolStripTextBoxCommand.Text = stringCommand + "," + BitConverter.ToString(a);
            stringCommand = stringCommand + "," + BitConverter.ToString(a);//Finnal string


        }


        //////public void ThreadRoutineTest(GlobalEnum.ImageS8Command imageS8Command)
        //////{
        //////    string stringHexCommand = "";
        //////    try
        //////    {
        //////        switch (imageS8Command)
        //////        {
        //////            case GlobalEnum.ImageS8Command.CheckPrinterReady:
        //////                stringHexCommand = IntToHexString((int)GlobalEnum.ImageS8Command.CheckPrinterReady);
        //////                this.SendToSerial(stringHexCommand);
        //////                this.PrinterReady = this.ReadFromSerial(true);
        //////                break;

        //////            case GlobalEnum.ImageS8Command.RequestCounterValue:
        //////                stringHexCommand = IntToHexString((int)GlobalEnum.ImageS8Command.RequestCounterValue) + ",00,01,01";
        //////                this.SendToSerial(stringHexCommand + "," + BitConverter.ToString(GlobalStaticFunction.CheckSumHEXString(stringHexCommand)));
        //////                if (this.ReadFromSerial(true)) this.TryPickupCounterValue();
        //////                break;

        //////            default:
        //////                break;
        //////        }

        //////        return;

        //////        this.SendToSerial("0A,00,32,01,0A,02,38,54,48,45,50,20,53,54,20,02,34,42,30,30,31,38,2D,31,38,30,31,20,0A,02,35,30,37,2F,30,39,2F,31,33,20,02,38,1C,02,38,20,4D,45,54,1E,1E,1E,0D,1D");

        //////        if (this.ReadFromSerial(true))
        //////        {


        //////            string stringReadFrom = ByteArrayToHexString(readResultBuffer);
        //////            stringReadFrom = stringReadFrom.Trim();
        //////            this.MainStatus = stringReadFrom;

        //////        }

        //////    }
        //////    catch (Exception exception)
        //////    {
        //////        this.LoopRoutine = false;
        //////        this.MainStatus = exception.Message;

        //////        this.LedRedOn = true;
        //////        this.NotifyPropertyChanged("LedStatus");
        //////    }
        //////}

        #endregion Public Thread test


        #endregion Public Thread

    }


    public class PrinterFaultStatus
    {
        public GlobalVariables.PrinterFaultCategory PrinterFaultCategoryID { get; set; }
        public bool FaultState { get; set; }
        public string FaultStatus { get; set; }

        public PrinterFaultStatus(GlobalVariables.PrinterFaultCategory printerFaultCategoryID)
        {
            this.PrinterFaultCategoryID = printerFaultCategoryID;
            this.FaultState = false;
            this.FaultStatus = "";
        }
    }

}
