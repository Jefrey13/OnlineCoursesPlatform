﻿//using FluentValidation;
//using OnlineCoursesPlatform.DTO.RequestDTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OnlineCoursesPlatform.DTO.Validators
//{
//    public class LoginRequestValidator : AbstractValidator<LoginRequestDTO>
//    {
//        public LoginRequestValidator()
//        {
//            RuleFor(x => x.Email)
//                .NotEmpty().WithMessage("Email is required")
//                .EmailAddress().WithMessage("A valid email is required");

//            RuleFor(x => x.Password)
//                .NotEmpty().WithMessage("Password is required");
//        }
//    }
//}
