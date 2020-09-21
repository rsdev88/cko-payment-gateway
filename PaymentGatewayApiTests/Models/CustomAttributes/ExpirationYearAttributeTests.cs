using NUnit.Framework;
using PaymentGatewayApi.Models.CustomAttributes;
using System;

namespace PaymentGatewayApiTests.Models.CustomAttributes
{
    [TestFixture]
    public class ExpirationYearAttributeTests
    {
        [TestCase(-3, false)]
        [TestCase(-2, false)]
        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, true)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        [TestCase(9, true)]
        [TestCase(10, true)]
        [TestCase(11, true)]
        [TestCase(12, true)]
        [TestCase(13, true)]
        [TestCase(14, true)]
        [TestCase(15, true)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        public void AttributeShouldOnlyTreatValuesInTheNextFifteenYearsAsValid(int inputYear, bool expectedResult)
        {
            //Arrange
            var input = DateTime.Now.AddYears(inputYear).ToString("yy");
            var attribute = new ExpirationYearAttribute();

            //Act
            var result = attribute.IsValid(input);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
