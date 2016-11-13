using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbraco.MoreContentFinders
{
    public class ContentFinderInsertion
    {
        public string FQN;
        public string InsertBefore;

        public ContentFinderInsertion(string fqn, string insertBefore)
        {
            FQN = fqn;
            InsertBefore = insertBefore;
        }

        public static ContentFinderInsertion Create(JToken jDefinition)
        {
            string fqn  = jDefinition["FQN"] != null
                        ? jDefinition["FQN"].ToString()
                        : String.Empty;

            string insertBefore = jDefinition["InsertBefore"] != null
                                ? jDefinition["InsertBefore"].ToString()
                                : String.Empty;

            return new ContentFinderInsertion(fqn, insertBefore);
        }
    }
}
