using FluentValidation.AspNetCore;
using AutoMapper;
using DynamicObjectAPI.Core.Mappings;
using DynamicObjectAPI.Data;
using DynamicObjectAPI.Service.Services.Abstract;
using DynamicObjectAPI.Service.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using DynamicObjectAPI.Data.Repositories.Abstract;
using DynamicObjectAPI.Data.Repositories;
using DynamicObjectAPI.Core.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<DynamicObjectDtoValidator>());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDynamicObjectRepository, DynamicObjectRepository>();
builder.Services.AddScoped<IDynamicObjectService, DynamicObjectService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
