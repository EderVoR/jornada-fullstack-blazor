using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories
{
	public class GetCategoryByIdEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
			=> app.MapGet("/{id}", HandlerAsync)
				.WithName("Categories: Get By Id")
				.WithSummary("Retorna Categoria por Id")
				.WithDescription("Retorna Categoria por Id")
				.WithOrder(4)
				.Produces<Response<Category?>>();

		private static async Task<IResult> HandlerAsync(ICategoryHandler handler, long id)
		{

			var request = new GetCategoryByIdRequest
			{
				UserId = ApiConfiguration.UserId,
				Id = id
			};
				
			var result = await handler.GetBtyIdAsync(request);
			return result.IsSuccess
				? TypedResults.Ok(result)
				: TypedResults.BadRequest(result);

		}
	}
}
