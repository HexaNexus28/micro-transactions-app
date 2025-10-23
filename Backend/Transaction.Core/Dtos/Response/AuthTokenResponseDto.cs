using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Dtos.Response
{
    public class AuthTokenResponseDto
    {
        public DateTime EmissionDate { get; set; } = DateTime.UtcNow;

        public DateTime ExpirationDate {  get; set; } 


        public AuthTokenResponseDto (DateTime EmissionDate, DateTime ExpirationDate)
        {
            ExpirationDate = EmissionDate.AddHours(1);
        }
    }
}
