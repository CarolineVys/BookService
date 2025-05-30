using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using BookService.Configuration;
using BookService.Repositories;
using BookService.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var secret = await AmazonSecretManager.GetSecretsFromAWS("BookService/test");

builder.Configuration.AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(secret))).Build();
var connectionString = builder.Configuration.GetConnectionString("BookService");
builder.Services.AddDbContext<BookContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddHealthChecks();//.AddDbContextCheck<BookContext>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("BookService"));
builder.Services.AddScoped(cfg => cfg.GetService<IOptions<AppSettings>>().Value);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKey"], builder.Configuration["AWS:SecretKey"]);
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .Build();
    });
});


builder.Services.AddScoped<IAwsService, AwsService>();
builder.Services.AddScoped<IBookService, BookService.Services.BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Logging.AddAWSProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;

    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseCors();

//app.UseMiddleware<BookService.Configuration.ExceptionHandlerMiddleware>();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
