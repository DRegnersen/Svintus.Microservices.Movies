using AutoFixture.Kernel;
using Moq;

namespace Svintus.Movies.XunitKit.AutoFixture.Moq;

internal interface IMockConfigurator
{
    void Configure(Mock mock, ISpecimenContext context);
}