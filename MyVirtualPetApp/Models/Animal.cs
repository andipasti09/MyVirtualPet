using System;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualPet.Models
{
    /// <summary>
    /// An abstract class to represent an Animal owned by a user. The real animals are classes inherited from this one.
    /// Animals have several metrics like Hunger which increases over time, as well as happiness decreasing. 
    /// The animal can be interacted with to enhance those metrics.
    /// </summary>
    /// <remarks>
    /// Hunger could have named repletion to have "increasing" and "decreasing" of both features the same effect, but would be harder to understand. 
    /// Hunger is more obvious; so beware the opposite meaning of those values
    /// </remarks>
    public abstract class Animal
    {
        /// <summary>
        /// An enum defining all possible types of animals that could be created
        /// </summary>
        public enum AnimalType
        {
            Cat = 0,
            Dog = 1,
            Parrot = 2
        }

        // Defines the range between -10 to +10 where hunger and happiness can move
        private const int MAX_MIN_RANGE = 10;

        // the factor how much an action like stroking/feeding restores the animal's index
        // for simplicity this is set to a constant
        private const int REFRESH_FACTOR = 2;

        // both of the animal's metrics
        protected int hungerIndex = 0;
        protected int happyIndex = 0;

        /// <summary>
        /// The animal's current hunger value. Minus values are good.
        /// </summary>
        public int Hunger { get { return hungerIndex; } }

        /// <summary>
        /// The animal's current happy value. Positive values are good.
        /// </summary>
        public int Happy { get { return happyIndex; } }

        /// <summary>
        /// Unique identifier along all animals
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The User this animal belongs to
        /// </summary>
        [Required]
        public ulong UserId { get; set; }

        private string name;

        /// <summary>
        /// The animal needs to have a non blank name.
        /// </summary>
        [Required]
        public string Name 
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Not a valid name");
                }
                name = value;
            }
        }

        protected AnimalType animalType;

        /// <summary>
        /// The type of animal as string representation. See <see cref="AnimalType"/>
        /// </summary>
        public string AnimalTypus
        {
            get { return animalType.ToString(); }
        }

        public void DecreaseHappiness()
        {
            happyIndex = Math.Max(-MAX_MIN_RANGE, happyIndex - GetHappyDecreaseFactor());
        }

        public void IncreaseHappiness()
        {
            happyIndex = Math.Min(MAX_MIN_RANGE, happyIndex + REFRESH_FACTOR);
        }

        public void IncreaseHunger()
        {
            hungerIndex = Math.Min(MAX_MIN_RANGE, hungerIndex + GetHungerIncreaseFactor());
        }

        public void DecreaseHunger()
        {
            hungerIndex = Math.Max(-MAX_MIN_RANGE, hungerIndex - REFRESH_FACTOR);
        }

        protected abstract int GetHungerIncreaseFactor();

        protected abstract int GetHappyDecreaseFactor();

    }
}
