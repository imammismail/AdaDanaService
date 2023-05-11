using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaDanaService.Dtos;
using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IWalletService
    {
        Task TopUp(int userId, int saldo);
        Task CashOut(int userId, int saldo);
        Task<int> GetBalance(int userId);
        Task AddWallet(Wallet wallet);
    }
}