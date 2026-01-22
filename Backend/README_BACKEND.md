# Backend Documentation - ASP.NET Core API

Documentation complète du backend pour l'application Micro-Transactions RPG.

---

## Architecture Backend

### Clean Architecture Pattern
```
┌─────────────────────────────────────────────────────────┐
│                   Presentation Layer                      │
│                 (Controllers + DTOs)                     │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                   Business Layer                         │
│                 (Services + Logic)                       │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                    Core Layer                            │
│           (Entities + Interfaces + Config)              │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                    Data Layer                            │
│           (DbContext + Repositories + UoW)              │
└─────────────────────────────────────────────────────────┘
```

### Stack Technique
- **ASP.NET Core 8** - Framework web moderne
- **Entity Framework Core 9** - ORM base de données
- **SQL Server LocalDB** - Base de données
- **AutoMapper** - Mapping objets
- **JWT Bearer** - Authentification
- **Swagger/OpenAPI** - Documentation API

---

## Structure des Projets

### Transaction.API - Presentation Layer
```
Transaction.API/
├── Controllers/          # API Controllers
│   ├── UserController.cs
│   ├── TransactionController.cs
│   ├── ItemController.cs
│   └── AuthTokenController.cs
├── Program.cs           # Configuration startup
├── appsettings.json    # Configuration app
└── Migrations/          # Database migrations
```

### Transaction.Core - Core Layer (Interfaces & Entities)
```
Transaction.Core/
├── Entities/            # Domain entities (pures)
│   ├── User.cs
│   ├── Transaction.cs
│   ├── Item.cs
│   └── AuthToken.cs
├── DTOs/               # Data Transfer Objects
│   ├── Request/
│   └── Response/
├── Interfaces/         # Contrats (interfaces uniquement)
│   ├── Services/        # Interfaces services métier
│   │   ├── IUserService.cs
│   │   ├── ITransactionService.cs
│   │   └── IItemService.cs
│   └── Repositories/     # Interfaces repositories
│       ├── IUserRepository.cs
│       ├── ITransactionRepository.cs
│       └── IItemRepository.cs
├── Configuration/      # Settings classes
│   └── JwtSettings.cs
├── Services/           # Services utilitaires uniquement
│   └── JwtService.cs   # Service JWT (utilitaire)
└── Mapping/            # AutoMapper profiles
```

### Transaction.Business - Business Layer (Logic Implementation)
```
Transaction.Business/
└── Services/           # Implémentation services métier
    ├── UserService.cs      # Logique métier utilisateur
    ├── TransactionService.cs  # Logique métier transactions
    ├── ItemService.cs      # Logique métier items
    └── AuthTokenService.cs  # Logique métier tokens
```

### Transaction.Data - Data Layer (Persistence)
```
Transaction.Data/
├── Context/            # EF Core DbContext
│   └── AppDbContext.cs
├── Repositories/       # Implémentation repositories
│   ├── UserRepository.cs
│   ├── TransactionRepository.cs
│   ├── ItemRepository.cs
│   └── AuthTokenRepository.cs
├── UnitOfWork/         # Unit of Work pattern
│   └── UnitOfWork.cs
└── DTOs/               # Internal DTOs
```

---

## 🗄️ Base de Données

### Entity Framework Core Configuration
```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Transaction.API")));
```

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MicroTransactionDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Entités Principales
```csharp
// User Entity
[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public virtual ICollection<AuthToken> AuthTokens { get; set; } = [];
    public virtual ICollection<Transact> Transactions { get; set; } = [];
}
```

### Migrations
```bash
# Créer migration
dotnet ef migrations add InitialCreate

# Mettre à jour base de données
dotnet ef database update

# Générer script SQL
dotnet ef migrations script
```

---

## 🔐 JWT Authentication

### Configuration JWT
```csharp
// Program.cs
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});
```

### JWT Service
```csharp
public interface IJwtService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
```

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "ThisIsMySecretKeyForJwtToken2024VerySecureKeyThatIsLongEnough",
    "Issuer": "TransactionAPI",
    "Audience": "TransactionClient",
    "ExpirationMinutes": 60
  }
}
```

---

## 🌐 API Controllers

### UserController
```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto dto)
    {
        var result = await _userService.CreateUserAsync(dto);
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _userService.LoginWithTokenAsync(dto);
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Authorize] // Protégé par JWT
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
    }
}
```

### TransactionController
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Toutes les routes protégées
public class TransactionController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _transactService.CreateTransactionAsync(dto);
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetTransactionsByUserId(int id)
    {
        var result = await _transactService.GetTransactionsByUserIdAsync(id);
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
    }
}
```

