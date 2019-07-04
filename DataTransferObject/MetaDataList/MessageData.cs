using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class MessageData
    {
        private int packID;
        private int packSubQueueID;
        private string printedBarcode;
        private string remarks;

        public MessageData(string printedBarcode)
        {
            this.packID = -1;
            this.printedBarcode = printedBarcode;
        }

        #region Public Properties


        public int PackID
        {
            get { return this.packID; }
            set { this.packID = value; }
        }

        public int PackSubQueueID
        {
            get { return this.packSubQueueID; }
            set { this.packSubQueueID = value; }
        }

        public string PrintedBarcode
        {
            get
            {
                return this.printedBarcode;
            }
        }


        public string Remarks
        {
            get
            {
                return this.remarks;
            }

            set
            {
                if (value != this.remarks)
                {
                    this.remarks = value;
                }
            }
        }


        #endregion Public Properties

        public MessageData ShallowClone()
        {
            return (MessageData)this.MemberwiseClone();
        }

    }
}
