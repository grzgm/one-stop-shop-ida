using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models
{
    public class UserItem
    {
        [Key]
        [StringLength(255)]
        public string MicrosoftId { get; set; }
        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(64)]
        public string Surname { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
    }
}
