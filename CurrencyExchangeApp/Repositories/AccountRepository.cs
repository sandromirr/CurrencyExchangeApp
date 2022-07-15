﻿using Microsoft.EntityFrameworkCore;
using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using CurrencyExchangeApp.Models.Exceptions;

namespace CurrencyExchangeApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public AccountRepository(CurrencyExchangeDbContext _dbContext, IConfiguration _configuration)
        {
            this._dbContext = _dbContext;
            this._configuration = _configuration;
        }

        public Task<bool> AccountExcists(string personalNumber)
        {
            var account = _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();
            return account;
        }

        public async Task Create(CreateAccountViewModel createAccountViewModel)
        {
            var account = new Account()
            {
                Name = createAccountViewModel.Name,
                Surname = createAccountViewModel.Surname,
                PersonalNumber = createAccountViewModel.PersonalNumber,
                RecommenderNumber = createAccountViewModel.RecommenderNumber
            };

            var accontExists = _dbContext.Account.Where(x => x.PersonalNumber == createAccountViewModel.PersonalNumber).Any();

            if (accontExists) 
            {
                throw new CurrencyExchangeException($"Account with personal number {account.PersonalNumber} already created account", CurrencyExhangeExceptionEnum.AccountExists);
            }

            var recommenderAccount = _dbContext.Account.Where(x => x.PersonalNumber == account.RecommenderNumber).Any();

            if (!recommenderAccount && account.PersonalNumber != account.RecommenderNumber)
            {
                throw new CurrencyExchangeException($"There is not recommender account", CurrencyExhangeExceptionEnum.NotFoundRecomderAccount);
            }

            await _dbContext.Account.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Account?> GetAccountByPersonalNumber(string personalNumber)
        {
            var accountExcists = await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).AnyAsync();

            if (!accountExcists)
            {
                throw new Exception("Account does not excists");
            }

            var account = await _dbContext.Account.Where(x => x.PersonalNumber == personalNumber).FirstOrDefaultAsync();

            return account;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            var accounts = await _dbContext.Account.ToListAsync();
            return accounts;
        }
    }
}
