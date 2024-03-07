using MAAI.ScriptAI;
using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Mappers.Identity.User;
using MAContracts.Contracts.Mappers.Movie;
using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.AI;
using MAContracts.Contracts.Services.Identity;
using MAContracts.Contracts.Services.Identity.User;
using MAContracts.Contracts.Services.Movie;
using MADTOs.Mappers;
using MAModels.EntityFrameworkModels;
using MAModels.EntityFrameworkModels.Identity;
using MAServices.Mappers;
using MAServices.Mappers.Identity.User;
using MAServices.Mappers.Movie;
using MAServices.Services;
using MAServices.Services.AI;
using MAServices.Services.identity;
using MAServices.Services.Identity.User;
using MAServices.Services.Movie;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region CONNECTION TO DATABASE

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MainDbConnection")
        )
);

#endregion

#region DIPENDENCIES ON MOVIE ADVISOR SERVICES

#region TRANSIENT SERVICES

builder.Services.AddTransient<IMovieServices, MovieServices>();

builder.Services.AddTransient<IMultimediaServices, MultimediaServices>();

builder.Services.AddTransient<IReviewServices, ReviewServices>();

builder.Services.AddTransient<ITagServices, TagServices>();

builder.Services.AddTransient<IFileServices, FileServices>();

builder.Services.AddTransient<IObjectsMapperDtoServices, ObjectsMapperDtoServices>();

builder.Services.AddTransient<IMovieDtoObjectsMapper, MovieDtoObjectsMapper>();

builder.Services.AddTransient<IUserDtoObjectsMapper, UserDtoObjectsMapper>();

builder.Services.AddTransient<IReviewDtoObjectsMapper, ReviewDtoObjectsMapper>();

builder.Services.AddTransient<ITagDtoObjectsMapper, TagDtoObjectsMapper>();

builder.Services.AddTransient<IImageDtoObjectsMapper, ImageDtoObjectsMapper>();

#endregion

#region SINGLETON SERVICES

#endregion

#region SCOPED SERVICES

builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<IRecommendationServices, RecommendationServices>();

builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();

#endregion

#endregion

#region IDENTITY

builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(tokenOptions =>
{
    tokenOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])),
    };
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

#endregion

#region CONFIGURE CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:4200", "https://accounts.google.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()        
    );
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
