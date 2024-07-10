﻿using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Core.Handlers
{
	public interface ITransactionHandler
	{
		Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request);
		Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest  request);
		Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request);
		Task<Response<Transaction?>> GetBtyIdAsync(GetTransactionByIdRequest request);
		Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request);
	}
}
