using FluentValidation;
using DynamicObjectAPI.Core.DTOs;

namespace DynamicObjectAPI.Core.Validators
{
    public class DynamicObjectDtoValidator : AbstractValidator<DynamicObjectDto>
    {
        public DynamicObjectDtoValidator()
        {
            RuleFor(x => x.ObjectType)
                .NotEmpty().WithMessage("ObjectType is required.");

            RuleFor(x => x.Fields)
                .NotEmpty().WithMessage("Fields cannot be empty.")
                .Must(fields => fields != null && fields.All(f => !string.IsNullOrEmpty(f.Key) && f.Value != null))
                .WithMessage("Field keys and values must not be empty.");
        }
    }
}
