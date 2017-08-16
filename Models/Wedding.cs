using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding.Models
{
    public class Wedding {
        [Key]
        public int WeddingId {get;set;}
        
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage="No special characters.")]
        [Display(Name="Wedder One")]
        public string WedderOne {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage="No special characters.")]
        [Display(Name="Wedder Two")]
        public string WedderTwo {get;set;}

        [Required]
        [Display(Name="Wedding Date")]
        [FutureDate]
        [DataType(DataType.Date)]
        public DateTime WeddingDate {get;set;}

        [Required]
        [Display(Name="Address")]
        [RegularExpression(@"^[0-9A-Za-z\s-#]+$", ErrorMessage="No special characters.")]
        public string WeddingAddress {get;set;}

        [Required]
        [Display(Name="City")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage="Letters only.")]
        public string WeddingCity {get;set;}

        [Required]
        [Display(Name = "State")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage="State must be in 2-letter abbreviated form.")]
        public string WeddingState {get;set;}

        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int UserId {get;set;}
        public User User {get;set;}

        public List<Guest> Guests {get;set;}

        public Wedding() {
            Guests = new List<Guest>();
        }

    }
}