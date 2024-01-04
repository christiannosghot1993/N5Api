using Microsoft.EntityFrameworkCore;
using RepositoryPatternDesign;
using RepositoryPatternDesign.Models;
using UnitOfWorkPatternDesign;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<N5CodeChallengeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
