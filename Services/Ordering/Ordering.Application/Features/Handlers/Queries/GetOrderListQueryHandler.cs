using Mapster;
using MediatR;
using Ordering.Application.Features.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Handlers.Queries
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, IList<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderListQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IList<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserNameAsync(request.UserName);
            return orderList.Adapt<IList<OrderResponse>>();
        }
    }
}
