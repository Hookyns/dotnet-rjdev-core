using System.Net;
using RJDev.Core.Essentials.AppStrings;
using RJDev.Core.Essentials.Results;
using Xunit;

namespace RJDev.Core.Essentials.Tests
{
    public class ResultTests
    {
        private static readonly AppString ErrorAppString = new("tests.error", "Test error.");
        
        [Fact]
        public void OkResult_Ctor()
        {
            IResult result = new Result(true);
            
            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
        }
        
        [Fact]
        public void OkResult_StaticMethod()
        {
            IResult result = Result.Ok();
            
            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_Errorless()
        {
            IResult result = new Result(false);
            
            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_ErrorOnly()
        {
            IResult result = new Result(ErrorAppString);
            
            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
        }

        [Fact]
        public void NegativeResult_Ctor_StatusAndError()
        {
            IResult result = new Result((int)HttpStatusCode.BadRequest, ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectAndError()
        {
            IResult result = new Result(nameof(IResult), ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
            Assert.Equal(nameof(IResult), result.Subject);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectStatusAndError()
        {
            IResult result = new Result(nameof(IResult), (int)HttpStatusCode.BadRequest, ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
            Assert.Equal(nameof(IResult), result.Subject);
        }

        [Fact]
        public void OkResult_Ctor_FromResult()
        {
            IResult oldResult = Result.Ok();
            IResult result = new Result(oldResult);

            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_FromResult()
        {
            IResult oldResult = new Result(nameof(IResult), (int)HttpStatusCode.BadRequest, ErrorAppString);
            IResult result = new Result(oldResult);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
            Assert.Equal(nameof(IResult), result.Subject);
        }

        // [Fact]
        // public void CastResult()
        // {
        //     IResult<bool> result = Result.OkResult(true);
        //     Assert.True(result.IsOk);
        //     Assert.True(result.Value);
        //     Assert.Empty(result.Errors);
        //
        //     IResult errorResult = Result.ErrorResult(ErrorAppString);
        //     Assert.False(errorResult.IsOk);
        //     Assert.Single(errorResult.Errors, ErrorAppString);
        // }

        [Fact]
        public void PositiveThenPositive()
        {
            IResult result = Result.Ok().Then(_ => Result.Ok());
            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void PositiveThenNegative()
        {
            IResult result = Result.Ok().Then(_ => Result.Error(ErrorAppString));
            Assert.False(result.IsOk);
            Assert.Single(result.Errors, ErrorAppString);
        }

        [Fact]
        public void NegativeThen_ShouldFail()
        {
            IResult result = Result.Error().Then(_ => Assert.Fail("This should not be executed"));
            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
        }
    }
}