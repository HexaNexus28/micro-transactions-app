using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Entities
{
    [Table("Transactions")]
    public  class Transact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; } = [];

       
    }
}
