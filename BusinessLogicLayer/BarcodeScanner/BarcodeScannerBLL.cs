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

using Global.Class.Library;
using DataTransferObject;

using DataAccessLayer;
using DataAccessLayer.DataDetailTableAdapters;


namespace BusinessLogicLayer.BarcodeScanner
{
    public class BarcodeScannerBLL : CommonThreadProperty
    {
        public bool MyTest; //Test only
        public bool MyHold;//Test only

        private FillingLineData privateFillingLineData;

        private TcpClient barcodeTcpClient;
        private NetworkStream barcodeNetworkStream;

        private GlobalVariables.BarcodeScannerName barcodeScannerName;
        private IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        private int portNumber = 2112;





        private SerialPort serialPort;


        private MessageQueue matchingPackList;
        private MessageQueue packInOneCarton;

        private DataDetail.DataDetailCartonDataTable cartonDataTable;

        private bool onPrinting;
        private bool resetMessage;
        private bool bufferReset;

        //barcodeScannerNameID
        private BarcodeScannerStatus[] barcodeScannerStatus;

        #region Contructor

        //Initialize
        //READ, WRITE Registry
        //Servernme + database name
        //Toolbar enable


        #region Test-Demo
        private System.Timers.Timer timerTest = new System.Timers.Timer(1000);



        #endregion Test-Demo


