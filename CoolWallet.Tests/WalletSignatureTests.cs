using Newtonsoft.Json;
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
    public class WalletSignatureTests
    {
        #region IsValid

        [Test]
        public void IsValid_ValidSignature_True()
        {
            var signature = new WalletSignature()
            {
                PartsThreshold = 3,
                PartsTotal = 6
            };

            var result = signature.IsValid();

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_ZeroSharesThreshold_False()
        {
            var expectedMessage = Strings.SharesThresholdCannotBeLessThanOne;
            var signature = new WalletSignature()
            {
                PartsThreshold = 0,
                PartsTotal = 10
            };

            string message;
            var result = signature.IsValid(out message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_NegativeSharesThreshold_False()
        {
            var expectedMessage = Strings.SharesThresholdCannotBeLessThanOne;
            var signature = new WalletSignature()
            {
                PartsThreshold = -6,
                PartsTotal = 10
            };

            string message;
            var result = signature.IsValid(out message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_ZeroSharesTotal_False()
        {
            var expectedMessage = Strings.SharesTotalCannotBeLessThanOne;
            var signature = new WalletSignature()
            {
                PartsThreshold = 3,
                PartsTotal = 0
            };

            string message;
            var result = signature.IsValid(out message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_NegativeSharesTotal_False()
        {
            var expectedMessage = Strings.SharesTotalCannotBeLessThanOne;
            var signature = new WalletSignature()
            {
                PartsThreshold = 3,
                PartsTotal = -8
            };

            string message;
            var result = signature.IsValid(out message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void IsValid_SharesTotalLessThanSharesThreshold_False()
        {
            var expectedMessage = Strings.SharesThresholdCannotBeLessThanSharesTotal;
            var signature = new WalletSignature()
            {
                PartsThreshold = 3,
                PartsTotal = 2
            };

            string message;
            var result = signature.IsValid(out message);

            Assert.IsFalse(result);
            Assert.That(message, Is.EqualTo(expectedMessage));
        }

#endregion

        #region Equals

        [Test]
        public void Equals_EqualSignaturesInDifferentObjects_True()
        {
            var signature1 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 5
            };

            var signature2 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 5
            };

            // Different objects with the same values
            Assert.That(signature1, Is.EqualTo(signature2));
            Assert.IsFalse(signature1 == signature2);

            // Compare to deserialized object
            var json = JsonConvert.SerializeObject(signature1);
            var deserialized = JsonConvert.DeserializeObject<WalletSignature>(json);

            Assert.That(deserialized, Is.EqualTo(signature1));
            Assert.That(deserialized, Is.EqualTo(signature2));
        }

        [Test]
        public void Equals_DifferentSignaturesInDifferentObjects_False()
        {
            var signature1 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 5
            };

            var signature2 = new WalletSignature()
            {
                Version = 1,
                PartsThreshold = 2,
                PartsTotal = 7
            };

            // Different objects with the different values
            Assert.That(signature1, Is.Not.EqualTo(signature2));
            Assert.IsFalse(signature1 == signature2);
        }

        #endregion

        #region GetShortNotation
        
        [Test]
        public void GetShortNotation_ValidSignature_ValidShortNotation()
        {
            var expectedShortNotation = "12|5|10";
            
            var signature = new WalletSignature()
            {
                Version = 12,
                PartsThreshold = 5,
                PartsTotal = 10
            };

            var shortNotation = signature.GetShortNotation();

            Assert.That(shortNotation, Is.EqualTo(expectedShortNotation));
        }

        #endregion

        #region InterpretShortNotation

        [Test]
        public void InterpretShortNotationByConstructor_ValidNotationAndData_ValidSignature()
        {
            var shortNotation = "1|3|10";

            var signature = new WalletSignature(shortNotation);

            Assert.That(signature.Version, Is.EqualTo(1));
            Assert.That(signature.PartsThreshold, Is.EqualTo(3));
            Assert.That(signature.PartsTotal, Is.EqualTo(10));
        }

        [Test]
        public void InterpretShortNotation_ValidNotationAndData_ValidSignature()
        {
            var shortNotation = "1|3|10";

            var signature = new WalletSignature();
            signature.InterpretShortNotation(shortNotation);

            Assert.That(signature.Version, Is.EqualTo(1));
            Assert.That(signature.PartsThreshold, Is.EqualTo(3));
            Assert.That(signature.PartsTotal, Is.EqualTo(10));
        }

        [Test]
        public void InterpretShortNotationByConstructor_ValidNotationInvalidData_InvalidSignature()
        {
            var shortNotation = "1|10|3";

            var signature = new WalletSignature(shortNotation);

            Assert.IsFalse(signature.IsValid());
        }

        [Test]
        public void InterpretShortNotation_ValidNotationInvalidData_InvalidSignature()
        {
            var shortNotation = "1|10|3";

            var signature = new WalletSignature();
            var result = signature.InterpretShortNotation(shortNotation);

            Assert.IsFalse(result);
        }

        [Test]
        public void InterpretShortNotation_IncompleteNotation_InvalidSignature()
        {
            var shortNotation = "1|10";

            var signature = new WalletSignature();
            var result = signature.InterpretShortNotation(shortNotation);

            Assert.IsFalse(result);
        }

        [Test]
        public void InterpretShortNotation_Null_InvalidSignature()
        {
            var signature = new WalletSignature();
            var result = signature.InterpretShortNotation(null);

            Assert.IsFalse(result);
        }

        #endregion
    }
}
