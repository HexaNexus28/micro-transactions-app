using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Entities
{
    [Table("Items")]
     public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
       
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty ;

        [Required]
        public float Price { get; set; }

        public virtual ICollection<Transact> Transactions { get; set; } = [];

    }
}
