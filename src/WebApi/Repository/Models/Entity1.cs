using System.ComponentModel.DataAnnotations;

namespace EfProblemSample.WebApi.Repository.Models
{
    public class Entity1
    {
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public string AUniqueProperty { get; set; }

        [Required, MaxLength(50)]
        public string SomeProperty { get; set; }
    }
}