using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppConfigurationConverter.Data
{
    public class AzureAppConfigureJsonFormatConverter : IFormatConverter
    {
        private Dictionary<string, string> dictValue;

        public string FormatConverter(string jsonObject, string startingKey)
        {
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(jsonObject);
                dictValue = new Dictionary<string, string>();
                ConvertToDict(obj, startingKey);
                List<AzureAppConfiguration> azureAppConfigurations = new List<AzureAppConfiguration>();
                foreach (var keyValue in dictValue)
                {
                    azureAppConfigurations.Add(
                        new AzureAppConfiguration()
                        {
                            name = keyValue.Key,
                            value = keyValue.Value,
                            slotSetting = false
                        });
                }

                return JsonConvert.SerializeObject(azureAppConfigurations);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private Dictionary<string,string> ConvertToDict(dynamic obj, string startingKey)
        {
            if (!string.IsNullOrEmpty(startingKey))
            {
                startingKey += ":";
            }

            foreach (var keyValue in obj)
            {
                string str = Convert.ToString(keyValue.Value?.Type);
                if (str == null)
                {
                    ConvertToDict(keyValue.Value, $"{startingKey}{keyValue.Name}");
                }
                else if (str.ToLower() == "array")
                {
                    for (int i = 0; i < keyValue.Value.Count; i++)
                    {
                        ConvertToDict(keyValue.Value[i], $"{startingKey}{keyValue.Name}:[{i}]");
                    }
                }
                else
                {
                    dictValue.Add($"{startingKey}{keyValue.Name}", keyValue.Value.Value.ToString());
                }
            }

            return dictValue;
        }
    }

    public class AzureAppConfiguration
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool slotSetting { get; set; }
    }
}
