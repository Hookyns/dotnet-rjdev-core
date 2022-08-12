using RJDev.Core.Essentials.AppStrings;
using RJDev.Core.Essentials.Results;
using Xunit;

namespace RJDev.Core.Essentials.Tests
{
    public class ValueResultTests
    {
        private static readonly AppString ErrorAppString = new("tests.error", "Test error.");

        private class Foo
        {
        }

        [Fact]
        public void PositiveResult()
        {
            IResult<bool> result = new Result<bool>(true, true);
            
            Assert.True(result.Ok);
            Assert.True(result.Value);
            Assert.Empty(result.Errors);
        }
        
        [Fact]
        public void OkResult()
        {
            IResult<bool> result = Result.OkResult(true);
            
            Assert.True(result.Ok);
            Assert.True(result.Value);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeErrorLessResult()
        {
            IResult result = Result.ErrorResult<bool>();
            
            Assert.False(result.Ok);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult()
        {
            IResult<bool> result = Result.ErrorResult<bool>(ErrorAppString);
            
            Assert.False(result.Ok);
            Assert.False(result.Value);
            Assert.Single(result.Errors, ErrorAppString);
        }

        [Fact]
        public void CastResult()
        {
            IResult<int> result = Result.OkResult(false).Cast(1);
            Assert.True(result.Ok);
            Assert.True(result.Value == 1);
            Assert.Empty(result.Errors);
            
            IResult errorResult = Result.ErrorResult<bool>(ErrorAppString).Cast<int>();
            Assert.False(errorResult.Ok);
            Assert.Single(errorResult.Errors, ErrorAppString);
        }

        [Fact]
        public void ThenPositive()
        {
            IResult<Foo> result = Result.OkResult(true).Then(() => Result.OkResult(new Foo()));
            Assert.True(result.Ok);
            Assert.NotNull(result.Value);
            Assert.IsType<Foo>(result.Value);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ThenNegative()
        {
            IResult<Foo> result = Result.OkResult(true).Then(() => Result.ErrorResult<Foo>(ErrorAppString));
            Assert.False(result.Ok);
            Assert.Null(result.Value);
            Assert.Single(result.Errors, ErrorAppString);
        }
    }
}