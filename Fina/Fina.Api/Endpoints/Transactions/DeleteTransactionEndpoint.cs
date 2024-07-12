using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions
{
	public class DeleteTransactionEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
			=> app.MapDelete("/", HandlerAsync)
				.WithName("Transactions: Delete")
				.WithSummary("Deleta uma transação")
				.WithDescription("Deleta uma transação")
				.WithOrder(3)
				.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandlerAsync(ITransactionHandler handler, long id)
		{
			var transaction = new DeleteTransactionRequest
			{
				Id = id,
				UserId = ApiConfiguration.UserId,
			};

			var result = await handler.DeleteAsync(transaction);
			return result.IsSuccess
				? TypedResults.Ok(result)
				: TypedResults.BadRequest(result);
		}
	}
}
