using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Models
{
    /// <summary>
    /// A cat is a possible instance of an <c>Animal</c>
    /// </summary>
    public class Cat : Animal
    {
        public Cat()
        {
            this.animalType = AnimalType.Cat;
        }

        protected override int GetHappyDecreaseFactor()
        {
            return 1;
        }

        protected override int GetHungerIncreaseFactor()
        {
            return 2;
        }
    }
}
