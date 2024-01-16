using MAModels.EntityFrameworkModels;
using Microsoft.Extensions.Hosting;

namespace MAAI
{
    public class MABSCore : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private static List<User> Users = new List<User>();

        public MABSCore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {  
                await Task.Delay(new TimeSpan(3, 0, 0));
            }
        }
    }
}
