using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string str)
        {
            if (str == "DalObject")
                return DalObject.DalObject.Instance;
            //else if (str == "DalXml") ;
            //do something
            else
                throw new Exception(); //fix
        }
    }
}
