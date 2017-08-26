using NUnit.Framework;
using CoolWallet.Core;
using CoolWallet.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Tests
{
    [TestFixture]
    public class WalletTests
    {
        #region Constructor for Private Key

        private WalletSignature ValidSignature = new WalletSignature()
        {
            Version = 1,
            PartsThreshold = 2,
            PartsTotal = 8
        };

        [Test]
        public void Constructor_ValidSignature_NoExceptionExpected()
        {
            Assert.DoesNotThrow(() =>
            {
                var signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 4
                };

                var part1 = new WalletPart()
                {
                    Signature = signature,
                    Data = "0e1-1-5ca6e43983ccf56aa90fd6b66ea844a7c22a92dd92343888"
                };

                var part2 = new WalletPart()
                {
                    Signature = signature,
                    Data = "5d1-5-c67046150ecd502cdbc15f7a67e990a646febd88c229d3ea"
                };

                var wallet1 = new Wallet(signature, "ExampleOfPrivateKey");
                var wallet2 = new Wallet(new List<IWalletPart> { part1, part2 });
            });
        }

        [Test]
        public void Constructor_NullSignature_NullArgumentExceptionExpected()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var wallet1 = new Wallet(null, "ExampleOfPrivateKey");
            });
        }

        [Test]
        public void Constructor_InvalidSignature_ArgumentExceptionExpected()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var invalidSignature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 8,
                    PartsTotal = 4
                };

                var wallet1 = new Wallet(invalidSignature, "ExampleOfPrivateKey");
            });
        }

        [Test]
        public void Constructor_ValidData_NoExceptionExpected()
        {
            Assert.DoesNotThrow(() =>
            {
                var wallet = new Wallet(ValidSignature, "ExampleOfValidPrivateKey");
            });
        }

        [Test]
        public void Constructor_NullData_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                string nullData = null;
                var wallet = new Wallet(ValidSignature, nullData);
            });
        }

        [Test]
        public void Constructor_EmptyData_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var wallet = new Wallet(ValidSignature, default(string));
            });
        }

        [Test]
        public void Constructor_ValidInitialization_ValidPartsQuantity()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var wallet = new Wallet(signature, "ExampleOfPrivateKey");

            // Validate quantity of parts
            Assert.That(wallet.Parts.Count, Is.EqualTo(signature.PartsTotal));
        }

        [Test]
        public void Constructor_ValidInitialization_ValidPartsContent()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var wallet = new Wallet(signature, "constant forest adore false green weave stop guy fur freeze giggle clock");

            // Validate parts's content
            Console.WriteLine("Signature: " + signature.GetShortNotation());
            foreach(var part in wallet.Parts)
            {
                Assert.That(part.Signature.Equals(signature));
                Assert.That(part.Data, Is.Not.Null.Or.Empty);

                Console.WriteLine("Data:      " + part.Data);
            }            
        }

        [Test]
        public void Constructor_NullParts_ArgumentNullException()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            Assert.Throws<ArgumentNullException>(() => {
                var wallet = new Wallet(default(List<WalletPart>));
            });
        }

        [Test]
        public void Constructor_NotEnoughParts_ArgumentExceptionExpected()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var part = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "datadatadata"
            };

            Assert.Throws<ArgumentException>(() =>
            {

                var wallet = new Wallet(new List<WalletPart>() { part });

            }, Strings.NotEnoughShares);
        }

        [Test]
        public void Constructor_DuplicateParts_ArgumentExceptionExpected()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var partA = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "datadatadata"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "datadatadata"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB });
            });
        }

        [Test]
        public void Constructor_ValidPartsWithDuplicates_NoExceptionExpected()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var partA = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "0e1-1-5ca6e43983ccf56aa90fd6b66ea844a7c22a92dd92343888"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "631-7-8b1b1703484d828fe2a61b9c63497aa68494aa226a272657"
            };

            var partC = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "631-7-8b1b1703484d828fe2a61b9c63497aa68494aa226a272657"
            };

            Assert.DoesNotThrow(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB, partC });
            });
        }

        [Test]
        public void Constructor_ValidPartsWithDuplicates_ExpectedTrueQuantity()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var partA = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "631-7-8b1b1703484d828fe2a61b9c63497aa68494aa226a272657"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "5d1-5-c67046150ecd502cdbc15f7a67e990a646febd88c229d3ea"
            };

            var partC = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "5d1-5-c67046150ecd502cdbc15f7a67e990a646febd88c229d3ea"
            };

            var wallet = new Wallet(new List<WalletPart>() { partA, partB, partC });
            
            Assert.That(wallet.Parts.Count, Is.Not.EqualTo(3));
            Assert.That(wallet.Parts.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_EnoughValidParts_NoExceptionExpected()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var partA = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "0e1-1-5ca6e43983ccf56aa90fd6b66ea844a7c22a92dd92343888"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "631-7-8b1b1703484d828fe2a61b9c63497aa68494aa226a272657"
            };

            Assert.DoesNotThrow(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB });
            });
        }

        [Test]
        public void Constructor_InvalidParts_ArgumentExceptionExpected()
        {
            var partA = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 1
                },
                Data = "datadatadata"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 1
                },
                Data = "datadatadata22222"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB });
            });
        }

        [Test]
        public void Constructor_EnoughPartsWithDifferentSignatures_ArgumentExceptionExpected()
        {
            var partA = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 2,
                    PartsThreshold = 2,
                    PartsTotal = 4
                },

                Data = "datadatadata"
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 4
                },
                Data = "datadatadata"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB });
            }, Strings.SharesHaveDifferentSignatures);
        }

        [Test]
        public void Constructor_ValidSignatureAndPrivateKey_ValidReconstructedWallet()
        {
            var signature1 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var signature2 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var privateKey = "A private key";

            var originalWallet = new Wallet(signature1, privateKey);

            var reconstructedWallet = new Wallet(originalWallet.Parts);

            Assert.That(reconstructedWallet.PrivateKey, Is.EqualTo(reconstructedWallet.PrivateKey));
        }

        [Test]
        public void Constructor_DifferentParts_RecoveredValidPrivateKey()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var partA = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "0e1-1-5ca6e43983cff56aa90fd6b66ea844a7c22a92dd92343888" // Corrupt data
            };

            var partB = new WalletPart()
            {
                Signature = new WalletSignature(signature.GetShortNotation()),
                Data = "631-7-8b1b1703484d828fe2a61b9c63497aa68494aa226a272657"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                var wallet = new Wallet(new List<WalletPart>() { partA, partB });
            }, Strings.ChecksumExceptionSharesCorrupt);
        }

        [Test]
        public void Constructor_ValidParts_ValidPrivateKey()
        {
            var signature = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 4
            };

            var privateKey = "ExampleOfPrivateKey";
            var wallet = new Wallet(signature, privateKey);
            
            var recoveredWallet = new Wallet(
                new List<IWalletPart>()
                {
                        new WalletPart(wallet.Parts.First().GetShortNotation()),
                        new WalletPart(wallet.Parts.Last().GetShortNotation()),
                });
            
            Assert.That(recoveredWallet.PrivateKey, Is.EqualTo(privateKey));
            Assert.That(recoveredWallet.PrivateKey, Is.EqualTo(wallet.PrivateKey));
        }

        //[Test]
        //public void Constructor_ValidPartsFromDifferentWallets_InvalidPrivateKey()
        //{
        //    var signature = new WalletSignature()
        //    {
        //        Version = 1,
        //        PartsThreshold = 2,
        //        PartsTotal = 4
        //    };

        //    var privateKey1 = "ExampleOfPrivateKey1";
        //    var walletA = new Wallet(signature, privateKey1);

        //    var privateKey2 = "ExampleOfPrivateKey2";
        //    var walletB = new Wallet(signature, privateKey2);

        //    var wallet = new Wallet(
        //        new List<IWalletPart>()
        //        {
        //                walletA.Parts.FirstOrDefault(),
        //                walletB.Parts.FirstOrDefault()
        //        });

        //    Assert.IsNotNull(wallet.PrivateKey);
        //    Assert.IsNotEmpty(wallet.PrivateKey);
        //    //Assert.That(wallet.PrivateKey, Is.Not.EqualTo(privateKey1));
        //    //Assert.That(wallet.PrivateKey, Is.Not.EqualTo(privateKey2));
        //}

        #endregion
    }
}
