namespace TestResult9;

public class ResultHelperTest
{
    [Fact]
    public void ToResult_NullValue()
    {
        var value = (int?)null;
        var result = value.ToResult();
        Assert.True(result.IsNull);
        Assert.Null(result.Value);
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void ToResult_NullValueStr()
    {
        var value = (string?)null;
        var result= value.ToResult();
        Assert.True(result.IsNull);
        Assert.Null(result.Value);
        Assert.False(result.IsSuccessful);
    }
    
    [Fact]
    public void ToResultNotNull_NullValue()
    {
        var result = Result<int>.Null;
        Assert.True(result.IsNull);
        Assert.False(result.IsSuccessful);
    }
    
    [Fact]
    public void ToResultNotNullOrEmptyTest()
    {
        var result = "test".ToResultNotNullOrEmpty();
        Assert.True(result.IsSuccessful);
        Assert.Equal("test", result.Value);
        result= "".ToResultNotNullOrEmpty();
        Assert.False(result.IsSuccessful);
        Assert.Equal(Error.Empty(), result.Error);
    }
    [Fact]
    public void ToResultNotNullOrWhiteSpace_Test()
    {
        var result = "    ".ToResultNotNullOrWhiteSpace();
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void ToResultNotNull_Test2()
    {
        var result = new List<int>().ToEnumerableResult();
        Assert.True(result.IsSuccessful);
        var lst = result.Value;
        Assert.NotNull(lst);
    }
    [Fact]
    public void ToResultNotNullOrEmpty_Test2()
    {
        var result = new List<int>().ToEnumerableResultNotEmpty();
        Assert.False(result.IsSuccessful);
        Assert.Equal(Error.Empty(), result.Error);
    }
    

    [Fact]
    public void ResultString_Test()
    {
        var r1 = "test".ToResult();
        var r2 = "".ToResult();
        var r3 = Result<string>.Null;
        var r4 = "  ".ToResult();
        Assert.False(r1.IsNullOrWhiteSpace());
        Assert.True(r2.IsNullOrEmpty());
        Assert.True(r3.IsNull);
        Assert.True(r4.IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrEmpty_Test()
    {
        var r1=new List<int>().ToResult();
        Assert.True(r1.IsNullOrEmpty());
    }
}