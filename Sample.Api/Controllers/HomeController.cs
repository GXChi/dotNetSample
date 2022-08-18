using Sample.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    public class HomeController : ApiController
    {
        [Route("Home/SignatureFormatter")]
        [HttpPost]
        public dynamic SignatureFormatter(string data)
        {
            var privateKey = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC41UkfW5ScjC90
hd+gB/i0NA84fLXG9cVfIHxNFkXw5LvOjWDZpWAjbJoNflVebDbvYbk+TSV3Xhrk
KunMGP9rrQAEGQNA84vbb9ejKfUZaD7SQF6jiSVEgWXHVl/Y7AIbnbX7BeJ7pRgE
s7rABqHFl/BbDaGUVjMkhzaNTibG5Q5WLxHi+yMBCJcpM1jH26gcsfikkrKEV2Av
6ScIaCr9w9ikPGlnNK3chXiOA5DuhRUMidHN/nqPGWUCzK7JciFyPSdVjRFGVt70
cPUN7BrERaxoYTc9tr/G61G7lW6VRNOrt4uE2app5yeF9BF73bg/1CHY3cGF0yCZ
BwlROaSbAgMBAAECggEALHUk2vWIt+TiLzlaMiBoulOBS7l3ca1A9aIXM+WFGi2y
XLrVzlQAXVTscw1tlEIcLpUHXQxqVB1fA32s8aobmbcpd1La3P8Xp0buC7F2Yk+Y
OLNB8Dz3KVSkef91G3SdBy52jLP8J3hs4AipBTIzLBdhOU18jXT0ZTdesDtZiWI6
eRwEZBnnkw/u6AuWNyN9JQ66kZKGHmLezqgmlo+vQbuYytxIUAVxRXzLcW7zlAME
o/aoAapaEFOSW4LfkvKHblM/E1xoybh7vcfRmhqM7MY54bpNK5iDp9hl/eUxPaGW
8oN8nAmJpn9cy7oOq8Pju16ThWV3CQB0gK/uvCsuUQKBgQD31NfFcgszcQC4OTzv
2GkYsElXnsAasJpPMb59g8aG7SGa48oE8iNDz03+R9g25qah9mHDNTLXxWVmZFvT
vDz2oX+PtrOJFatIBeQIONchigKwDobf3d+u8zlZ9zti33QQncxiXkCnMs3N61+L
2AK/eI26e3Dlmhp3By6cJ+CAywKBgQC+7N/o7afw2Jt+TzXlaWHxK+vpV8jezC3R
YVAVWTvLW+7gqhuBXiYwS4dY6dBbxvKiKq/r98UBmCszohlm0Pzn09sV92HyK6n3
WD90McT/vAy3kzvwAhVTdDoo1vOkVDUZjBSadQr5W2ELiVXrFh6OB0OQRBU2b2uQ
C9E9z+ABcQKBgAHVhGdEqcqOqoeyfL31pwl0vIY2eYt5BRtS7er137E97gK8btFM
jXBCB3y7QWFeoZsPPLf6G882+cIhOgC+IuFcXppMqtV34JWe3YomCYmJAO4WnW7o
sHAWArWku77GkpBQ6qNLuUTDfnxoooNhbNvH45yerz4zvsBkwDaNoP6zAoGBAJO5
i/YaDFtZ/dXZTAZZZ6LHLR27An0A1mKvPNuH07AhTc8fT3Dle/7aMbsmDkU0xf+b
r8qQRypYpvSoc6cOBOELFvUEGBDcoAH1kgNBjv+gbOaNUU7/DakowOdgiduYHA3M
SRTJgtg1T/0xIf1ne/TAwhIZ25/mzf8CZb/B27zBAoGAbBnJ7LYCGNBwUmlpaC4o
jqFmvmjAIooyCz8VfimFJUSuiFxROHjjOriw+KKlIga0PWwKdCduvcHwDWtDwypK
UDHdj1WoHVNDtiM1UTZ0RuJ5NVJvLldO/X40vKPzI68UCXnfOJlkTLRABUh47C5i
WVI/LS3JswzDY95SGNWWFAQ=";
            //var result = RSAHelper.SignatureFormatter(privateKey, data);
            //return result;
            byte[] data1;
            using (var ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, data == null ? "null" : data);
                data1 = ms.ToArray();
            }


            byte[] strEncryptedSignatureData = null;
            RSACryption.SignatureFormatter(privateKey, data1, ref strEncryptedSignatureData);
            return strEncryptedSignatureData;
        }

        [Route("Home/SignatureDeformatter")]
        [HttpPost]
        public string SignatureDeformatter(string data, string sign)
        {
            var publicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuNVJH1uUnIwvdIXfoAf4
tDQPOHy1xvXFXyB8TRZF8OS7zo1g2aVgI2yaDX5VXmw272G5Pk0ld14a5CrpzBj/
a60ABBkDQPOL22/Xoyn1GWg+0kBeo4klRIFlx1Zf2OwCG521+wXie6UYBLO6wAah
xZfwWw2hlFYzJIc2jU4mxuUOVi8R4vsjAQiXKTNYx9uoHLH4pJKyhFdgL+knCGgq
/cPYpDxpZzSt3IV4jgOQ7oUVDInRzf56jxllAsyuyXIhcj0nVY0RRlbe9HD1Dewa
xEWsaGE3Pba/xutRu5VulUTTq7eLhNmqaecnhfQRe924P9Qh2N3BhdMgmQcJUTmk
mwIDAQAB";

            //var result = RSAHelper.SignatureDeformatter(publicKey, data, sign);
            //return result;
            if (RSACryption.SignatureDeformatter(publicKey, data, sign))
                return "验证成功";
            return "验证失败";
        }
    }
}
