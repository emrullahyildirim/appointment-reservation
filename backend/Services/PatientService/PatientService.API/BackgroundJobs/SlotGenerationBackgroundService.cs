using Business.Abstract;

namespace PatientService.API.BackgroundJobs
{
    public class SlotGenerationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SlotGenerationBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("SlotGenerationBackgroundService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var slotService = scope.ServiceProvider
                        .GetRequiredService<IAppointmentSlotService>();

                    await slotService.GenerateFutureSlotsAsync();
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "Slot generation failed");
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}
