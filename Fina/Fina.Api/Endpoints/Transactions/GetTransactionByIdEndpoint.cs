using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions
{
	public class GetTransactionByIdEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
			=> app.MapGet("/{id}", HandlerAsync)
				.WithName("Transactions: Get By Id")
				.WithSummary("Retorna uma transação")
				.WithDescription("Retorna uma transação")
				.WithOrder(4)
				.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandlerAsync(ITransactionHandler handler, long id)
		{
			var transaction = new GetTransactionByIdRequest
			{
				Id = id,
				UserId = ApiConfiguration.UserId,
			};

			var result = await handler.GetByIdAsync(transaction);
			return result.IsSuccess
				? TypedResults.Ok(result)
				: TypedResults.BadRequest(result);
		}
	}
}
