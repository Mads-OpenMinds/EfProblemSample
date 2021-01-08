using System.ComponentModel.DataAnnotations;

namespace EfProblemSample.WebApi.Repository.Models
{
    public class Entity2
    {
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public string SomeOtherProperty { get; set; }
    }
}
