using Friendships.Write.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Friendships.Write.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}