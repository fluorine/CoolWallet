using Moserware.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Core
{
    /// <summary>
    /// Full representation of a Shared Cold Wallet.
    /// </summary>
    public class Wallet : IWallet
    {
        public IWalletSignature Signature { get; }

        public string PrivateKey { get; private set; }

        public IEnumerable<IWalletPart> Parts { get; private set; }

        private Wallet(IWalletSignature signature)
        {
            // Validate signature
            if(signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            if (!signature.IsValid(out string message))
            {
                throw new ArgumentException(message);
            }

            // Add data
            Signature = signature;
        }

        /// <summary>
        /// Constructor to create a new wallet using a private key.
        /// </summary>
        /// <param name="signature">Signature of the wallet.</param>
        /// <param name="privateKey">Private key to divide</param>
        public Wallet(IWalletSignature signature, string privateKey) : this(signature)
        {
            // Validate private key
            if (string.IsNullOrWhiteSpace(privateKey)) {
                throw new ArgumentException($"Argument '{nameof(privateKey)}' is null or white space.");
            }

            PrivateKey = privateKey;

            // Produce wallet parts
            ProduceWalletParts();
        }
        
        public Wallet(IEnumerable<IWalletPart> parts) : this(parts?.FirstOrDefault()?.Signature)
        {
            if(parts == null || !parts.Any())
            {
                throw new ArgumentNullException(nameof(parts));
            }

            var signature = parts.First().Signature;

            // Check if all parts have the same signatures
            if(!parts.All(p => p.Signature.Equals(signature)))
            {
                throw new ArgumentException(Properties.Strings.SharesHaveDifferentSignatures);
            }

            // Get only unique parts. Discard shares with duplicate data.
            var _parts = parts.GroupBy(i => i.Data).Select(i => i.First());
            
            // Check if there are enough parts
            if(_parts.Count() < signature.PartsThreshold)
            {
                throw new ArgumentException(Properties.Strings.NotEnoughShares);
            }

            // Validate parts
            foreach(var part in _parts)
            {
                if(part.IsValid(out string message)) continue;

                throw new ArgumentException("Invalid part: " + message);
            }

            Parts = _parts;

            // Produce private key
            var shares = _parts
                .Take(signature.PartsThreshold)
                .Select(i => i.Data)
                .ToArray();

            try
            {
                PrivateKey = SecretCombiner.Combine(shares).RecoveredTextString;
            } catch (InvalidChecksumShareException)
            {
                throw new ArgumentException(Properties.Strings.ChecksumExceptionSharesCorrupt);
            }
        }

        public bool Equals(IWallet other)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            return Signature.IsValid() && (PrivateKey != null || (Parts != null && Parts.Any()));
        }

        public bool IsValid(out string message)
        {
            throw new NotImplementedException();
        }
        
        private void ProduceWalletParts()
        {
            if (PrivateKey == null || Signature == null) return;

            var _parts = new List<WalletPart>(Signature.PartsTotal);

            // Generate Parts from the Private Key
            var shares = SecretSplitter
                .SplitMessage(PrivateKey, Signature.PartsThreshold, Signature.PartsTotal);

            foreach(var share in shares)
            {
                _parts.Add(new WalletPart()
                {
                    Signature = Signature,
                    Data = share
                });
            }

            Parts = _parts;
        }
    }
}
