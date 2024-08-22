﻿using Fina.Api.Data;
using Fina.Api.Handlers;
using Fina.Core;
using Fina.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Api
{
	public static class BuilderExtension
	{
		public static void AddConfiguration(this WebApplicationBuilder builder)
		{
			ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DbConnection") ?? string.Empty;
			Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
			Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
		}

		public static void AddDocumentation(this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(x =>
			{
				// Cria os nomes de entidades para o nomespace
				x.CustomSchemaIds(n => n.FullName);
			});
		}

		public static void AddDataContext(this WebApplicationBuilder builder)
		{
			builder.Services
				.AddDbContext<AppDbContext>(x =>
					{
						x.UseNpgsql(ApiConfiguration.ConnectionString);
					});			
		}

		//Configuração do CORS
		public static void AddCrossOrigin(this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(
				options => options.AddPolicy(
					ApiConfiguration.CorsPolicyName,
						policy => policy
							.WithOrigins([
								Configuration.BackendUrl,
								Configuration.FrontendUrl
								])
							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials()
					));
		}

		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services
				.AddTransient<ICategoryHandler, CategoryHandler>();
			builder.Services
				.AddTransient<ITransactionHandler, TransactionHandler>();
		}
	}
}
