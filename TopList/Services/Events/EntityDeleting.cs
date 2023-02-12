using MediatR;

namespace TopList.Services.Events
{
    public class EntityDeleting : INotification
    {
        public long EntityId { get; set; }
    }
}
