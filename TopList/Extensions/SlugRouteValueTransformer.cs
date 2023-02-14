using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using TopList.Repo;

namespace TopList.Extensions
{
    public class SlugRouteValueTransformer : DynamicRouteValueTransformer
    {
        private readonly IRepository<TopList.Entity.Base.Entity> _entityRepository;

        public SlugRouteValueTransformer(IRepository<TopList.Entity.Base.Entity> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public override async ValueTask<RouteValueDictionary?> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var requestPath = httpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(requestPath) && requestPath[0] == '/')
            {
                // Trim the leading slash
                requestPath = requestPath.Substring(1);
            }

            var entity = await _entityRepository
                .Query()
                .Include(x => x.EntityType)
                .FirstOrDefaultAsync(x => x.Slug == requestPath);

            if (entity == null)
            {
                return null;
            }

            return new RouteValueDictionary
            {
                { "area", entity.EntityType.AreaName },
                { "controller", entity.EntityType.RoutingController },
                { "action", entity.EntityType.RoutingAction },
                { "id", entity.EntityId }
            };
        }
    }
}
