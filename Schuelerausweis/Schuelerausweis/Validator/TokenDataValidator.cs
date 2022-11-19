using FluentValidation;
using Schuelerausweis.Models;

namespace Schuelerausweis.Validator;

public class TokenDataValidator : AbstractValidator<TokenData>
{
    public TokenDataValidator()
    {
        RuleFor(x => x.Id);
        RuleFor(x => x.Token).
    }
}