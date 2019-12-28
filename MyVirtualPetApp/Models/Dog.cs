using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Models
{
    /// <summary>
    /// A Dog as a possible instance of an Animal.
    /// </summary>
    public class Dog : Animal
    {
        public Dog()
        {
            animalType = AnimalType.Dog;
        }

        protected override int GetHungerIncreaseFactor()
        {
            return 2;
        }

        protected override int GetHappyDecreaseFactor()
        {
            return 2;
        }
    }
}
