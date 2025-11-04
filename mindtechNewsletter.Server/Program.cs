using Microsoft.EntityFrameworkCore;
using mindtechNewsletter.Server.Data;
using mindtechNewsletter.Server.Repositories;
using mindtechNewsletter.Server.Services;

var builder = WebApplication.CreateBuilder(args);

//banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=newsletter.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

//autoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//repository e service
builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
builder.Services.AddScoped<ISubscriberService, SubscriberService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
