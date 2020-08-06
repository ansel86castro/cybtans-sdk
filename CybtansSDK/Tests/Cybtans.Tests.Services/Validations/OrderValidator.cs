using Cybtans.Tests.Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services.Validations
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description can not be empty");
            RuleFor(x => x.OrderStateId).GreaterThan(0);               
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<OrderDto> context, CancellationToken cancellation = default)
        {            
            return base.ValidateAsync(context, cancellation);
        }
    }
}
