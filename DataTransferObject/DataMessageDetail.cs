using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Global.Class.Library;

namespace DataTransferObject
{
    public class DataMessageDetail : NotifyPropertyChangeObject
    {
        private DateTime sendDate;
        private string sendType;
        private string sendMessage;

        private double counterValueBefore;
        private double counterValueAfter;

        private double counterAutonicsBefore;
        private double counterAutonicsAfter;

        private string description;
        private string remarks;


        public DataMessageDetail()
            : this(DateTime.Now, "", "", 0, 0, 0, 0, "", "")
        {
        }

        public DataMessageDetail(DateTime sendDate, string sendType, string sendMessage, double counterValueBefore, double counterValueAfter, double counterAutonicsBefore, double counterAutonicsAfter, string description, string remarks)
        {
            GlobalDefaultValue.Apply(this);

            this.SendDate = sendDate;
            this.SendType = sendType;
            this.SendMessage = sendMessage;

            this.CounterValueBefore = counterValueBefore;
            this.CounterValueAfter = counterValueAfter;

            this.CounterAutonicsBefore = counterAutonicsBefore;
            this.CounterAutonicsAfter = counterAutonicsAfter;

            this.Description = description;
            this.Remarks = remarks;
        }


        #region Properties

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime SendDate
        {
            get { return this.sendDate; }
            set { ApplyPropertyChange<DataMessageDetail, DateTime>(ref this.sendDate, o => o.SendDate, value); }
        }

        [DefaultValue("")]
        public string SendType
        {
            get { return this.sendType; }
            set { ApplyPropertyChange<DataMessageDetail, string>(ref this.sendType, o => o.SendType, value); }
        }

        [DefaultValue("")]
        public string SendMessage
        {
            get { return this.sendMessage; }
            set { ApplyPropertyChange<DataMessageDetail, string>(ref this.sendMessage, o => o.SendMessage, value); }
        }


        [DefaultValue(0)]
        public double CounterValueBefore
        {
            get { return this.counterValueBefore; }
            set { ApplyPropertyChange<DataMessageDetail, double>(ref this.counterValueBefore, o => o.CounterValueBefore, Math.Round(value, GlobalVariables.Round0Amount)); }
        }

        [DefaultValue(0)]
        public double CounterValueAfter
        {
            get { return this.counterValueAfter; }
            set { ApplyPropertyChange<DataMessageDetail, double>(ref this.counterValueAfter, o => o.CounterValueAfter, Math.Round(value, GlobalVariables.Round0Amount)); }
        }


        [DefaultValue(0)]
        public double CounterAutonicsBefore
        {
            get { return this.counterAutonicsBefore; }
            set { ApplyPropertyChange<DataMessageDetail, double>(ref this.counterAutonicsBefore, o => o.CounterAutonicsBefore, Math.Round(value, GlobalVariables.Round0Amount)); }
        }

        [DefaultValue(0)]
        public double CounterAutonicsAfter
        {
            get { return this.counterAutonicsAfter; }
            set { ApplyPropertyChange<DataMessageDetail, double>(ref this.counterAutonicsAfter, o => o.CounterAutonicsAfter, Math.Round(value, GlobalVariables.Round0Amount)); }
        }


        [DefaultValue("")]
        public string Description
        {
            get { return this.description; }
            set { ApplyPropertyChange<DataMessageDetail, string>(ref this.description, o => o.Description, value); }
        }

        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<DataMessageDetail, string>(ref this.remarks, o => o.Remarks, value); }
        }


        protected override System.Collections.Generic.List<ValidationRule> CreateRules()
        {
            List<ValidationRule> validationRules = base.CreateRules();
            //validationRules.Add(new SimpleValidationRule("CommonID", "Please fill a common ID.", delegate { return (this.CommonID > 0); }));
            //validationRules.Add(new SimpleValidationRule("CommonValue", "Please fill a common value.", delegate { return (this.CommonValue > 0); }));

            return validationRules;
        }

        #endregion
    }
}
