using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
       public void ActivateSimulator(int id, Action updateDelegate, Func<bool> stopDelegate)
        {
            new Simulator(id, updateDelegate, stopDelegate, this);
        }

    }
}
