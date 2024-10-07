
// ReSharper disable SuspiciousTypeConversion.Global

namespace TestResult9;

public class MaybeTest
{
    // Constructors
    [Fact]
    public void MaybeHasValue()
    {
        var input = new Maybe<int>(1);
        Assert.True(input.HasValue);
    }

    [Fact]
    public void MayBeHasNotValue()
    {
        var input = new Maybe<int>();
        Assert.False(input.HasValue);
    }
    
    [Fact]
    public void NoneProperty()
    {
        var input = Maybe<int>.None;
        Assert.True(input.HasNoValue);
    }

    [Fact]
    public void Equals1()
    {
        var input1 = new Maybe<int>(1);
        var input2 = new Maybe<int>(1);
        var input3=new Maybe<int>(2);
        var input4=new Maybe<int>();
        Assert.True(input1.Equals(input2));
        Assert.False(input1.Equals(input3));
        Assert.False(input1.Equals(input4));
    }

    [Fact]
    public void Equals2()
    {
        var input1=new Maybe<int>(1);
        var i2 = 1;
        var i3 = 2;
        Assert.True(input1.Equals(i2));
        Assert.False(input1.Equals(i3));
    }

    [Fact]
    public void Equals3()
    {
        var input1 = new Maybe<int>(1);
        var input2= new Maybe<int>(1);
        var input3= new Maybe<int>(2);
        var i3 = 1;
        var i4 = 2;
        Assert.True(Equals(input1,input2));
        Assert.False(Equals(input1,input3));
        Assert.True(Equals(input1,i3));
        Assert.False(Equals(input1,i4));
        Assert.False(Equals(input1,null));
    }

    [Fact]
    public void ExecuteTest()
    {
        var r1 = 0;
        var r2 = 0;
        var input1= new Maybe<int>(1);
        var input2= new Maybe<int>();
        input1.Execute(i=>r1=i);
        input2.Execute(_ => r2 = 2);
        Assert.Equal(1,r1);
        Assert.NotEqual(2,r2);
    }

    [Fact]
    public void MapTest()
    {
        var input1= new Maybe<int>(1);
        var input2= new Maybe<int>();
        var r1= input1.Map(i => i + 1);
        var r2= input2.Map(i => i + 1);
        Assert.Equal(new Maybe<int>(2), r1);
        Assert.Equal(new Maybe<int>(), r2);
    }

    [Fact]
    public void MatchTest()
    {
        var input1= new Maybe<int>(1);
        var input2= new Maybe<int>();
        var r1= input1.Match(i => i + 1, () => 0);
        var r2= input2.Match(i => i + 1, () => 4);
        Assert.Equal(2, r1);
        Assert.Equal(4, r2);
    }

    [Fact]
    public void OrTest()
    {
        var input1 = new Maybe<int>(1);
        var input2 = new Maybe<int>(2);
        var input3=new Maybe<int>();
        var result1 = input1.Or(input2);
        var result2= input3.Or(input2);
        Assert.Equal(input1, result1);
        Assert.Equal(input2, result2);
    }

    [Fact]
    public void SwitchTest()
    {
        var input1 = new Maybe<int>(1);
        var input2 = new Maybe<int>();
        var r1 = 0;
        var r2 = 0;
        input1.Switch(i => r1 = i, () => r1 = 4);
        input2.Switch(i => r2 = i, () => r2 = 4);
        Assert.Equal(1,r1);
        Assert.Equal(4,r2);
    }

    [Fact]
    public void ToResultTest()
    {
        var input1= new Maybe<int>(1);
        var input2= new Maybe<int>();
        var r1= input1.ToResult();
        var r2= input2.ToResult();
        Assert.Equal(1, r1);
        Assert.True(r2.IsFailed);
    }

    [Fact]
    public void ValueOrDefaultTest()
    {
        var input1=new Maybe<int>(1);
        var input2=new Maybe<int>();
        var result3 = input1.ValueOrDefault();
        var result4 = input2.ValueOrDefault();
        Assert.Equal(1,result3);
        Assert.Equal(0, result4);
    }

    [Fact]
    public void ValueOrDefaultTest2()
    {
        var input2=new Maybe<int>();
        var result4= input2.ValueOrDefault(2);
        Assert.Equal(2,result4);
    }

    [Fact]
    public void ValueOrTrown()
    {
        var input2=new Maybe<int>();
        Assert.Throws<InvalidOperationException>(() => input2.ValueOrThrow());
    }

    [Fact]
    public void WhereTest()
    {
        var input1=new Maybe<int>(1);
        var r1=input1.Where(i => i == 1);
        var r2=input1.Where(i => i == 2);
        Assert.Equal(1,r1);
        Assert.True(r2.HasNoValue);
    }
}
