﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.DTO.RequestDTO
{
    public class VerifyOtpRequestDTO
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
    }
}
