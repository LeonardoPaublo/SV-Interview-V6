using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwistedFizzBuzz;
using Moq;
using System.Net.Http;
using System.Net;
using Moq.Protected;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace TwistedFizzBuzz.Tests
{
    [TestClass]
    public class FizzBuzzGeneratorTests
    {
        private FizzBuzzGenerator _fizzBuzzGenerator;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            var configData = new Dictionary<string, string>
            {
                { "ApiUrl", "https://pie-healthy-swift.glitch.me/" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _fizzBuzzGenerator = new FizzBuzzGenerator(_httpClient, _configuration);
        }

        [TestMethod]
        public void GenerateFizzBuzz_ShouldReturnExpectedResults()
        {
            // Arrange
            List<int> numbers = new List<int> { 1, 3, 5, 15 };
            Dictionary<int, string> tokens = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            // Act
            var results = _fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens).ToList();

            // Assert
            List<string> expected = new List<string> { "1", "Fizz", "Buzz", "FizzBuzz" };
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        public void GenerateFizzBuzz_ShouldHandleNegativeNumbers()
        {
            // Arrange
            List<int> numbers = new List<int> { -9, -5, -15 };
            Dictionary<int, string> tokens = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            // Act
            var results = _fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens).ToList();

            // Assert
            List<string> expected = new List<string> { "Fizz", "Buzz", "FizzBuzz" };
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        public void GenerateFizzBuzz_ShouldReturnNumbersIfNoTokensMatch()
        {
            // Arrange
            List<int> numbers = new List<int> { 2, 4, 7 };
            Dictionary<int, string> tokens = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            // Act
            var results = _fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens).ToList();

            // Assert
            List<string> expected = new List<string> { "2", "4", "7" };
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        public async Task GetTokensFromApiAsync_ShouldReturnExpectedTokens()
        {
            // Arrange
            var fakeResponse = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(fakeResponse);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var tokens = await _fizzBuzzGenerator.GetTokensFromApiAsync();

            // Assert
            CollectionAssert.AreEqual(fakeResponse, tokens);
        }

        [TestMethod]
        public async Task GetTokensFromApiAsync_ShouldHandleEmptyResponse()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });

            // Act
            var tokens = await _fizzBuzzGenerator.GetTokensFromApiAsync();

            // Assert
            Assert.AreEqual(0, tokens.Count);
        }

        [TestMethod]
        public async Task GetTokensFromApiAsync_ShouldHandleHttpErrors()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var tokens = await _fizzBuzzGenerator.GetTokensFromApiAsync();

            // Assert
            Assert.AreEqual(0, tokens.Count);
        }

        [TestMethod]
        public void GenerateFizzBuzz_ShouldReturnEmptyListForEmptyInput()
        {
            // Arrange
            List<int> numbers = new List<int>();
            Dictionary<int, string> tokens = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            // Act
            var results = _fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens).ToList();

            // Assert
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GenerateFizzBuzz_ShouldReturnEmptyListForNullInput()
        {
            // Arrange
            Dictionary<int, string> tokens = new Dictionary<int, string>
            {
                { 3, "Fizz" },
                { 5, "Buzz" }
            };

            // Act
            var results = _fizzBuzzGenerator.GenerateFizzBuzz(null, tokens).ToList();

            // Assert
            Assert.AreEqual(0, results.Count);
        }
    }
}