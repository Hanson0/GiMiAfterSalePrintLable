using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class LabelGetAndCheckProperties : CommonProperties
    {
        private string value;

        private int insertIndex;


        [Category("Insert"), Description("Value，插入内容")]

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
        [Category("Insert"), Description("InsertIndex，插入位置")]
        public int InsertIndex
        {
            get
            {
                return insertIndex;
            }

            set
            {
                insertIndex = value;
            }
        }
    }

}
