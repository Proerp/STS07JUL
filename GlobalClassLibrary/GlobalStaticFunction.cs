using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Reflection;

namespace Global.Class.Library
{
    public class GlobalStaticFunction
    {

        public static int DateToContinuosMonth()
        {
            return GlobalStaticFunction.DateToContinuosMonth(DateTime.Now);
        }

        public static int DateToContinuosMonth(DateTime dateTime)
        {
            //DateTime currentDateTime = new DateTime();
            return (dateTime.Year - 2013) * 12 + dateTime.Month - 5;
        }

        public static int CountStringOccurrences(string mainString, string patternString)
        {
            // Loop through all instances of the string 'mainString'.
            int i = 0;
            int countStringOccurrences = 0;

            while ((i = mainString.IndexOf(patternString, i)) != -1)
            {
                i += patternString.Length;
                countStringOccurrences++;
            }
            return countStringOccurrences;
        }



        public static string TextToASCII(string s)
        {

            //string str = char.ConvertFromUtf32(65); //ASCII to Text

            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in s)
            {
                Console.WriteLine(System.Convert.ToInt32(c));

                stringBuilder.Append(System.Convert.ToInt32(c).ToString());

            }
            return stringBuilder.ToString();

        }


        public static string TextToHEX(string text)//string text = "abcd"
        {
            //Text To Hex

            char[] chars = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in chars)
            {
                stringBuilder.Append(((Int16)c).ToString("X2"));
                stringBuilder.Append(",");
            }
            String textAsHex = stringBuilder.ToString();
            Console.WriteLine(textAsHex);
            return textAsHex;

        }

        public static string NumericTextToHEX(string text) //string text = "10"
        {
            // convert a textual representation of a number to hex

            Int32 number;
            if (Int32.TryParse(text, out number))
            {
                String textAsHex = number.ToString("X2");
                Console.WriteLine(textAsHex);

                return textAsHex;
            }
            else
                return "";

        }




        //----------------------

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }



        //----------------------



        /// <summary>
        /// 2 byte arraries take XOR operation
        /// </summary>
        /// <param name="byStream1">first byte array</param>
        /// <param name="byStream2">second byte array</param>
        /// <returns>the result, a byte array what is the XOR result of byStrea</returns>
        public static byte[] XORBytes(byte[] byteFile1, byte[] byteFile2)
        {
            //as we know, the length of two byte arraies may be not equal,
            //in this case,we need  loop the longer one, use the zero instead of the blanks. 
            int iLength1 = byteFile1.Length;
            int iLength2 = byteFile2.Length;
            int iDeffAbs = System.Math.Abs(iLength1 - iLength2);//discrepancy 
            bool bStreamIsBigger = (iLength1 > iLength2); //true, if the first array is longer than second, otherwise false.
            int iMaxLength = bStreamIsBigger ? iLength1 : iLength2;

            byte biZero = (byte)0; // the zero will be XOR with the discrepancy

            byte[] byResult = new byte[iMaxLength]; // the result array of first array XOR with second array

            //loop......
            for (int i = 0; i < iMaxLength; i++)
            {
                //in the shorter range, both first and second array have byte
                if (i < (iMaxLength - iDeffAbs))
                {
                    byResult[i] = (byte)(byteFile1[i] ^ byteFile2[i]);
                }
                else // in the discrepancy range, one of the array have not byte, we instead of zero
                {
                    //first byte array is longer
                    if (bStreamIsBigger)
                    {
                        byResult[i] = (byte)(byteFile1[i] ^ biZero);
                    }
                    else//second byte array is longer
                    {
                        byResult[i] = (byte)(biZero ^ byteFile2[i]);
                    }
                }
            }

            return byResult;
        }


        public static byte[] CheckSumHEXString  (string stringReadFrom)
        {
            string[] arrayBarcode = stringReadFrom.Split(new Char[] { (char) 44});
            byte [] result = new byte[0];
            for (int i = 0; i < arrayBarcode.Length; i++)
            {
                result = i ==0? GlobalStaticFunction.StringToByteArrayFastest(arrayBarcode[i] ): GlobalStaticFunction.XORBytes(result, GlobalStaticFunction.StringToByteArrayFastest(arrayBarcode[i]));

            }

            return result;


            

        }





        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        public static byte[] HexStringToByteArray(string s)
        {
            try
            {
                s = s.Replace(" ", "");
                byte[] buffer = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }



        public static string IntToHexString(int data) //Le Minh Hiep
        {
            return Convert.ToString(data, 16).PadLeft(2, '0').ToUpper();
        }






        #region CopyProperties

        public static void CopyProperties(object sourceInstance, object destinationInstance) //Copy properties from sourceInstance to destinationInstance with THE SAME OBJECT CLASS
        {
            PropertyInfo[] propertyInfoList = sourceInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(destinationInstance, propertyInfo.GetValue(sourceInstance, null), null);
                }
            }
        }




        //void Copy(SomeObject srcInstance, SomeObject destInstance) //Copy properties from sourceInstance to destinationInstance with THE SAME/ DIFFERENT OBJECT CLASS
        //{
        //    PropertyInfo[] srcPropInfos = srcInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    PropertyInfo[] destPropInfos = srcInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo pi in srcPropInfos)
        //    {
        //        if (pi.CanWrite())
        //        {
        //            PropertyInfo destPI = Array<PropertyInfo>.Find(destPropInfos, delegate(PropertyInfo dpi)
        //            {
        //                return (0 == string.Compare(dpi.Name, pi.Name, true));
        //            });

        //            Debug.Assert(destPI != null && destPI.CanWrite());
        //            destPI.SetValue(destInstance, pi.GetValue(srcInstance, null), null);
        //        }
        //    }
        //}


        #endregion CopyProperties

        #region Object Clone Noticed

        //http://msdn.microsoft.com/en-us/library/system.object.memberwiseclone.aspx

        //        Object.MemberwiseClone Method
        //            protected Object MemberwiseClone()

        //            The MemberwiseClone method creates a shallow copy by creating a new object, and then copying the nonstatic fields of the current object to the new object. If a field is a value type, a bit-by-bit copy of the field is performed. If a field is a reference type, the reference is copied but the referred object is not; therefore, the original object and its clone refer to the same object.
        //For example, consider an object called X that references objects A and B. Object B, in turn, references object C. A shallow copy of X creates new object X2 that also references objects A and B. In contrast, a deep copy of X creates a new object X2 that references the new objects A2 and B2, which are copies of A and B. B2, in turn, references the new object C2, which is a copy of C. The example illustrates the difference between a shallow and a deep copy operation.
        //There are numerous ways to implement a deep copy operation if the shallow copy operation performed by the MemberwiseClone method does not meet your needs. These include the following:
        //Call a class constructor of the object to be copied to create a second object with property values taken from the first object. This assumes that the values of an object are entirely defined by its class constructor.
        //Call the MemberwiseClone method to create a shallow copy of an object, and then assign new objects whose values are the same as the original object to any properties or fields whose values are reference types. The DeepCopy method in the example illustrates this approach.
        //Serialize the object to be deep copied, and then restore the serialized data to a different object variable.
        //Use reflection with recursion to perform the deep copy operation.


        //    using System;

        //public class IdInfo
        //{
        //    public int IdNumber;

        //    public IdInfo(int IdNumber)
        //    {
        //        this.IdNumber = IdNumber;
        //    }
        //}

        //public class Person 
        //{
        //    public int Age;
        //    public string Name;
        //    public IdInfo IdInfo;

        //    public Person ShallowCopy()
        //    {
        //       return (Person)this.MemberwiseClone();
        //    }

        //    public Person DeepCopy()
        //    {
        //       Person other = (Person) this.MemberwiseClone(); 
        //       other.IdInfo = new IdInfo(this.IdInfo.IdNumber);
        //       return other;
        //    }
        //}

        //public class Example
        //{
        //    public static void Main()
        //    {
        //        // Create an instance of Person and assign values to its fields.
        //        Person p1 = new Person();
        //        p1.Age = 42;
        //        p1.Name = "Sam";
        //        p1.IdInfo = new IdInfo(6565);

        //        // Perform a shallow copy of p1 and assign it to p2.
        //        Person p2 = (Person) p1.ShallowCopy();

        //        // Display values of p1, p2
        //        Console.WriteLine("Original values of p1 and p2:");
        //        Console.WriteLine("   p1 instance values: ");
        //        DisplayValues(p1);
        //        Console.WriteLine("   p2 instance values:");
        //        DisplayValues(p2);

        //        // Change the value of p1 properties and display the values of p1 and p2.
        //        p1.Age = 32;
        //        p1.Name = "Frank";
        //        p1.IdInfo.IdNumber = 7878;
        //        Console.WriteLine("\nValues of p1 and p2 after changes to p1:");
        //        Console.WriteLine("   p1 instance values: ");
        //        DisplayValues(p1);
        //        Console.WriteLine("   p2 instance values:");
        //        DisplayValues(p2);

        //        // Make a deep copy of p1 and assign it to p3.
        //        Person p3 = p1.DeepCopy();
        //        // Change the members of the p1 class to new values to show the deep copy.
        //        p1.Name = "George";
        //        p1.Age = 39;
        //        p1.IdInfo.IdNumber = 8641;
        //        Console.WriteLine("\nValues of p1 and p3 after changes to p1:");
        //        Console.WriteLine("   p1 instance values: ");
        //        DisplayValues(p1);
        //        Console.WriteLine("   p3 instance values:");
        //        DisplayValues(p3);
        //    }

        //    public static void DisplayValues(Person p)
        //    {
        //        Console.WriteLine("      Name: {0:s}, Age: {1:d}", p.Name, p.Age);
        //        Console.WriteLine("      Value: {0:d}", p.IdInfo.IdNumber);
        //    }
        //}
        //// The example displays the following output: 
        ////       Original values of p1 and p2: 
        ////          p1 instance values: 
        ////             Name: Sam, Age: 42 
        ////             Value: 6565 
        ////          p2 instance values: 
        ////             Name: Sam, Age: 42 
        ////             Value: 6565 
        ////        
        ////       Values of p1 and p2 after changes to p1: 
        ////          p1 instance values: 
        ////             Name: Frank, Age: 32 
        ////             Value: 7878 
        ////          p2 instance values: 
        ////             Name: Sam, Age: 42 
        ////             Value: 7878 
        ////        
        ////       Values of p1 and p3 after changes to p1: 
        ////          p1 instance values: 
        ////             Name: George, Age: 39 
        ////             Value: 8641 
        ////          p3 instance values: 
        ////             Name: Frank, Age: 32 
        ////             Value: 7878









        //http://social.msdn.microsoft.com/forums/en-US/csharplanguage/thread/ec44e562-0ccc-4517-804a-e9bbb64f1035

        //ShallowClone + DeepClone ---

        //Hi hellomvc,

        //Try to follow this example :

        ///*
        //This example assumes some basic understanding of following terms :

        //-Serialization
        //-Streams
        //-Shallow Copy
        //-Deep Copy
        //*/

        //[Serializable]
        //public class Clonable : ICloneable
        //{
        //    public Clonable Pointer;
        //    public String Name;

        //    public Clonable()
        //    {
        //        Pointer = null;
        //        Name = String.Empty;
        //    }

        //    #region ICloneable Members

        //    public Object Clone()
        //    {
        //        return this.MemberwiseClone();
        //    }

        //    #endregion

        //    public Object DeepClone()
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        BinaryFormatter bf = new BinaryFormatter();

        //        bf.Serialize(ms, this);

        //        ms.Position = 0;

        //        Object ret = bf.Deserialize(ms);

        //        ms.Close();

        //        return ret;
        //    }

        //    public static void Test()
        //    {
        //        Clonable c1 = new Clonable(),c2 = new Clonable();

        //        c1.Name = "Original Object 1";
        //        c2.Name = "Original Object 2";
        //        c1.Pointer = c2;

        //        MessageBox.Show(c1.Pointer.Equals(c2).ToString()); // True

        //        Clonable c3 = (Clonable)c1.Clone(); // Shallow Copy

        //        MessageBox.Show(c3.Equals(c1).ToString()); // False
        //        MessageBox.Show(c3.Pointer.Equals(c2).ToString()); // True

        //        Clonable c4 = (Clonable)c1.DeepClone(); // Deep Copy

        //        MessageBox.Show(c4.Equals(c1).ToString()); // False
        //        MessageBox.Show(c4.Pointer.Equals(c2).ToString()); // False
        //    }
        //}

        //in Main(), add following line :

        //Clonable.Test();

        #endregion Object Clone Noticed

        public static List<Control> GetAllControls(Control controlContainer, List<Control> controlList)
        {
            foreach (Control control in controlContainer.Controls)
            {
                controlList.Add(control);
                if (control.Controls.Count > 0) controlList = GetAllControls(control, controlList);
            }

            return controlList;
        }
        public static List<Control> GetAllControls(Control controlContainer)
        {
            return GetAllControls(controlContainer, new List<Control>());
        }


    }
}
