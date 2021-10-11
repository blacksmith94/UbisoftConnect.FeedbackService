using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using UbisoftConnect.Configurations;
using Microsoft.OpenApi.Models;
using Autofac;
using UbisoftConnect.SqlDataAccess;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UbisoftConnect.WebAPI.Middleware;

namespace UbisoftConnect.WebAPI
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private string binPath;
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="env"> Host environment, will be used to determine the service configuration </param>
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
						 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
						 .AddEnvironmentVariables();
			binPath = System.IO.Directory.GetParent(typeof(Program).Assembly.Location).FullName;
			configuration = builder.Build();
		}

		/// <summary>
		/// Use this method to add services into the container.
		/// </summary>
		/// <param name="services"> IServiceCollection used for the registration of services </param>
		public void ConfigureServices(IServiceCollection services)
		{
			var serviceConfiguration = configuration.GetSection("FeedbackServiceConfiguration").Get<FeedbackServiceConfiguration>();
			services.AddSingleton(serviceConfiguration);

			//All validators will be registered automatically (Fluent Validation)
			services.AddMvc()
					.AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>())
					.AddNewtonsoftJson();

			//Add Swagger documentation, this will let the developers see the API documentation in an intertactive web application
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Ubisoft Connect Feedback Service",
					Version = "v1",
					Description = "This service allows users to rate their game session by giving a score from 1 to 5, they can also leave a comment"
				});
			});

			//Add automapper service, this will let developers define DTO <-> Model conversion in a scalable way.
			services.AddAutoMapper(typeof(Startup));

			services.AddControllers();

			//Add DbContext using SQLite
			services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
			{
				var dbSourceText = "Data Source=";
				var dbName = serviceConfiguration.DatabaseConnection.Replace(dbSourceText, "", StringComparison.OrdinalIgnoreCase);
				var databaseLocation = System.IO.Path.Combine(this.binPath, dbName);
				optionsBuilder.UseSqlite($"{dbSourceText}{databaseLocation}");

			}, ServiceLifetime.Transient);

			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new Module.WebAPI());
		}

		/// <summary>
		/// This method configures the HTTP request pipeline and further configurations.
		/// </summary>
		/// <param name="app"> Application builder used to customize the request pipeline </param>
		/// <param name="env"> Host environment will determine how to initialize the DB </param>
		/// <param name="dbContext"> The database context, will be used to initialize the DB </param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext dbContext)
		{
			//Check db integrity
			if (env.IsEnvironment("Test"))
			{
				dbContext.Database.EnsureDeleted();
			}
			dbContext.Database.EnsureCreated();

			//Middleware
			if (env.IsEnvironment("Debug"))
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseMiddleware<ExceptionHandler>();
			//app.UseExceptionHandler("/Error");
			app.UseRouting();
			app.UseEndpoints(e => e.MapControllers());

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ubisoft Connect");
			});
		}
	}
}
