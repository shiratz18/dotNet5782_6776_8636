using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal
{
    partial class DalObject : IDal
    {
        private static DalObject instance = null;
        private static readonly object padlock = new object();

        public static DalObject Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DalObject();
                        }
                    }
                }
                return instance;
            }
        }

        private DalObject()
        {
            DataSource.Initialize();
        }
    }
}


