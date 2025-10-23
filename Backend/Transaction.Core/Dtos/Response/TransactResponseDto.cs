using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Dtos.Response
{
    public class TransactResponseDto
    {
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public List<ItemResponseDto> Items { get; set; } = [];


    }
}
