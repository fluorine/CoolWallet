using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolWallet.Core.Properties;

namespace CoolWallet.Core
{
    public class WalletSignature : IWalletSignature
    {
        public WalletSignature() { }

        public WalletSignature(string shortNotation)
        {
            InterpretShortNotation(shortNotation);
        }
        
        public int Version { get; set; } = 1;
        
        public int PartsThreshold { get; set; }
        
        public int PartsTotal { get; set; }

        public bool IsValid(out string message)
        {
            message = null;

            if(PartsTotal < 1)
            {
                message = Strings.SharesTotalCannotBeLessThanOne;
                return false;
            }

            if(PartsThreshold < 1)
            {
                message = Strings.SharesThresholdCannotBeLessThanOne;
                return false;
            }

            if(PartsThreshold > PartsTotal)
            {
                message = Strings.SharesThresholdCannotBeLessThanSharesTotal;
                return false;
            }

            return true;
        }
        
        public bool IsValid()
        {
            return IsValid(out string message);
        }
        
        public bool Equals(IWalletSignature other)
        {
            return Version == other.Version
                && PartsThreshold == other.PartsThreshold
                && PartsTotal == other.PartsTotal;
        }

        public string GetShortNotation()
        {
            if (!IsValid()) return null;

            return $"{Version}|{PartsThreshold}|{PartsTotal}";
        }

        public bool InterpretShortNotation(string shortNotation)
        {
            // Validate
            if (shortNotation == null) return false;

            var tokens = shortNotation.Split('|');

            if (tokens.Length != 3) return false;

            // Parse fields
            var validParse = int.TryParse(tokens[0], out int version);
            validParse = int.TryParse(tokens[1], out int threshold) && validParse;
            validParse = int.TryParse(tokens[2], out int total) && validParse;

            if (!validParse) return false;

            // Fill the object's fields
            // and validate its state.
            Version = version;
            PartsThreshold = threshold;
            PartsTotal = total;

            if (!IsValid()) return false;

            return true;
        }
    }
}
