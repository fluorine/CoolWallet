using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    public interface IShortNotationFormat
    {
        /// <summary>
        /// Get short notation to represent an object
        /// with the least text possible. Useful to
        /// encode data in QR codes.
        /// </summary>
        /// <returns>A short notation of the object.
        /// Null if object does not have a valid state.
        /// </returns>
        string GetShortNotation();

        /// <summary>
        /// Interpret a given short notation for the
        /// object to fill key properties.
        /// </summary>
        /// <param name="shortNotation">Short notation text.</param>
        /// <returns>
        /// True if short notation was parsed successfully.
        /// False otherwise.
        /// </returns>
        bool InterpretShortNotation(string shortNotation);
    }
}
