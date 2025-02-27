# TwistedFizzBuzz

TwistedFizzBuzz is a C# project that extends the traditional FizzBuzz problem by allowing customizable divisors and tokens. It also includes multiple console applications demonstrating its usage and unit tests to ensure reliability.

## Project Structure

- **TwistedFizzBuzzLib** - The core library containing the `FizzBuzzGenerator` class.
- **TwistedFizzBuzzApp** - A standard FizzBuzz implementation from 1 to 100.
- **TwistedFizzBuzzCustomApp** - A customizable FizzBuzz implementation with user-defined divisors and tokens.
- **TwistedFizzBuzz.Tests** - MSTest unit tests for the TwistedFizzBuzz library.

## Prerequisites

- .NET SDK (latest LTS recommended)
- Visual Studio or VS Code (optional but recommended)

## Installation

Clone the repository:
```sh
git clone https://github.com/LeonardoPaublo/SV-Interview-V6.git
cd TwistedFizzBuzz
```

Restore dependencies:
```sh
dotnet restore
```

## Running the Console Applications

### Standard FizzBuzz
```sh
dotnet run --project TwistedFizzBuzzApp
```

### Custom FizzBuzz
```sh
dotnet run --project TwistedFizzBuzzCustomApp
```

This will run FizzBuzz with:
- Multiples of 5 → "Fizz"
- Multiples of 9 → "Buzz"
- Multiples of 27 → "Bar"
- Multiples of multiple numbers → Concatenated output

## Running Tests

To execute the unit tests, run:
```sh
dotnet test
```

## Troubleshooting

### Parsed Numbers Are Empty
Ensure the input format for number ranges is correct. Example of valid input:
```
"-20-127"
```

### API Token Retrieval Fails
If using `GetTokensFromApiAsync()`, verify `_apiUrl` is correctly set in `appsettings.json` or environment variables.