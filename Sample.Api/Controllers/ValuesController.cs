using Sample.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            DateTime dateTime = DateTime.Now;
            var dateTimeStamp = DateHelper.ConvertDateTimeInt(dateTime);


            string dateTimeCache = "";
            string dateTimeStampCache = "";
            if (CacheHelper.Get("dateTime") == null)
            {
                CacheHelper.Insert("dateTime", dateTime.ToString("yyyy-MM-dd dd:mm:ss"), 1);
            }
            else
            {
               dateTimeCache = CacheHelper.Get("dateTime").ToString();
            }

            if (CacheHelper.Get("dateTimeStamp") == null)
            {
                CacheHelper.Insert("dateTimeStamp", dateTimeStamp, 2);
            }
            else
            {
                dateTimeStampCache = CacheHelper.Get("dateTimeStamp").ToString();
            }

            return new string[] { $"dateTime is {dateTime.ToString("yyyy-MM-dd dd:mm:ss")}；dateTimeStamp is {dateTimeStamp}", $"dateTimeCache is {dateTimeCache}；dateTimeStampCache is {dateTimeStampCache}" };
        }

        // GET api/values/5
        public string Get(string timeStamp)
        {
            var dateTime = DateHelper.GetTime(timeStamp);
            return $"timeStamp is {timeStamp}；dateTime is {dateTime.ToString("yyyy-MM-dd dd:mm:ss")} ";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
