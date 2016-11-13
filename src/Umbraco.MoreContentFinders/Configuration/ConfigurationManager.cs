using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;

namespace Umbraco.MoreContentFinders
{
    public class ConfigurationManager
    {
        protected static JArray JDefinitions;

        public static void Initialize(string fileName)
        {
            string configFileName = Path.Combine(Umbraco.Core.IO.SystemDirectories.Config, fileName);

            string filePath = HttpContext.Current.Server.MapPath(configFileName);

            try
            {
                string configFileContents = File.ReadAllText(filePath);
                JDefinitions = JsonConvert.DeserializeObject(configFileContents) as JArray;
            }
            catch (Exception e)
            {
                LogHelper.Error(typeof(ConfigurationManager), "An error ocurred when loading configuration file", e);
            }

            if (JDefinitions == null)
            {
                JDefinitions = new JArray();
                LogHelper.Warn(typeof(ConfigurationManager), "Null configuration");
            }
            
        }

        public static T GetContentFinderConfig<T>(string fqn, string configKey = "Config")
        {
            JToken jDefinition = JDefinitions.First(d => d["FQN"].ToString() == fqn);
            return jDefinition[configKey].ToObject<T>();
        }

        public static List<ContentFinderInsertion> GetInsertions()
        {
            return JDefinitions.Select(jd => ContentFinderInsertion.Create(jd)).ToList();
        }
    }
}