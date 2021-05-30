using Cybtans.Tests.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Tests.Validations
{

    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Value).NotNull().SetValidator(new OrderValidator());
        }
    }

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
