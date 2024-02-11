using System.Net;
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
        public void OkResult_Ctor()
        {
            IResult<bool> result = new Result<bool>(true, true);
            
            Assert.True(result.IsOk);
            Assert.True(result.Value);
            Assert.Empty(result.Errors);
        }
        
        [Fact]
        public void OkResult_StaticMethod()
        {
            IResult<bool> result = Result.Ok(true);
            
            Assert.True(result.IsOk);
            Assert.True(result.Value);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_Errorless()
        {
            IResult<bool> result = new Result<bool>(false, false);
            
            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
            Assert.False(result.Value);
        }

        [Fact]
        public void NegativeResult_StaticMethod_Errorless()
        {
            IResult<bool> result = Result.Error<bool>();

            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_StaticMethod_ErrorOnly()
        {
            IResult<bool> result = Result.Error<bool>(ErrorAppString);
            
            Assert.False(result.IsOk);
            Assert.False(result.Value);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        }

        [Fact]
        public void NegativeResult_Ctor_StatusAndError()
        {
            IResult<bool> result = new Result<bool>((int)HttpStatusCode.BadRequest, ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectAndError()
        {
            IResult<bool> result = new Result<bool>(nameof(IResult<bool>), ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal(nameof(IResult<bool>), result.Subject);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectStatusAndError()
        {
            IResult<bool> result = new Result<bool>(nameof(IResult<bool>), (int)HttpStatusCode.BadRequest, ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
            Assert.Equal(nameof(IResult<bool>), result.Subject);
        }

        // [Fact]
        // public void ThenPositive()
        // {
        //     IResult<Foo> result = Result.OkResult(true).Then(() => Result.OkResult(new Foo()));
        //     Assert.True(result.IsOk);
        //     Assert.NotNull(result.Value);
        //     Assert.IsType<Foo>(result.Value);
        //     Assert.Empty(result.Errors);
        // }
        //
        // [Fact]
        // public void ThenNegative()
        // {
        //     IResult<Foo> result = Result.OkResult(true).Then(() => Result.ErrorResult<Foo>(ErrorAppString));
        //     Assert.False(result.IsOk);
        //     Assert.Null(result.Value);
        //     Assert.Single(result.Errors, ErrorAppString);
        // }

        [Fact]
        public void PositiveThenPositive()
        {
            IResult<int> result = Result.Ok(true).Then(prevResult =>
            {
                Assert.True(prevResult.IsOk);
                Assert.True(prevResult.Value);
                return Result.Ok(1);
            });

            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void PositiveThenNegative()
        {
            IResult<int> result = Result.Ok(true).Then(prevResult =>
            {
                Assert.True(prevResult.IsOk);
                Assert.True(prevResult.Value);
                return Result.Error<int>(ErrorAppString);
            });

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        }

        [Fact]
        public void NegativeThen_ShouldFail()
        {
            IResult<int> result = Result.Error<bool>().Then(_ =>
            {
                Assert.Fail("This should not be executed");
                return Result.Error<int>();
            });

            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Negative_ChangeType()
        {
            var oldResult = Result.Error<bool>(ErrorAppString);
            IResult<int> result = Result.Error<bool, int>(oldResult);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        }

        [Fact]
        public void Negative_ChangeType2()
        {
            var oldResult = Result.Error<bool>(ErrorAppString);
            IResult<int> result = Result<int>.Error(oldResult);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        }
    }
}