using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand.Executer;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Executer;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AILinkFactoryAuto.GenJts.SmartBraceletJts
{
    public partial class CreatJtsForm : Form, IGenJts
    {
        public string ProjectPath { get; set; }
        public CreatJtsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseLabelProperties properties = new ParseLabelProperties()
            {
                MacLocation=1,
                //ImeiLocation = 1,
                //SnLocation = 2,
            };
            TaskItemManager taskItemManager = new TaskItemManager(properties);
            taskItemManager.MesPreCheckProperties.EnableCheckMac = true;
            taskItemManager.TaskItemParseLabel.Enable = true;
            //生成log
            taskItemManager.DeinitProperties.LogType = LogType.MAC;


            //打开WIFI UART
            OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
            configOpenWifiUart.PortName = "COM5";
            configOpenWifiUart.BaudRate = 115200;
            configOpenWifiUart.Dtr = true;
            configOpenWifiUart.Rts = true;
            configOpenWifiUart.EndLine = "\\r\\n";
            configOpenWifiUart.Timeout = 10 * 1000;
            configOpenWifiUart.RetryCount = 0;
            configOpenWifiUart.AtType = AtType.Manual;
            configOpenWifiUart.SleepTimeAfterFindDut = 1000;
            TaskItem openWifiUartItem = new TaskItem();
            openWifiUartItem.Enable = true;
            openWifiUartItem.Item = "打开AT指令串口";//Open Wifi Uart
            openWifiUartItem.CommonProperties = configOpenWifiUart;
            openWifiUartItem.Executer = new OpenPhoneExecutor();
            taskItemManager.Put(openWifiUartItem);

            //发现设备
            TaskItem findDevice = new TaskItem();
            findDevice.Enable = true;
            findDevice.Item = "发现设备";//Find Device
            findDevice.CommonProperties = new FindDeviceProperties()
            {
                TestPowerOnAT = "",
                AtCommandInterval = 1000,
                EndLine = "\r\n",//\r\n
                AtCommandOk = "ERROR",//+NOTICE:SCANFINISH
                Timeout = 10 * 1000,
                RetryCount = 0,
                SleepTimeBefore = 200,
            };
            findDevice.Executer = new FindDeviceExecuter();
            taskItemManager.Put(findDevice);

            //读取License文件至内存
            if (true)
            {
                TaskItem readBinItem = new TaskItem();
                readBinItem.Enable = true;
                readBinItem.Item = "读取License文件";//Read MAP From File
                readBinItem.CommonProperties = new ReadMapProperties()
                {
                    BinFolderPath = "./bin",
                    RetryCount = 0,
                };
                readBinItem.Executer = new ReadMapExecuter();
                taskItemManager.Put(readBinItem);
            }

            //发AT+LIC?命令
            if (true)
            {
                TaskItem getLicenseItem = new TaskItem();
                getLicenseItem.Enable = true;
                getLicenseItem.Item = "检查产品License";//Check BT MAC
                getLicenseItem.CommonProperties = new GetLicenseProperties()
                {
                    RetryCount = 0,

                    AtCommand = "AT+LIC?",
                    EndLine = "\r\n",//\r\n
                    AtCommandOk = "OK",
                    AtCommandError = "ERROR",
                    CheckInfo = new string[] { "{BinFileString}" },
                    AtCommandInterval = 2000,

                    IsToUpper=true,
                    PortName = "COM25",
                    BaudRate = 115200,
                    Dtr = false,
                    Rts = false,

                };

                getLicenseItem.Executer = new GetLicenseExecuter();
                taskItemManager.Put(getLicenseItem);
            }

            //关闭WIFI UART
            TaskItem closeWifiUartItem = new TaskItem();
            closeWifiUartItem.Enable = true;
            closeWifiUartItem.Item = "关闭WIFI串口";//Close WIFI Uart
            closeWifiUartItem.Executer = new ClosePhoneExecutor();
            closeWifiUartItem.CommonProperties = new CommonProperties()
            {
                ExecuteCondition = ExecuteCondition.ALWAYS,
                RetryCount = 0,
            };
            taskItemManager.Put(closeWifiUartItem);

            ////打开BT
            //OpenPhoneProperties configOpenPhone = new OpenPhoneProperties();
            //configOpenPhone.PortName = "COM17";
            //configOpenPhone.BaudRate = 2400;
            //configOpenPhone.Dtr = true;
            //configOpenPhone.Rts = true;
            //configOpenPhone.EndLine = "\\r\\n";
            //configOpenPhone.Timeout = 10 * 1000;
            //configOpenPhone.RetryCount = 0;
            //configOpenPhone.AtType = AtType.Manual;
            //configOpenPhone.SleepTimeAfterFindDut = 1000;
            //TaskItem openPhoneItem = new TaskItem();
            //openPhoneItem.Enable = true;
            //openPhoneItem.Item = "打开BT串口";//Open BT Uart
            //openPhoneItem.CommonProperties = configOpenPhone;
            //openPhoneItem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(openPhoneItem);

            ////读BT MAC，若读到则放到 MAC_BT中即可，后续fetch sn将自动+1，而不是取。若未读到，则info一下即可
            //if (true)
            //{

            //    TaskItem isitWrittenMac = new TaskItem();
            //    isitWrittenMac.Enable = true;
            //    isitWrittenMac.Item = "BT MAC预读";//Check BT MAC
            //    isitWrittenMac.CommonProperties = new IsitWrittenToTheBtMacProperties()
            //    {
            //        ResultType = AILinkFactoryAuto.Task.SmartBracelet.Property.IsitWrittenToTheBtMacProperties.GetBtMacRetType.ThrowExcption,
            //        ImesPreCheck = true,

            //        Timeout = 3 * 1000,
            //        RetryCount = 0,

            //        AtCommand = "AT+GETBLEMAC",
            //        AtCommandError = "ERROR",

            //        GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})",
            //        GlobalVariblesKey = "{MAC_BT}",

            //        AtCommandInterval = 1000,
            //        SleepTimeBefore = 1500,
            //    };

            //    isitWrittenMac.Executer = new IsitWrittenToTheBtMacExecuter();
            //    taskItemManager.Put(isitWrittenMac);
            //}


            ////写BT 固件
            //TaskItem writeBtBin = new TaskItem();
            //writeBtBin.Enable = true;
            //writeBtBin.Item = "写BT固件";//Write BT Bin
            //writeBtBin.CommonProperties = new WriteBtBinProperties()
            //{
            //    EraseCmd = "nrfjprog.exe --eraseall -f NRF52",
            //    ProgramCmd = "nrfjprog.exe --program {0} --verify -f NRF52",
            //    BinFileFullPath = " ./bin/whole.hex",
            //    ResetCmd = "nrfjprog.exe --reset -f NRF52",
            //    CmdCommandInterval = 500,

            //    Timeout = 10 * 1000,
            //    RetryCount = 0,
            //    SleepTimeBefore = 100,
            //};
            //writeBtBin.Executer = new WriteBtBinExecuter();
            //taskItemManager.Put(writeBtBin);

            ////BT版本号检查
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "BLE固件版本号检查";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.AtCommand = "AT+GETINFO";

            //    checkBtVersionProperties.AtCommandOk = "GET INFO OK";
            //    checkBtVersionProperties.AtCommandError = "ERROR";
            //    checkBtVersionProperties.CheckInfo = new string[] { "AI_WRIST_V1.15", "Apr  9 2020 18:58:47" };

            //    checkBtVersionProperties.AtCommandInterval = 1000;
            //    checkBtVersionProperties.SleepTimeBefore = 800;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}

            //////读BT MAC，判断是否要取号
            ////if (true)
            ////{
            ////    //读BT MAC
            ////    TaskItem getBTMacItem = new TaskItem();
            ////    getBTMacItem.Enable = true;
            ////    getBTMacItem.Item = "读BT MAC";//GET BT MAC
            ////    AtCommandProperties configGetBtMac = new AtCommandProperties();
            ////    configGetBtMac.AtCommand = "AT+GETBLEMAC";

            ////    configGetBtMac.AtCommandOk = "OK";
            ////    configGetBtMac.AtCommandError = "ERROR";

            ////    configGetBtMac.GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})";//KBLE MAC:112233445566OK
            ////    configGetBtMac.GlobalVariblesKey = "{MoudleBtMac}";

            ////    configGetBtMac.AtCommandInterval = 800;
            ////    configGetBtMac.SleepTimeBefore = 800;
            ////    getBTMacItem.CommonProperties = configGetBtMac;
            ////    getBTMacItem.Executer = new AtCommandExecuter();
            ////    taskItemManager.Put(getBTMacItem);
            ////}

            ////判断是否要取号,不取则+1赋值,并通知不从imes取号
            ////if (true)
            ////{
            ////    TaskItem isFetchMac = new TaskItem();
            ////    isFetchMac.Enable = true;
            ////    isFetchMac.Item = "取MAC号方式自动判断";//Check BT MAC
            ////    isFetchMac.CommonProperties = new AccessToMacProperties()
            ////    {
            ////        Timeout = 3 * 1000,
            ////        RetryCount = 0,

            ////        AtCommand = "AT+GETBLEMAC",
            ////        AtCommandError = "ERROR",

            ////        GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})",
            ////        GlobalVariblesKey = "{MoudleBtMac}",

            ////        AtCommandInterval = 800,
            ////        SleepTimeBefore = 800,
            ////    };

            ////    isFetchMac.Executer = new AccessToMacExecuter();
            ////    taskItemManager.Put(isFetchMac);
            ////}

            ////从IMES取wifi MAC，bt mac会自动放入全局变量
            //if (true)
            //{
            //    MesFetchSnProperties mesFetchSnProperties = new MesFetchSnProperties();
            //    mesFetchSnProperties.RetryCount = 0;
            //    mesFetchSnProperties.IsNeedInc1 = true;
            //    taskItemManager.AppendMesFetchSn(mesFetchSnProperties);

            //}


            ////写BT MAC
            //TaskItem writeBTMacItem = new TaskItem();
            //writeBTMacItem.Enable = true;
            //writeBTMacItem.Item = "写BT MAC";//Write BT MAC
            //AtCommandProperties configWriteBtMac = new AtCommandProperties();
            //configWriteBtMac.AtCommand = "AT+SETBLEMAC={MAC_BT}";

            //configWriteBtMac.AtCommandOk = "reboot system";
            //configWriteBtMac.AtCommandError = "ERROR";
            //configWriteBtMac.AtCommandInterval = 800;
            //configWriteBtMac.SleepTimeBefore = 3000;
            //writeBTMacItem.CommonProperties = configWriteBtMac;
            //writeBTMacItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(writeBTMacItem);

            ////读BT MAC
            //if (true)
            //{
            //    TaskItem getBTMacItem = new TaskItem();
            //    getBTMacItem.Enable = true;
            //    getBTMacItem.Item = "读BT MAC";//GET BT MAC
            //    AtCommandProperties configGetBtMac = new AtCommandProperties();
            //    configGetBtMac.AtCommand = "AT+GETBLEMAC";

            //    configGetBtMac.AtCommandOk = "OK";
            //    configGetBtMac.AtCommandError = "ERROR";

            //    configGetBtMac.GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})";//KBLE MAC:112233445566OK
            //    configGetBtMac.GlobalVariblesKey = "{MoudleBtMac}";

            //    configGetBtMac.AtCommandInterval = 800;
            //    getBTMacItem.CommonProperties = configGetBtMac;
            //    getBTMacItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(getBTMacItem);
            //}
            ////生成BT MAC二维码
            //if (true)
            //{
            //    TaskItem showBtMacQrCodeItem = new TaskItem();
            //    showBtMacQrCodeItem.Enable = true;
            //    showBtMacQrCodeItem.Item = "展示BT MAC二维码";//Check BT RSSI
            //    showBtMacQrCodeItem.CommonProperties = new ShowBtMacQrCodeProperties()
            //    {
            //        RetryCount = 3,
            //        StrQrcode = "http://www.baidu.com",
            //        ShowTime = 8000,
            //    };
            //    showBtMacQrCodeItem.Executer = new ShowBtMacQrCodeExecuter();
            //    taskItemManager.Put(showBtMacQrCodeItem);
            //}

            ////校验BT MAC
            ////************待补充：来自IMES的BtMac应存在全局变量中*************
            //TaskItem checkBtMac = new TaskItem();
            //checkBtMac.Enable = true;
            //checkBtMac.Item = "校验BT MAC";//Check BT MAC
            //checkBtMac.CommonProperties = new CheckBtMacProperties()
            //{
            //    Timeout = 3 * 1000,
            //    RetryCount = 0,
            //};
            //checkBtMac.Executer = new CheckBtMacExecuter();
            //taskItemManager.Put(checkBtMac);

            ////设置BT FACMODE
            //TaskItem setBtFacModeItem = new TaskItem();
            //setBtFacModeItem.Enable = true;
            //setBtFacModeItem.Item = "设置BT 进入工厂模式";//Set Bt FactoryMode
            //AtCommandProperties configSetBtFacMode = new AtCommandProperties();
            //configSetBtFacMode.AtCommand = "AT+SETFACMODE=1";

            //configSetBtFacMode.AtCommandOk = "OK";
            //configSetBtFacMode.AtCommandError = "ERROR";
            //configSetBtFacMode.AtCommandInterval = 800;
            //configSetBtFacMode.SleepTimeAfter = 7000;
            //setBtFacModeItem.CommonProperties = configSetBtFacMode;
            //setBtFacModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(setBtFacModeItem);
            ////BT串口发WIFI 软断电电指令
            //TaskItem wifiPowerOff = new TaskItem();
            //wifiPowerOff.Enable = true;
            //wifiPowerOff.Item = "Wifi 断电";//Wifi PowerOff
            //AtCommandProperties configWifiPowerOff = new AtCommandProperties();
            //configWifiPowerOff.AtCommand = "AT+SETGPSWIFIPWR=0";

            //configWifiPowerOff.AtCommandOk = "OK";
            //configWifiPowerOff.AtCommandError = "ERROR";
            //configWifiPowerOff.CheckInfo = new string[] { "wifi power off" };
            //configWifiPowerOff.AtCommandInterval = 3000;
            ////configWifiPowerOff.SleepTimeBefore = 6000;
            //configWifiPowerOff.Timeout = 12000;
            //configWifiPowerOff.RetryCount = 3;
            //wifiPowerOff.CommonProperties = configWifiPowerOff;
            //wifiPowerOff.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiPowerOff);

            ////BT串口发WIFI 软上电指令
            //TaskItem wifiPowerOn = new TaskItem();
            //wifiPowerOn.Enable = true;
            //wifiPowerOn.Item = "Wifi 上电";//Wifi PowerOn
            //AtCommandProperties configWifiPowerOn = new AtCommandProperties();
            //configWifiPowerOn.AtCommand = "AT+SETGPSWIFIPWR=1";

            //configWifiPowerOn.AtCommandOk = "OK";
            //configWifiPowerOn.AtCommandError = "ERROR";
            //configWifiPowerOn.CheckInfo = new string[] { "wifi power on" };
            //configWifiPowerOn.AtCommandInterval = 2000;
            //configWifiPowerOn.Timeout = 3000;
            //configWifiPowerOff.RetryCount = 3;
            //wifiPowerOn.CommonProperties = configWifiPowerOn;
            //wifiPowerOn.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiPowerOn);

            ////BT发检查BT广播命令
            //TaskItem btBroadcastItem = new TaskItem();
            //btBroadcastItem.Enable = true;
            //btBroadcastItem.Item = "BT 开始广播";//BT Broadcast
            //AtCommandProperties configBtBroadcast = new AtCommandProperties();
            //configBtBroadcast.AtCommand = "AT+OPENBLEADV";

            //configBtBroadcast.AtCommandOk = "OK";
            //configBtBroadcast.AtCommandError = "ERROR";
            //configBtBroadcast.AtCommandInterval = 600;
            //configBtBroadcast.Timeout = 2000;
            //configBtBroadcast.RetryCount = 3;
            //btBroadcastItem.CommonProperties = configBtBroadcast;
            //btBroadcastItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btBroadcastItem);

            ////关闭BT UART
            //TaskItem closePhoneItem = new TaskItem();
            //closePhoneItem.Enable = true;
            //closePhoneItem.Item = "关闭BT 串口";//Close BT Uart
            //closePhoneItem.Executer = new ClosePhoneExecutor();
            //closePhoneItem.CommonProperties = new CommonProperties()
            //{
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //    RetryCount = 0,
            //};
            //taskItemManager.Put(closePhoneItem);

            //#region BT扫描工具
            ////打开蓝牙扫描工具 UART
            //OpenPhoneProperties configOpentToolUart = new OpenPhoneProperties();
            //configOpentToolUart.PortName = "COM21";
            //configOpentToolUart.BaudRate = 115200;
            //configOpentToolUart.Dtr = true;
            //configOpentToolUart.Rts = true;
            //configOpentToolUart.EndLine = "\\r\\n";
            //configOpentToolUart.Timeout = 10 * 1000;
            //configOpentToolUart.RetryCount = 0;
            //configOpentToolUart.AtType = AtType.Manual;
            //configOpentToolUart.SleepTimeAfterFindDut = 1000;
            //TaskItem configOpentToolUartTtem = new TaskItem();
            //configOpentToolUartTtem.Enable = true;
            //configOpentToolUartTtem.Item = "打开蓝牙扫描工具的串口";//Open Scan Tool Uart
            //configOpentToolUartTtem.CommonProperties = configOpentToolUart;
            //configOpentToolUartTtem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(configOpentToolUartTtem);

            ////发送开始扫描命令并获取BT RSSI
            //TaskItem getBtRssiItem = new TaskItem();
            //getBtRssiItem.Enable = true;
            //getBtRssiItem.Item = "开始扫描BT广播包并获取BT RSSI";//Satrt Scan And Get BT RSSI
            //GetBtRssiAtCommandProperties configGetBtRssiPro = new GetBtRssiAtCommandProperties();//GetBtRssiAtCommandProperties
            //configGetBtRssiPro.AtCommand = "AT+INQ";

            //configGetBtRssiPro.AtCommandOk = "OK";
            //configGetBtRssiPro.AtCommandError = "ERROR";
            //configGetBtRssiPro.CheckInfo = new string[] { "INQS" };
            ////********************************************正则表达式中存在变量的情况：该产品的蓝牙MAC后的值*****************************************
            //configGetBtRssiPro.GlobalVariblesKeyPattern = ":RSSI:([0-9-]{3,4})[:]";//"BLE MAC->([0-9A-F:]{17})";
            //configGetBtRssiPro.GlobalVariblesKey = "{MoudleBtRssi}";

            //configGetBtRssiPro.AtCommandInterval = 7000;
            //configGetBtRssiPro.Timeout = 7000;
            //configGetBtRssiPro.RetryCount = 2;
            //getBtRssiItem.CommonProperties = configGetBtRssiPro;
            //getBtRssiItem.Executer = new GetBtRssiAtCommandExecuter();//GetBtRssiAtCommandExecuter
            //taskItemManager.Put(getBtRssiItem);

            ////检查BT RSSI
            //if (true)
            //{
            //    TaskItem checkBtRssiItem = new TaskItem();
            //    checkBtRssiItem.Enable = true;
            //    checkBtRssiItem.Item = "检查BT RSSI";//Check BT RSSI
            //    checkBtRssiItem.CommonProperties = new CheckBtRssiProperties()
            //    {
            //        MaxValue = -10,
            //        MinValue = -90,
            //        Timeout = 2 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkBtRssiItem.Executer = new CheckBtRssiExecuter();
            //    taskItemManager.Put(checkBtRssiItem);
            //}

            ////停止扫描
            //TaskItem stopScanBtRssiItem = new TaskItem();
            //stopScanBtRssiItem.Enable = true;
            //stopScanBtRssiItem.Item = "停止扫描";//Stop Scan BT RSSI
            //AtCommandProperties configStopScanBtRssiPro = new AtCommandProperties();
            //configStopScanBtRssiPro.AtCommand = "AT+SINQ";

            //configStopScanBtRssiPro.AtCommandOk = "INQE";
            //configStopScanBtRssiPro.AtCommandError = "ERROR";
            //configStopScanBtRssiPro.ExecuteCondition = ExecuteCondition.ALWAYS;

            //configStopScanBtRssiPro.AtCommandInterval = 800;
            //configStopScanBtRssiPro.Timeout = 2000;
            //configStopScanBtRssiPro.RetryCount = 0;
            //stopScanBtRssiItem.CommonProperties = configStopScanBtRssiPro;
            //stopScanBtRssiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(stopScanBtRssiItem);

            ////关闭蓝牙扫描工具 UART
            //TaskItem closeScanToolItem = new TaskItem();
            //closeScanToolItem.Enable = true;
            //closeScanToolItem.Item = "关闭BT扫描工具串口";//Close Scan Tool Uart
            //closeScanToolItem.Executer = new ClosePhoneExecutor();
            //closeScanToolItem.CommonProperties = new CommonProperties()
            //{
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //    RetryCount = 0,
            //};
            //taskItemManager.Put(closeScanToolItem);
            //#endregion

            ////打开WIFI UART
            ////OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
            ////configOpenWifiUart.PortName = "COM18";
            ////configOpenWifiUart.BaudRate = 115200;
            ////configOpenWifiUart.Dtr = true;
            ////configOpenWifiUart.Rts = true;
            ////configOpenWifiUart.EndLine = "\\r\\n";
            ////configOpenWifiUart.Timeout = 10 * 1000;
            ////configOpenWifiUart.RetryCount = 0;
            ////configOpenWifiUart.AtType = AtType.Manual;
            ////configOpenWifiUart.SleepTimeAfterFindDut = 1000;
            ////TaskItem openWifiUartItem = new TaskItem();
            ////openWifiUartItem.Enable = true;
            ////openWifiUartItem.Item = "Open Wifi Uart";
            ////openWifiUartItem.CommonProperties = configOpenWifiUart;
            ////openWifiUartItem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(openWifiUartItem);


            ////读取WIFI MAP文件至内存********并更新MAC至内存*************
            ////if (true)
            ////{
            ////    TaskItem readMapItem = new TaskItem();
            ////    readMapItem.Enable = true;
            ////    readMapItem.Item = "读取WIFI MAP文件并更新MAC至内存";//Read MAP From File
            ////    readMapItem.CommonProperties = new ReadMapProperties()
            ////    {
            ////        MapFilePath = "./Map/8710BX.map",
            ////        Timeout = 2 * 1000,
            ////        RetryCount = 0,
            ////    };
            ////    readMapItem.Executer = new ReadMapExecuter();
            ////    taskItemManager.Put(readMapItem);
            ////}


            ////写WIFI MAP  使用comdut自定义来写
            //TaskItem writeWifiMapItem = new TaskItem();
            //writeWifiMapItem.Enable = true;
            //writeWifiMapItem.Item = "写WIFI MAP";//
            //writeWifiMapItem.CommonProperties = new WriteWifiMapProperties()
            //{
            //    //取MAC文件一行一行的写（16个字节）
            //    WriteMapAtCommand = "iwpriv config_set wmap,{0},{1}",
            //    Address = "0x00",
            //    AtCommandInterval = 50,
            //    AtCommandOk = "[MEM] After do cmd",
            //    AtCommandError = "unknown command",

            //    Timeout = 20 * 1000,//5
            //    RetryCount = 0,
            //    RetryInterval = 50,
            //};
            //writeWifiMapItem.Executer = new WriteWifiMapExecuter();
            //taskItemManager.Put(writeWifiMapItem);




            ////写WIFI MAC
            ////TaskItem writeWifiMacItem = new TaskItem();
            ////writeWifiMacItem.Enable = true;
            ////writeWifiMacItem.Item = "Write WIFI MAC";
            ////AtCommandProperties configWriteWifiMac = new AtCommandProperties();
            ////configWriteWifiMac.AtCommand = "iwpriv config_set wmap,0x0a,{MAC}";

            ////configWriteWifiMac.AtCommandOk = "Private Message: 0x";
            ////configWriteWifiMac.AtCommandError = "unknown command";
            ////configWriteWifiMac.AtCommandInterval = 800;
            ////writeWifiMacItem.CommonProperties = configWriteWifiMac;
            ////writeWifiMacItem.Executer = new AtCommandExecuter();
            ////taskItemManager.Put(writeWifiMacItem);

            //////读并校验WIFI MAC


            ////读并WIFI MAP
            //TaskItem getWifiMoudleMapItem = new TaskItem();
            //getWifiMoudleMapItem.Enable = true;
            //getWifiMoudleMapItem.Item = "读产品内部WIFI Map";//Get Wifi Moudle Map
            //AtCommandProperties configGetWifiMoudleMap = new AtCommandProperties();
            //configGetWifiMoudleMap.AtCommand = "iwpriv config_get rmap,00,512";

            //configGetWifiMoudleMap.AtCommandOk = "Private Message: 0x";
            //configGetWifiMoudleMap.AtCommandError = "unknown command";

            //configGetWifiMoudleMap.GlobalVariblesKeyPattern = "Private Message: ([0-9A-Fx ]{2560})";//"BLE MAC->([0-9A-F:]{17})";
            //configGetWifiMoudleMap.GlobalVariblesKey = "{MoudleWifiMap}";

            //configGetWifiMoudleMap.AtCommandInterval = 800;
            //configGetWifiMoudleMap.RetryCount = 5;
            //getWifiMoudleMapItem.CommonProperties = configGetWifiMoudleMap;
            //getWifiMoudleMapItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiMoudleMapItem);


            ////校验WIFI MAP
            //TaskItem checkWifiMapItem = new TaskItem();
            //checkWifiMapItem.Enable = true;
            //checkWifiMapItem.Item = "校验WIFI Map";//Check Wifi Map
            //checkWifiMapItem.CommonProperties = new CheckWifiMapProperties()
            //{
            //    Timeout = 3 * 1000,
            //    RetryCount = 0,
            //};
            //checkWifiMapItem.Executer = new CheckWifiMapExecuter();
            //taskItemManager.Put(checkWifiMapItem);



            ////WIFI退出MP模式，进入用户模式
            //TaskItem wifiIntoUserMode = new TaskItem();
            //wifiIntoUserMode.Enable = true;
            //wifiIntoUserMode.Item = "WIFI进入用户模式";//Wifi Into User Mode
            //AtCommandProperties configWifiIntoUserProperties = new AtCommandProperties();
            //configWifiIntoUserProperties.AtCommand = "ATSC";
            ////****************************进入用户模式是否回复OK？***********************
            //configWifiIntoUserProperties.AtCommandOk = "Enter Image 2 ";
            //configWifiIntoUserProperties.AtCommandError = "ERROR";
            ////configWifiIntoUserProperties.CheckInfo = new string[] { "[ATSC]:", "AT_SYSTEM_CLEAR_OTA_SIGNATURE" };
            //configWifiIntoUserProperties.AtCommandInterval = 4000;
            //wifiIntoUserMode.CommonProperties = configWifiIntoUserProperties;
            //wifiIntoUserMode.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiIntoUserMode);


            ////检查WIFI版本号 关键字
            //TaskItem getWifiVersionItem = new TaskItem();
            //getWifiVersionItem.Enable = true;
            //getWifiVersionItem.Item = "检查WIFI版本号";//Get Wifi Version
            //AtCommandProperties configGetWifiVersionProperties = new AtCommandProperties();
            //configGetWifiVersionProperties.AtCommand = "ATS?";
            //configGetWifiVersionProperties.AtCommandOk = "[ATS?]: SW VERSION:";
            //configGetWifiVersionProperties.AtCommandError = "unknown command";
            //configGetWifiVersionProperties.CheckInfo = new string[] { "SW VERSION: v.3.4" };
            ////configGetWifiVersionProperties.GlobalVariblesKeyPattern = "SW VERSION: ([v0-9.]{5})";//"BLE MAC->([0-9A-F:]{17})";
            ////configGetWifiVersionProperties.GlobalVariblesKey = "{MoudleWifiVersion}";


            //configGetWifiVersionProperties.AtCommandInterval = 800;
            //configGetWifiVersionProperties.RetryCount = 3;
            //getWifiVersionItem.CommonProperties = configGetWifiVersionProperties;
            //getWifiVersionItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiVersionItem);




            ////关闭WIFI UART
            ////TaskItem closeWifiUartItem = new TaskItem();
            ////closeWifiUartItem.Enable = true;
            ////closeWifiUartItem.Item = "Close WIFI Uart";
            ////closeWifiUartItem.Executer = new ClosePhoneExecutor();
            ////closeWifiUartItem.CommonProperties = new CommonProperties()
            ////{
            ////    ExecuteCondition = ExecuteCondition.ALWAYS,
            ////    RetryCount = 0,
            ////};
            //taskItemManager.Put(closeWifiUartItem);




            ////打开BT UART->版本号检查->信号强度检查->心率检查
            ////OpenPhoneProperties configOpenBtUart = new OpenPhoneProperties();
            ////configOpenBtUart.PortName = "COM17";
            ////configOpenBtUart.BaudRate = 2400;
            ////configOpenBtUart.Dtr = true;
            ////configOpenBtUart.Rts = true;
            ////configOpenBtUart.EndLine = "\\r\\n";
            ////configOpenBtUart.Timeout = 10 * 1000;
            ////configOpenBtUart.RetryCount = 0;
            ////configOpenBtUart.AtType = AtType.Manual;
            ////configOpenBtUart.SleepTimeAfterFindDut = 1000;
            ////TaskItem openBtUartItem = new TaskItem();
            ////openBtUartItem.Enable = true;
            ////openBtUartItem.Item = "Open BT Uart";
            ////openBtUartItem.CommonProperties = configOpenBtUart;
            ////openBtUartItem.Executer = new OpenPhoneExecutor();
            ////taskItemManager.Put(openBtUartItem);
            //taskItemManager.Put(openPhoneItem);

            ////NB 软件版本号检查 AT+CGMR 关键字
            //TaskItem nbSoftVersionCheck = new TaskItem();
            //nbSoftVersionCheck.Enable = true;
            //nbSoftVersionCheck.Item = "检查NB软件版本号";//NB Soft Version Check
            //AtCommandProperties configNbSoftVersionCheck = new AtCommandProperties();
            //configNbSoftVersionCheck.AtCommand = "AT+CGMR";

            //configNbSoftVersionCheck.AtCommandOk = "OK";
            //configNbSoftVersionCheck.AtCommandError = "ERROR";
            //configNbSoftVersionCheck.CheckInfo = new string[] { "SOTFWAREVER:AI_LITEOS_NB15_B300SP5_V1.9_20200229" };
            //configNbSoftVersionCheck.AtCommandInterval = 1500;
            //configNbSoftVersionCheck.SleepTimeBefore = 500;
            //nbSoftVersionCheck.CommonProperties = configNbSoftVersionCheck;
            //nbSoftVersionCheck.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbSoftVersionCheck);

            //# region NB IMEI号等
            ////在IMEI等号之前先发AT+CFUN=1
            //TaskItem nbAtCfun = new TaskItem();
            //nbAtCfun.Enable = true;
            //nbAtCfun.Item = "设置BT CFUN为1";//AT CFUN
            //AtCommandProperties confignbAtCfun = new AtCommandProperties();
            //confignbAtCfun.AtCommand = "AT+CFUN=1";

            //confignbAtCfun.AtCommandOk = "OK";
            //confignbAtCfun.AtCommandError = "ERROR";
            //confignbAtCfun.AtCommandInterval = 4000;
            //confignbAtCfun.RetryCount = 3;
            //confignbAtCfun.SleepTimeBefore = 200;
            //nbAtCfun.CommonProperties = confignbAtCfun;
            //nbAtCfun.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbAtCfun);

            ////获取IMEI
            //TaskItem getNbImeiItem = new TaskItem();
            //getNbImeiItem.Enable = true;
            //getNbImeiItem.Item = "获取NB IMEI";//Get NB IMEI
            //AtCommandProperties configGetNbImeiItem = new AtCommandProperties();
            //configGetNbImeiItem.AtCommand = "AT+CGSN=1";
            //configGetNbImeiItem.AtCommandOk = "OK";
            //configGetNbImeiItem.AtCommandError = "ERROR";

            //configGetNbImeiItem.GlobalVariblesKeyPattern = "CGSN:([0-9]{15})";//"Private Message: ([0-9A-Fx ]{2560})";//"BLE MAC->([0-9A-F:]{17})";
            //configGetNbImeiItem.GlobalVariblesKey = "{NBIMEI}";

            //configGetNbImeiItem.AtCommandInterval = 800;
            //configGetNbImeiItem.RetryCount = 3;
            //getNbImeiItem.CommonProperties = configGetNbImeiItem;
            //getNbImeiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbImeiItem);

            ////检查IMEI标签与内部一致性
            //TaskItem checkNbImei = new TaskItem();
            //checkNbImei.Enable = true;
            //checkNbImei.Item = "检查NB IMEI内部与标签一致性";//Check NB IMEI
            //checkNbImei.CommonProperties = new CheckNbImeiProperties()
            //{
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkNbImei.Executer = new CheckNbImeiExecuter();
            //taskItemManager.Put(checkNbImei);


            ////打印标签的IMEI 设置为NB IMEI
            //TaskItem printLabelImeiChangetToNbImeiItem = new TaskItem();
            //printLabelImeiChangetToNbImeiItem.Enable = true;
            //printLabelImeiChangetToNbImeiItem.Item = "打印标签IMEI设置为NB IMEI";//LOG MAC CHANGE TO BT MAC
            //printLabelImeiChangetToNbImeiItem.CommonProperties = new PrintLabelImeiChangetToNbImeiProperties()
            //{
            //    RetryCount = 0,
            //};
            //printLabelImeiChangetToNbImeiItem.Executer = new PrintLabelImeiChangetToNbImeiExecuter();
            //taskItemManager.Put(printLabelImeiChangetToNbImeiItem);

            ////
            //TaskItem getNbImsiItem = new TaskItem();
            //getNbImsiItem.Enable = true;
            //getNbImsiItem.Item = "获取NB IMSI";//Get NB IMSI
            //AtCommandProperties configGetNbImsiItem = new AtCommandProperties();
            //configGetNbImsiItem.AtCommand = "AT+CIMI";
            //configGetNbImsiItem.AtCommandOk = "OK";
            //configGetNbImsiItem.AtCommandError = "ERROR";

            //configGetNbImsiItem.GlobalVariblesKeyPattern = "\r\n([0-9]{15})";
            //configGetNbImsiItem.GlobalVariblesKey = "{IMSI}";

            //configGetNbImsiItem.AtCommandInterval = 800;
            //configGetNbImsiItem.RetryCount = 3;
            //getNbImsiItem.CommonProperties = configGetNbImsiItem;
            //getNbImsiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbImsiItem);

            ////获取ICCID
            //TaskItem getNbIccidItem = new TaskItem();
            //getNbIccidItem.Enable = true;
            //getNbIccidItem.Item = "获取NB ICCID";//Get NB ICCID
            //AtCommandProperties configGetNbIccidItem = new AtCommandProperties();
            //configGetNbIccidItem.AtCommand = "AT+NCCID";

            //configGetNbIccidItem.AtCommandOk = "OK";
            //configGetNbIccidItem.AtCommandError = "ERROR";

            //configGetNbIccidItem.GlobalVariblesKeyPattern = "NCCID:([0-9]{20})";
            //configGetNbIccidItem.GlobalVariblesKey = "{ICCID}";

            //configGetNbIccidItem.AtCommandInterval = 800;
            //configGetNbIccidItem.RetryCount = 3;
            //getNbIccidItem.CommonProperties = configGetNbIccidItem;
            //getNbIccidItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbIccidItem);
            //#endregion



            //#region BT/NB 功能检查
            ////附网，AT+CGATT=1
            ////*********************返回什么为PASS????  : OK************************
            //TaskItem nbClingNetItem = new TaskItem();
            //nbClingNetItem.Enable = true;
            //nbClingNetItem.Item = "NB 附网";//NB Cling Net
            //AtCommandProperties configNbClingNetProperties = new AtCommandProperties();
            //configNbClingNetProperties.AtCommand = "AT+CGATT=1";

            //configNbClingNetProperties.AtCommandOk = "OK";
            //configNbClingNetProperties.AtCommandError = "ERROR";
            ////configNbNetCheckProperties.CheckInfo = new string[] { "...1" };
            //configNbClingNetProperties.AtCommandInterval = 2000;
            //configNbClingNetProperties.SleepTimeBefore = 100;
            //nbClingNetItem.CommonProperties = configNbClingNetProperties;
            //nbClingNetItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbClingNetItem);

            ////附网检查 AT+CGATT?  检查是否为1
            ////*********************返回什么为PASS????  :1 ************************
            //TaskItem nbNetCheckItem = new TaskItem();
            //nbNetCheckItem.Enable = true;
            //nbNetCheckItem.Item = "NB附网检查";//B Net CheckN
            //AtCommandProperties configNbNetCheckProperties = new AtCommandProperties();
            //configNbNetCheckProperties.AtCommand = "AT+CGATT?";

            //configNbNetCheckProperties.AtCommandOk = "OK";
            //configNbNetCheckProperties.AtCommandError = "ERROR";
            //configNbNetCheckProperties.CheckInfo = new string[] { "CGATT:1" };
            //configNbNetCheckProperties.AtCommandInterval = 5000;
            //configNbNetCheckProperties.SleepTimeBefore = 100;
            //nbNetCheckItem.CommonProperties = configNbNetCheckProperties;
            //nbNetCheckItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbNetCheckItem);



            ////信号强度CSQ获取
            //TaskItem getCsqItem = new TaskItem();
            //getCsqItem.Enable = true;
            //getCsqItem.Item = "获取NB信号强度";//Get NB CSQ
            //AtCommandProperties configGetCsqProperties = new AtCommandProperties();
            //configGetCsqProperties.AtCommand = "AT+CSQ";
            //configGetCsqProperties.AtCommandOk = "OK";
            //configGetCsqProperties.AtCommandError = "ERROR";

            //configGetCsqProperties.GlobalVariblesKeyPattern = "CSQ:([0-9]{1,2})[,]";
            //configGetCsqProperties.GlobalVariblesKey = "{NBCSQ}";

            //configGetCsqProperties.AtCommandInterval = 1000;
            //configGetCsqProperties.RetryCount = 3;
            //getCsqItem.CommonProperties = configGetCsqProperties;
            //getCsqItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getCsqItem);

            ////检查信号强度
            //TaskItem checkCsqItem = new TaskItem();
            //checkCsqItem.Enable = true;
            //checkCsqItem.Item = "检查NB信号强度";//Check Csq
            //checkCsqItem.CommonProperties = new CheckCsqProperties()
            //{
            //    MaxValue = 100,
            //    MinValue = 1,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkCsqItem.Executer = new CheckCsqExecuter();
            //taskItemManager.Put(checkCsqItem);


            ////获取WIFI探针值
            //TaskItem getWifiProbeItem = new TaskItem();
            //getWifiProbeItem.Enable = true;
            //getWifiProbeItem.Item = "获取WIFI探针-信号强度值";//Get WIFI PROBE
            //AtCommandProperties configGetWifiProbeProperties = new AtCommandProperties();
            //configGetWifiProbeProperties.AtCommand = "AT+AIGPSWIFI=WIFI,2";
            //configGetWifiProbeProperties.AtCommandOk = "OK";
            //configGetWifiProbeProperties.AtCommandError = "ERROR";

            //configGetWifiProbeProperties.GlobalVariblesKeyPattern = "7405a58fa8cb:([0-9-]{1,3}),";

            ////configCheckWifiProbeProperties.GlobalVariblesKeyPattern = "[0-9,:]]*]";
            //configGetWifiProbeProperties.GlobalVariblesKey = "{NBWIFIPROBE}";

            //configGetWifiProbeProperties.AtCommandInterval = 5000;
            //configGetWifiProbeProperties.RetryCount = 3;
            //getWifiProbeItem.CommonProperties = configGetWifiProbeProperties;
            //getWifiProbeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiProbeItem);

            ////WIFI探针检查
            //TaskItem checkWifiProbeItem = new TaskItem();
            //checkWifiProbeItem.Enable = true;
            //checkWifiProbeItem.Item = "WIFI探针-信号强度检查";
            //checkWifiProbeItem.CommonProperties = new CheckWifiProbeProperties()
            //{
            //    MaxValue = -10,
            //    MinValue = -90,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkWifiProbeItem.Executer = new CheckWifiProbeExecuter();
            //taskItemManager.Put(checkWifiProbeItem);



            ////计步器检查  
            //TaskItem checkPedometer = new TaskItem();
            //checkPedometer.Enable = true;
            //checkPedometer.Item = "计步器检查";
            //AtCommandProperties configCheckPedometer = new AtCommandProperties();
            //configCheckPedometer.AtCommand = "AT+PEDOTEST";//AT+PEDOTEST

            //configCheckPedometer.AtCommandOk = "OK";
            //configCheckPedometer.AtCommandError = "ERROR";
            //configCheckPedometer.AtCommandInterval = 1000;
            //checkPedometer.CommonProperties = configCheckPedometer;
            //checkPedometer.Executer = new AtCommandExecuter();
            //taskItemManager.Put(checkPedometer);


            ////温度传感器值获取
            //TaskItem getTemp = new TaskItem();
            //getTemp.Enable = true;
            //getTemp.Item = "温度传感器值获取";//Get Temp
            //AtCommandProperties configGetTemp = new AtCommandProperties();
            //configGetTemp.AtCommand = "AT+TEMPTEST";//

            //configGetTemp.AtCommandOk = "OK";
            //configGetTemp.AtCommandError = "ERROR";
            //configGetTemp.GlobalVariblesKeyPattern = "TEMP:([0-9.]*)[\r\n]";//TEMP:1519.7\r\n
            //configGetTemp.GlobalVariblesKey = "{NBTemp}";
            //configGetTemp.AtCommandInterval = 2000;
            //getTemp.CommonProperties = configGetTemp;
            //getTemp.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getTemp);

            ////温度传感器检查
            //TaskItem checkTempItem = new TaskItem();
            //checkTempItem.Enable = true;
            //checkTempItem.Item = "温度传感器检查";//Check Temp
            //checkTempItem.CommonProperties = new CheckTempProperties()
            //{
            //    MaxValue = 5000,
            //    MinValue = 5,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkTempItem.Executer = new CheckTempExecuter();
            //taskItemManager.Put(checkTempItem);

            ////射频获取
            //TaskItem taskItem10 = new TaskItem();
            //taskItem10.Enable = true;
            //taskItem10.Item = "射频获取";//Get Signal Power
            //AtCommandProperties config10 = new AtCommandProperties();
            //config10.AtCommand = "AT+NUESTATS";
            //config10.AtCommandOk = "OK";
            //config10.AtCommandError = "ERROR";
            //config10.AtCommandInterval = 1000;
            //config10.GlobalVariblesKey = "{SIGNAL_POWER}";
            //config10.GlobalVariblesKeyPattern = "Signal power\\:([0-9\\-]+)\\r\\n";
            //taskItem10.CommonProperties = config10;
            //taskItem10.Executer = new AtCommandExecuter();
            //taskItemManager.Put(taskItem10);

            ////射频对比
            //TaskItem comparePower = new TaskItem();
            //comparePower.Enable = true;
            //comparePower.Item = "射频检查";//Compare Signal Power
            //ConditionProperties configComparePower = new ConditionProperties()
            //{
            //    RetryCount = 0
            //};
            //configComparePower.ConditionItems = new ConditionItem[]{
            //    new ConditionItem()
            //    {
            //        Key = "{SIGNAL_POWER}",
            //        Condition = Condition.GREATER_EQUAL,
            //        Value = "-1000"
            //    }
            //};
            //comparePower.CommonProperties = configComparePower;
            //comparePower.Executer = new ConditionExecuter();
            //taskItemManager.Put(comparePower);
            ////AT+HRMTEST

            ////BT 退出工厂模式
            //TaskItem btOutFactoryModeItem = new TaskItem();
            //btOutFactoryModeItem.Enable = true;
            //btOutFactoryModeItem.Item = "BT 退出工厂模式";//Get Signal Power
            //AtCommandProperties configBtOutFactoryMode = new AtCommandProperties();
            //configBtOutFactoryMode.AtCommand = "AT+SETFACMODE=0";
            //configBtOutFactoryMode.AtCommandOk = "OK";
            //configBtOutFactoryMode.AtCommandError = "ERROR";
            //configBtOutFactoryMode.AtCommandInterval = 1000;
            //btOutFactoryModeItem.CommonProperties = configBtOutFactoryMode;
            //btOutFactoryModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btOutFactoryModeItem);


            ////进入出厂状态
            //TaskItem btInDefaultModeItem = new TaskItem();
            //btInDefaultModeItem.Enable = true;
            //btInDefaultModeItem.Item = "进入出厂状态";//Get Signal Power
            //AtCommandProperties configInDefaultMode = new AtCommandProperties();
            //configInDefaultMode.AtCommand = "AT+FACOUT";
            //configInDefaultMode.AtCommandOk = "OK";
            //configInDefaultMode.AtCommandError = "ERROR";
            //configInDefaultMode.AtCommandInterval = 1000;
            //btInDefaultModeItem.CommonProperties = configInDefaultMode;
            //btInDefaultModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btInDefaultModeItem);

            ////出厂状态检查
            //if (true)
            //{
            //    TaskItem taskItem = new TaskItem();
            //    taskItem.Enable = true;
            //    taskItem.Item = "出厂状态检查";//Write BT MAC
            //    AtCommandProperties atCommandProperties = new AtCommandProperties();
            //    atCommandProperties.AtCommand = "AT+GETINFO";

            //    atCommandProperties.AtCommandOk = "GET INFO OK";
            //    atCommandProperties.AtCommandError = "ERROR";
            //    atCommandProperties.CheckInfo = new string[] { "FACOUT:1" };

            //    atCommandProperties.AtCommandInterval = 1000;

            //    taskItem.CommonProperties = atCommandProperties;
            //    taskItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(taskItem);
            //}


            //#endregion


            ////关闭BT UART
            ////TaskItem closeBTUartItem = new TaskItem();
            ////closeBTUartItem.Enable = true;
            ////closeBTUartItem.Item = "Close BT Uart";
            ////closeBTUartItem.Executer = new ClosePhoneExecutor();
            ////closeBTUartItem.CommonProperties = new CommonProperties()
            ////{
            ////    ExecuteCondition = ExecuteCondition.ALWAYS,
            ////    RetryCount = 0,
            ////};
            ////taskItemManager.Put(closeBTUartItem);
            //taskItemManager.Put(closePhoneItem);


            ////LOG MAC CHANGE TO BT MAC
            //TaskItem logMacChangeToBtMacItem = new TaskItem();
            //logMacChangeToBtMacItem.Enable = true;
            //logMacChangeToBtMacItem.Item = "LOG MAC为BT MAC";//LOG MAC CHANGE TO BT MAC
            //logMacChangeToBtMacItem.CommonProperties = new LogMacChangeToBtMacProperties()
            //{
            //    RetryCount = 0,
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //};
            //logMacChangeToBtMacItem.Executer = new LogMacChangeToBtMacExecuter();
            //taskItemManager.Put(logMacChangeToBtMacItem);


            ////TaskItem writeImeiItem = new TaskItem();
            ////writeImeiItem.Enable = true;
            ////writeImeiItem.Item = "Write Imei";
            ////AtCommandProperties configWriteBtMac = new AtCommandProperties();
            ////configWriteBtMac.AtCommand = "AT^IMEI={IMEI}";
            ////configWriteBtMac.AtCommandOk = "OK";
            ////configWriteBtMac.AtCommandError = "ERROR";
            ////configWriteBtMac.AtCommandInterval = 100;
            ////writeImeiItem.CommonProperties = configWriteBtMac;
            ////writeImeiItem.Executer = new AtCommandExecuter();
            ////taskItemManager.Put(writeImeiItem);


            ////标签打印
            //if (true)
            //{
            //    PrintLabelProperties printLabelProperties = new PrintLabelProperties()
            //    {
            //        RetryCount = 0,
            //        Timeout = 8000,
            //        SleepTimeAfter = 5000,
            //    };
            //    taskItemManager.AppendPrintLabel(printLabelProperties);
            //}


            //StringBuilder jtsFileName = new StringBuilder();
            //jtsFileName.Append("Hi3861Check");

            string jtsFileName = "Hi3861Check";
            taskItemManager.Save2Jts(this, 1, jtsFileName);

            //jtsFileName.Append(".jts");
            //JtsUtils.SaveJsonFile(jtsFileName.ToString(), taskItemList);

            Box.ShowInfoOk(jtsFileName + "\r\n成功");


            //OpenPhoneExecutor
            //OpenPhoneProperties

            //    AtCommandExecuter
            //    AtCommandProperties

            //    ClosePhoneExecutor

        }
    }
}
