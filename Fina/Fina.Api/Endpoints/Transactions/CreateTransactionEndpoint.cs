using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions
{
	public class CreateTransactionEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
			=> app.MapPost("/", HandlerAsync)
				.WithName("Transactions: Create")
				.WithDescription("Cria uma transação")
				.WithSummary("Cria uma transação")
				.WithOrder(1)
				.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandlerAsync(ITransactionHandler handler, CreateTransactionRequest request)
		{
			request.UserId = ApiConfiguration.UserId;
			var result = await handler.CreateAsync(request);
			return result.IsSuccess
				? TypedResults.Created($"/{result.Data?.Id}", result)
				: TypedResults.BadRequest(result);
		}
	}
}
