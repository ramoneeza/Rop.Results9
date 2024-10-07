namespace TestResult9
{
    public class ResultTest
    {
        [Fact]
        public void VoidResult_Test()
        {
            var result = (VoidResult)true;
            Assert.True(result.IsSuccessful);
            Assert.False(result.IsFailed);
        }

        [Fact]
        public void VoidResult_Test2()
        {
            var result = (VoidResult)false;
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsFailed);
        }

        [Fact]
        public void Result_IsNull()
        {
            var result = new Result<string>((string?)null);
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Result_IsNotNull()
        {
            var result = new Result<string>("test");
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Result_IsNull_Error()
        {
            var result = new Result<string>(Error.Fail());
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsFailed);
        }

        [Fact]
        public void Result_IsNotNull_Error()
        {
            var result = Result<string>.Null;
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsFailed);
        }

        [Fact]
        public void Result_ValueOrThrow()
        {
            var result = new Result<string>("test");
            var m = result.Value;
            Assert.Equal("test", m);
        }

        [Fact]
        public void Result_ValueOrThrow_Error()
        {
            var result = new Result<string>(Error.Fail());
            Assert.Throws<ResultException>(() => result.ValueOrThrow());
        }

        [Fact]
        public void Result_NotNullValueOrThrow()
        {
            var result = new Result<string>("test");
            Assert.Equal("test", result.ValueOrThrow());
        }

        [Fact]
        public void Result_NotNullValueOrThrow_Error()
        {
            var result = new Result<string>(Error.Fail());
            Assert.Throws<ResultException>(() => result.ValueOrThrow());
        }

        [Fact]
        public void Result_Implicit_Convert()
        {
            Result<string> result = "test";
            Assert.Equal("test", result.ValueOrDefault(""));
        }

        [Fact]
        public void Result_Implicit_Convert_Null_Error()
        {
            Result<string> result = Error.Fail();
            Assert.Throws<ResultException>(() => result.ValueOrThrow());
        }

        [Fact]
        public void Result_NotNull()
        {
            Result<string> result = "test";
            Assert.Equal("test", result.IfFailed(""));
        }

        [Fact]
        public void Result_NotNull_Error()
        {
            Result<string> result = Error.Fail();
            Assert.Throws<ResultException>(() => result.ValueOrThrow());
        }

        [Fact]
        public void Result_MapNull()
        {
            Result<string> result = Result<string>.Null;
            var rr = result.MapFailed("hola");
            Assert.True(rr.IsSuccessful);
            Assert.False(rr.IsFailed);
            Assert.Equal("hola", rr.ValueOrThrow());
        }

        [Fact]
        public void Result_Map()
        {
            Result<string> result = "test";
            var r = result.Map<int>(s => s.Length);
            Assert.Equal(4, r.ValueOrThrow());
        }

        [Fact]
        public void Result_Map_Error()
        {
            Result<string> result = Error.Fail();
            var r = result.Map<int>(s => s.Length);
            Assert.False(r.IsSuccessful);
            Assert.True(r.IsFailed);
        }

        [Fact]
        public void Result_Map_Null()
        {
            Result<string> result = new Result<string>((string?)null);
            var r = result.Map<int>(s => s.Length);
            Assert.False(r.IsSuccessful);
            Assert.True(r.IsFailed);
        }

        [Fact]
        public void Result_Map2()
        {
            Result<string> result = "hola";
            var r = result.Map(s => s.Length);
            Assert.Equal(4, r.ValueOrThrow());
        }

        [Fact]
        public void Result_Map2_Error()
        {
            Result<string> result = Error.Fail();
            var r = result.Map(s => s.Length);
            Assert.False(r.IsSuccessful);
            Assert.True(r.IsFailed);
        }


        [Fact]
        public void Result_Equals()
        {
            Result<string> result = "test";
            Result<string> result2 = "test";
            Assert.True(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals2()
        {
            Result<string> result = "test";
            Result<string> result2 = "test2";
            Assert.False(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals3()
        {
            Result<string> result = "test";
            Result<string> result2 = Error.Fail();
            Assert.False(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals4()
        {
            Result<string> result = Error.Fail();
            Result<string> result2 = Error.Fail();
            Assert.True(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals5()
        {
            Result<string> result = Error.Fail();
            Result<string> result2 = null!;
            Assert.False(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals6()
        {
            Result<string> result = new Result<string>((string?)null);
            Result<string> result2 = new Result<string>((string?)null);
            Assert.True(result.Equals(result2));
        }

        [Fact]
        public void Result_Equals7()
        {
            Result<string> result = "hola";
            var v2 = "hola";
            Assert.True(result.Equals(v2));
        }

        [Fact]
        public void Result_Equals8()
        {
            Result<string> result = "hola";
            var v2 = "hola2";
            Assert.False(result.Equals(v2));
        }

        [Fact]
        public void Result_Equals9()
        {
            Result<string> result = Error.Fail();
            var v2 = "hola";
            Assert.False(result.Equals(v2));
        }

        [Fact]
        public void Result_GetHashCode()
        {
            Result<string> result = "test";
            Result<string> result2 = "test";
            Assert.Equal(result.GetHashCode(), result2.GetHashCode());
        }

        [Fact]
        public void Result_GetHashCode2()
        {
            Result<string> result = "test";
            Result<string> result2 = "test2";
            Assert.NotEqual(result.GetHashCode(), result2.GetHashCode());
        }

        [Fact]
        public void Result_GetHashCode3()
        {
            Result<string> result = "test";
            Result<string> result2 = Error.Fail();
            Assert.NotEqual(result.GetHashCode(), result2.GetHashCode());
        }

        [Fact]
        public void Result_Deconstruct()
        {
            Result<string> result = "test";
            var (value, error) = result;
            Assert.Equal("test", value);
            Assert.Null(error);
        }

        [Fact]
        public void Result_Deconstruct2()
        {
            Result<string> result = Error.Fail();
            var (value, error) = result;
            Assert.Null(value);
            Assert.Equal(Error.Fail(), error);
        }

        [Fact]
        public void Result_Deconstruct3()
        {
            Result<string> result = new Result<string>((string?)null);
            var (value, error) = result;
            Assert.Null(value);
            Assert.NotNull(error);
        }

        [Fact]
        public void Result_Match()
        {
            Result<string> result = "test";
            var r = result.Match(
                s => s.Length,
                e => 0
            );
            Assert.Equal(4, r);
        }

        [Fact]
        public void Result_Match2()
        {
            Result<string> result = Error.Fail();
            var r = result.Match(
                s => s.Length,
                e => 0
            );
            Assert.Equal(0, r);
        }

        [Fact]
        public void Result_Match3()
        {
            Result<string> result = new Result<string>((string?)null);
            var r = result.Match<int>(s => s.Length, e => 0);
            Assert.Equal(0, r);
        }
    }
}
