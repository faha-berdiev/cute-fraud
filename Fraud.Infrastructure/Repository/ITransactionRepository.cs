﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface ITransactionRepository : IDisposable
    {
        public Task Create(Transaction entity);
        public Task<IReadOnlyList<Transaction>> FindByDateRange(string cardToken, DateTimeOffset dateFrom, DateTimeOffset dateTo);
        public Task<IReadOnlyList<Transaction>> FindLimit(string cardToken, int limit = 100);
    }
}