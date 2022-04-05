﻿using System;
using System.Linq;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    public class AmountAnalyzer : ITransactionAnalyzer
    {
        private readonly int _fraudPriorityStep = 15;
        private readonly int _intervalInSeconds = 12 * 3600; // Six hours
        private readonly int _analyzeIterationsCount = 3;
        
        public TransactionAnalyzerResult AnalyzeTransactions(in Transaction[] transactions)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            var fraudPriority = 0;
            
            var transactionAmountAvg = transactions.Average(x => x.Amount);
            var transactionsByAmount = transactions
                .Where(x => (DateTimeOffset.Now.ToUnixTimeSeconds() - x.DateCreatedUnix) < _intervalInSeconds)
                .OrderBy(x => x.DateCreatedUnix)
                .ThenBy(x => x.Amount)
                .ToArray();

            for (var i = 0; i < _analyzeIterationsCount; i++)
            {
                if (transactionsByAmount[i]?.Amount > transactionAmountAvg)
                    fraudPriority += _fraudPriorityStep;
            }

            return new TransactionAnalyzerResult(fraudPriority, transactions[0].CardToken);
        }
    }
}