using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.MoreContentFinders;
using Umbraco.Web.Routing;

namespace Umbraco.Extensions.EventHandlers
{
    public class ContentFinderRegistrar : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ConfigurationManager.Initialize("MoreContentFinders.json");

            foreach (ContentFinderInsertion d in ConfigurationManager.GetInsertions())
            {
                Type existingType = Type.GetType(d.InsertBefore);
                Type newType = Type.GetType(d.FQN);

                try
                {
                    ContentFinderResolver.Current.InsertTypeBefore(existingType, newType);
                }
                catch (Exception e)
                {
                    e.Data.Add("InsertBefore", d.InsertBefore);
                    e.Data.Add("FQN", d.FQN);

                    LogHelper.Error(typeof(ContentFinderRegistrar), "Could not process ContentFinderInsertion", e);
                }
            }

        }
    }
}