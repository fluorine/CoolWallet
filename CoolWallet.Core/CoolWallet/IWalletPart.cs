using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    public interface IWalletPart : IShortNotationFormat, IValidable, IEquatable<IWalletPart>
    {
        IWalletSignature Signature { get; set; }

        string Data { get; set; }
    }
}
