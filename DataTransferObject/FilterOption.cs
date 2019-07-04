using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Global.Class.Library;

namespace DataTransferObject
{
    public class FillterOption
    {
        private DateTime lowerFillterDate;
        public DateTime LowerFillterDate
        {
            get { return lowerFillterDate; }
            set { lowerFillterDate = value; }
        }

        private DateTime upperFillterDate;
        public DateTime UpperFillterDate
        {
            get { return upperFillterDate; }
            set { upperFillterDate = value; }
        }

        private int fillingLineID;
        public int FillingLineID
        {
            get { return fillingLineID; }
            set { fillingLineID = value; }
        }

        private DateTime[] arrayDownLoadInterval;
        private string downLoadInterval;
        public string DownLoadInterval
        {
            get { return downLoadInterval; }
            set
            {
                try
                {
                    string[] stringDownLoadIntervalList = value.Split(new Char[] { ';' });
                    arrayDownLoadInterval = new DateTime[stringDownLoadIntervalList.Count()];
                    for (int i = 0; i < stringDownLoadIntervalList.Count(); i++)
                    {
                        arrayDownLoadInterval[i] = DateTime.Parse(stringDownLoadIntervalList[i]);
                    }

                    downLoadInterval = value;
                    GlobalRegistry.Write("DownLoadInterval", this.DownLoadInterval);
                }
                catch (Exception exception)
                {
                    arrayDownLoadInterval = null;
                    throw exception;
                }
            }
        }

        public DateTime[] DownLoadIntervalArray
        {
            get { return arrayDownLoadInterval; }
        }

        private int upLoadInterval;
        public int UpLoadInterval
        {
            get { return upLoadInterval; }
            set { upLoadInterval = value; GlobalRegistry.Write("UpLoadInterval", this.UpLoadInterval.ToString()); }
        }

        public FillterOption()
        {
            try
            {
                this.FillingLineID = 0;

                this.LowerFillterDate = DateTime.Today.AddDays(-1);
                this.UpperFillterDate = DateTime.Today.AddDays(365);


                string savedDownLoadInterval = GlobalRegistry.Read("DownLoadInterval");
                this.DownLoadInterval = savedDownLoadInterval == null ? "8:00" : savedDownLoadInterval;

                string savedUpLoadInterval = GlobalRegistry.Read("UpLoadInterval"); int lastUpLoadInterval;
                if (int.TryParse(savedUpLoadInterval, out lastUpLoadInterval)) this.UpLoadInterval = lastUpLoadInterval; else this.UpLoadInterval = 60;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }




    }
}
