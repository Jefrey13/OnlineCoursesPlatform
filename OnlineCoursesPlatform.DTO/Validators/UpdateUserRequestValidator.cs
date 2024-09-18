using FluentValidation;
using OnlineCoursesPlatform.DTO.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.DTO.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDTO>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");
        }
    }
}
