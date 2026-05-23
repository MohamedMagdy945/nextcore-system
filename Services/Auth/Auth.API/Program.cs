using Auth.API.Configurations;
using Auth.Application;
namespace Auth.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCustomLocalization();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddApplicationServices();

            var app = builder.Build();

            app.UseCustomLocalization();

            //app.UseHttpsRedirection(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}