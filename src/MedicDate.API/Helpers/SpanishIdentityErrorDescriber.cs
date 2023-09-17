using System.Reflection;
using MedicDate.API.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace MedicDate.API.Helpers;

public class SpanishIdentityErrorDescriber : IdentityErrorDescriber
{
  private readonly IStringLocalizer _localizer;

  public SpanishIdentityErrorDescriber(IStringLocalizerFactory factory)
  {
    var type = typeof(SharedResource);
    var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName ?? "");
    _localizer = factory.Create("SharedResource", assemblyName.Name ?? "");
  }

  public override IdentityError DuplicateEmail(string email)
  {
    return new IdentityError()
    {
      Code = nameof(DuplicateEmail),
      Description = string.Format(_localizer["Email {0} is already taken."], email)
    };
  }

  public override IdentityError DefaultError()
  {
    return new IdentityError
    {
      Code = nameof(DefaultError),
      Description = _localizer["An unknown failure has occurred."]
    };
  }

  public override IdentityError PasswordMismatch()
  {
    return new IdentityError
    {
      Code = nameof(PasswordMismatch),
      Description = _localizer["Incorrect password."]
    };
  }

  public override IdentityError LoginAlreadyAssociated()
  {
    return new IdentityError
    {
      Code = nameof(LoginAlreadyAssociated),
      Description = _localizer["A user with this login already exists."]
    };
  }

  public override IdentityError InvalidUserName(string? userName)
  {
    return new IdentityError
    {
      Code = nameof(InvalidUserName),
      Description = string.Format(
        _localizer["User name {0} is invalid, can only contain letters or digits."],
        userName
      )
    };
  }

  public override IdentityError InvalidEmail(string? email)
  {
    return new IdentityError
    {
      Code = nameof(InvalidEmail),
      Description = string.Format(_localizer["Email {0} is invalid."], email)
    };
  }

  public override IdentityError DuplicateUserName(string userName)
  {
    return new IdentityError
    {
      Code = nameof(DuplicateUserName),
      Description = string.Format(_localizer["User Name {0} is already taken."], userName)
    };
  }

  //public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(_localizer["Email {0} is already taken."] , email) }; }
  public override IdentityError InvalidRoleName(string? role)
  {
    return new IdentityError
    {
      Code = nameof(InvalidRoleName),
      Description = string.Format(_localizer["Role name {0} is invalid."], role)
    };
  }

  public override IdentityError DuplicateRoleName(string role)
  {
    return new IdentityError
    {
      Code = nameof(DuplicateRoleName),
      Description = string.Format(_localizer["Role name {0} is already taken."], role)
    };
  }

  public override IdentityError UserAlreadyHasPassword()
  {
    return new IdentityError
    {
      Code = nameof(UserAlreadyHasPassword),
      Description = _localizer["User already has a password set."]
    };
  }

  public override IdentityError UserAlreadyInRole(string role)
  {
    return new IdentityError
    {
      Code = nameof(UserAlreadyInRole),
      Description = string.Format(_localizer["User already in role {0}."], role)
    };
  }

  public override IdentityError PasswordTooShort(int length)
  {
    return new IdentityError
    {
      Code = nameof(PasswordTooShort),
      Description = string.Format(_localizer["Passwords must be at least {0} characters."], length)
    };
  }

  public override IdentityError PasswordRequiresNonAlphanumeric()
  {
    return new IdentityError
    {
      Code = nameof(PasswordRequiresNonAlphanumeric),
      Description = _localizer["Passwords must have at least one non alphanumeric character."]
    };
  }

  public override IdentityError PasswordRequiresDigit()
  {
    return new IdentityError
    {
      Code = nameof(PasswordRequiresDigit),
      Description = _localizer["Passwords must have at least one digit ('0'-'9')."]
    };
  }

  public override IdentityError PasswordRequiresLower()
  {
    return new IdentityError
    {
      Code = nameof(PasswordRequiresLower),
      Description = _localizer["Passwords must have at least one lowercase ('a'-'z')."]
    };
  }

  public override IdentityError PasswordRequiresUpper()
  {
    return new IdentityError
    {
      Code = nameof(PasswordRequiresUpper),
      Description = _localizer["Passwords must have at least one uppercase ('A'-'Z')."]
    };
  }
}
