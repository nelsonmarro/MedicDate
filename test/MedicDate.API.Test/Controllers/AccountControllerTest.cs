using System.Net;
using System.Text.Json;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Domain.ApplicationServices;
using MedicDate.Domain.ApplicationServices.IApplicationServices;
using MedicDate.Domain.DomainServices;
using MedicDate.Domain.DomainServices.IDomainServices;
using MedicDate.Shared.Models.Auth;
using MedicDate.Test.Shared;
using MedicDate.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace MedicDate.API.Controllers;

public class AccountControllerTest : BaseDbTest
{
  private readonly Mock<IEmailSender> _emailSenderMock = new();

  private readonly LoginRequestDto _invalidLoginRequest =
    new() { Email = "nelsonmarro66@gmail.com", Password = "ln145hh23*" };

  private readonly ITestOutputHelper _iTestOutputHelper;

  private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock = new();

  private readonly RegisterUserDto _newUser =
    new()
    {
      Nombre = "Nelson",
      Apellidos = "Marro",
      Email = "nelsonmarro66@gmail.com",
      Password = "Nelson123*",
      ConfirmPassword = "Nelson123*"
    };

  private readonly LoginRequestDto _validLoginRequest =
    new() { Email = "nelsonmarro66@gmail.com", Password = "Nelson123*" };

  private IAccountService? _accountService;

  private RoleManager<AppRole>? _roleManager;

  private AccountController? _sut;

  private ITokenBuilderService? _tokenBuilderService;

  private UserManager<ApplicationUser>? _userManager;

  private AccountControllerTest(ITestOutputHelper iTestOutputHelper)
  {
    _iTestOutputHelper = iTestOutputHelper;
    _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
    _emailSenderMock = new Mock<IEmailSender>();
  }

  private async Task<ActionResult> CreateUserAsync(string dbName)
  {
    _sut = await BuildAccountControllerAsync(dbName);
    return await _sut.RegisterAsync(_newUser);
  }

  // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Shared/MockHelpers.cs
  // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Identity.Test/SignInManagerTest.cs.

  private async Task<AccountController> BuildAccountControllerAsync(string dbName)
  {
    var context = BuildDbContext(dbName);

    var userStore = new UserStore<
      ApplicationUser,
      AppRole,
      ApplicationDbContext,
      string,
      IdentityUserClaim<string>,
      ApplicationUserRole,
      IdentityUserLogin<string>,
      IdentityUserToken<string>,
      IdentityRoleClaim<string>
    >(context);

    var serviceProvider = BuildServiceProviderForUserManager();
    _userManager = BuildUserManager(userStore, serviceProvider);

    _tokenBuilderService = new TokenBuilderService(_userManager);

    var httpContext = new DefaultHttpContext();
    MockAuth(httpContext);

    var signInManager = SetupSignInManager(_userManager, httpContext);

    var doctorRole = new AppRole { Name = Sd.ROLE_DOCTOR };
    var roleStore = new RoleStore<AppRole>(context);
    _roleManager = BuildRoleManager(roleStore);
    await _roleManager.CreateAsync(doctorRole);
    _newUser.RolesIds = new List<string> { doctorRole.Id };

    var jwtSettings = new JwtSettings
    {
      ExpiryInMinutes = "60",
      SecretKey = "@NcRfUjXn2r5u8x/A?D(G+KaPdSgVkYp3s6v9y$B&E)H@McQeThWmZq4t7w!z%C*",
      ValidAudience = "https://localhost:5005",
      ValidIssuer = "MedicDateAPI"
    };

    _jwtSettingsMock.Setup(x => x.Value).Returns(jwtSettings);

    _accountService = new AccountService(
      _userManager,
      signInManager,
      _roleManager,
      _tokenBuilderService,
      _jwtSettingsMock.Object,
      _emailSenderMock.Object
    );

    return new AccountController(_accountService);
  }

  private IServiceProvider BuildServiceProviderForUserManager()
  {
    var config = new ConfigurationBuilder().Build();
    var services = new ServiceCollection();

    services.AddIdentity<ApplicationUser, AppRole>().AddDefaultTokenProviders();
    return services.BuildServiceProvider();
  }

  private UserManager<TUser> BuildUserManager<TUser>(
    IUserStore<TUser>? store = null,
    IServiceProvider? serviceProvider = null
  )
    where TUser : class
  {
    store ??= new Mock<IUserStore<TUser>>().Object;
    var options = new Mock<IOptions<IdentityOptions>>();
    var idOptions = new IdentityOptions { Lockout = { AllowedForNewUsers = false } };

    options.Setup(o => o.Value).Returns(idOptions);

    var userValidators = new List<IUserValidator<TUser>>();

    var validator = new Mock<IUserValidator<TUser>>();
    userValidators.Add(validator.Object);
    var pwdValidators = new List<PasswordValidator<TUser>> { new() };

    var userManager = new UserManager<TUser>(
      store,
      options.Object,
      new PasswordHasher<TUser>(),
      userValidators,
      pwdValidators,
      new UpperInvariantLookupNormalizer(),
      new IdentityErrorDescriber(),
      serviceProvider!,
      new Mock<ILogger<UserManager<TUser>>>().Object
    );

    validator
      .Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
      .Returns(Task.FromResult(IdentityResult.Success))
      .Verifiable();

    var fakeUserTokenProvider = new Mock<IUserTwoFactorTokenProvider<TUser>>();

    fakeUserTokenProvider
      .Setup(
        x => x.CanGenerateTwoFactorTokenAsync(It.IsAny<UserManager<TUser>>(), It.IsAny<TUser>())
      )
      .ReturnsAsync(true);

    fakeUserTokenProvider
      .Setup(
        x => x.GenerateAsync(It.IsAny<string>(), It.IsAny<UserManager<TUser>>(), It.IsAny<TUser>())
      )
      .ReturnsAsync(string.Empty);

    fakeUserTokenProvider
      .Setup(
        x =>
          x.ValidateAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<UserManager<TUser>>(),
            It.IsAny<TUser>()
          )
      )
      .ReturnsAsync(false);

