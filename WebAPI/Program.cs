using Infrastructure.Mappers;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using Infrastructure.Mongo.Models;
using Infrastructure.Mongo.Repositories;
using Infrastructure.Mongo.Sevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using System.Text;
using WebAPI.Mapper;
using WebAPI.Services;
using Infrastructure.Mongo.Services;
using Infrastructure.Mongo;
using Microsoft.AspNetCore.HttpLogging;
using Infrastructure.EF.Models;
using Infrastructure.EF.Repositories;
using Infrastructure.EF.Services;
using Infrastructure.EF.Sevices;
using System.Transactions;
using Infrastructure.EF.Context;
using WebAPI.Hubs;

public class RequestBodyStoringMiddleware
{
    private readonly RequestDelegate _next;

    public RequestBodyStoringMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Request.EnableBuffering();
        string body;
        using (var streamReader = new System.IO.StreamReader(
            httpContext.Request.Body, System.Text.Encoding.UTF8, leaveOpen: true))
            body = await streamReader.ReadToEndAsync();

        httpContext.Request.Body.Position = 0;

        httpContext.Items["body"] = body;
        await _next(httpContext);
    }
}

public partial class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Computer Shot API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
              {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
                });
        });

        builder.Services.AddSignalR();

        //builder.Services.AddScoped<IUserService, UserServiceMongo>();
        //builder.Services.AddScoped<IComputerService, ComputerServiceMongo>();
        //builder.Services.AddScoped<IOrderService, OrderServiceMongo>();

        //builder.Services.AddSingleton<IPaypalService, PaypalService>();
        //builder.Services.AddScoped<IPasswordAlgorithm, Hashing>();
        //builder.Services.AddScoped<WebAPI.Services.IAuthorizationService, AuthorizationService>();
        //builder.Services.AddScoped<IEmailService, EmailService>();

        ////builder.Services.AddAutoMapper(typeof(MapperEF));
        //builder.Services.AddAutoMapper(typeof(MapperMongo));
        //builder.Services.AddAutoMapper(typeof(MapperProfile));

        //builder.Services.AddScoped<IBaseRepository<ComputerMongo, ObjectId?>, ComputerMongoRepository>();
        //builder.Services.AddScoped<IBaseRepository<UserMongo, ObjectId?>, UserMongoRepository>();
        //builder.Services.AddScoped<IBaseRepository<OrderMongo, ObjectId?>, OrderMongoRepository>();
        //builder.Services.AddScoped<ITransactionManager, TransactionManagerMongo>();

        builder.Services.AddHttpClient();

        builder.Services.AddHttpLogging(options => // <--- Setup logging
        {
            // Specify all that you need here:
            options.LoggingFields = HttpLoggingFields.RequestHeaders |
                                    HttpLoggingFields.RequestBody |
                                    HttpLoggingFields.ResponseHeaders |
                                    HttpLoggingFields.ResponseBody;

        });


        //builder.Services.AddLogging(opt =>
        //{
        //    opt.ClearProviders();
        //    opt.AddConsole();
        //    opt.SetMinimumLevel(LogLevel.Trace);
        //});

        //ef 
        builder.Services.AddScoped<IUserService, UserServiceEF>();
        builder.Services.AddScoped<IComputerService, ComputerServiceEF>();
        builder.Services.AddScoped<IOrderService, OrderServiceEF>();

        builder.Services.AddSingleton<IPaypalService, PaypalService>();
        builder.Services.AddScoped<IPasswordAlgorithm, Hashing>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddAutoMapper(typeof(MapperEF));
        builder.Services.AddAutoMapper(typeof(MapperProfile));

        builder.Services.AddScoped<IBaseRepository<ComputerEntity, int>, ComputerEntityRepository>();
        builder.Services.AddScoped<IBaseRepository<UserEntity, int>, UserEntityRepository>();
        builder.Services.AddScoped<IBaseRepository<OrderEntity, int>, OrderEntityRepository>();
        builder.Services.AddScoped<ITransactionManager, TransactionManagerEF>();

        //ef database
        builder.Services.AddDbContext<DataContext>();

        //mongo database
        //builder.Services.Configure<MongoDbSettings>(
        //    builder.Configuration.GetSection("MongoDb"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateIssuer = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                 .WithOrigins("http://localhost:3000")
                    //.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseMiddleware<RequestBodyStoringMiddleware>();

        //app.UseRouting();
        app.UseCors();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseHttpLogging();

        app.MapHub<OrderHub>("/orderHub");

        app.Run();
    }
}
