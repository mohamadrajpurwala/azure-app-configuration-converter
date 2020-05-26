using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppConfigurationConverter.Data
{
    public interface IFormatConverter
    {
        string FormatConverter(string jsonObject, string startingKey);
    }
}
