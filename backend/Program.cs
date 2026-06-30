using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Casino Emulator API",
        Version = "v1",
        Description = "REST API для казино-емулятора"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть JWT-токен, отриманий з ендпоінта /auth/login."
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Casino API v1");
    options.DocumentTitle = "Casino Emulator";
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Casino API V1");
        c.RoutePrefix = "swagger"; 
    });
}


app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

var welcome = app.Configuration["AppSettings:WelcomeMessage"];
var version = app.Configuration["AppSettings:Version"];
var projectName = app.Configuration["AppSettings:ProjectName"];
var slogan = app.Configuration["AppSettings:Slogan"];

app.Logger.LogInformation(
    "Запуск {Project} версії {Version} в середовищі {Env}",
    projectName,
    version,
    app.Environment.EnvironmentName
);


app.MapGet("/", () =>
{
    app.Logger.LogInformation("Обробка запиту на головну сторінку казино");

    return new
    {
        Project = projectName,
        Message = welcome,
        Slogan = slogan,
        Version = version
    };
});

app.MapPost("/auth/register", async (RegisterDto dto, AppDbContext db) =>
{
    if (await db.Users.AnyAsync(u => u.Name == dto.Name))
        return Results.Conflict("Користувач з таким іменем вже існує.");

    var user = new User
    {
        Name = dto.Name,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = "Player",
        Balance = 1000
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", new
    {
        user.Id,
        user.Name,
        user.Role,
        user.Balance
    });
})
.WithTags("Auth");

app.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, IConfiguration config) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Name == dto.Name);

    if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = CreateToken(user, config);

    return Results.Ok(new
    {
        access_token = token,
        token_type = "Bearer",
        player = user.Name,
        balance = user.Balance
    });
})
.WithTags("Auth");

app.MapGet("/auth/me", async (ClaimsPrincipal principal, AppDbContext db) =>
{
    var id = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

    var user = await db.Users.FindAsync(id);

    if (user == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        user.Id,
        user.Name,
        user.Role,
        user.Balance
    });
})
.RequireAuthorization()
.WithTags("Auth");

app.MapGet("/users", async (AppDbContext db) =>
    await db.Users.ToListAsync()
);
app.MapPost("/users", async (AppDbContext db, User user) =>
{
    user.Bets = new List<Bet>();
    if (user.Balance <= 0) user.Balance = 1000;

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
});

app.MapGet("/users/{id}", async (AppDbContext db, int id) =>
{
    var user = await db.Users.FindAsync(id);

    if (user == null)
        return Results.NotFound(new { message = "User not found" });

    return Results.Ok(user);
});

app.MapGet("/users/{id}/balance", async (AppDbContext db, int id) =>
{
    var user = await db.Users.FindAsync(id);

    if (user == null)
        return Results.NotFound("User not found");

    return Results.Ok(new { user.Id, user.Name, user.Balance });
});

app.MapPut("/users/{id}", async (int id, AppDbContext db, User updatedUser) =>
{
    var user = await db.Users.FindAsync(id);

    if (user == null)
        return Results.NotFound("User not found");

    user.Name = updatedUser.Name;

    await db.SaveChangesAsync();

    return Results.Ok(user);
});

app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);

    if (user == null)
        return Results.NotFound("User not found");

    db.Users.Remove(user);
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "User deleted" });
});

static string CreateToken(User user, IConfiguration config)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
    );

    var creds = new SigningCredentials(
        key,
        SecurityAlgorithms.HmacSha256
    );

    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.Run();

public record RegisterDto(string Name, string Password);
record LoginDto(string Name, string Password);