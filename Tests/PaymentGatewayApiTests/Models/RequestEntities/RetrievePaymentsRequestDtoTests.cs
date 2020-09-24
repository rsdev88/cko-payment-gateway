using NUnit.Framework;
using PaymentGatewayApi.Models.RequestEntities;
using System;
using System.Linq;

namespace PaymentGatewayApiTests.Models.RequestEntities
{
    [TestFixture]
    public class RetrievePaymentsRequestDtoTests
    {
        [TestCase("TransactionId")]
        public void ModelWithoutRequiredPropertiesShouldFailValidation(string propertyName)
        {
            //Arrange
            var model = new RetrievePaymentsRequestDto();

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.IsTrue(results.Any(result => result.MemberNames.Contains(propertyName) && result.ErrorMessage.Contains("required")));
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", false)]
        [TestCase("11111111-1111-1111-1111-111111111111", false)]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [TestCase("zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz", true)]
        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa", true)]
        [TestCase("aaaaaaaa-aaaa-aaaa-aaa-aaaaaaaaaaaa", true)]
        [TestCase("aaaaaaaa-aaaa-aaa-aaaa-aaaaaaaaaaaa", true)]
        [TestCase("aaaaaaaa-aaa-aaaa-aaaa-aaaaaaaaaaaa", true)]
        [TestCase("aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", true)]
        [TestCase("aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", true)]
        public void ValidateTransactionId(string transactionId, bool shouldFailValidation)
        {
            //Arrange
            Guid? guid;
            try
            {
                guid = new Guid(transactionId);
            }
            catch
            {
                guid = null;
            }

            var model = new RetrievePaymentsRequestDto()
            {
                TransactionId = guid
            };

            //Act
            var results = ModelValidator.ValidateModel(model);

            //Assert
            Assert.AreEqual(shouldFailValidation, results.Any(result => result.MemberNames.Contains("TransactionId")));
        }
    }
}
