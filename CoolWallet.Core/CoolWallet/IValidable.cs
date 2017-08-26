using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    public interface IValidable
    {
        /// <summary>
        /// Check if signature is valid.
        /// </summary>
        /// <returns>True if valid. False otherwise</returns>
        bool IsValid();

        /// <summary>
        /// Check if signature is valid.
        /// </summary>
        /// <param name="message">Message explaining why signature is invalid.</param>
        /// <returns>True if valid. False otherwise</returns>
        bool IsValid(out string message);
    }
}
