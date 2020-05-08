using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class FindDeviceExecuter : IExecuter
    {
        FindDeviceProperties config;

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as FindDeviceProperties;
            ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;

            //int ret = -1;
            //string returnString = "";
            string endLine = config.EndLine;

            TimeUtils.Execute(() =>
            {
                comDut.Write(config.TestPowerOnAT + endLine);
                //comDut.Write("AT+CSQ\r\n");

                Thread.Sleep(config.AtCommandInterval);
                string response = comDut.ReadExisting();
                //comDut.DtrEnable = false;
                //comDut.RtsEnable = false;

                //log.Info("AT Response=" + response);


                if (!string.IsNullOrEmpty(config.AtCommandOk))
                {
                    if (response.Contains(config.AtCommandOk))
                    {
                        log.Info(string.Format("检测到产品上电，AT response=[{0}]  contain =[{1}]", response, config.AtCommandOk));
                        //跳出循环执行
                        return true;
                        //throw new BaseException(string.Format("AT response=[{0}] not contain error=[{1}]", response, config.AtCommandOk));
                    }
                }


                ////ret = ExeCommand(config, out returnString);
                //if (ret != 0)
                //{
                //    log.Fail(string.Format("详情：{0}，检测设备失败 \r\n", returnString));
                //    //throw new BaseException(string.Format("详情：{0}，检测设备失败，FAIL\r\n", returnString));

                //}
                //if (returnString.Contains("\tdevice\r\n\r\n"))
                //{
                //    log.Info(string.Format("详情：{0}，检测到设备 \r\n", returnString));
                //    //跳出循环执行
                //    return true;
                //}



                //继续循环执行
                return false;
            }, config.Timeout);

        }
    }
}
