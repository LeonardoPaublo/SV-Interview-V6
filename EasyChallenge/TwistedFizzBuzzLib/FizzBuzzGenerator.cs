using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace TwistedFizzBuzz
{
    public class FizzBuzzGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public FizzBuzzGenerator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = configuration["FizzBuzzApiUrl"] ?? "https://pie-healthy-swift.glitch.me/";
        }

        public IEnumerable<string> GenerateFizzBuzz(IEnumerable<int> numbers, Dictionary<int, string> customTokens)
        {
            if (numbers == null || customTokens == null)
            {
                yield break;
            }

            foreach (var number in numbers)
            {
                string result = string.Concat(customTokens
                    .Where(kv => number % kv.Key == 0)
                    .Select(kv => kv.Value));

                yield return string.IsNullOrEmpty(result) ? number.ToString() : result;
            }
        }


        public async Task<Dictionary<int, string>> GetTokensFromApiAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_apiUrl);
                return JsonConvert.DeserializeObject<Dictionary<int, string>>(response) ?? new Dictionary<int, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching API tokens: {ex.Message}");
                return new Dictionary<int, string>();
            }
        }

        public IEnumerable<int> ParseNumberInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty");

            HashSet<int> numbers = new HashSet<int>();
            var parts = input.Split(',');

            foreach (var part in parts)
            {
                if (part.Contains('-'))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(part, @"(-?\d+)-(-?\d+)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int start) && int.TryParse(match.Groups[2].Value, out int end))
                    {
                        foreach (var num in Enumerable.Range(Math.Min(start, end), Math.Abs(end - start) + 1))
                        {
                            numbers.Add(num);
                        }
                    }
                }
            }

            return numbers.OrderBy(n => n);
        }
    }
}