using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Svintus.Movies.XunitKit.BasicTypes;
using Svintus.Movies.XunitKit.Options;

namespace Svintus.Movies.XunitKit.AutoFixture.Moq;

public class AutoMoqDataAttribute() : AutoDataAttribute(() => new Fixture().Customize(
    new CompositeCustomization(
        new AutoMoqCustomization { ConfigureMembers = false },
        new OptionsCustomization(),
        new BasicTypesCustomization()
    )
));