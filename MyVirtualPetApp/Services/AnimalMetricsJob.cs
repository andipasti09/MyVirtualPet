using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyVirtualPet.Models;

namespace MyVirtualPet.Services
{
    /// <summary>
    /// This background service calculates the metrics of all available animals in the service
    /// </summary>
    /// <remarks>This services creates a running thread to calculate the animals' metrics.
    /// That means one thread handles all animals.
    /// Another approach would have been to use a scheduler like Quartz.NET</remarks>
    public class AnimalMetricsJob : BackgroundService
    {
        private readonly ILogger<AnimalMetricsJob> logger;

        private IAnimalService animalService;

        public AnimalMetricsJob(ILogger<AnimalMetricsJob> logger, IAnimalService animalService)
        {
            this.logger = logger;
            this.animalService = animalService;
        }

        public Task CalculateAllAnimalMetrics(List<Animal> animalsList, CancellationToken cancelToken)
        {
            return Task.Run(() => {

                foreach (Animal pet in animalsList)
                {
                    pet.DecreaseHappiness();
                    pet.IncreaseHunger();
                    animalService.UpdateAnimal(pet);
                }
                logger.LogInformation("Finished {0} animals to calculate",
                                  animalsList.Count);

                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }
                // use any kind of sleep value to slower down the calculation process
                Thread.Sleep(10 * 1000);
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Animal async background service started");
            while (!stoppingToken.IsCancellationRequested)
            {
                // handover cancel token to shut down gracefully
                await CalculateAllAnimalMetrics(animalService.GetAllAnimals(), stoppingToken);
                
            }
        }
    }
}
