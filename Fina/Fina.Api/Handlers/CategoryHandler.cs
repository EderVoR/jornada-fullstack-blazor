using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
	public class CategoryHandler(AppDbContext _context) : ICategoryHandler
	{
		public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
		{
			var category = new Category
			{
				UserId = request.UserId,
				Title = request.Title,
				Description = request.Description
			};

			try
			{
				await _context.Categories.AddAsync(category);
				await _context.SaveChangesAsync();

				return new Response<Category?>(category, 201, "Categoria criada com sucesso.");
			}
			catch (Exception ex)
			{
				return new Response<Category?>(null, 401, "Erro ao registrar categoria.");
			}
		}

		public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
		{
			try
			{
				var category = await _context.Categories
					.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Id == request.Id);

				if (category == null)
					return new Response<Category?>(null, 404, "Categoria não localizado");

				_context.Categories.Remove(category);
				await _context.SaveChangesAsync();

				return new Response<Category?>(category, message: "Categoria excluida com sucesso.");

			}
			catch (Exception ex)
			{
				return new Response<Category?>(null, 500, "Cadastro não pode ser excluido");
			}
		}

		public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
		{
			try
			{
				var query = _context.Categories
					.AsNoTracking()
					.Where(x => x.UserId == request.UserId)
					.OrderBy(x => x.Title);

				var category = await query
					.Skip((request.PageNumber - 1) * request.PageSize)
					.Take(request.PageSize)
					.ToListAsync();

				var count = await query.CountAsync();

				return new PagedResponse<List<Category>?>(category, count, request.PageNumber, request.PageSize);
			}
			catch(Exception ex)
			{
				return new PagedResponse<List<Category>?>(null, 500, "Erro ao localizar as categorias");
			}
		}

		public async Task<Response<Category?>> GetBtyIdAsync(GetCategoryByIdRequest request)
		{
			try
			{
				var category = await _context.Categories
					.AsNoTracking()
					.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

				return category is null
					? new Response<Category?>(null, 404, "Categoria não localizada")
					: new Response<Category?>(category);
			}
			catch(Exception ex)
			{
				return new Response<Category?>(null, 500, "Não foi possivel localizar a categoia");
			}
		}

		public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
		{
			try
			{
				var category = await _context.Categories
					.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

				if (category == null)
					return new Response<Category?>(null, 404, "Categoria não encontrada.");

				category.Title = request.Title;
				category.Description = request.Description;

				_context.Categories.Update(category);
				await _context.SaveChangesAsync();
				
				return new Response<Category?>(category, message: "Categoria atualizada.");
			}
			catch(Exception ex)
			{
				return new Response<Category?>(null, 401, "Erro ao atualizar a categoria");
			}
		}
	}
}
