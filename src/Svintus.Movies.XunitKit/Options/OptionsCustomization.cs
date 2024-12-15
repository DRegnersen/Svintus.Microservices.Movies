using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.Extensions.Options;

namespace Svintus.Movies.XunitKit.Options;

public class OptionsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new FilteringSpecimenBuilder(new OptionsBuilder(), new OptionsSpecification()));
    }
}

file class OptionsSpecification : IRequestSpecification
{
    public bool IsSatisfiedBy(object request)
    {
        return request is Type { IsGenericType: true } type && typeof(IOptions<>) == type.GetGenericTypeDefinition();
    }
}