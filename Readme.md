# Rop.Results9

`Rop.Result` is a C# library that provides a way to handle errors without exceptions. 
Instead of throwing an exception, functions return a `Result` object that can be either a `Success` or a `Failed` state.

## Installation
To install the library, you can use the NuGet package manager. In the Visual Studio terminal, run the following command:

```
Install-Package Rop.Results9
```

## Usage
To use the library, you need to import the `Rop.Result9` namespace:

```csharp
using Rop.Result9;
```

Then you can create `Result` objects using implicit conversions:


```csharp

public Result<int> Divide1(int dividend, int divisor)
    {
        if (divisor == 0)
        {
            return Error.Fail("Cannot divide by zero.");
        }
        else
        {
            return dividend / divisor;
        }
    }
```

You can also use the `Result` object in a fluent way:

```csharp
public VoidResult ShowDivide(int divident,int divisor)
    {
        var result = Divide1(divident, divisor);
        return result.Map(
            v => Console.WriteLine($"The result is {result.Value}"),
            e => Console.WriteLine($"Error: {result.Error!.Value}")
        );
    }

public VoidResult ShowDivide2(int divident,int divisor)
    {
        var result = Divide1(divident, divisor);
        if (result.IsFailed) 
            return result;
        Console.WriteLine($"The result is {result.Value!}");
        return VoidResult.Ok;
    }
```



