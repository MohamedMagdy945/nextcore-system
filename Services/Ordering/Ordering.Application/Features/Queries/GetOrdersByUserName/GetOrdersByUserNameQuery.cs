using Mapster;
using MediatR;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Queries.GetOrdersByUserName
{
    public record GetOrdersByUserNameQuery(string UserName) : IRequest<IList<OrderResponse>>;
    public class GetOrdersByUserNameHandler : IRequestHandler<GetOrdersByUserNameQuery, IList<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByUserNameHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IList<OrderResponse>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserNameAsync(request.UserName);
            return orderList.Adapt<IList<OrderResponse>>();
        }
    }
}
