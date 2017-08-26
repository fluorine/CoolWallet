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
    public class WalletPartTests
    {
        #region Equals

        [Test]
        public void Equals_SameDataSameSignature_True()
        {
            var walletPartA = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var walletPartB = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            Assert.That(walletPartA, Is.EqualTo(walletPartB));
        }

        [Test]
        public void Equals_DifferentDataAndDifferentSignature_False()
        {
            var walletPartA = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var walletPartB = new WalletPart()
            {
                Signature = new WalletSignature("1|4|8"),
                Data = "11baa341-49a7-48c7-b390-9b2f38d855d0"
            };

            Assert.That(walletPartA, Is.Not.EqualTo(walletPartB));
        }

        [Test]
        public void Equals_DifferentDataSameSignature_False()
        {
            var walletPartA = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var walletPartB = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "11baa341-49a7-48c7-b390-9b2f38d855d0"
            };

            Assert.That(walletPartA, Is.Not.EqualTo(walletPartB));
        }


        [Test]
        public void Equals_SameDataDifferentSignature_False()
        {
            var walletPartA = new WalletPart()
            {
                Signature = new WalletSignature("1|5|20"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var walletPartB = new WalletPart()
            {
                Signature = new WalletSignature("1|2|4"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            Assert.That(walletPartA, Is.Not.EqualTo(walletPartB));
        }

        #endregion

        #region IsValid

        [Test]
        public void IsValid_ValidWalletPart_True()
        {
            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|2|3"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            Assert.That(part.IsValid());
        }
        
        [Test]
        public void IsValid_NullDataInvalidSignature_False()
        {
            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|10|5"),
                Data = null
            };

            Assert.That(part.IsValid(), Is.False);
        }

        [Test]
        public void IsValid_ValidDataInvalidSignature_False()
        {
            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|10|5"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            Assert.That(part.IsValid(), Is.False);
        }

        [Test]
        public void IsValid_NullDataValidSignature_False()
        {
            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|5|10"),
                Data = null
            };

            Assert.That(part.IsValid(), Is.False);
        }

        [Test]
        public void IsValid_EmptyDataValidSignature_False()
        {
            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|5|10"),
                Data = " "
            };

            Assert.That(part.IsValid(), Is.False);
        }

        [Test]
        public void IsValid_NullSignatureValidData_False()
        {
            var part = new WalletPart()
            {
                Signature = null,
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            Assert.That(part.IsValid(), Is.False);
        }

        [Test]
        public void IsValid_InvalidSignatureValidData_InvalidSignatureMessage()
        {

            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|10|5"),
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var expectedMessage = Strings.SharesThresholdCannotBeLessThanSharesTotal;

            part.IsValid(out string message);

            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_ValidSignatureBlankData_BlankDataMessage()
        {

            var part = new WalletPart()
            {
                Signature = new WalletSignature("1|5|10"),
                Data = ""
            };

            var expectedMessage = Strings.DataIsNullOrEmpty;

            part.IsValid(out string message);

            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_NullSignatureValidData_EmptySignatureMessage()
        {
            var part = new WalletPart()
            {
                Signature = null,
                Data = "0aac471a-813f-49f8-9f87-1559bd462e93"
            };

            var expectedMessage = Strings.SignatureIsNull;

            part.IsValid(out string message);

            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        #endregion

        #region GetShortNotation

        [Test]
        public void GetShortNotation_ValidWalletPart_ValidShortNotation()
        {
            var walletPart = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 4
                },

                Data = "5edb71e0-ac40-40a3-a22d-70220058eba5"
            };

            var expectedShortNotation = "1|2|4|5edb71e0-ac40-40a3-a22d-70220058eba5";

            Assert.That(walletPart.GetShortNotation(), Is.EqualTo(expectedShortNotation));
        }

        [Test]
        public void GetShortNotation_InvalidSignature_NullShortNotation()
        {
            var walletPart = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 22,
                    PartsTotal = 4
                },

                Data = "5edb71e0-ac40-40a3-a22d-70220058eba5"
            };

            Assert.IsNull(walletPart.GetShortNotation());
        }

        [Test]
        public void GetShortNotation_InvalidData_NullShortNotation()
        {
            var walletPart = new WalletPart()
            {
                Signature = new WalletSignature()
                {
                    Version = 1,
                    PartsThreshold = 2,
                    PartsTotal = 4
                },

                Data = null
            };

            Assert.IsNull(walletPart.GetShortNotation());
        }
        
        #endregion

        #region InterpretShortNotation

        [Test]
        public void InterpretShortNotation_ValidShortNotation_ValidWalletPart()
        {
            var validShortNotation = "1|2|4|5edb71e0-ac40-40a3-a22d-70220058eba5";

            var walletPart = new WalletPart();

            var result = walletPart.InterpretShortNotation(validShortNotation);

            Assert.IsTrue(result);
            Assert.That(walletPart.Signature.Version, Is.EqualTo(1));
            Assert.That(walletPart.Signature.PartsThreshold, Is.EqualTo(2));
            Assert.That(walletPart.Signature.PartsTotal, Is.EqualTo(4));
            Assert.That(walletPart.Data, Is.EqualTo("5edb71e0-ac40-40a3-a22d-70220058eba5"));
        }

        [Test]
        public void InterpretShortNotationByConstructor_ValidShortNotation_ValidWalletPart()
        {
            var validShortNotation = "1|2|4|5edb71e0-ac40-40a3-a22d-70220058eba5";

            var walletPart = new WalletPart(validShortNotation);

            Assert.That(walletPart.Signature.Version, Is.EqualTo(1));
            Assert.That(walletPart.Signature.PartsThreshold, Is.EqualTo(2));
            Assert.That(walletPart.Signature.PartsTotal, Is.EqualTo(4));
            Assert.That(walletPart.Data, Is.EqualTo("5edb71e0-ac40-40a3-a22d-70220058eba5"));
        }

        [Test]
        public void InterpretShortNotation_TruncatedInvalidShortNotation_InvalidWalletPart()
        {
            var validShortNotation = "1|4|5edb71e0-ac40-40a3-a22d-70220058eba5";

            var walletPart = new WalletPart();

            var result = walletPart.InterpretShortNotation(validShortNotation);

            Assert.IsFalse(result);
        }

        [Test]
        public void InterpretShortNotation_NullShortNotation_InvalidWalletPart()
        {
            var walletPart = new WalletPart();

            var result = walletPart.InterpretShortNotation(null);

            Assert.IsFalse(result);
        }


        [Test]
        public void InterpretShortNotation_TooLongShortNotation_InvalidWalletPart()
        {
            var invalidShortNotation = "1|4|5edb71e0-ac40-40a3-a22d-70220058eba5|1111|2222|3333";
            
            var walletPart = new WalletPart();

            var result = walletPart.InterpretShortNotation(invalidShortNotation);

            Assert.IsFalse(result);
        }

        [Test]
        public void InterpretShortNotation_ValidShortNotationWithExtraDelimiters_ValidWalletPart()
        {
            var validShortNotation = "|1|2|4|5edb71e0-ac40-40a3-a22d-70220058eba5|";

            var walletPart = new WalletPart(validShortNotation);

            Assert.That(walletPart.Signature.Version, Is.EqualTo(1));
            Assert.That(walletPart.Signature.PartsThreshold, Is.EqualTo(2));
            Assert.That(walletPart.Signature.PartsTotal, Is.EqualTo(4));
            Assert.That(walletPart.Data, Is.EqualTo("5edb71e0-ac40-40a3-a22d-70220058eba5"));
        }

        #endregion

    }
}
