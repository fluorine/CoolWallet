using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    public interface IWallet : IValidable, IEquatable<IWallet>
    {
        IWalletSignature Signature { get; }

        string PrivateKey { get; }

        IEnumerable<IWalletPart> Parts { get; }
    }
}
