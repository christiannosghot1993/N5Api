using CqrsPatternDesign.Permisos;
using CqrsPatternDesign.PermisosCommand;
using CqrsPatternDesign.PermisosQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RepositoryPatternDesign;
using RepositoryPatternDesign.Models;
using System.Reflection;
using UnitOfWorkPatternDesign;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "N5 API REST", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<N5CodeChallengeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IPermisoCommandHandler), typeof(PermisoCommandHandler));
builder.Services.AddScoped(typeof(IPermisoQueryHandler), typeof(PermisoQueryHandler));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5 API REST");
    });
//}

app.UseCors(builder =>
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
