using System;
using System.Linq;
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
        public void OkResult_StaticMethod()
        {
            IResult result = Result.Ok();

            Assert.True(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_Errorless()
        {
            IResult result = Result.Error();

            Assert.False(result.IsOk);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void NegativeResult_Ctor_ErrorOnly()
        {
            IResult result = Result.Error(ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        }

        [Fact]
        public void NegativeResult_Ctor_StatusAndError()
        {
            IResult result = Result.Error((int)HttpStatusCode.BadRequest, ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectAndError()
        {
            IResult result = Result.Error(nameof(IResult), ErrorAppString);

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal(nameof(IResult), result.Subject);
        }

        [Fact]
        public void NegativeResult_Ctor_SubjectStatusAndError()
        {
            IResult result = Result.Error(
                nameof(IResult),
                (int)HttpStatusCode.BadRequest,
                ErrorAppString
            );

            Assert.False(result.IsOk);
            Assert.Single(result.Errors, err => err.Message == ErrorAppString);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
            Assert.Equal(nameof(IResult), result.Subject);
        }

        [Fact]
        public void CastResult_Ok()
        {
            IResult result = Result.Ok();
            var casted = result.Cast<int>(666);

            Assert.True(casted.IsOk);
            Assert.Equal(666, casted.Value);
        }

        [Fact]
        public void CastResult_Ok_ThrowWhenToValue()
        {
            IResult result = Result.Ok();

            Assert.Throws<ArgumentNullException>(() => result.Cast<bool>());
        }

        [Fact]
        public void CastResult_Error()
        {
            IResult result = Result.Error(nameof(CastResult_Error), 400, ErrorAppString);
            var casted = result.Cast<int>();

            Assert.False(casted.IsOk);
            Assert.Equal(400, casted.Status);
            Assert.Equal(nameof(CastResult_Error), casted.Subject);
            Assert.Collection(casted.Errors, err => Assert.Equal(ErrorAppString, err.Message));
        }

        [Fact]
        public void CastTypedResult_Ok()
        {
            IResult<int> result = Result.Ok(666);
            var casted = result.Cast<bool>(true);

            Assert.True(casted.IsOk);
            Assert.True(casted.Value);
        }

        [Fact]
        public void CastTypedResult_Error()
        {
            IResult<int> result = Result.Error<int>();
            var casted = result.Cast<bool>();

            Assert.False(casted.IsOk);
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

        // [Fact]
        // public void PositiveThenPositive()
        // {
        //     IResult result = Result.Ok().Then(_ => Result.Ok());
        //     Assert.True(result.IsOk);
        //     Assert.Empty(result.Errors);
        // }
        //
        // [Fact]
        // public void PositiveThenNegative()
        // {
        //     IResult result = Result.Ok().Then(_ => Result.Error(ErrorAppString));
        //     Assert.False(result.IsOk);
        //     Assert.Single(result.Errors, err => err.Message == ErrorAppString);
        // }
        //
        // [Fact]
        // public void NegativeThen_ShouldFail()
        // {
        //     IResult result = Result.Error().Then(_ => Assert.Fail("This should not be executed"));
        //     Assert.False(result.IsOk);
        //     Assert.Empty(result.Errors);
        // }
    }
}
