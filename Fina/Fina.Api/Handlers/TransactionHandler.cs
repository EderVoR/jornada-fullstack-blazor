using Fina.Api.Data;
using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
	public class TransactionHandler(AppDbContext _context) : ITransactionHandler
	{
		public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
		{
			try
			{
				if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
					request.Amount *= -1;

				var transaction = new Transaction
				{
					Amount = request.Amount,
					CategoryId = request.CategoryId,
					CreatedAt = DateTime.Now,
					PaidOrReceivedAt = request.PaidOrReceiveAt,
					Title = request.Title,
					Type = request.Type,
					UserId = request.UserId
				};

				await _context.Transactions.AddAsync(transaction);
				await _context.SaveChangesAsync();

				return new Response<Transaction>(transaction, 201, message: "Trasação cadastrada com sucesso");
			}
			catch (Exception ex)
			{
				return new Response<Transaction?>(null, 500, "Não foi possivel criar uma transição");
			}
		}

		public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
		{
			try
			{
				var transaction = await _context.Transactions
					.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

				if (transaction == null)
					return new Response<Transaction?>(null, 404, "Transação não localizada");

				_context.Transactions.Remove(transaction);
				await _context.SaveChangesAsync();

				return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso.");
			}
			catch(Exception ex)
			{
				return new Response<Transaction?>(null, 500, "Não foi possivel excluir a transação");
			}
		}

		public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
		{
			try
			{
				var transaction = await _context.Transactions
					.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

				return transaction is null
					? new Response<Transaction?>(null, 404, "Transação não localizada")
					: new Response<Transaction?>(transaction);
			}
			catch(Exception ex)
			{
				return new Response<Transaction?>(null, 500, "Não foi possivel localizar a transação");
			}
		}

		public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
		{
			try
			{
				request.StartDate ??= DateTime.Now.GetFirstDay();
				request.EndDate ??= DateTime.Now.GetLastDay();

				var query = _context.Transactions
					.AsNoTracking()
					.Where(t => t.PaidOrReceivedAt >= request.StartDate &&
								t.PaidOrReceivedAt <= request.EndDate &&
								t.UserId == request.UserId)
					.OrderBy(x => x.PaidOrReceivedAt);

				var transaction = await query
										.Skip((request.PageNumber - 1) * request.PageSize)
										.Take(request.PageSize)
										.ToListAsync();

				var count = await query.CountAsync();

				return new PagedResponse<List<Transaction>?>(transaction, count, request.PageNumber, request.PageSize);
			}
			catch(Exception ex)
			{
				return new PagedResponse<List<Transaction>?>(null, 500, "Nenhum periodo ");
			}
		}

		public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
		{
			try
			{
				if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
					request.Amount *= -1;

				var transaction = await _context.Transactions
					.FirstOrDefaultAsync(f => f.Id == request.Id && f.UserId == request.UserId);

				if (transaction == null)
					return new Response<Transaction?>(null, 404, "Transação não localizada");

				transaction.CategoryId = request.CategoryId;
				transaction.Amount = request.Amount;
				transaction.Title = request.Title;
				transaction.Type = request.Type;
				transaction.PaidOrReceivedAt = request.PaidOrReceiveAt;

				_context.Transactions.Update(transaction);
				await _context.SaveChangesAsync();

				return new Response<Transaction?>(transaction);
			}
			catch(Exception ex)
			{
				return new Response<Transaction?>(null, 500, "Não foi possivel atualizar a transação");
			}
		}
	}
}
