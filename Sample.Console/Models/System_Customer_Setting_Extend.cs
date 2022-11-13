using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Models
{
    public class RB_Customer_Setting_Extend
    {
        public int CustomerID { get; set; }
        public string SmsDepartmentIds { get; set; }
        public DateTime SmsStartTime { get; set; }
        public int SmsEndTime { get; set; }
        public bool IsOpenWarehouseAreaResponsibility { get; set; }
        public int IsSetNumDefaultZero { get; set; }
        public int IsUseTableVariable { get; set; }

        public int IsOpenAmazonStorageShare { get; set; }
        public int IsOpenAmazonAdvertShare { get; set; }
        public int IsCheckSupplierUpdate { get; set; }
        public int CustomExportSkuLimitNum { get; set; }
        public int IsCheckSupplierUpdateAuto { get; set; }

    }
}
