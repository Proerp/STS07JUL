using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

using Global.Class.Library;


namespace BusinessLogicLayer
{
    public class IOPortRS232 : CommonThreadProperty
    {
        private SerialPort serialPort;
        private System.Timers.Timer timerReadTimeOut;

        private bool waitToRead = false;
        private byte[] readResultBuffer = new byte[0];

        private Stopwatch stopwatchPin1;

        public IOPortRS232(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, bool dtrEnable) //GlobalVariables.PortName, 38400, Parity.None, 8, StopBits.One: dtrEnable = true
        {
            try
            {
                this.serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

                this.serialPort.DtrEnable = dtrEnable;
                this.serialPort.RtsEnable = true; //Use this pin to alarm

                this.serialPort.ReadTimeout = 500;

                this.serialPort.NewLine = GlobalVariables.charETX.ToString();

                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                this.serialPort.PinChanged += new SerialPinChangedEventHandler(serialPort_PinChanged);

                this.timerReadTimeOut = new System.Timers.Timer(500);
                this.timerReadTimeOut.Elapsed += new System.Timers.ElapsedEventHandler(timerReadTimeOut_Elapsed);

                this.stopwatchPin1 = new Stopwatch();
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #region Event Handler

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(30);

                int bytesToRead = serialPort.BytesToRead;
                this.MainStatus = bytesToRead.ToString();
                if (bytesToRead > 0)
                {
                    lock (this.readResultBuffer)
                    {
                        this.readResultBuffer = new byte[bytesToRead];
                        serialPort.Read(this.readResultBuffer, 0, bytesToRead);
                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        void serialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialPinChange.CDChanged)
                {
                    if (!this.stopwatchPin1.IsRunning || this.stopwatchPin1.ElapsedMilliseconds >= 1000 * 30) //30 second
                    {
                        this.CDChanged = true;
                        this.stopwatchPin1.Restart();
                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        void timerReadTimeOut_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.waitToRead = false;
        }

        #endregion Event Handler

        public bool Connect()
        {
            try
            {
                if (this.serialPort.IsOpen) this.serialPort.Close();

                this.serialPort.Open();

                if (!this.serialPort.IsOpen) throw new System.InvalidOperationException("NMVN: Can not connect to COMPORT: " + this.serialPort.PortName);

                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (this.serialPort.IsOpen) this.serialPort.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public void SendToSerial(string stringHex)
        {
            try
            {
                stringHex = stringHex.Replace(" ", "");
                stringHex = stringHex.Replace("/", "");
                stringHex = stringHex.Replace(",", "");
                stringHex = stringHex.Replace("-", "");

                byte[] buff = GlobalStaticFunction.HexStringToByteArray(stringHex);

                if (this.serialPort.IsOpen)
                {
                    this.serialPort.ReadExisting();//Clear the port before send
                    this.readResultBuffer = new byte[0];//Clear buffer before send
                    this.serialPort.Write(buff, 0, buff.Length);

                    this.MainStatus = stringHex;
                    this.MainStatus = "#";
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        public bool ReadFromSerial(bool responseACK, out byte[] returnResultBuffer)
        {
            try
            {
                returnResultBuffer = new byte[0];

                this.waitToRead = true;
                this.timerReadTimeOut.Enabled = true;





                ////string  s = (DateTime.Now.Ticks).ToString("###########00000000000");
                ////if (this.serialPort.PortName == "COM3")
                ////    s = s.Substring(s.Length - 5, 5) + "00000";
                ////else
                ////    s = s.Substring(s.Length - 13, 13) + "00000000000";

                ////returnResultBuffer = Encoding.ASCII.GetBytes(s  );
                //////returnResultBuffer = new byte[] { 56, 56, (byte)(50 + (DateTime.Now.Second % 8)), (byte)(50 + (DateTime.Now.Second % 9)), 56, 56, (byte)(50 + (DateTime.Now.Second % 8)), 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56 };
                ////return true; //TEST ONLY




                while (this.waitToRead)
                {
                    lock (this.readResultBuffer)
                    {
                        if (this.readResultBuffer.Length > 0)
                        {
                            returnResultBuffer = this.readResultBuffer.Clone() as byte[];
                            break;
                        }
                    }
                }

                this.timerReadTimeOut.Enabled = false;


                if (returnResultBuffer.Length > 0)
                {
                    if (responseACK)
                        return returnResultBuffer[0] == GlobalVariables.ACK;
                    else
                        return true;
                }
                else
                    return false;


            }
            catch
            {
                returnResultBuffer = new byte[0];
                return false;
            }
        }


        public bool ReadFromSerial(bool responseACK)
        {
            byte[] resultBuffer = new byte[0];
            return this.ReadFromSerial(responseACK, out resultBuffer);
        }


        public bool ReadFromSerial(bool responseACK, ref string returnResultString)
        {
            bool returnValue; byte[] resultBuffer = new byte[0];
            returnValue = this.ReadFromSerial(responseACK, out resultBuffer);

            returnResultString = Encoding.ASCII.GetString(resultBuffer);
            returnResultString = returnResultString.Replace("\0", "");

            return returnValue;
        }


        public void SetPinRTS(bool enabledOrDisabled)
        {
            try
            {
                if (this.serialPort.RtsEnable != enabledOrDisabled)
                    this.serialPort.RtsEnable = enabledOrDisabled;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

    }
}
