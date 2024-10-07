namespace TestResult9;

public class ErrorTest
{
    [Fact]
    public void CreateTest()
    {
        var now=DateTime.Now;
        var e = new Error<DateTime>(now);
        Assert.Equal(now,e.Data);
        // ReSharper disable once SpecifyACultureInStringConversionExplicitly
        Assert.Equal(now.ToString(),e.Value);
    }

    
    
    
    
}