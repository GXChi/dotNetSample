using DotLiquid;
using Sample.Console.Business;
using Sample.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace Sample.Console
{
    internal class Program
    {

        static void Main(string[] args)
        {
            List<System_Customer_Setting> scsList = new List<System_Customer_Setting>()
            {
                  new System_Customer_Setting() { Key = "BuyerAssignType", Value = "1", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "DefaultWareHouseID", Value = "21592", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "CheckOrderBlackList", Value = "False", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "CheckOrderProfit", Value = "False", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "ShipFeePerG", Value = "0.0000", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "LlLoignPwd", Value = "E903LKAJFD3", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "SetTheMaximumNumberOfPackages", Value = "300", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "OrderNotes1688", Value = "写于外箱", IsEnable = 0, Type = 0 },
                  new System_Customer_Setting() { Key = "SmsDepartmentIds", Value = "21,314,25465,675,6786,8768", IsEnable = 0, Type = 1 },
                  new System_Customer_Setting() { Key = "SmsStartTime", Value = "2022/11/13 11:25:00", IsEnable = 0, Type = 1 },
                  new System_Customer_Setting() { Key = "IsOpenWarehouseAreaResponsibility", Value = "TRUE", IsEnable = 0, Type = 1 },
                  new System_Customer_Setting() { Key = "CustomExportSkuLimitNum", Value = "100000", IsEnable = 0, Type = 1 }
            };


            RB_Customer_Setting rcs = null;
            RB_Customer_Setting_Extend scse = null;
            if (rcs == null)
            {
                rcs = new RB_Customer_Setting();
                PropertyInfo[] propertys = rcs.GetType().GetProperties();
                foreach (var item in propertys)
                {
                    var scs = scsList.Where(x => x.Key == item.Name && x.Type == 0).FirstOrDefault();
                    if (scs != null)
                    {
                        var value = Convert.ChangeType(scs.Value, item.PropertyType);
                        item.SetValue(rcs, value, null);
                    }
                }

                scse = new RB_Customer_Setting_Extend();
                PropertyInfo[] scsePropertys = scse.GetType().GetProperties();
                foreach (var item in scsePropertys)
                {
                    var scs = scsList.Where(x => x.Key == item.Name && x.Type == 1).FirstOrDefault();
                    if (scs != null)
                    {
                        var value = Convert.ChangeType(scs.Value, item.PropertyType);
                        item.SetValue(scse, value, null);
                    }
                }
            }


            rcs = null;
            scse = null;
            if (rcs == null)
            {
                rcs = new RB_Customer_Setting();
                scse = new RB_Customer_Setting_Extend();
                foreach (var item in scsList)
                {
                    if (item.Type == 0)
                    {
                        fsHelper.SetObjectPropertyValue(rcs, item.Key, item.Value);
                    }
                    else if (item.Type == 1)
                    {
                        fsHelper.SetObjectPropertyValue(scse, item.Key, item.Value);
                    }
                }
            }

            List<System_Customer_Setting> newList = new List<System_Customer_Setting>();
            PropertyInfo[] propertys1 = rcs.GetType().GetProperties();
            foreach (var item in propertys1)
            {
                var value = fsHelper.GetPropertyValue(rcs, item.Name);
                //var value = item.GetValue(item.Name);
                if (!string.IsNullOrEmpty(value))
                {
                    System_Customer_Setting scs = new System_Customer_Setting();
                    scs.Key = item.Name;
                    scs.Value = value.ToString();
                    scs.Type = 0;
                    scs.IsEnable = 0;
                    newList.Add(scs);
                }               
            }

            PropertyInfo[] propertys2 = scse.GetType().GetProperties();
            foreach (var item in propertys1)
            {
                //var value = item.GetValue(item.Name).ToString();
                var value = fsHelper.GetPropertyValue(scse, item.Name);
                if (!string.IsNullOrEmpty(value))
                {
                    System_Customer_Setting scs = new System_Customer_Setting();
                    scs.Key = item.Name;
                    scs.Value = value.ToString();
                    scs.Type = 1;
                    scs.IsEnable = 0;
                    newList.Add(scs);
                }
            }


          
            if (rcs.CustomerID != null)
            {
                rcs.GetType().GetProperty("CustomerID");
            }
            if (rcs.BuyerAssignType != null)
            {
                rcs.GetType().GetProperty("BuyerAssignType");
            }
            System.Console.ReadKey();
        }
    }

}
