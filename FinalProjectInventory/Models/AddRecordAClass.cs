using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectInventory.Models
{
    public class AddRecordAClass
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public int Quantity { get; set; }

        public string IssuedBy { get; set; }

        public string VerifiedBy { get; set; }

        public DateTime DateSend { get; set; }

        public DateTime DateReceived { get; set; }

        [NotMapped]
        public IFormFile FileData { get; set; }

        public string FilePath { get; set; }
    }
}
