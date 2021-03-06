﻿using NUnit.Framework;
using PaymentGatewayApi.Models.RequestEntities;
using System;
using System.Linq;
using static PaymentGatewayApi.Models.Enums.PaymentEnums;

namespace PaymentGatewayApiTests.Models.RequestEntities
{
    public class ProcessPaymentRequestDtoTests
    {
        [TestCase("CardNumber")]
        [TestCase("CardHolder")]
        [TestCase("CardType")]
        [TestCase("ExpirationMonth")]
        [TestCase("ExpirationYear")]
        [TestCase("PaymentAmount")]
        [TestCase("Currency")]
        [TestCase("Cvv")]
        public void ModelWithoutRequiredPropertiesShouldFailValidation(string propertyName)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto();
            
            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.IsTrue(results.Any(result => result.MemberNames.Contains(propertyName) && result.ErrorMessage.Contains("required")));
        }

        [TestCase("5500000000000004", false)] //MasterCard
        [TestCase("4111111111111111", false)] //Visa
        [TestCase("340000000000009", false)] //Amex
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("12345678901234567890", true)]
        [TestCase("abcdefghijklmnop", true)]
        public void ValidateCardNumber(string cardNumber, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                CardNumber = cardNumber
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("CardNumber")));
        }

        [TestCase(0, false)] //MasterCard
        [TestCase(1, false)] //Visa
        [TestCase(2, false)] //Amex
        [TestCase(-1, true)]
        [TestCase(3, true)]
        public void ValidateCardType(int cardType, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                CardType = (CardType)cardType
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("CardType")));
        }

        [TestCase("01", false)]
        [TestCase("02", false)]
        [TestCase("03", false)]
        [TestCase("04", false)]
        [TestCase("05", false)]
        [TestCase("06", false)]
        [TestCase("07", false)]
        [TestCase("08", false)]
        [TestCase("09", false)]
        [TestCase("10", false)]
        [TestCase("11", false)]
        [TestCase("12", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("001", true)]
        [TestCase("a", true)]
        [TestCase("12a", true)]
        [TestCase("a12a", true)]
        public void ValidateExpirationMonth(string expirationMonth, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                ExpirationMonth = expirationMonth,
                ExpirationYear = DateTime.Now.AddYears(1).ToString("yy") //The tests for the custom validation logic are in the CustomAttributes folder.
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("ExpirationMonth")));
        }

        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("111", true)]
        [TestCase("1111", true)]
        [TestCase("1a", true)]
        [TestCase("a1", true)]
        [TestCase("abc", true)]
        //The tests for the custom validation logic which require the current date are in the CustomAttributes folder.
        public void ValidateExpirationYearFormat(string expirationYear, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                ExpirationYear = expirationYear 
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("ExpirationYear")));
        }

        [TestCase(0.00, false)]
        [TestCase(1.00, false)]
        [TestCase(100.00, false)]
        [TestCase(1000.00, false)]
        [TestCase(10000.00, false)]
        [TestCase(10000.00, false)]
        [TestCase(100000.00, false)]
        [TestCase(1000000.00, false)]
        [TestCase(10000000.00, false)]
        [TestCase(100000000.00, false)]
        [TestCase(1000000000.00, false)]
        [TestCase(10000000000.00, false)]
        [TestCase(100000000000.00, false)]
        [TestCase(1000000000000.00, false)]
        [TestCase(10000000000000.00, false)]
        [TestCase(-0.01, true)]
        [TestCase(-0.001, true)]
        [TestCase(-0.0001, true)]
        [TestCase(-0.00001, true)]
        [TestCase(-0.000001, true)]
        [TestCase(-0.0000001, true)]
        [TestCase(-0.00000001, true)]
        [TestCase(-0.000000001, true)]
        [TestCase(-0.0000000001, true)]
        [TestCase(-0.00000000001, true)]
        //The tests for the custom validation logic which require the current date are in the CustomAttributes folder.
        public void ValidatePaymentAmount(decimal paymentAmount, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                PaymentAmount = paymentAmount
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("PaymentAmount")));
        }

        [TestCase(0, false)] //British pounds
        [TestCase(1, false)] //US dollars
        [TestCase(2, false)] //Euros
        [TestCase(3, false)] //Swiss francs
        [TestCase(4, false)] //Singaporean dollars
        [TestCase(5, false)] //United Arab Emirates dirhams
        [TestCase(6, false)] //Hong Kong dollars
        [TestCase(7, false)] //Brazilian reals
        [TestCase(8, false)] //Mauritian rupees
        [TestCase(9, false)] //Austrailian dollars
        [TestCase(10, false)] //New Zealand dollars
        [TestCase(11, false)] //Canadian dollars
        [TestCase(-1, true)]
        [TestCase(12, true)]

        public void ValidateCurrency(int currency, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                Currency = (SupportedCurrencies)currency
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("Currency")));
        }

        [TestCase("123", false)]
        [TestCase("1234", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("12", true)]
        [TestCase("12 ", true)]
        [TestCase("12a", true)]
        [TestCase("12ab", true)]
        [TestCase("12345", true)]
        [TestCase("  1", true)]
        public void ValidateCvv(string cvv, bool shouldFailValidation)
        {
            //Arrange
            var model = new ProcessPaymentRequestDto()
            {
                Cvv = cvv
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("Cvv")));
        }
    }
}
