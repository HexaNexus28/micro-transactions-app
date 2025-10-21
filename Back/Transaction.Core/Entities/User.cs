using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Entities
{
    [Table("Users")]
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }=string.Empty;

        [Required, StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = string.Empty;

        public virtual ICollection<AuthToken> AuthTokens { get; set; } = [];

        public virtual ICollection<Transact> Transactions { get; set; } = [];


    }
}
