using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Dtos.Response
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string Email {  get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public List<AuthTokenResponseDto> AuthTokens { get; set; } = [];

        public List<TransactResponseDto> Transactions { get; set; } = [];
    }
}
