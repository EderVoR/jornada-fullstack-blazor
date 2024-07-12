using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions
{
	public class GetTransactionByPeriodEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
			=> app.MapGet("/", HandlerAsync)
				.WithName("Transactions: Get By Period")
				.WithSummary("Retorna uma transação")
				.WithDescription("Retorna uma transação")
				.WithOrder(5)
				.Produces<Response<Transaction>>();

		private static async Task<IResult> HandlerAsync(ITransactionHandler handler,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null,
		[FromQuery] int pageNumber = Configuration.DefaultPageNumber,
		[FromQuery] int pageSize = Configuration.DefaultPageSize)
		{
			var request = new GetTransactionByPeriodRequest
			{
				EndDate = endDate,
				StartDate = startDate,
				PageNumber = pageNumber,
				PageSize = pageSize,
				UserId = ApiConfiguration.UserId
			};

			var result = await handler.GetByPeriodAsync(request);
			return result.IsSuccess
				? TypedResults.Ok(result)
				: TypedResults.BadRequest(result);
		}
	}	
}