---

## 🔄 Services & Business Logic

### Flow d'Architecture
```
📱 API Controller
       ↓
🏢 Business Layer (Services Implementation)
       ↓
🔌 Core Layer (Interfaces)
       ↓
💾 Data Layer (Repositories Implementation)
```

### UserService (Business Layer)
```csharp
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;     // Data Layer
    private readonly IMapper _mapper;            // Core
    private readonly IJwtService _jwtService;    // Core (utilitaire)

    public async Task<ApiResponse<UserResponseDto>> LoginWithTokenAsync(LoginRequestDto loginDto)
    {
        // 1. Utiliser repository (Data Layer)
        var user = (await _unitOfWork.Users.FindAsync(u => u.Email == loginDto.Email)).FirstOrDefault();
        
        // 2. Logique métier (Business Layer)
        if (user == null)
            return ApiResponse<UserResponseDto>.ErrorResponse("User not found", 404);

        // 3. Utiliser service core (JWT)
        var token = _jwtService.GenerateToken(user);
        
        // 4. Mapper et retourner
        var userDto = _mapper.Map<UserResponseDto>(user);
        userDto.Token = token;
        
        return ApiResponse<UserResponseDto>.SuccessResponse(userDto, "Login successful");
    }
}
```

### Pattern Repository + Unit of Work
```csharp
// Generic Repository
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

// Unit of Work
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITransactionRepository Transactions { get; }
    IItemRepository Items { get; }
    IAuthTokenRepository AuthTokens { get; }
    
    Task<int> SaveChangesAsync();
}
```

---

## 📦 DTOs & Mapping

### Request DTOs
```csharp
// Login Request
public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

// Register Request
public class RegisterRequestDto
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
```

### Response DTOs
```csharp
public class UserResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public List<AuthTokenResponseDto> AuthTokens { get; set; } = [];
    public List<TransactResponseDto> Transactions { get; set; } = [];
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    
    public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
    {
        return new ApiResponse<T> { Success = true, Data = data, Message = message };
    }
    
    public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
    {
        return new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}
```

### AutoMapper Configuration
```csharp
// Program.cs
builder.Services.AddAutoMapper(
    typeof(Program).Assembly,
    typeof(UserMappingProfile).Assembly,
    typeof(TransactionMappingProfile).Assembly
);

// Mapping Profile
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());
        
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}
```

---

## 🛡️ Sécurité & Validation

### Data Annotations
```csharp
public class TransactionRequestDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<int> ItemIds { get; set; } = new();
}
```

### Custom Validation
```csharp
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value as string;
        if (string.IsNullOrEmpty(email))
            return ValidationResult.Success;

        // Vérifier unicité en base de données
        // Implementation...
        
        return ValidationResult.Success;
    }
}
```

### CORS Configuration
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

app.UseCors("DevelopmentPolicy");
```

---

## 📊 Monitoring & Logging

### Serilog Configuration
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

### Health Checks
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddCheck("Database", new SqlConnectionHealthCheck(connectionString));

app.MapHealthChecks("/health");
```

---

## 🚀 Déploiement & Configuration

### Environment Variables
```bash
# Production
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=prod-server;Database=ProdDB;
JwtSettings__SecretKey=super-secret-production-key
JwtSettings__Issuer=ProductionAPI
JwtSettings__Audience=ProductionClient
```

