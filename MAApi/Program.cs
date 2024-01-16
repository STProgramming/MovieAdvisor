using MAAI;
using MAAI.Interfaces;
using MAAI.ScriptAI;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using MAServices.MovieServices;
using MAServices.Services;
using Microsoft.EntityFrameworkCore;

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

#region CREATE NEW DIPENDENCIES ON MOVIE ADVISOR SERVICES

builder.Services.AddTransient<IMovieServices, MovieServices>();

builder.Services.AddTransient<IReviewServices, ReviewServices>();

builder.Services.AddTransient<ITagServices, TagServices>();

builder.Services.AddTransient<IFileServices, FileServices>();

builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<IMAAIRecommender, MAAIRecommender>();

builder.Services.AddHostedService<MABSCore>();

#endregion

#region CONFIGURE CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: MyAllowSpecificOrigins,
      builder => {
          builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
      });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

app.Run();
