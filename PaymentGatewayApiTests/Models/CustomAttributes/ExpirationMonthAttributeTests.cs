using NUnit.Framework;
using PaymentGatewayApi.Models.CustomAttributes;
using PaymentGatewayApi.Models.RequestEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApiTests.Models.CustomAttributes
{
    [TestFixture]
    public class ExpirationMonthAttributeTests : ExpirationMonthAttribute
    {
        [TestCase("00", false)]
        [TestCase("01", true)]
        [TestCase("02", true)]
        [TestCase("03", true)]
        [TestCase("04", true)]
        [TestCase("05", true)]
        [TestCase("06", true)]
        [TestCase("07", true)]
        [TestCase("08", true)]
        [TestCase("09", true)]
        [TestCase("10", true)]
        [TestCase("11", true)]
        [TestCase("12", true)]
        [TestCase("13", false)]
        public void AttributeShouldOnlyTreatValuesOneToTwelveAsValidForFutureYears(string inputMonth, bool expectedOutput)
        {
            //Arrange
            var expirationYear = DateTime.Now.AddYears(1).ToString("yy");
            var model = new ProcessPaymentRequestDto() { ExpirationYear = expirationYear };
            var validationContext2 = new ValidationContext(model);

            //Act
            var result = IsValid(inputMonth, validationContext2);

            //Assert
            Assert.AreEqual(expectedOutput, string.IsNullOrEmpty(result?.ErrorMessage));
        }

        [TestCase("00")]
        [TestCase("01")]
        [TestCase("02")]
        [TestCase("03")]
        [TestCase("04")]
        [TestCase("05")]
        [TestCase("06")]
        [TestCase("07")]
        [TestCase("08")]
        [TestCase("09")]
        [TestCase("10")]
        [TestCase("11")]
        [TestCase("12")]
        [TestCase("13")]
        public void AttributeShouldOnlyTreatCurrentOrFurtureMonthsAsValidForFutureYears(string inputMonth)
        {
            //Arrange
            var expirationYear = DateTime.Now.ToString("yy");
            var model = new ProcessPaymentRequestDto() { ExpirationYear = expirationYear };
            var validationContext2 = new ValidationContext(model);

            int inputMonthAsInt;
            int.TryParse(inputMonth, out inputMonthAsInt);
            var expectedOutput = inputMonthAsInt >= DateTime.Now.Month && inputMonthAsInt <= 12;

            //Act
            var result = IsValid(inputMonth, validationContext2);

            //Assert
            Assert.AreEqual(expectedOutput, string.IsNullOrEmpty(result?.ErrorMessage));
        }
    }
}
