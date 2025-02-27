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

        Console.WriteLine("Standard FizzBuzz from 1 to 100:");

        var numbers = fizzBuzzGenerator.ParseNumberInput("1-100");
        Dictionary<int, string> tokens = new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } };

        foreach (string result in fizzBuzzGenerator.GenerateFizzBuzz(numbers, tokens))
        {
            Console.WriteLine(result);
        }
    }
}