using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaDanaService.Dtos;

namespace AdaDanaService.Data
{
    public interface IWalletService
    {
        Task TopUp(int userId, int saldo);
        Task CashOut(int userId, int saldo);
        Task<int> GetBalance();
    }
}