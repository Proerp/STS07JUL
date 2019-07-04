﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Global.Class.Library;


namespace DataTransferObject.MetaDataList
{
    public class ListEmployeeDetail : NotifyPropertyChangeObject
    {
        private int commonID;
        private double commonValue;

        private string remarks;


        public ListEmployeeDetail()
            : this(0, 0, "")
        {
        }

        public ListEmployeeDetail(int commonID, double commonValue, string remarks)
        {
            GlobalDefaultValue.Apply(this);

            this.CommonID = commonID;
            this.CommonValue = commonValue;

            this.Remarks = remarks;
        }


        #region Properties

        [DefaultValue(0)]
        public int CommonID
        {
            get { return this.commonID; }
            set { ApplyPropertyChange<ListEmployeeDetail, int>(ref this.commonID, o => o.CommonID, value); }
        }


        [DefaultValue(0)]
        public double CommonValue
        {
            get { return this.commonValue; }
            set
            {
                value = Math.Round(value, GlobalVariables.Round0Quantity);
                ApplyPropertyChange<ListEmployeeDetail, double>(ref this.commonValue, o => o.CommonValue, value);
            }
        }


        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<ListEmployeeDetail, string>(ref this.remarks, o => o.Remarks, value); }
        }

        #endregion

        protected override System.Collections.Generic.List<ValidationRule> CreateRules()
        {
            List<ValidationRule> validationRules = base.CreateRules();
            validationRules.Add(new SimpleValidationRule("CommonID", "Please fill a common ID.", delegate { return (this.CommonID > 0); }));
            validationRules.Add(new SimpleValidationRule("CommonValue", "Please fill a common value.", delegate { return (this.CommonValue > 0); }));

            return validationRules;
        }
    }
}
