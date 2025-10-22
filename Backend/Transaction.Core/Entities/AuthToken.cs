using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Entities
{
    [Table("AuthTokens")]
    public class AuthToken
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EmissionDate { get; set; } 

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }


        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public virtual User User { get; set; } = null!;


    }
}
