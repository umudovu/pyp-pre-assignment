using Microsoft.EntityFrameworkCore;
using pyp_pre_assignment.Data;

namespace pyp_pre_assignment.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
