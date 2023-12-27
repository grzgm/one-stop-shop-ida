using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models
{
    public class LunchTodayItem
    {
        [Key]
        [ForeignKey(nameof(UserItem))]
        [StringLength(255)]
        [Required]
        public string MicrosoftId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        [StringLength(45)]
        public string? Office { get; set; }
    }
    public class LunchTodayItemFrontend
    {
        public DateTime? RegistrationDate { get; set; }
        public string? Office { get; set; }

        public LunchTodayItemFrontend(DateTime? registrationDate, string office)
        {
            RegistrationDate = registrationDate;
            Office = office;
        }

        public LunchTodayItemFrontend(LunchTodayItem lunchTodayItem)
        {
            RegistrationDate = lunchTodayItem.RegistrationDate;
            Office = lunchTodayItem.Office;
        }
    }
}