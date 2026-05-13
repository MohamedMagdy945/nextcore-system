using Basket.Application.GerpcService;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Basket.Infrastructure
{
    public static class InfrastructureRegistrationService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            services.AddScoped<IBasketRepository, BasketRepository>();

            var discountUrl = configuration.GetValue<string>("GrpcSettings:DiscountUrl");

            services.AddScoped<DiscountGrpcSerivce>();

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(cfg =>
            {
                cfg.Address = new Uri(discountUrl!);
            });

            return services;
        }
    }
}