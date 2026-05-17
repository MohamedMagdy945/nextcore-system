using MediatR;

namespace Ordering.Application.Features.Commands
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteOrderCommand(int id)
        {
            Id = id;
        }
    }
}
