using System;
using Umbraco.Core;
using Umbraco.Web.Routing;
using umbraco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;

namespace Umbraco.MoreContentFinders
{
    public class FallbackRootNodesContentFinder : ContentFinderByNiceUrl
    {
        protected Config _Config;

        public FallbackRootNodesContentFinder()
        {
            _Config = ConfigurationManager.GetContentFinderConfig<Config>(GetType().FullName);
        }

        public override bool TryFindContent(PublishedContentRequest docRequest)
        {
            if (!docRequest.HasDomain)
                return false;

            if (_Config.Rules.Count == 0)
                return false;

            Rule rule = _Config.Rules.First(r => r.DomainUri.Equals(docRequest.DomainUri));

            if (rule == null)
                return false;

            foreach (string nodeId in rule.NodeIds)
            {
                IPublishedContent node = FindContent(nodeId, docRequest);

                if (node != null){
                    docRequest.PublishedContent = node;
                    node = null;
                    return true;
                }
            }

            return false;
        }

        protected IPublishedContent FindContent(string rootId, PublishedContentRequest docRequest)
        {
            string route = rootId + DomainHelper.PathRelativeToDomain(docRequest.DomainUri, docRequest.Uri.GetAbsolutePathDecoded());

            return FindContent(docRequest, route);
        }

        protected class Config
        {
            public List<Rule> Rules;

            public Config()
            {
                Rules = new List<Rule>();
            }
        }

        protected class Rule
        {
            public Uri DomainUri;
            public List<string> NodeIds;
        }
    }
}