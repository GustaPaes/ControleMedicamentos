using FluentValidation;
using FluentValidation.Validators;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class ValidadorFuncionario : AbstractValidator<Funcionario>
    {
        public ValidadorFuncionario()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Deve ser inserido um nome")
                .NotEmpty().WithMessage("Deve ser inserido um nome");

            RuleFor(x => x.Login)
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Deve conter um email válido")
                .NotNull().NotEmpty();


            RuleFor(x => x.Senha)
                .MinimumLength(8).WithMessage("A senha deve conter pelo menos 8 caracteres")
                .NotNull().NotEmpty();

        }
    }
}
