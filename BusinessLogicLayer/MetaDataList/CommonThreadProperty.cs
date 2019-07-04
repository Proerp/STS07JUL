using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//using Global.Class.Library;
using DataTransferObject;

namespace BusinessLogicLayer
{
    public class CommonThreadProperty : GlobalNotifyPropertyChanged 
    {
        private FillingLineData fillingLineData;

        private bool loopRoutine = false;

        private string mainStatus;

        private bool ledGreenOn;
        private bool ledAmberOn;
        private bool ledRedOn;


        private bool cdChanged;

        
        #region Public Properties


        public FillingLineData FillingLineData
        {
            get
            {
                return this.fillingLineData;
            }

            protected set
            {
                this.fillingLineData = value;
            }
        }


        public bool LoopRoutine
        {
            get
            {
                return this.loopRoutine;
            }

            set
            {
                if (this.loopRoutine != value)
                {
                    this.loopRoutine = value;
                }
            }
        }


        public string MainStatus
        {
            get
            {
                return this.mainStatus;
            }

            set //protected 
            {
                if (this.mainStatus != value)
                {
                    this.mainStatus = value;
                    if (this.mainStatus != "") this.NotifyPropertyChanged("MainStatus");
                }
            }
        }


        public bool LedGreenOn
        {
            get
            {
                return this.ledGreenOn;
            }
            protected set
            {
                this.ledGreenOn = value;
            }
        }

        public bool LedAmberOn
        {
            get
            {
                return this.ledAmberOn;
            }
            protected set
            {
                this.ledAmberOn = value;
            }

        }

        public bool LedRedOn
        {
            get
            {
                return this.ledRedOn;
            }
            protected set
            {
                this.ledRedOn = value;
            }
        }



        public bool CDChanged
        {
            get
            {
                return this.cdChanged;
            }
            set
            {
                this.cdChanged = value;
            }
        }


        #endregion Puclic Properties

    }

}
