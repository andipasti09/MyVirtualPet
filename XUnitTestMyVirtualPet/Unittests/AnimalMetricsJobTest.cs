using Microsoft.Extensions.Logging;
using Moq;
using MyVirtualPet.Models;
using MyVirtualPet.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestMyVirtualPet
{
    public class AnimalMetricsJobTest
    {
        AnimalMetricsJob underTest;

        Mock<IAnimalService> animalService;

        public AnimalMetricsJobTest()
        {
            var logger = new Mock<ILogger<AnimalMetricsJob>>();
            animalService = new Mock<IAnimalService>();
            underTest = new AnimalMetricsJob(logger.Object, animalService.Object);
        }

        [Fact]
        public void TestCalculatingAnimals()
        {
            List<Animal> animals = createAnimalList();
            Task task = underTest.CalculateAllAnimalMetrics(animals, new CancellationToken());

            task.Wait();

            foreach (Animal pet in animals) {
                Assert.True(pet.Happy < 0);
                Assert.True(pet.Hunger > 0);
            }
            
        }

        private List<Animal> createAnimalList()
        {
            List<Animal> list = new List<Animal>();
            list.Add(new Parrot());
            list.Add(new Cat());
            list.Add(new Dog());
            return list;
        }
    }
}
