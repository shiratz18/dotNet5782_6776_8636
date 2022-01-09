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
        /// <summary>
        /// Activate the simulator
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <param name="updateDelegate">Delegate for updating the PL</param>
        /// <param name="stopDelegate">Delegate for stopping the simulator</param>
        public void ActivateSimulator(int id, Action updateDelegate, Func<bool> stopDelegate)
        {
            new Simulator(id, updateDelegate, stopDelegate, this);
        }
    }
}
