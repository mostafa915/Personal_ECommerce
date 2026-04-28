using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Reviews
{
    public class ReviewRequestValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewRequestValidator()
        {
            RuleFor(x => x.Comment)
                .MaximumLength(1000);

            RuleFor(x => x.Rating)
                .GreaterThan(0);

            RuleFor(x => x.Rating)
                .LessThanOrEqualTo(5);
               
        }
    }
}
