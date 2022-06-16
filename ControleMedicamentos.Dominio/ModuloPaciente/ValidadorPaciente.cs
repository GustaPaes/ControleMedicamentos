using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class ValidadorPaciente : AbstractValidator<Paciente>
    {
        public ValidadorPaciente()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Deve ser inserido um nome")
                .NotEmpty().WithMessage("Deve ser inserido um nome");

            RuleFor(x => x.CartaoSUS)
                .MinimumLength(8).WithMessage("O cartão do SUS deve ter pelo menos 8 dígitos")
                .NotNull().NotEmpty();

            RuleFor(x => Regex.IsMatch(x.CartaoSUS, "[^0-9]+", RegexOptions.IgnoreCase))
                .NotEqual(true)
                .WithMessage("O cartão do SUS só pode conter números");


        }
    }
}
