using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SportClub.DAL.EF;

namespace SportClub.BLL.Infrastructure
{
    public static class SportClubContextExtensions
    {
        public static void AddSportClubContext(this IServiceCollection services, string connection)
        {
            services.AddDbContext<SportClubContext>(options => options.UseSqlServer(connection));
        }
    }
}