### Docker Configuration
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Transaction.API/Transaction.API.csproj", "Transaction.API/"]
COPY ["Transaction.Core/Transaction.Core.csproj", "Transaction.Core/"]
COPY ["Transaction.Business/Transaction.Business.csproj", "Transaction.Business/"]
COPY ["Transaction.Data/Transaction.Data.csproj", "Transaction.Data/"]
RUN dotnet restore "Transaction.API/Transaction.API.csproj"
COPY . .
WORKDIR "/src/Transaction.API"
RUN dotnet build "Transaction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transaction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transaction.API.dll"]
```

---

## 🧪 Tests & Qualité

### Tests Unitaires
```csharp
[Test]
public async Task Login_ValidCredentials_ReturnsToken()
{
    // Arrange
    var loginDto = new LoginRequestDto { Email = "test@test.com", Password = "password" };
    _mockUnitOfWork.Setup(x => x.Users.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
        .ReturnsAsync(new List<User> { new User { Id = 1, Email = "test@test.com", PasswordHash = "password" } });

    // Act
    var result = await _userService.LoginWithTokenAsync(loginDto);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.IsNotNull(result.Data?.Token);
}
```

### Integration Tests
```csharp
public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsOk()
    {
        var loginDto = new { Email = "test@test.com", Password = "password" };
        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/user/login", content);

        response.EnsureSuccessStatusCode();
    }
}
```

---

## 🔄 Bonnes Pratiques

### Code Organization
- **Single Responsibility**: Chaque classe a une seule responsabilité
- **Dependency Injection**: Injection via constructeur
- **Interface Segregation**: Interfaces spécifiques et petites
- **Don't Repeat Yourself**: Éviter la duplication de code

### Error Handling
```csharp
public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
{
    try
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return ApiResponse<UserResponseDto>.NotFoundResponse($"User with ID {id} not found");

        var userDto = _mapper.Map<UserResponseDto>(user);
        return ApiResponse<UserResponseDto>.SuccessResponse(userDto);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving user {UserId}", id);
        return ApiResponse<UserResponseDto>.ErrorResponse("An error occurred", 500);
    }
}
```

### Async/Await Patterns
```csharp
// Toujours utiliser async/await ensemble
public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(RegisterRequestDto dto)
{
    var user = _mapper.Map<User>(dto);
    await _unitOfWork.Users.AddAsync(user);
    await _unitOfWork.SaveChangesAsync();
    
    var userDto = _mapper.Map<UserResponseDto>(user);
    return ApiResponse<UserResponseDto>.SuccessResponse(userDto);
}
```

---

## 📈 Performance & Optimisations

### Entity Framework Optimizations
```csharp
// Query optimisation
public async Task<User?> GetUserWithTransactionsAsync(int userId)
{
    return await _context.Users
        .Include(u => u.Transactions)
            .ThenInclude(t => t.Items)
        .FirstOrDefaultAsync(u => u.Id == userId);
}

// NoTracking pour les lectures
public async Task<IEnumerable<User>> GetAllUsersAsync()
{
    return await _context.Users
        .AsNoTracking()
        .ToListAsync();
}
```

### Caching Strategy
```csharp
public class CachedUserService : IUserService
{
    private readonly IMemoryCache _cache;
    private readonly IUserService _userService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
    {
        string cacheKey = $"user_{id}";
        
        if (_cache.TryGetValue(cacheKey, out UserResponseDto? cachedUser))
        {
            return ApiResponse<UserResponseDto>.SuccessResponse(cachedUser!);
        }

        var result = await _userService.GetUserByIdAsync(id);
        if (result.Success && result.Data != null)
        {
            _cache.Set(cacheKey, result.Data, _cacheDuration);
        }

        return result;
    }
}
```

---

## 🔍 Debugging & Troubleshooting

### Common Issues & Solutions

#### JWT Token Issues
```csharp
// Vérifier configuration JWT
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero // Important pour les tests
};
```

#### Database Connection Issues
```bash
# Vérifier connection string
dotnet ef dbcontext info

# Recréer base de données
dotnet ef database drop
dotnet ef database update
```

#### CORS Issues
```csharp
// Développer: autoriser toutes origines
// Production: spécifier origines autorisées
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", builder =>
    {
        builder.WithOrigins("https://frontend.example.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});
```

---

## 🎯 Roadmap Backend

### Court Terme
- [ ] **Password Hashing** avec BCrypt
- [ ] **Refresh Tokens** pour sessions prolongées
- [ ] **Rate Limiting** contre attaques
- [ ] **Input Validation** avancée

### Moyen Terme
- [ ] **Role-Based Access Control** (RBAC)
- [ ] **Audit Logging** complet
- [ ] **Background Jobs** avec Hangfire
- [ ] **Caching distribué** avec Redis

### Long Terme
- [ ] **Microservices** architecture
- [ ] **Event Sourcing** pattern
- [ ] **GraphQL** API alternative
- [ ] **gRPC** pour communication services

---

*Documentation maintenue par l'équipe backend. Dernière mise à jour: Janvier 2024*
