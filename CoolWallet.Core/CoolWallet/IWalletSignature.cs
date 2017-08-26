using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    public interface IWalletSignature : IShortNotationFormat, IValidable, IEquatable<IWalletSignature>
    {
        /// <summary>
        /// Version used to generate the parts of the Cold Wallet.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Numbers of parts required to recompose a Wallet (Private Key).
        /// It is a numerator.
        /// </summary>
        int PartsThreshold { get; }

        /// <summary>
        /// Total of parts generated from the Wallet (Privete Key).
        /// It is a denominator.
        /// </summary>
        int PartsTotal { get; }
    }
}
