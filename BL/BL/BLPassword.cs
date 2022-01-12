using System.Runtime.CompilerServices;
namespace BL
{
    partial class BL
    {
        #region Get access code
        /// <summary>
        /// Returns the access code for worker interface
        /// </summary>
        /// <returns>The access code</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetAccessCode()
        {
            lock (Data)
            {
                return Data.GetAccessCode();
            }
        }
        #endregion
    }
}
