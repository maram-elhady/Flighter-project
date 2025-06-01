using Flighter.Services;


namespace Flighter.Helper
{
    public class RemoveExpiredBookingsService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveExpiredBookingsService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                    await paymentService.RemoveExpiredPayLaterAsync();
                    await paymentService.RemoveExpiredPayPendingAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
