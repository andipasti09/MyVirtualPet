using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Models
{
    /// <summary>
    /// A Parrot is an instance of an <c>Animal</c>. It gets unhappy pretty fast, so it needs a lot of attention.
    /// </summary>
    public class Parrot : Animal
    {
        public Parrot()
        {
            this.animalType = AnimalType.Parrot;
        }

        protected override int GetHappyDecreaseFactor()
        {
            return 3;
        }

        protected override int GetHungerIncreaseFactor()
        {
            return 1;
        }
    }
}
