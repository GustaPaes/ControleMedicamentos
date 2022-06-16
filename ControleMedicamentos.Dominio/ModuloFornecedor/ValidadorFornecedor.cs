using FluentValidation;
using FluentValidation.Validators;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class ValidadorFornecedor : AbstractValidator<Fornecedor>
    {
        public ValidadorFornecedor()
        {
            RuleFor(x => x.Nome)
                 .NotNull().WithMessage("Deve ser inserido um nome")
                 .NotEmpty().WithMessage("Deve ser inserido um nome");

            RuleFor(x => x.Email)
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Deve conter um email válido")
                .NotNull().WithMessage("Deve ser inserido um email")
                .NotEmpty().WithMessage("Deve ser inserido um email");

            RuleFor(x => x.Telefone)
                .MinimumLength(9).WithMessage("Deve ser inserido um telefone com no mínimo 9 caracteres")
                .NotNull().WithMessage("Deve ser inserido um número")
                .NotEmpty().WithMessage("Deve ser inserido um nome");


            RuleFor(x => x.Cidade)
                .MinimumLength(4).WithMessage("A cidade deve conter pelo menos 4 caracteres")
                .NotNull().WithMessage("Deve ser inserido uma cidade")
                .NotEmpty().WithMessage("Deve ser inserido uma cidade");

            RuleFor(x => x.Estado)
                .MinimumLength(3).WithMessage("O estado deve conter pelo menos 3 caracteres")
                .NotNull().WithMessage("Deve ser inserido um estado")
                .NotEmpty().WithMessage("Deve ser inserido um estado");
        }
    }
}
