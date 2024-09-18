using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.DTO.ResponseDTO
{
    public class UserResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
