﻿using TopList.Repo;
using MediatR;
using TopList.Services.Events;

namespace TopList.Services.BaseServices
{
    public class EntityService : IEntityService
    {
        private readonly IRepository<TopList.Entity.Base.Entity> _entityRepository;
        private readonly IMediator _mediator;

        public EntityService(IRepository<TopList.Entity.Base.Entity> entityRepository, IMediator mediator)
        {
            _entityRepository = entityRepository;
            _mediator = mediator;
        }

        public string ToSafeSlug(string slug, long entityId, string entityTypeId)
        {
            var i = 2;
            while (true)
            {
                var entity = _entityRepository.Query().FirstOrDefault(x => x.Slug == slug);
                if (entity != null && !(entity.EntityId == entityId && entity.EntityTypeId == entityTypeId))
                {
                    slug = string.Format("{0}-{1}", slug, i);
                    i++;
                }
                else
                {
                    break;
                }
            }

            return slug;
        }

        public TopList.Entity.Base.Entity Get(long entityId, string entityTypeId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
        }

        public void Add(string name, string slug, long entityId, string entityTypeId)
        {
            var entity = new TopList.Entity.Base.Entity
            {
                Name = name,
                Slug = slug,
                EntityId = entityId,
                EntityTypeId = entityTypeId
            };

            _entityRepository.Add(entity);
        }

        public void Update(string newName, string newSlug, long entityId, string entityTypeId)
        {
            var entity = _entityRepository.Query().First(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            entity.Name = newName;
            entity.Slug = newSlug;
        }

        public async Task Remove(long entityId, string entityTypeId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);

            if (entity != null)
            {
                await _mediator.Publish(new EntityDeleting { EntityId = entity.Id });
                _entityRepository.Remove(entity);
            }
        }
    }
}
