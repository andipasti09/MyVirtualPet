using MyVirtualPet.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyVirtualPet.Controllers
{
    /// <summary>
    /// This is a dto for creating an animal.
    /// </summary>
    /// <remarks>As animal is an abstract class, a instantiation of this class is not possible</remarks>
    [DataContract]
    public class AnimalRequest
    {
        [DataMember(IsRequired = true)]
        [Required]
        public Animal.AnimalType Type { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public ulong UserId { get; set; }
    }
}
