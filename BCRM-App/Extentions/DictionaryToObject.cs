using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCRM_App.Extentions
{
    public static class DictionaryExtention
    {
        public static T DictionaryToObject<T>(IDictionary<string, dynamic> dictionary)
        {

            var jsonData = JsonConvert.SerializeObject(dictionary);
            var dataObj = JsonConvert.DeserializeObject<T>(jsonData);
            return dataObj;
        }
    }
}
