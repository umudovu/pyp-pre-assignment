using System.ComponentModel.DataAnnotations;

namespace pyp_pre_assignment.Dtos
{
    public class DataFilterDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? AcceptorEmail { get; set; }
        [Required]
        public Filter Filter { get; set; }
    }

    public enum Filter
    {
        Segment = 1,
        Country,
        Product,
        Discount
    }
}
