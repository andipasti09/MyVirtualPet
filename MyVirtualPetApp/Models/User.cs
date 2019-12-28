using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MyVirtualPet.Models
{
    /// <summary>
    /// Represents a person with internal ID and public account name that can own several animals to play with.
    /// The relation between user and animal is modelled in class <c>Animal</c>.
    /// </summary>
    [DataContract]
    public class User
    {
        [DataMember(Name ="ID")]
        public ulong ID { get; set; }

        private string accountName;

        [DataMember(Name ="accountName", IsRequired = true)]
        [Required]
        public String AccountName
        {
            get { return accountName;  }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("name must not be null or empty!");
                }
                accountName = value;
            }
        }
    }
}
