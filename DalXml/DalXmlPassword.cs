using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    partial class DalXml
    {
        #region Get access code
        /// <summary>
        /// Returns the access code for worker interface
        /// </summary>
        /// <returns>The access code</returns>
        public string GetAccessCode()
        {
            List<string> config = XmlTools.LoadListFromXMLSerializer<string>(@"ConfigXml.xml");
            return config[6];
        }
        #endregion
    }
}
