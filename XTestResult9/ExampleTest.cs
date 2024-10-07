using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestResult9;

public class ExampleTest
{
    [Fact]
    public void Divide1Text()
    {
        var result = Divide1(10, 2);
        Assert.True(result.IsSuccessful);
        Assert.Equal(5, result.Value);
        result = Divide1(10, 0);
        Assert.True(result.IsFailed);
        Assert.Equal("Cannot divide by zero.", result.Error!.Value);
    }
    [Fact]
    public void ShowDivideTest()
    {
        var result = ShowDivide(10, 2);
        Assert.True(result.IsSuccessful);
        result = ShowDivide(10, 0);
        Assert.True(result.IsFailed);
    }
    [Fact]
    public void ShowDivide2Test()
    {
        var result = ShowDivide2(10, 2);
        Assert.True(result.IsSuccessful);
        result = ShowDivide2(10, 0);
        Assert.True(result.IsFailed);
    }

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
}