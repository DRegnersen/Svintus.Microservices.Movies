using AutoFixture.Kernel;
using Microsoft.Extensions.Options;
using Moq;
using Svintus.Movies.XunitKit.AutoFixture.Moq;

namespace Svintus.Movies.XunitKit.Options;

internal class OptionsBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        if (request is not Type { IsGenericType: true } optionsType)
        {
            throw new ArgumentException("Requested type is not a generic type", nameof(request));
        }

        var argumentType = optionsType.GetGenericArguments().Single();

        var mockType = typeof(Mock<>).MakeGenericType(optionsType);
        var mock = (Mock)context.Resolve(mockType);

        var mockConfigurationType = MakeMockConfigurationType(optionsType, mockType, argumentType);
        var mockConfigurator = (IMockConfigurator?)Activator.CreateInstance(mockConfigurationType);

        mockConfigurator!.Configure(mock, context);
        return mock.Object;
    }

    private static Type MakeMockConfigurationType(Type optionsType, Type mockType, Type argumentType)
    {
        if (typeof(IOptions<>) == optionsType.GetGenericTypeDefinition())
        {
            return typeof(OptionsMockConfigurator<,>).MakeGenericType(mockType, argumentType);
        }

        throw new NotSupportedException($"Options type {optionsType} is not supported");
    }
}