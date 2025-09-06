using FluentValidation;
using SmiTo.Application.DTOs;

namespace SmiTo.Application.Validators;

public class CreatURLRequestValidator : AbstractValidator<CreateURLRequest>
{
    public CreatURLRequestValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty().WithMessage("Original URL is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Original URL must be a valid absolute URL.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.")
            .When(x => x.ExpiresAt.HasValue);
    }
}
