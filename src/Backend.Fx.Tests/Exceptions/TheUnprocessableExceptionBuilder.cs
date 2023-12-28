using Backend.Fx.Exceptions;
using JetBrains.Annotations;
using Xunit;

namespace Backend.Fx.Tests.Exceptions
{
    public class TheUnprocessableExceptionBuilder
    {
        [Fact]
        public void AddsExceptionWhenAggregateIsNull()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.AddNotFoundWhenNull<SomeEntity>(1111, null!);
            Assert.Throws<UnprocessableException>(() => sut.Dispose());
        }

        [Fact]
        public void AddsNoExceptionWhenAggregateIsNotNull()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.AddNotFoundWhenNull(1111, new SomeEntity());
            sut.Dispose();
        }

        [Fact]
        public void DoesNotThrowExceptionWhenNotAddingConditionalError()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.AddIf(false, "something is broken");
            sut.Dispose();
        }

        [Fact]
        public void ThrowsExceptionWhenAddingConditionalError()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.AddIf(true, "something is broken");
            Assert.Throws<UnprocessableException>(() => sut.Dispose());
        }
        
        [Fact]
        public void ThrowsExceptionWhenAddingConditionalKeyedError()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.AddIf("the key", true, "something is broken");
            Assert.Throws<UnprocessableException>(() => sut.Dispose());
        }

        [Fact]
        public void ThrowsExceptionWhenAddingError()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.Add("something is broken");
            Assert.Throws<UnprocessableException>(() => sut.Dispose());
        }
        
        [Fact]
        public void ThrowsExceptionWhenAddingKeyedError()
        {
            IExceptionBuilder sut = UnprocessableException.UseBuilder();
            sut.Add("theKey", "something is broken");
            Assert.Throws<UnprocessableException>(() => sut.Dispose());
        }

        [UsedImplicitly]
        private class SomeEntity
        {
            [UsedImplicitly]
            public int Id { get; }
        }
    }
}