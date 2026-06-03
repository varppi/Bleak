using System.Runtime.CompilerServices;

namespace Bleak
{
    public static class ActionService 
    {
        public static void AddActionService(this IServiceCollection services)
        {
            services.AddScoped<Actions>();
        }
    }
}
