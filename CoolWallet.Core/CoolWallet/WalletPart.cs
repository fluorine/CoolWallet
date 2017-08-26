using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    /// <summary>
    /// Wallet parts used to recompose a cold wallet.
    /// </summary>
    public class WalletPart : IWalletPart
    {
        public IWalletSignature Signature { get; set; }

        public string Data { get; set; }

        public WalletPart() { }

        public WalletPart(string shortNotation)
        {
            InterpretShortNotation(shortNotation);
        }

        public bool Equals(IWalletPart other)
        {
            return Signature.Equals(other.Signature) && Data.Equals(other.Data);
        }

        public string GetShortNotation()
        {
            if (!IsValid()) return null;

            return $"{Signature.GetShortNotation()}|{Data}";
        }

        public bool InterpretShortNotation(string rawShortNotation)
        {
            if (rawShortNotation == null) return false;

            var shortNotation = rawShortNotation.Trim().Trim('|');

            var delimiterOccurences = shortNotation.Count(i => i == '|');

            if (delimiterOccurences != 3) return false;

            var lastIndex = shortNotation.LastIndexOf('|');

            if (lastIndex < 0) return false;

            // Set notation
            var signatureNotation = shortNotation.Substring(0, lastIndex).Trim('|');

            var signature = new WalletSignature();
            var isValidSignature = signature.InterpretShortNotation(signatureNotation);

            // Set Data
            var data = shortNotation.Substring(lastIndex, shortNotation.Length - lastIndex).Trim().Trim('|');
            var isValidData = data.Length > 0;

            // Set valid data to wallet part
            if (!isValidSignature || !isValidData) return false;

            Signature = signature;
            Data = data;

            return true;
        }

        public bool IsValid()
        {
            return IsValid(out string message);
        }

        public bool IsValid(out string message)
        {
            if(Signature == null)
            {
                message = Properties.Strings.SignatureIsNull;
                return false;
            }

            if (!Signature.IsValid(out message))
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(Data))
            {
                message = Properties.Strings.DataIsNullOrEmpty;
                return false;
            }

            return true;
        }
    }
}
