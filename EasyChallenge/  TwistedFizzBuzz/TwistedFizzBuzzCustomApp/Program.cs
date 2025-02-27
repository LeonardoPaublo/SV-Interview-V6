using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TwistedFizzBuzz;

class Program
{
    static void Main()
    {
        HttpClient httpClient = new HttpClient();
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
        FizzBuzzGenerator fizzBuzzGenerator = new FizzBuzzGenerator(httpClient, config);

        Console.WriteLine("Custom FizzBuzz from -20 to 127:");

        var numbers = fizzBuzzGenerator.ParseNumberInput("-20-127");
        Dictionary<int, string> tokens = new Dictionary<int, string>
        {
            { 5, "Fizz" },
            { 9, "Buzz" },
            { 27, "Bar" }
        };

        foreach (string result in fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens))
        {
            Console.WriteLine(result);
        }
    }
}