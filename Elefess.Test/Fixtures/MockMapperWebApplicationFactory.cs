using Elefess.TestHost.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Test;

public sealed class MockMapperWebApplicationFactory : MapperWebApplicationFactory<MockLfsOidMapper>
{
    protected override void AddMapper(IServiceCollection services) => services.AddOidMapper<MockLfsOidMapper>();
}