using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCRM_App.Extentions
{
    public static class IConfigurationSectionExtentions
    {
        public static IConfigurationSection BuildSectionSetting(this IConfigurationSection configurationSection, string key)
        {
            return configurationSection._BuildSectionSetting(key.Split('.').ToList());
        }

        public static IConfigurationSection _BuildSectionSetting(this IConfigurationSection configurationSection, List<string> key)
        {
            if (key == null || key.Count <= 1) return configurationSection;
            key.Remove(key[0]);
            return configurationSection.GetSection(key[0])._BuildSectionSetting(key);
        }
    }
}
