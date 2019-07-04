using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace Global.Class.Library
{
    public class GlobalNetSockets
    {
        #region Common Stream action

        
        public static void WriteToStream(NetworkStream networkStream, string stringToWrite)
        {
            try
            {
                stringToWrite = stringToWrite.Replace("/", "");
                stringToWrite = stringToWrite.Replace(",", "");
                if (networkStream.CanWrite)
                {
                    //Byte[] arrayBytesWriteTo = Encoding.ASCII.GetBytes(stringToWrite); /MAY IN DOMINO: COMMAND LA ASCII, NEN SU DUNG CAU LENH NAY
                    byte[] arrayBytesWriteTo = GlobalStaticFunction.HexStringToByteArray(stringToWrite); //MAY IN IMAJE: COMMAND LA HEX, NEN SU DUNG CAU LENH NAY

                    if (networkStream != null) networkStream.Write(arrayBytesWriteTo, 0, arrayBytesWriteTo.Length);
                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be written");
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        public static string ReadFromStream(TcpClient tcpClient, NetworkStream networkStream)
        {
            try
            {
                if (networkStream.CanRead)
                {
                    byte[] arrayBytesReadFrom = new byte[tcpClient.ReceiveBufferSize]; // Reads NetworkStream into a byte buffer. 
                    networkStream.Read(arrayBytesReadFrom, 0, (int)tcpClient.ReceiveBufferSize); // This method blocks until at least one byte is read (Read can return anything from 0 to numBytesToRead).

                    string stringReadFrom = Encoding.ASCII.GetString(arrayBytesReadFrom);

                    stringReadFrom = stringReadFrom.Replace("\0", "");
                    //stringReadFrom = stringReadFrom.Replace(GlobalVariables.charESC.ToString(), "");
                    //stringReadFrom = stringReadFrom.Replace(GlobalVariables.charEOT.ToString(), "");

                    return stringReadFrom;
                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be read");
                }
            }
            catch (Exception exception)
            { throw exception; }
        }
        
        #endregion Common Stream action

    }
}