    userManager.RegisterTokenProvider("Default", fakeUserTokenProvider.Object);

    return userManager;
  }

  private SignInManager<TUser> SetupSignInManager<TUser>(
    UserManager<TUser> manager,
    HttpContext context,
    ILogger? logger = null,
    IdentityOptions? identityOptions = null,
    IAuthenticationSchemeProvider? schemeProvider = null
  )
    where TUser : class
  {
    var contextAccessor = new Mock<IHttpContextAccessor>();
    contextAccessor.Setup(a => a.HttpContext).Returns(context);
    identityOptions ??= new IdentityOptions();
    var options = new Mock<IOptions<IdentityOptions>>();
    options.Setup(a => a.Value).Returns(identityOptions);
    var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);
    schemeProvider ??= new Mock<IAuthenticationSchemeProvider>().Object;
    var sm = new SignInManager<TUser>(
      manager,
      contextAccessor.Object,
      claimsFactory,
      options.Object,
      null!,
      schemeProvider,
      new DefaultUserConfirmation<TUser>()
    );
    sm.Logger = logger ?? new Mock<ILogger<SignInManager<TUser>>>().Object;
    return sm;
  }

  private Mock<IAuthenticationService> MockAuth(HttpContext context)
  {
    var auth = new Mock<IAuthenticationService>();
    context.RequestServices = new ServiceCollection()
      .AddSingleton(auth.Object)
      .BuildServiceProvider();

    return auth;
  }

  private RoleManager<TRole> BuildRoleManager<TRole>(IRoleStore<TRole>? store = null)
    where TRole : class
  {
    store ??= new Mock<IRoleStore<TRole>>().Object;
    var roles = new List<IRoleValidator<TRole>> { new RoleValidator<TRole>() };
    return new RoleManager<TRole>(
      store,
      roles,
      new UpperInvariantLookupNormalizer(),
      new IdentityErrorDescriber(),
      null!
    );
  }

  public class RegisterAsync : AccountControllerTest
  {
    public RegisterAsync(ITestOutputHelper iTestOutputHelper)
      : base(iTestOutputHelper) { }

    [Fact]
    public async Task Should_create_an_User_and_assign_doctor_role()
    {
      //Arrange
      var dbName = Guid.NewGuid().ToString();

      //Act
      var result = await CreateUserAsync(dbName);
      var successResult = result as GenericActionResult;

      //Assert
      Assert.Equal(HttpStatusCode.OK, successResult?.HttpStatusCode);

      var user = await _userManager!.Users.AsNoTracking().FirstOrDefaultAsync();

      Assert.NotNull(user);

      var isInDoctorRole = await _userManager.IsInRoleAsync(user!, Sd.ROLE_DOCTOR);

      Assert.True(isInDoctorRole);
    }
  }

  public class LoginAsync : AccountControllerTest
  {
    public LoginAsync(ITestOutputHelper iTestOutputHelper)
      : base(iTestOutputHelper) { }

    [Fact]
    public async Task Should_return_propper_error_LoginResponseDto_when_Login_fails()
    {
      //Arrange
      var dbName = Guid.NewGuid().ToString();
      await CreateUserAsync(dbName);

      //Act
      var loginResult = await _sut!.LoginAsync(_invalidLoginRequest);
      var errorResult = loginResult.Result as GenericActionResult;
      var loginResultDto = JsonSerializer.Deserialize<LoginResponseDto>(
        errorResult?.ResponseBody ?? ""
      );

      //Assert
      Assert.Equal(HttpStatusCode.BadRequest, errorResult?.HttpStatusCode);
      Assert.False(loginResultDto?.IsAuthSuccessful);
      Assert.Equal("Inicio de sesión incorrecto.", loginResultDto?.ErrorMessage);
    }

    [Fact]
    public async Task Should_response_a_success_LoginResponseDto_when_the_user_credentials_are_valid()
    {
      //Arrange
      var dbName = Guid.NewGuid().ToString();
      await CreateUserAsync(dbName);

      //Act
      var loginResult = await _sut!.LoginAsync(_validLoginRequest);

      //Assert
      Assert.NotNull(loginResult.Value);
      Assert.NotNull(loginResult.Value?.Token);
      Assert.NotNull(loginResult.Value?.RefreshToken);
      Assert.True(loginResult.Value?.IsAuthSuccessful);
    }
  }
}
