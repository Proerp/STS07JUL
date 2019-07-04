using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


using Global.Class.Library;
using DataTransferObject;



namespace BusinessLogicLayer.InkjetDominoPrinter
{
    public class InkjetDominoPrinterBLL : CommonThreadProperty
    {

        private TcpClient inkJetTcpClient;
        private NetworkStream inkJetNetworkStream;

        private GlobalVariables.DominoPrinterName dominoPrinterNameID;

        private FillingLineData privateFillingLineData;


        private IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        private int portNumber = 7000;

        private string lastNACKCode;

        private bool onPrinting;
        private bool resetMessage;


        #region Contructor

        public InkjetDominoPrinterBLL(GlobalVariables.DominoPrinterName dominoPrinterNameID, FillingLineData fillingLineData)
        {

            try
            {
                this.FillingLineData = fillingLineData;
                this.privateFillingLineData = this.FillingLineData.ShallowClone();

                this.dominoPrinterNameID = dominoPrinterNameID;

                this.ipAddress = IPAddress.Parse(GlobalVariables.IpAddress(this.DominoPrinterNameID));



            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
            }

        }

        #endregion Contructor



        #region Public Properties


        public GlobalVariables.DominoPrinterName DominoPrinterNameID
        {
            get
            {
                return this.dominoPrinterNameID;
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


        public void StartPrint() { this.OnPrinting = true; }
        public void StopPrint() { this.OnPrinting = false; }


        public string BatchSerialNumber { get { return this.privateFillingLineData.BatchSerialNumber; } }

        public string MonthSerialNumber { get { return this.privateFillingLineData.MonthSerialNumber; } }

        public string BatchCartonNumber { get { return this.privateFillingLineData.BatchCartonNumber; } }

        public string MonthCartonNumber { get { return this.privateFillingLineData.MonthCartonNumber; } }



        #endregion Public Properties


        #region Message Configuration

        private string FirstMessageLine() //Only DominoPrinterName.CartonInkJet: HAS SERIAL NUMBER (BUT WILL BE UPDATE MANUAL FOR EACH CARTON - BECAUSE: [EAN BARCODE] DOES NOT ALLOW INSERT SERIAL NUMBER) ===> FOR THIS: BatchSerialNumber FOR EVERY PACK: NEVER USE
        {
            return this.privateFillingLineData.BatchNo + (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet ? "/" + "  " + "/" + this.privateFillingLineData.BatchCartonNumber.Substring(2) : "");
        }

        private string SecondMessageLine()
        {
            return this.privateFillingLineData.ProductCode + " " + DateTime.Now.ToString("dd/MM/yy");
            //return "NSX " + GlobalVariables.charESC + "/n/1/A/" + GlobalVariables.charESC + "/n/1/F/" + GlobalVariables.charESC + "/n/1/D/";
        }

        private string ThirdMessageLine(int serialNumberIndentity) //serialNumberIndentity = 1 when print as text on first line, 2 when insert into 2D Barcode
        {
            string serialNumberFormat = ""; //Numeric Serial Only, No Alpha Serial, Zero Leading, 6 Digit: 000001 -> 999999, Step 1, Start this.privateFillingLineData.MonthSerialNumber, Repeat: 0
            if (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.DegitInkJet || this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.BarCodeInkJet)
                //////---- Don use this Startup Serial Value, because some Dimino printer do no work!!! - DON't KNOW!!! serialNumberFormat = GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/" + this.privateFillingLineData.MonthSerialNumber + "/00000/N/"; //WITH START VALUE---No need to update serial number
                serialNumberFormat = GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/000000/00000/N/"; //WITH START VALUE = 1 ---> NEED TO UPDATE serial number
            else //this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet
                serialNumberFormat = this.privateFillingLineData.MonthCartonNumber.Substring(1); //---Dont use counter (This will be updated MANUALLY for each carton)


            return (this.DominoPrinterNameID != GlobalVariables.DominoPrinterName.BarCodeInkJet ? this.privateFillingLineData.ProductCode : "") + this.privateFillingLineData.SettingMonthID.ToString("00") + "/" + this.privateFillingLineData.FillingLineCode + "/" + serialNumberFormat;
        }


        private string EANInitialize(string twelveDigitCode)
        {

            int iSum = 0; int iDigit = 0;

            twelveDigitCode = twelveDigitCode.Replace("/", "");

            // Calculate the checksum digit here.
            for (int i = twelveDigitCode.Length; i >= 1; i--)
            {
                iDigit = Convert.ToInt32(twelveDigitCode.Substring(i - 1, 1));
                if (i % 2 == 0)
                {	// odd
                    iSum += iDigit * 3;
                }
                else
                {	// even
                    iSum += iDigit * 1;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            return twelveDigitCode + iCheckSum.ToString();

            #region Checksum rule
            ////Checksum rule
            ////Lấy tổng tất cả các số ở vị trí lẻ (1,3,5,7,9,11) được một số A.
            ////Lấy tổng tất cả các số ở vị trí chẵn (2,4,6,8,10,12). Tổng này nhân với 3 được một số (B).
            ////Lấy tổng của A và B được số A+B.
            ////Lấy phần dư trong phép chia của A+B cho 10, gọi là số x. Nếu số dư này bằng 0 thì số kiểm tra bằng 0, nếu nó khác 0 thì số kiểm tra là phần bù (10-x) của số dư đó.

            //Generate check sum number
            //            --**************************************
            //-- Name: Check Digit for EAN13 Barcode
            //-- Description:Calculates the Check Digit for a EAN13 Barcode
            //-- By: Conradude
            //--
            //-- Inputs:SELECT dbo.fu_EAN13CheckDigit('600100700010')
            //--
            //-- Returns:String with 13 Digits
            //--
            //-- Side Effects:Hair Growth on the palms of your hand
            //--
            //--This code is copyrighted and has-- limited warranties.Please see http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=891&lngWId=5--for details.--**************************************

            //CREATE FUNCTION fu_EAN13CheckDigit (@Barcode varchar(12))																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																								-->>>><<><<<<<>>>>>>>>>>>>>>>>< <><><>												
            //RETURNS varchar(13)
            //AS
            //BEGIN
            //DECLARE @SUM int ,
            //    @COUNTER int,
            //    @RETURN varchar(13),
            //    @Val1 int,
            //    @Val2 int	
            //SET @COUNTER = 1
            //SET @SUM = 0
            //WHILE @Counter < 13
            //BEGIN
            //    SET @VAL1 = SUBSTRING(@Barcode,@counter,1) * 1
            //    SET @VAL2 = SUBSTRING(@Barcode,@counter + 1,1) * 3
            //    SET @SUM = @VAL1 + @SUM	
            //    SET @SUM = @VAL2 + @SUM
            //    SET @Counter = @Counter + 2
            //END
            //SET @Counter = ROUND(@SUM + 5,-1)
            //SET @Return = @BARCODE + CONVERT(varchar,((@Counter - @SUM)))
            //RETURN @Return
            //END
            #endregion


        }

        private string WholeMessageLine()
        {
            if (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.DegitInkJet)
                return GlobalVariables.charESC + "u/1/ " + GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1);
            else if (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.BarCodeInkJet)
                return GlobalVariables.charESC + "u/3/" + GlobalVariables.charESC + "/z/1/0/26/20/20/1/0/0/0/" + this.FirstMessageLine() + " " + this.SecondMessageLine() + " " + this.ThirdMessageLine(2) + "/" + GlobalVariables.charESC + "/z/0" + //2D Barcode
                       GlobalVariables.charESC + "u/1/" + this.FirstMessageLine() + "/" +  //First Line
                       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.SecondMessageLine() +   //Second Line
                       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1);     //Third Line   
            else //this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet
            {
                //string thirdMessageLine = EANInitialize(this.ThirdMessageLine(1));
                //return GlobalVariables.charESC + "u/2/" + GlobalVariables.charESC + "/q/4/@/" + thirdMessageLine + "/@/" + GlobalVariables.charESC + "/q/0" +   //EAN13 Barcode: the first digit MUST be inserted again at the end as the digit number 14   + thirdMessageLine.Substring(0,1)
                //           GlobalVariables.charESC + "u/1/  " + this.FirstMessageLine() + "/" + //First Line
                //           GlobalVariables.charESC + "/r/  " + GlobalVariables.charESC + "u/1/" + thirdMessageLine + "/ " + DateTime.Now.ToString("dd/MM/yy");


                return GlobalVariables.charESC + "u/2/" + GlobalVariables.charESC + "/q/6/" + this.ThirdMessageLine(1) + "/" + this.privateFillingLineData.BatchCartonNumber.Substring(2) + GlobalVariables.charESC + "/q/0" +
                           GlobalVariables.charESC + "u/1/  " + this.FirstMessageLine() + "/" + //First Line
                           GlobalVariables.charESC + "/r/  " + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1) + "/ " + DateTime.Now.ToString("dd/MM/yy");

            }
        }


        #endregion Message Configuration


        #region Public Method


        private bool Connect()
        {
            try
            {
                this.MainStatus = "Try to connect....";

                this.inkJetTcpClient = new TcpClient();

                if (!this.inkJetTcpClient.Connected)
                {
                    this.inkJetTcpClient.Connect(this.IpAddress, this.PortNumber);
                    this.inkJetNetworkStream = inkJetTcpClient.GetStream();
                }

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

                //if (this.inkJetTcpClient.Connected) --- Theoryly, it should CHECK this.inkJetTcpClient.Connected BEFORE close. BUT: DON'T KNOW why GlobalVariables.DominoPrinterName.CartonInkJet DISCONECTED ALREADY!!!! Should check this cerefully later!
                //{
                if (this.inkJetNetworkStream != null) { this.inkJetNetworkStream.Close(); this.inkJetNetworkStream.Dispose(); }

                if (this.inkJetTcpClient != null) this.inkJetTcpClient.Close();
                //}


                this.LedGreenOn = false;
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


        private void WriteToStream(string stringWriteTo)
        {
            try
            {
                GlobalNetSockets.WriteToStream(inkJetNetworkStream, stringWriteTo);
            }
            catch (Exception exception)
            { throw exception; }
        }

        private bool ReadFromStream(ref string stringReadFrom, bool waitForACK)
        {
            return ReadFromStream(ref stringReadFrom, waitForACK, "", 0);
        }

        private bool ReadFromStream(ref string stringReadFrom, bool waitForACK, string commandCode, long commandLength)
        {
            try
            {
                stringReadFrom = GlobalNetSockets.ReadFromStream(inkJetTcpClient, inkJetNetworkStream);

                if (waitForACK)
                {
                    if (stringReadFrom.ElementAt(0) == GlobalVariables.charACK)
                        return true;
                    else
                    {
                        if (stringReadFrom.ElementAt(0) == GlobalVariables.charNACK && stringReadFrom.Length >= 4) lastNACKCode = stringReadFrom.Substring(1, 3); //[0]: NACK + [1][2][3]: 3 Digit --- Error Code
                        return false;
                    }
                }
                else if (commandLength == 0 || stringReadFrom.Length >= commandLength)
                {   //stringReadFrom(0): ESC;----stringReadFrom(1): COMMAND;----stringReadFrom(2->N): PARAMETER;----stringReadFrom(stringReadFrom.Length): EOT
                    if (stringReadFrom.ElementAt(0) == GlobalVariables.charESC && stringReadFrom.ElementAt(1) == commandCode.ElementAt(0) && stringReadFrom.ElementAt(stringReadFrom.Length - 1) == GlobalVariables.charEOT) return true; else return false;
                }
                else return false;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }






        private bool WaitForPrintingAcknowledge(ref string stringReadFrom)
        {
            try
            {
                bool returnValue = false;
                this.inkJetNetworkStream.ReadTimeout = 300; //Default = -1; 


                stringReadFrom = GlobalNetSockets.ReadFromStream(inkJetTcpClient, inkJetNetworkStream);

                if (stringReadFrom == GlobalVariables.charPrintingACK.ToString())   //OK State
                {
                    returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID]++;
                }
                else//  !=  : Need to check some other condition
                {
                    int countPrintingACK = GlobalStaticFunction.CountStringOccurrences(stringReadFrom, GlobalVariables.charPrintingACK.ToString());

                    if (countPrintingACK == 1)      //OK: but in case of receive only one PrintingACK, following by something: Ignore. Later: Maybe ADD SOME CODE to SHOW on screen what stringReadFrom is
                    {
                        this.MainStatus = "NMVN: Some extra thing received: " + stringReadFrom;
                        returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID]++;                      
                    }
                    else if (countPrintingACK > 1)
                    {
                        this.MainStatus = "NMVN: Receive more than one printing acknowledge, a message may printed more than one times";
                        returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID] = GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID] + countPrintingACK;   
                    }
                    else // countPrintingACK < 1    :   Fail
                    {
                        this.MainStatus = "NMVN: Random received: " + stringReadFrom;
                        returnValue = false;
                    }
                }

                this.inkJetNetworkStream.ReadTimeout = -1; //Default = -1
                return returnValue;
            }

            catch (Exception exception)
            {
                //Ignore when timeout
                if (exception.Message != "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.") this.MainStatus = exception.Message;

                this.inkJetNetworkStream.ReadTimeout = -1; //Default = -1
                return false;
            }
        }



