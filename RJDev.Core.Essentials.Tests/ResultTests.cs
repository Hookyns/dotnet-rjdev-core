using RJDev.Core.Essentials.AppStrings;
using RJDev.Core.Essentials.Results;
using Xunit;

namespace RJDev.Core.Essentials.Tests
{
    public class ResultTests
    {
        private static readonly AppString ErrorAppString = new("tests.error", "Test error.");
        
        [Fact]
        public void PositiveResult()
        {
            IResult result = new Result(true);
            
            Assert.True(result.Ok);
            Assert.Empty(result.Errors);
        }
        
        [Fact]
        public void OkResult()
        {
            IResult result = Result.OkResult();
            
            Assert.True(result.Ok);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeErrorLessResult()
        {
            IResult result = new Result(false);
            
            Assert.False(result.Ok);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult()
        {
            IResult result = new Result(ErrorAppString);
            
            Assert.False(result.Ok);
            Assert.Single(result.Errors, ErrorAppString);
        }

        [Fact]
        public void CastResult()
        {
            IResult<bool> result = Result.OkResult().Cast(true);
            Assert.True(result.Ok);
            Assert.True(result.Value);
            Assert.Empty(result.Errors);
            
            IResult errorResult = Result.ErrorResult(ErrorAppString).Cast<bool>();
            Assert.False(errorResult.Ok);
            Assert.Single(errorResult.Errors, ErrorAppString);
        }

        [Fact]
        public void ThenPositive()
        {
            IResult result = Result.OkResult().Then(() => Result.OkResult());
            Assert.True(result.Ok);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ThenNegative()
        {
            IResult result = Result.OkResult().Then(() => Result.ErrorResult(ErrorAppString));
            Assert.False(result.Ok);
            Assert.Single(result.Errors, ErrorAppString);
        }
    }
}