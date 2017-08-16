using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding.Models
{
    public class Guest {
        [Key]
        public int GuestId {get;set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int WeddingId {get;set;}
        public Wedding Wedding {get;set;}
        
        public int UserId {get;set;}
        public User User {get;set;}

    }
}