        private bool lfStatusLED(ref string stringReadFrom)
        {//DISPLAY 3 LEDS STATUS
            try
            {
                this.LedGreenOn = ((int)stringReadFrom.ElementAt(7) & int.Parse("1")) == 1;
                this.LedAmberOn = ((int)stringReadFrom.ElementAt(7) & int.Parse("2")) == 2;
                this.LedRedOn = ((int)stringReadFrom.ElementAt(7) & int.Parse("3")) == 3;

                this.NotifyPropertyChanged("LedStatus");

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }


        private string lStatusHHMM;

        private bool lfStatusHistory(ref string stringReadFrom)
        {
            try
            {
                if (lStatusHHMM != "" + stringReadFrom.ElementAt(7) + stringReadFrom.ElementAt(8) + stringReadFrom.ElementAt(9) + stringReadFrom.ElementAt(10))
                {
                    lStatusHHMM = "" + stringReadFrom.ElementAt(7) + stringReadFrom.ElementAt(8) + stringReadFrom.ElementAt(9) + stringReadFrom.ElementAt(10);
                    this.MainStatus = "" + stringReadFrom.ElementAt(3) + stringReadFrom.ElementAt(4) + stringReadFrom.ElementAt(5);//Get the status TEXT & Maybe: Add to database
                }
                return true;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }
        }


        private bool lfStatusAlert(ref string stringReadFrom)
        {
            return true;
        }

        //Private Function lfStatusAlert(ByRef lInReceive() As Byte) As Boolean
        //    Static lAlertArray() As Byte '//lAlertArray: DE LUU LAI Alert TRUOC DO, KHI Alert THAY DOI => UPDATE TO DATABASE
        //    Dim lAlertNew As Boolean, i As Long

        //On Error GoTo ARRAY_INIT '//KIEM TRA, TRONG T/H CHU KHOI TAO CHO lAlertArray THI REDIM lAlertArray (0)
        //    If LBound(lAlertArray()) >= 0 Then GoTo ARRAY_OK
        //ARRAY_INIT:
        //    ReDim lAlertArray(0)

        //ARRAY_OK:
        //On Error GoTo ERR_HANDLER

        //    lAlertNew = UBound(lAlertArray) <> UBound(lInReceive)
        //    If Not lAlertNew Then
        //        For i = LBound(lInReceive) To UBound(lInReceive)
        //            If lInReceive(i) <> lAlertArray(i) Then lAlertNew = True: Exit For
        //        Next i
        //    End If
        //    If lAlertNew Then
        //        'DISPLAY NEW ALERT
        //        'SAVE NEW ALERT
        //        'Debug.Print "ALERT - " + Chr(lInReceive(5)) + Chr(lInReceive(6)) + Chr(lInReceive(7))
        //    End If

        //    lfStatusAlert = True
        //ERR_RESUME:
        //    Exit Function
        //ERR_HANDLER:
        //    Call psShowError: lfStatusAlert = False: GoTo ERR_RESUME
        //End Function







        #endregion Public Method


        #region Public Thread

        public void ThreadRoutine()
        {
            this.privateFillingLineData = this.FillingLineData.ShallowClone();


            string stringReadFrom = ""; bool lPrinterReady = false;
            bool ReadyToPrint = false; bool HeadEnable = false;


            this.LoopRoutine = true; this.StopPrint();


            //This command line is specific to CartonInkJet ON FillingLine.Pail (Just here only for this specific)
            if (this.FillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail && this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet) { this.LedGreenOn = true; return; } //DO NOTHING


            try
            {

                if (!this.Connect()) throw new System.InvalidOperationException("NMVN: Can not connect to printer.");

                #region INITIALISATION PRINTER
                do  //INITIALISATION COMMAND
                {
                    this.WriteToStream(GlobalVariables.charESC + "/A/?/" + GlobalVariables.charEOT);   //A: Printer Identity
                    if (this.ReadFromStream(ref stringReadFrom, false, "A", 14)) lPrinterReady = true; //A: Printer Identity OK"

                    if (lPrinterReady)
                    {
                        do //CHECK PRINTER READY TO PRINT
                        {
                            this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status
                            if (this.ReadFromStream(ref stringReadFrom, false, "O", 9)) { lfStatusLED(ref stringReadFrom); ReadyToPrint = this.LedGreenOn; this.LedGreenOn = false; } //After Set LED, If LedGreenOn => ReadyToPrint

                            if (!ReadyToPrint)
                            {
                                this.WriteToStream(GlobalVariables.charESC + "/O/S/1/" + GlobalVariables.charEOT); //O/S/1: Turn on ink-jet
                                if (this.ReadFromStream(ref stringReadFrom, true))
                                {
                                    this.MainStatus = "Turn printer sequencing on. Please wait for 5 minutes";
                                    Thread.Sleep(50000);
                                }
                                else throw new System.InvalidOperationException("NMVN: Can not turn printer sequencing on: " + stringReadFrom);
                            }
                            else //ReadyToPrint: OK
                            {
                                this.WriteToStream(GlobalVariables.charESC + "/Q/1/?/" + GlobalVariables.charEOT);    //Q: HEAD ENABLE: ENABLE

                                if (this.ReadFromStream(ref stringReadFrom, false, "Q", 5))
                                {
                                    if (stringReadFrom.ElementAt(3).ToString() == "Y")
                                        HeadEnable = true;
                                    else
                                    {
                                        this.WriteToStream(GlobalVariables.charESC + "/Q/1/Y/" + GlobalVariables.charEOT);
                                        if (this.ReadFromStream(ref stringReadFrom, true))
                                        {
                                            this.MainStatus = "Head enable. Please wait for 10 seconds";
                                            Thread.Sleep(10000);
                                        }
                                        else throw new System.InvalidOperationException("NMVN: Can not enable head: " + stringReadFrom);
                                    }
                                }
                            }

                        } while (this.LoopRoutine && (!ReadyToPrint || !HeadEnable));
                    }
                    else
                    {
                        this.MainStatus = "Can not connect to printer. Trying to connect ... Press disconnect to exit.";
                    }
                } while (this.LoopRoutine && !lPrinterReady && !ReadyToPrint && !HeadEnable);

                #endregion INITIALISATION COMMAND


                //P: Message To Head Assugment '//CHU Y QUAN TRONG: DUA MSG SO 1 LEN DAU IN (SAN SANG KHI IN, BOI VI KHI IN TA STORAGE MSG VAO VI TRI SO 1 MA KHONG QUAN TAM DEN VI TRI SO 2, 3,...)
                this.WriteToStream(GlobalVariables.charESC + "/P/1/001/" + GlobalVariables.charEOT);
                if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("NMVN: Can not assign message to head: " + stringReadFrom);


                //C: Set Clock
                this.WriteToStream(GlobalVariables.charESC + "/C/" + DateTime.Now.ToString("yyyy/MM/dd/00/hh/mm/ss") + "/" + GlobalVariables.charEOT);     //C: Set Clock
                if (!this.ReadFromStream(ref stringReadFrom, true)) throw new System.InvalidOperationException("NMVN: Can not synchronize clock: " + stringReadFrom);

                //T: Reset Product Counting
                this.WriteToStream(GlobalVariables.charESC + "/T/1/0/" + GlobalVariables.charEOT);
                if (!this.ReadFromStream(ref stringReadFrom, true)) throw new System.InvalidOperationException("NMVN: Can not reset counting: " + stringReadFrom);



                #region Status
                //SET STATUS

                this.WriteToStream(GlobalVariables.charESC + "/0/N/0/" + GlobalVariables.charEOT);     //0: Status Report Mode: OFF: NO ERROR REPORTING
                if (!this.ReadFromStream(ref stringReadFrom, true)) throw new System.InvalidOperationException("NMVN: Can not set status report mode: " + stringReadFrom);

                //co gang viet cho nay cho hay hay
                //this.WriteToStream( GlobalVariables.charESC + "/1/C/?/" + GlobalVariables.charEOT) ;   //1: REQUEST CURRENT STATUS
                //if (!this.ReadFromStream(ref stringReadFrom, false, "1", 12) )
                //    throw new System.InvalidOperationException("NMVN: Can not request current status: " + stringReadFrom);
                //else
                ////        Debug.Print "STATUS " + Chr(lInReceive(3)) + Chr(lInReceive(4)) + Chr(lInReceive(5))
                ////'        If Not ((lInReceive(3) = Asc("0") Or lInReceive(3) = Asc("1")) And lInReceive(4) = Asc("0") And lInReceive(5) = Asc("0")) Then GoTo ERR_HANDLER   'NOT (READY OR WARNING)
                ////    End If
                #endregion Status


                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    if (!this.OnPrinting)
                    {
                        #region Reset Message: Clear message

                        if (this.resetMessage)
                        {
                            this.WriteToStream(GlobalVariables.charESC + "/S/001/  /" + GlobalVariables.charEOT);//S: Message Storage: Set Blank (Two Space)
                            if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(500); else throw new System.InvalidOperationException("NMVN: Can not set message: " + stringReadFrom);

                            this.WriteToStream(GlobalVariables.charESC + "/I/1/ /" + GlobalVariables.charEOT); //SET OF: Print Acknowledgement Flags I
                            if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(50); else throw new System.InvalidOperationException("NMVN: Can not set printing acknowledge: " + stringReadFrom);

                            this.MainStatus = "Ready to print";
                            this.resetMessage = false; //Setup first message: Only one times                            
                        }
                        #endregion Reset Message: Clear message
                    }

                    else //this.OnPrinting
                    {
                        #region Reset Message: Setup FirstMessage

                        if (this.resetMessage)
                        {
                            this.WriteToStream(GlobalVariables.charESC + "/S001/" + this.WholeMessageLine() + "/" + GlobalVariables.charEOT);
                            if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(750); else throw new System.InvalidOperationException("NMVN: Can not set message: " + stringReadFrom);



                            #region Update serial number - Note: Some DOMINO firmware version does not need to update serial number. Just set startup serial number only when insert serial number. BUT: FOR SURE, It will be updated FOR ALL
                            if (this.DominoPrinterNameID != GlobalVariables.DominoPrinterName.CartonInkJet) //ONLY CartonInkJet
                            {
                                //    U: UPDATE SERIAL NUMBER - Counter 1
                                this.WriteToStream(GlobalVariables.charESC + "/U/001/1/" + this.privateFillingLineData.MonthSerialNumber + "/" + GlobalVariables.charEOT);
                                if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("NMVN: Can not update serial number: " + stringReadFrom);


                                //    U: UPDATE SERIAL NUMBER - Counter 2
                                this.WriteToStream(GlobalVariables.charESC + "/U/001/2/" + this.privateFillingLineData.MonthSerialNumber + "/" + GlobalVariables.charEOT);
                                if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("NMVN: Can not update serial number: " + stringReadFrom);
                            }
                            #endregion Update serial number



                            this.WriteToStream(GlobalVariables.charESC + "/I/1/" + (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet ? "a" : " ") + "/" + GlobalVariables.charEOT); //SET ON for DominoPrinterName.CartonInkJet ONLY
                            if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(50); else throw new System.InvalidOperationException("NMVN: Can not set printing acknowledge: " + stringReadFrom);

                            this.MainStatus = "Printing ....";
                            this.resetMessage = false; //Setup first message: Only one times                            
                        }

                        #endregion Reset Message: Setup FirstMessage

                        #region setup message for every message. Only for DominoPrinterName.CartonInkJet

                        if (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet) //ONLY CartonInkJet
                        {
                            if (this.WaitForPrintingAcknowledge(ref stringReadFrom))
                            {
                                //Manual increase BatchCartonNumber & MonthCartonNumber
                                this.privateFillingLineData.BatchCartonNumber = (int.Parse(this.privateFillingLineData.BatchCartonNumber) + 1).ToString("0000000").Substring(1);//Format 7 digit, then cut 6 right digit: This will reset a 0 when reach the limit of 6 digit
                                this.privateFillingLineData.MonthCartonNumber = (int.Parse(this.privateFillingLineData.MonthCartonNumber) + 1).ToString("0000000").Substring(1);

                                Thread.Sleep(20); this.WriteToStream(GlobalVariables.charESC + "/S001/" + this.WholeMessageLine() + "/" + GlobalVariables.charEOT);//Required 20ms? For sure only, this maybe no need
                                if (this.ReadFromStream(ref stringReadFrom, true)) Thread.Sleep(300); //else throw new System.InvalidOperationException("NMVN: Can not set message: " + stringReadFrom);

                                //If place this COMMAND BEFORE increase BatchCartonNumber & MonthCartonNumber, It will show the printed value on User Interface
                                this.NotifyPropertyChanged("BatchCartonNumber"); //this.NotifyPropertyChanged("MonthCartonNumber"); //Just NotifyPropertyChanged only, no need to duplicate
                            }
                        }

                        #endregion setup message for every message. Only for DominoPrinterName.CartonInkJet

                        #region Read counter; only for DominoPrinterName.BarCodeInkJet
                        if (this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.BarCodeInkJet)
                        {
                            //    U: Read Counter 1 (ONLY COUNTER 1---COUNTER 2: THE SAME COUNTER 1: Principlely)
                            this.WriteToStream(GlobalVariables.charESC + "/U/001/1/?/" + GlobalVariables.charEOT);
                            if (this.ReadFromStream(ref stringReadFrom, false, "U", 13))
                            {
                                int serialNumber = 0;
                                if (int.TryParse(stringReadFrom.Substring(6, 6), out serialNumber) && int.Parse(this.privateFillingLineData.MonthSerialNumber) != ++serialNumber) //Increase serialNumber by 1: Because: this.privateFillingLineData.MonthSerialNumber MUST GO AHEAD BY 1
                                {
                                    this.privateFillingLineData.MonthSerialNumber = serialNumber.ToString("0000000").Substring(1);
                                    this.NotifyPropertyChanged("MonthSerialNumber");
                                }
                            }
                        }
                        #endregion Read counter
                    }


                    if (!this.OnPrinting || this.DominoPrinterNameID != GlobalVariables.DominoPrinterName.CartonInkJet)
                    {
                        #region Get current status

                        this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status
                        if (this.ReadFromStream(ref stringReadFrom, false, "O", 9))
                        {
                            lfStatusLED(ref stringReadFrom);
                            if (!this.LedGreenOn) throw new System.InvalidOperationException("Connection fail! Please check your printer.");
                        }


                        ////'//STATUS ONLY.BEGIN
                        //this.WriteToStream(GlobalVariables.charESC + "/1/H/?/" + GlobalVariables.charEOT);      //H: Request History Status
                        //if (this.ReadFromStream(ref stringReadFrom, false, "1", 12)) this.lfStatusHistory(ref stringReadFrom);

                        //this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);      //O/1: Get Current LED Status
                        //if (this.ReadFromStream(ref stringReadFrom, false, "O", 9)) this.lfStatusLED(ref stringReadFrom);

                        //this.WriteToStream(GlobalVariables.charESC + "/O/2/?/" + GlobalVariables.charEOT);      //O/2: Get Current Alert
                        //if (this.ReadFromStream(ref stringReadFrom, false, "O", 0)) this.lfStatusAlert(ref stringReadFrom);
                        ////'//STATUS ONLY.END
                        #endregion Get current status

                        Thread.Sleep(110);
                    }

                } //End while this.LoopRoutine


                //DISCONNECT.BEGIN
                this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status
                if (this.ReadFromStream(ref stringReadFrom, false, "O", 9)) lfStatusLED(ref stringReadFrom);

                //DISCONNECT.END


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



        #endregion Public Thread


    }
}
