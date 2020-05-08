using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class FindDeviceProperties : CommonProperties
    {
        private string testPowerOnAT;
        private int atCommandInterval;
        private string endLine;
        private string atCommandOk;


        [Category("AT"), Description("TestPowerOnAT")]
        public string TestPowerOnAT
        {
            get { return testPowerOnAT; }
            set { testPowerOnAT = value; }
        }
        [Category("AT"), Description("AtCommandInterval")]
        public int AtCommandInterval
        {
            get { return atCommandInterval; }
            set { atCommandInterval = value; }
        }
        [Category("AT"), Description("EndLine")]
        public string EndLine
        {
            get { return endLine; }
            set { endLine = value; }
        }

        [Category("AT"), Description("AtCommandOk")]
        public string AtCommandOk
        {
            get { return atCommandOk; }
            set { atCommandOk = value; }
        }


    }
}
