namespace TestResult9;

public class VoidResultTest
{
    [Fact]
    public void Create_Test()
    {
        var result = new VoidResult();
        Assert.True(result.IsSuccessful);
    }
    [Fact]
    public void Ok_Test()
    {
        var result = VoidResult.Ok;
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Success_Test()
    {
        var result = VoidResult.Success;
        Assert.True(result.IsSuccessful);
    }
    [Fact]
    public void Fail_Test()
    {
        var result = VoidResult.Fail();
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
    }
    // /generate tests for Fold Tee and Bind
    
    [Fact]
    public void Tee_Test()
    {
        var r1 = new VoidResult();
        var v1 = "";
        var r2 = VoidResult.Fail();
        var v2 = "";
        var teeResult = r1.DoAction(() => v1="hola");
        var teeFailResult= r2.DoAction(() => v2="hola");
        Assert.True(teeResult.IsSuccessful);
        Assert.Equal("hola", v1);
        Assert.True(teeFailResult.IsFailed);
        Assert.Equal("", v2);
    }
    [Fact]
    public void FromResult_Test()
    {
        var result = new Result<int>(42);
        var voidResult = VoidResult.FromResult(result);
        Assert.True(voidResult.IsSuccessful);
    }

    
    [Fact]
    public void Fold_Test()
    {
        var r1 = VoidResult.Ok;
        var r2 = VoidResult.Fail();
        var r3 = VoidResult.Ok;
        var r4 = VoidResult.Fail();
        var foldResult = VoidResult.Fold(r1, r2, r3, r4);
        Assert.False(foldResult.IsSuccessful);
    }

   

    [Fact]
    public void Match_Test()
    {
        var r1 = VoidResult.Ok;
        var r2 = VoidResult.Fail();
        string v=r1.Match(() => "success", _ => "fail");
        string v2 = r2.Match(() => "success", _ => "fail");
        Assert.Equal("success", v);
        Assert.Equal("fail", v2);
    }
}