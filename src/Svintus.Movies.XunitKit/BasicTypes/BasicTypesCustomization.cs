using AutoFixture;

namespace Svintus.Movies.XunitKit.BasicTypes;

public class BasicTypesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }
}