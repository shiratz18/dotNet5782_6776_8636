using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        /// <summary>
        /// constructor for DalObject class
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
    }
}


