using FluentValidation;

namespace Krisma.Application.UseCases.Developers.Commands.UpdateDev;

public class UpdateDeveloperCommandValidator : AbstractValidator<UpdateDeveloperCommand>
{
    public UpdateDeveloperCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id del desarrollador es obligatorio.");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("El Id de la organización es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");

        RuleFor(x => x.GitHubUserId)
            .GreaterThan(0).WithMessage("El ID de usuario de GitHub debe ser mayor a 0.");

        RuleFor(x => x.GitHubNodeId)
            .NotEmpty().WithMessage("El Node ID de GitHub es obligatorio.")
            .MaximumLength(100).WithMessage("El Node ID no puede exceder los 100 caracteres.");

        RuleFor(x => x.GitHubLogin)
            .NotEmpty().WithMessage("El usuario de GitHub es obligatorio.")
            .MaximumLength(39).WithMessage("El usuario de GitHub no puede exceder los 39 caracteres.")
            .Matches("^[A-Za-z0-9](?:[A-Za-z0-9-]{0,37}[A-Za-z0-9])?$").WithMessage("El formato del usuario de GitHub no es válido.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido.")
            .MaximumLength(254).WithMessage("El correo no puede exceder los 254 caracteres.");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("La fecha de contratación no puede estar en el futuro.");

        RuleFor(x => x.Seniority)
            .IsInEnum().WithMessage("El nivel de seniority proporcionado no es válido.");

        RuleFor(x => x.Position)
            .IsInEnum().WithMessage("La posición proporcionada no es válida.");

        RuleFor(x => x.Department)
            .IsInEnum().WithMessage("El departamento proporcionado no es válido.");
    }
}