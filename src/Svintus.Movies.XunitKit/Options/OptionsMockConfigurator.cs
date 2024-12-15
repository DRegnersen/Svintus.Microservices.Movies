using AutoFixture.Kernel;
using Microsoft.Extensions.Options;
using Moq;
using Svintus.Movies.XunitKit.AutoFixture.Moq;

namespace Svintus.Movies.XunitKit.Options;

// ReSharper disable once UnusedTypeParameter
internal class OptionsMockConfigurator<TMock, TOptionsArgument> : IMockConfigurator
    where TMock : Mock<IOptions<TOptionsArgument>>
    where TOptionsArgument : class, new()
{
    public void Configure(Mock mock, ISpecimenContext context)
    {
        Configure((Mock<IOptions<TOptionsArgument>>)mock, context);
    }

    private static void Configure(Mock<IOptions<TOptionsArgument>> mock, ISpecimenContext context)
    {
        mock.SetupGet(m => m.Value).Returns((TOptionsArgument)context.Resolve(typeof(TOptionsArgument)));
    }
}