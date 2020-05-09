using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class LabelGetAndCheckExecutor : IExecuter
    {
        LabelGetAndCheckProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as LabelGetAndCheckProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string[] data = configGv.Get(GlobalVaribles.LABEL_SN).Split(new[] { ',' });
            string gimiSn = data[0];
            CheckSn(gimiSn);

            gimiSn=gimiSn.Insert(config.InsertIndex, config.Value);
            configGv.Add("GimiSn", gimiSn);

            log.Info(string.Format("标签：{0}\r\n插入内容后：{1}", data[0], gimiSn));

            //try
            //{

            //    string strBtMac = configGv.Get("MAC_BT");
            //    string strMoudleBtMac = configGv.Get("MoudleBtMac");
            //    if (string.IsNullOrEmpty(strBtMac) && !string.IsNullOrEmpty(strMoudleBtMac))
            //    {
            //        configGv.Add("MAC", strMoudleBtMac);
            //    }
            //    else if (!string.IsNullOrEmpty(strBtMac) && !string.IsNullOrEmpty(strMoudleBtMac))//有标签BT MAC，则肯定经过一致性检查
            //    {
            //        configGv.Add("MAC", strMoudleBtMac);
            //    }
            //    else if (!string.IsNullOrEmpty(strBtMac) && string.IsNullOrEmpty(strMoudleBtMac))//有标签BT MAC，则肯定经过一致性检查
            //    {
            //        configGv.Add("MAC", strBtMac);
            //    }
            //    else
            //    {
            //        log.Fail(string.Format("LOG MAC设置为BT MAC失败\r\n,取号BT MAC{0}，模块BT MAC:{1} \r\n", strBtMac, strMoudleBtMac));
            //    }
            //    log.Info(string.Format("LOG MAC设置为BT MAC：\r\n{0} PASS \r\n", strMoudleBtMac));


            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("LOG MAC设置为BT MAC出错，" + ex.Message);
            //}

        }
        private void CheckSn(string sn)
        {
            string format = "^[0-9A-Z]{16}$";

            if (string.IsNullOrEmpty(sn))
            {
                throw new BaseException("SN null");
            }

            if (!Regex.IsMatch(sn, format))
            {
                throw new BaseException("SN invalid");
            }
        }

    }

}