        public BarcodeScannerBLL(FillingLineData fillingLineData)
        {
            try
            {

                timerTest.Enabled = true;
                timerTest.Elapsed += new System.Timers.ElapsedEventHandler(timerTest_Elapsed);

                base.FillingLineData = fillingLineData;
                this.privateFillingLineData = this.FillingLineData.ShallowClone();

                this.barcodeScannerName = GlobalVariables.BarcodeScannerName.MatchingScanner;

                this.ipAddress = IPAddress.Parse(GlobalVariables.IpAddress(this.BarcodeScannerName));






                this.barcodeScannerStatus = new BarcodeScannerStatus[3];
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.QualityScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.QualityScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.MatchingScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.MatchingScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.CartonScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.CartonScanner);





                this.serialPort = new SerialPort();
                this.serialPort.PortName = GlobalVariables.ImageS8PortName;
                this.serialPort.BaudRate = 9600;
                this.serialPort.NewLine = GlobalVariables.charETX.ToString();


                this.serialPort.ReadTimeout = 500;
                //this.serialPort.WriteTimeout = 500;
                //this.serialPort.Parity = value;
                //this.serialPort.DataBits = value;
                //this.serialPort.StopBits = value;
                //this.serialPort.Handshake = value;

                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                this.serialPort.PinChanged += new SerialPinChangedEventHandler(serialPort_PinChanged);



                this.matchingPackList = new MessageQueue(GlobalVariables.NoItemDiverter());
                this.packInOneCarton = new MessageQueue();
                this.cartonDataTable = new DataDetail.DataDetailCartonDataTable();


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
                //return;

                ////////////--------------DON'T LOAD. THIS IS VERY OK, AND READY TO LOAD PACK IF NEEDED.
                //////////////Initialize MatchingPackList and PackInOneCarton (of the this.FillingLineData.FillingLineID ONLY)
                //this.PackDataTable = this.PackTableAdapter.GetData();
                //foreach (DataDetail.DataDetailPackRow packRow in this.PackDataTable)
                //{
                //    if (packRow.FillingLineID == (int)this.FillingLineData.FillingLineID)
                //    {
                //        MessageData messageData = new MessageData(packRow.PackBarcode);
                //        messageData.PackID = packRow.PackID;
                //        messageData.PackSubQueueID = packRow.PackSubQueueID;

                //        if (packRow.PackStatus == (byte)GlobalVariables.BarcodeStatus.Normal)
                //            this.matchingPackList.AddPack(messageData);
                //        else if (packRow.PackStatus == (byte)GlobalVariables.BarcodeStatus.ReadyToCarton)
                //            this.packInOneCarton.AddPack(messageData);
                //    }
                //}
                //this.PackDataTable = null;

                //this.NotifyPropertyChanged("MatchingPackList");
                //this.NotifyPropertyChanged("PackInOneCarton");
                ////////////--------------


                //Initialize CartonList
                this.cartonDataTable = this.CartonTableAdapter.GetDataByCartonStatus((byte)GlobalVariables.BarcodeStatus.BlankBarcode);
                this.NotifyPropertyChanged("CartonList");


                this.CartonDateFrom = DateTime.Today;
                this.CartonDateTo = DateTime.Today.AddHours(24).AddSeconds(-1);
                this.LoadCartonDataTable();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

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

        public void LoadCartonDataTable()
        {
            try
            {
                this.cartonDataTable = this.CartonTableAdapter.GetDataByCatonDate(this.CartonDateFrom, this.CartonDateTo);
                this.NotifyPropertyChanged("CartonList");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion Contructor


        #region Public Properties


        public GlobalVariables.BarcodeScannerName BarcodeScannerName
        {
            get
            {
                return this.barcodeScannerName;
            }
        }


        public IPAddress IpAddress
        {
            get
            {
                return this.ipAddress;
            }
        }


        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }
        }




        public bool OnPrinting
        {
            get { return this.onPrinting; }
            private set { this.onPrinting = value; this.resetMessage = true; }
        }


        public void StartPrint() { this.bufferReset = true; this.OnPrinting = true; } //ONLY bufferReset WHEN StartPrint
        public void StopPrint() { this.OnPrinting = false; }






        public bool LedMCUQualityOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.QualityScanner - 1].MCUReady; }
        }

        public bool LedMCUMatchingOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.MatchingScanner - 1].MCUReady; }
        }

        public bool LedMCUCartonOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.CartonScanner - 1].MCUReady; }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID, bool mcuReady)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady != mcuReady)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady = mcuReady;
                this.NotifyPropertyChanged("LedMCU");
            }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID, string mcuStatus)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus != mcuStatus)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus = mcuStatus;
                this.MainStatus = barcodeScannerNameID.ToString() + ": " + mcuStatus;
            }

            this.SetBarcodeScannerStatus(barcodeScannerNameID, false);
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

        DataDetailPackTableAdapter packTableAdapter;
        protected DataDetailPackTableAdapter PackTableAdapter
        {
            get
            {
                if (packTableAdapter == null) packTableAdapter = new DataDetailPackTableAdapter();
                return packTableAdapter;
            }
        }


        DataDetail.DataDetailPackDataTable packDataTable;
        protected DataDetail.DataDetailPackDataTable PackDataTable
        {
            get
            {
                if (packDataTable == null) packDataTable = new DataDetail.DataDetailPackDataTable();
                return packDataTable;
            }
            set { this.packDataTable = value; }
        }





        public string BatchSerialNumber { get { return this.privateFillingLineData.BatchSerialNumber; } }

        public string MonthSerialNumber { get { return this.privateFillingLineData.MonthSerialNumber; } }

        public string BatchCartonNumber { get { return this.privateFillingLineData.BatchCartonNumber; } }

        public string MonthCartonNumber { get { return this.privateFillingLineData.MonthCartonNumber; } }

        public int MatchingPackCount { get { return this.matchingPackList.Count; } }
        public int PackInOneCartonCount { get { return this.packInOneCarton.Count; } }

        #endregion Public Properties

        #region Public Method

        public DataTable GetMatchingPackList()
        {
            if (this.matchingPackList != null)
            {
                lock (this.matchingPackList)
                {
                    return this.matchingPackList.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetPackInOneCarton()
        {
            if (this.packInOneCarton != null)
            {
                lock (this.packInOneCarton)
                {
                    return this.packInOneCarton.GetAllElements();
                }
            }
            else return null;
        }

        public DataDetail.DataDetailCartonDataTable GetCartonList()
        {
            if (this.cartonDataTable != null)
            {
                lock (this.cartonDataTable)
                {
                    return (DataDetail.DataDetailCartonDataTable)this.cartonDataTable.Copy();
                }
            }
            else return null;
        }

        private bool Connect(bool connectMatchingScanner)
        {
            try
            {
                this.MainStatus = "Try to connect....";

                if (this.serialPort.IsOpen) this.serialPort.Close();
                this.serialPort.Open();

                if (!this.serialPort.IsOpen) throw new System.InvalidOperationException("NMVN: Can not connect to COMPORT: " + this.serialPort.PortName);



                if (connectMatchingScanner)
                {
                    this.barcodeTcpClient = new TcpClient();

                    if (!this.barcodeTcpClient.Connected)
                    {
                        this.barcodeTcpClient.Connect(this.IpAddress, this.PortNumber);
                        this.barcodeNetworkStream = barcodeTcpClient.GetStream();
                    }
                }


                this.LedGreenOn = true;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");


                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }

        private bool Disconnect()
        {
            try
            {
                this.MainStatus = "Disconnect....";


                if (this.serialPort.IsOpen) this.serialPort.Close();



                //if (this.inkJetTcpClient.Connected) --- Theoryly, it should CHECK this.inkJetTcpClient.Connected BEFORE close. BUT: DON'T KNOW why GlobalVariables.DominoPrinterName.CartonInkJet DISCONECTED ALREADY!!!! Should check this cerefully later!
                //{
                if (this.barcodeNetworkStream != null) { this.barcodeNetworkStream.Close(); this.barcodeNetworkStream.Dispose(); }

                if (this.barcodeTcpClient != null) this.barcodeTcpClient.Close();
                //}


                this.LedGreenOn = false;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");

                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.QualityScanner, true);
                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.MatchingScanner, true);
                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.CartonScanner, true);



                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }

        private bool WaitForBarcode(ref string stringReadFrom)
        {
            try
            {
                this.barcodeNetworkStream.ReadTimeout = 120; //Default = -1; 

                stringReadFrom = GlobalNetSockets.ReadFromStream(barcodeTcpClient, barcodeNetworkStream).Trim();

                this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1
                return stringReadFrom != "";
            }

            catch (Exception exception)
            {
                this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1

                //Ignore when timeout
                if (exception.Message == "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.")
                {
                    stringReadFrom = "";
                    return false;
                }
                else
                    throw exception;




                ////this.LedGreenOn = this.barcodeTcpClient.Connected;//this.LedAmberOn = readyToPrint;
                ////this.LedRedOn = !this.barcodeTcpClient.Connected;
                ////this.NotifyPropertyChanged("LedStatus");


                ////this.MainStatus = this.LedGreenOn ? (this.onPrinting ? "Scanning ...." : "Ready to scan") : exception.Message;


            }
        }




        private MessageData AddDataDetailPack(string printedBarcode, int packSubQueueID)
        {
            lock (this.PackDataTable)
            {
                lock (this.PackTableAdapter)
                {
                    MessageData messageData = new MessageData(printedBarcode);
                    messageData.PackSubQueueID = packSubQueueID;

                    DataDetail.DataDetailPackRow dataDetailPackRow = this.PackDataTable.NewDataDetailPackRow();

                    dataDetailPackRow.PackDate = DateTime.Now;
                    dataDetailPackRow.PackBarcode = messageData.PrintedBarcode;
                    dataDetailPackRow.PackSubQueueID = messageData.PackSubQueueID;
                    dataDetailPackRow.PackStatus = (byte)GlobalVariables.BarcodeStatus.Normal;
                    dataDetailPackRow.FillingLineID = (int)this.FillingLineData.FillingLineID;
                    this.PackDataTable.AddDataDetailPackRow(dataDetailPackRow);

                    int rowsAffected = this.PackTableAdapter.Update(dataDetailPackRow);

                    if (rowsAffected == 1) //Save successfully
                    {
                        messageData.PackID = dataDetailPackRow.PackID;
                        this.PackDataTable.RemoveDataDetailPackRow(dataDetailPackRow);

                        return messageData;
                    }
                    else
                    {
                        this.MainStatus = "Insufficient save pack: " + printedBarcode;
                        return null;
                    }
                }
            }
        }


        private void MatchingAndAddCarton(string cartonBarcode)
        {
            if (GlobalVariables.IgnoreEmptyCarton && this.packInOneCarton.Count <= 0) return;

            lock (this.cartonDataTable)
            {
                lock (this.packInOneCarton)
                {
                    lock (this.CartonTableAdapter)
                    {
                        try
                        {
                            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                            {

                                DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.NewDataDetailCartonRow();

                                dataDetailCartonRow.CartonDate = DateTime.Now;
                                dataDetailCartonRow.CartonBarcode = cartonBarcode;
                                dataDetailCartonRow.CartonStatus = (byte)(cartonBarcode == GlobalVariables.BlankBarcode ? GlobalVariables.BarcodeStatus.BlankBarcode : (this.packInOneCarton.Count == this.packInOneCarton.NoItemPerCarton || this.FillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail ? GlobalVariables.BarcodeStatus.Normal : GlobalVariables.BarcodeStatus.EmptyCarton));
                                dataDetailCartonRow.FillingLineID = (int)this.FillingLineData.FillingLineID;

                                for (int i = 0; i < 24; i++)
                                {       //This use NON TYPED DATASET  -- Not so good, bescause the compiler can not detect error at compile time, but it is ok (I know exactly what in every row)
                                    dataDetailCartonRow["Pack" + i.ToString("00") + "Barcode"] = this.packInOneCarton.Count > i ? this.packInOneCarton.ElementAt(i).PrintedBarcode : "";
                                }

                                this.cartonDataTable.Rows.InsertAt(dataDetailCartonRow, 0);     //this.cartonDataTable.AddDataDetailCartonRow(dataDetailCartonRow); 

                                int rowsAffected = this.CartonTableAdapter.Update(dataDetailCartonRow);//Save

                                if (rowsAffected == 1) //Add Whole Carton Successfully
                                {
                                    if (this.packInOneCarton.Count > 0)   //Try to remove detail pack
                                    {
                                        if (UpdateDataDetailPack(GlobalVariables.BarcodeStatus.Deleted)) this.packInOneCarton = new MessageQueue(); else throw new Exception("Insufficient remove detailing pack: " + dataDetailCartonRow.CartonBarcode); //Only on if save ok
                                    }
                                }
                                else throw new Exception("Insufficient save carton: " + dataDetailCartonRow.CartonBarcode); //Only on if save ok


                                transactionScope.Complete(); //this.cartonDataTable.AcceptChanges();



                                #region Just maintain 300 rows only
                                if (this.cartonDataTable.Rows.Count > (this.FillingLineData.FillingLineID == Global.Class.Library.GlobalVariables.FillingLine.Pail ? 300 : 300))
                                {
                                    for (int j = this.cartonDataTable.Rows.Count - 1; j >= 0; j--)
                                    {
                                        DataDetail.DataDetailCartonRow cartonRow = this.cartonDataTable.Rows[j] as DataDetail.DataDetailCartonRow;
                                        if (cartonRow != null && cartonRow.CartonStatus != (byte)GlobalVariables.BarcodeStatus.BlankBarcode) { this.cartonDataTable.Rows.RemoveAt(j); break; }
                                    }
                                }
                                #endregion Just maintain 300 rows only


                            }

                        }
                        catch (System.Exception exception)
                        {
                            this.MainStatus = exception.Message;
                        }
                    }
                }
            }
        }

        private bool UpdateDataDetailPack(GlobalVariables.BarcodeStatus barcodeStatus)
        {
            string listOfPackID = ""; int rowsAffected = 0;
            for (int i = 0; i < this.packInOneCarton.NoItemPerCarton; i++)
            {
                listOfPackID = listOfPackID + (listOfPackID != "" ? ";" : "") + this.packInOneCarton.ElementAt(i).PackID.ToString();
            }

            lock (this.PackTableAdapter)
            {
                if (barcodeStatus == GlobalVariables.BarcodeStatus.Deleted)
                    rowsAffected = this.PackTableAdapter.DeleteListOfPack(listOfPackID);
                else
                    rowsAffected = this.PackTableAdapter.UpdateListOfPackStatus(listOfPackID, (byte)barcodeStatus);
            }

            return rowsAffected == this.packInOneCarton.NoItemPerCarton;
        }



        #endregion Public Method


        #region Public Thread

        public void ThreadRoutine()
        {
            this.privateFillingLineData = this.FillingLineData.ShallowClone();

            string stringReadFrom = ""; bool matchingPackListChanged = false; bool packInOneCartonChanged = false;

            this.LoopRoutine = true; this.StopPrint();




            try
            {

                if (this.Connect(false)) this.serialPort.NewLine = GlobalVariables.charETX.ToString(); else throw new System.InvalidOperationException("NMVN: Can not connect to comport.");

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    this.MainStatus = this.OnPrinting ? "Scanning..." : "Ready to scan";

                    ////////////if (this.FillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail)
                    ////////////{

                    ////////////    #region Read every pack

                    ////////////    if (this.WaitForBarcode(ref stringReadFrom) && this.OnPrinting)
                    ////////////    {
                    ////////////        string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

                    ////////////        foreach (string stringBarcode in arrayBarcode)
                    ////////////        {
                    ////////////            if (stringBarcode.Trim() != "" && stringBarcode.Trim() != "NoRead") //Add to MatchingPackList
                    ////////////            {
                    ////////////                lock (this.matchingPackList)
                    ////////////                {
                    ////////////                    MessageData messageData = this.AddDataDetailPack(stringBarcode + " " + this.privateFillingLineData.BatchSerialNumber, this.matchingPackList.NextPackSubQueueID);
                    ////////////                    if (messageData != null)
                    ////////////                    {
                    ////////////                        this.matchingPackList.Enqueue(messageData);
                    ////////////                        matchingPackListChanged = true;
                    ////////////                    }
                    ////////////                }

                    ////////////                this.privateFillingLineData.BatchSerialNumber = (int.Parse(this.privateFillingLineData.BatchSerialNumber) + 1).ToString("0000000").Substring(1);//Format 7 digit, then cut 6 right digit: This will reset a 0 when reach the limit of 6 digit
                    ////////////                this.NotifyPropertyChanged("BatchSerialNumber"); //APPEND TO stringBarcode, and then: Increase BatchSerialNumber by 1 PROGRAMMATICALLY BY SOFTWARE

                    ////////////            }
                    ////////////        }
                    ////////////    }

                    ////////////    #endregion Read every pack


                    ////////////    #region Make One Carton

                    ////////////    if (this.OnPrinting)
                    ////////////    {
                    ////////////        lock (this.packInOneCarton)
                    ////////////        {
                    ////////////            if (this.packInOneCarton.Count <= 0)
                    ////////////            {
                    ////////////                lock (this.matchingPackList)
                    ////////////                {                             //<Dequeue from matchingPackList to this.packInOneCarton>
                    ////////////                    this.packInOneCarton = this.matchingPackList.DequeuePack(); //This Dequeue successful WHEN There is enought MessageData IN THE COMMON GLOBAL matchingPackList, ELSE: this.packListInCarton is still NULL
                    ////////////                }

                    ////////////                if (this.packInOneCarton.Count > 0)
                    ////////////                {
                    ////////////                    matchingPackListChanged = true;
                    ////////////                    packInOneCartonChanged = true;

                    ////////////                    if (!UpdateDataDetailPack(GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Insufficient update pack status";
                    ////////////                }
                    ////////////            }
                    ////////////        }

                    ////////////    }
                    ////////////    #endregion Make One Carton


                    ////////////    if (matchingPackListChanged) { this.NotifyPropertyChanged("MatchingPackList"); matchingPackListChanged = false; }
                    ////////////    if (packInOneCartonChanged) { this.NotifyPropertyChanged("PackInOneCarton"); packInOneCartonChanged = false; }

                    ////////////}
                    Thread.Sleep(100);

                } //End while this.LoopRoutine
            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message;

                this.LedRedOn = true;
                this.NotifyPropertyChanged("LedStatus");
            }
            finally
            {
                this.Disconnect();
            }

        }





        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        { //Carton Barcode
            try
            {
                if (this.LoopRoutine)
                {
                    //string stringReadFrom = serialPort.ReadExisting();
                    string stringReadFrom = serialPort.ReadLine();
                    string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

                    foreach (string stringBarcode in arrayBarcode)
                    {
                        if (stringBarcode.Trim() != "" && stringBarcode.Trim() != "NoRead")
                            this.DemoRoutine(stringBarcode.Trim());
                    }



                    //char[] buff = new char[1];
                    //char[] xbuff = new char[1];

                    //buff[0] = e.KeyChar;
                    //xbuff[0] = 'x';

                    //serialPort1.Write(buff, 0, 1);
                    //System.Threading.Thread.Sleep(500);
                    //serialPort1.Write(xbuff, 0, 1);
                    //System.Array.Clear(buff, 0, 1);




                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        private bool pinChangedToSlow = false;

        void serialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                //if (this.FillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail) //FillingLine.Pail: No need to read [Blank]
                //{
                //    if (e.EventType == SerialPinChange.CDChanged && this.OnPrinting)
                //    {
                //        pinChangedToSlow = !pinChangedToSlow;
                //        if (pinChangedToSlow)
                //        {
                //            MatchingAndAddCarton(GlobalVariables.BlankBarcode);
                //            this.NotifyPropertyChanged("PackInOneCarton"); this.NotifyPropertyChanged("CartonList");
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #endregion Public Thread


        #region Test-Demo

        void timerTest_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.LoopRoutine)
            {
                //Random random = new Random();
                //int randomNumber = random.Next(100000, 999999);
                //this.DemoRoutine(randomNumber.ToString() + "8945561230");
            }
        }


        private void DemoRoutine(string stringReadFrom)
        {
            bool matchingPackListChanged = false; bool packInOneCartonChanged = false;


            try
            {

                #region Read every pack

                string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

                foreach (string stringBarcode in arrayBarcode)
                {
                    if (stringBarcode.Trim() != "" && stringBarcode.Trim() != "NoRead") //Add to MatchingPackList
                    {
                        lock (this.matchingPackList)
                        {
                            MessageData messageData = this.AddDataDetailPack(stringBarcode, this.matchingPackList.NextPackSubQueueID);
                            if (messageData != null)
                            {
                                this.matchingPackList.Enqueue(messageData);
                                matchingPackListChanged = true;
                            }
                        }
                    }
                }

                #endregion Read every pack


                #region Make One Carton

                lock (this.packInOneCarton)
                {
                    if (this.packInOneCarton.Count <= 0)
                    {
                        lock (this.matchingPackList)
                        {                             //<Dequeue from matchingPackList to this.packInOneCarton>
                            this.packInOneCarton = this.matchingPackList.DequeuePack(); //This Dequeue successful WHEN There is enought MessageData IN THE COMMON GLOBAL matchingPackList, ELSE: this.packListInCarton is still NULL
                        }

                        if (this.packInOneCarton.Count > 0)
                        {
                            matchingPackListChanged = true;
                            packInOneCartonChanged = true;

                            if (!UpdateDataDetailPack(GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Insufficient update pack status";
                        }
                    }

                }

                #endregion Make One Carton


                #region Packing a Carton
                if (this.matchingPackList.Count + 2 == this.packInOneCarton.NoItemPerCarton) //Save a carton
                {
                    MatchingAndAddCarton("11111");

                    this.NotifyPropertyChanged("PackInOneCarton"); this.NotifyPropertyChanged("CartonList");
                }
                #endregion Packing a Carton

                if (matchingPackListChanged) { this.NotifyPropertyChanged("MatchingPackList"); matchingPackListChanged = false; }
                if (packInOneCartonChanged) { this.NotifyPropertyChanged("PackInOneCarton"); packInOneCartonChanged = false; }


            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message;

                this.LedRedOn = true;
                this.NotifyPropertyChanged("LedStatus");
            }
            finally
            {
                this.Disconnect();
            }
        }

        #endregion Test-Demo





        #region Handle Exception


        /// <summary>
        /// If this.Count = noItemPerCarton then Reallocation an equal amount of messageData to every subQueue. Each subQueue will have the same NoItemPerSubQueue.
        /// </summary>
        /// <returns></returns>
        public bool ReAllocation()
        {
            if (this.matchingPackList.Count >= this.matchingPackList.NoItemPerCarton)
            {
                lock (this.matchingPackList)
                {
                    for (int subQueueID = 0; subQueueID < this.matchingPackList.NoSubQueue; subQueueID++)
                    {
                        while (this.matchingPackList.GetSubQueueCount(subQueueID) < (this.matchingPackList.NoItemPerCarton / this.matchingPackList.NoSubQueue) && this.matchingPackList.Count >= this.matchingPackList.NoItemPerCarton)
                        {
                            #region Get a message and swap its' PackSubQueueID

                            for (int i = 0; i < this.matchingPackList.NoSubQueue; i++)
                            {
                                if (this.matchingPackList.GetSubQueueCount(i) > (this.matchingPackList.NoItemPerCarton / this.matchingPackList.NoSubQueue))
                                {
                                    MessageData messageData = this.matchingPackList.ElementAt(i, this.matchingPackList.GetSubQueueCount(i) - 1).ShallowClone(); //Get the last pack of SubQueue(i)
                                    messageData.PackSubQueueID = subQueueID; //Set new PackSubQueueID

                                    lock (this.PackTableAdapter)
                                    {
                                        lock (this.PackDataTable)
                                        {
                                            if (this.PackTableAdapter.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.PackSubQueueID) == 1)
                                            {
                                                this.matchingPackList.Dequeue(messageData.PackID); //First: Remove from old subQueue
                                                this.matchingPackList.AddPack(messageData);//Next: Add to new subQueue
                                            }
                                            else throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack subqueue: " + messageData.PrintedBarcode);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }

                this.NotifyPropertyChanged("MatchingPackList");
                return true;
            }
            else return false;
        }






        public bool RemoveItemInMatchingPackList(int packID)
        {
            if (packID <= 0) return false;

            lock (this.matchingPackList)
            {
                if (this.matchingPackList.Count > 0)
                {
                    MessageData messageData = this.matchingPackList.Dequeue(packID);
                    if (messageData != null)
                    {
                        this.NotifyPropertyChanged("MatchingPackList");

                        lock (this.PackDataTable)
                        {
                            lock (this.PackTableAdapter)
                            {
                                if (this.PackTableAdapter.Delete(messageData.PackID) == 1) return true; //Delete successfully
                                else throw new System.ArgumentException("Fail to handle this pack", "Can not delete pack from the line");
                            }
                        }
                    }
                    else throw new System.ArgumentException("Fail to handle this pack", "Can not remove pack from the line");
                }
                else throw new System.ArgumentException("Fail to handle this pack", "No pack found on the line");
            }
        }


        public bool RemoveItemInPackInOneCarton(int packID)
        {
            if (packID <= 0) return false;

            lock (this.matchingPackList)
            {
                lock (this.packInOneCarton)
                {
                    if (this.matchingPackList.Count > 0 && this.packInOneCarton.Count == this.packInOneCarton.NoItemPerCarton)
                    {
                        MessageData messageData = this.matchingPackList.ElementAt(0).ShallowClone(); //Get the first pack

                        if (this.packInOneCarton.Replace(packID, messageData)) //messageData.PackSubQueueID: Will change to new value (new position) after replace
                        {
                            this.matchingPackList.Dequeue(messageData.PackID); //Dequeue the first pack

                            this.NotifyPropertyChanged("PackInOneCarton");
                            this.NotifyPropertyChanged("MatchingPackList");

                            lock (this.PackTableAdapter)
                            {
                                lock (this.PackDataTable)
                                {
                                    if (this.PackTableAdapter.Delete(packID) != 1) throw new System.ArgumentException("Fail to handle this pack", "Can not delete pack from the line");

                                    if (this.PackTableAdapter.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.PackSubQueueID) != 1) throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack subqueue");

                                    if (this.PackTableAdapter.UpdateListOfPackStatus(messageData.PackID.ToString(), (byte)GlobalVariables.BarcodeStatus.ReadyToCarton) == 1) return true;
                                    else throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack status");
                                }
                            }
                        }
                        else throw new System.ArgumentException("Fail to handle this pack", "Can not replace pack");
                    }
                    else throw new System.ArgumentException("Fail to handle this pack", "No pack found on the matching line");
                }
            }
        }


        public Boolean UndoCartonToPack(int cartonID)
        {
            if (cartonID <= 0) return false;

            lock (this.packInOneCarton)
            {
                if (this.packInOneCarton.Count <= 0)
                {
                    lock (this.cartonDataTable)
                    {
                        DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.FindByCartonID(cartonID);
                        if (dataDetailCartonRow != null && dataDetailCartonRow.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                        {
                            //COPY Data FROM cartonDataTable TO packInOneCarton
                            for (int i = 0; i < this.packInOneCarton.NoItemPerCarton; i++)
                            {                               //This use NON TYPED DATASET  -- Not so good, bescause the compiler can not detect error at compile time, but it is ok (I know exactly what in every row)
                                MessageData messageData = this.AddDataDetailPack(dataDetailCartonRow["Pack" + i.ToString("00") + "Barcode"].ToString(), packInOneCarton.NextPackSubQueueID);
                                if (messageData != null) this.packInOneCarton.Enqueue(messageData);
                            }

                            if (this.packInOneCarton.Count > 0) UpdateDataDetailPack(GlobalVariables.BarcodeStatus.ReadyToCarton);

                            this.NotifyPropertyChanged("PackInOneCarton");

                            lock (this.CartonTableAdapter)
                            {
                                //REMOVE data IN cartonDataTable
                                int rowsAffected = this.CartonTableAdapter.Delete(dataDetailCartonRow.CartonID);//Delete
                                if (rowsAffected == 1) this.cartonDataTable.Rows.Remove(dataDetailCartonRow); else throw new System.ArgumentException("Fail to handle this carton", "Insufficient remove carton");
                            }

                            this.NotifyPropertyChanged("CartonList");

                            return true;
                        }
                        else return false;
                    }
                }
                else throw new System.ArgumentException("Fail to handle this carton", "Another carton is on the line");
            }
        }


        public Boolean UpdateCartonBarcode(int cartonID, string cartonBarcode)
        {
            if (cartonID <= 0 || cartonBarcode.Length < 12) throw new Exception("Invalid barcode: It is required 12 letters barcode [" + cartonBarcode + "]");

            lock (this.cartonDataTable)
            {
                DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.FindByCartonID(cartonID);
                if (dataDetailCartonRow != null && dataDetailCartonRow.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                {
                    lock (this.CartonTableAdapter)
                    {
                        int rowsAffected = this.CartonTableAdapter.UpdateCartonBarcode((byte)GlobalVariables.BarcodeStatus.Normal, cartonBarcode, dataDetailCartonRow.CartonID);
                        if (rowsAffected == 1)
                        {
                            dataDetailCartonRow.CartonStatus = (byte)GlobalVariables.BarcodeStatus.Normal;
                            dataDetailCartonRow.CartonBarcode = cartonBarcode;
                        }
                        else throw new System.ArgumentException("Fail to handle this carton", "Insufficient update carton");
                    }

                    this.NotifyPropertyChanged("CartonList");

                    return true;
                }
                else return false;
            }

        }

        #endregion Handle Exception


    }

    public class BarcodeScannerStatus
    {
        public GlobalVariables.BarcodeScannerName BarcodeScannerNameID { get; set; }
        public bool MCUReady { get; set; }
        public string MCUStatus { get; set; }

        public BarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID)
        {
            this.BarcodeScannerNameID = barcodeScannerNameID;
            this.MCUReady = true;
            this.MCUStatus = "";
        }
    }
}
