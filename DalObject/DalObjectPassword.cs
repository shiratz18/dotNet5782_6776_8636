using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        #region Get access code
        /// <summary>
        /// Returns the access code for worker interface
        /// </summary>
        /// <returns>The access code</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetAccessCode()
        {
            return DataSource.Config.AccessCode;
        }
        #endregion
    }
}
