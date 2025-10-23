using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;

namespace Transaction.Core.Dtos.Request
{
    public  class TransactRequestDto
    {
        [DataType(DataType.DateTime)]
        public  DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public IEnumerable<Item> Items { get; set; } = [];
      

    }
}
