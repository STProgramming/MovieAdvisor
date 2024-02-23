using MAAI.ScriptAI;
using MAContracts.Contracts.Mappers;
using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.movie;
using MADTOs.Mappers;
using MAModels.EntityFrameworkModels;
using MAServices.Mappers;
using MAServices.Services;
using MAServices.Services.movie;
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

#endregion

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
