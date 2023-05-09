using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaDanaService.Dtos;
using AdaDanaService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdaDanaService.Data
{
    public class WalletDataService
    {
        private readonly AdaDanaContext _context;

        public WalletDataService(AdaDanaContext context)
        {
            _context = context;
        }

        public string GetUserRole(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user == null)
            {
                throw new Exception("Username is not match");
            }
            // var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);

            return user.Username;
        }


        public User GetUserByUsername(string username)
        {
            return _context.Users
                .Include(u => u.Wallets)
                .FirstOrDefault(u => u.Username == username);
        }

        public void TopUpWallet(string username, int saldo)
        {
            var user = GetUserByUsername(username);

            if (user == null)
            {
                throw new Exception($"User with username {username} not found.");
            }

            var wallet = user.Wallets.FirstOrDefault();

            if (wallet == null)
            {
                wallet = new Wallet { UserId = user.Id, Saldo = saldo };
                _context.Wallets.Add(wallet);
            }
            else
            {
                wallet.Saldo += saldo;
                _context.Wallets.Update(wallet);
            }

            _context.SaveChanges();
        }

        public Wallet GetWallet(int saldo)
        {
            return _context.Wallets.FirstOrDefault(w => w.Saldo == saldo);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        // public Wallet TopUpBalance(int userId, int saldo)
        // {
        //     var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);

        //     if (wallet != null)
        //     {
        //         wallet.Saldo += saldo;
        //     }
        //     else
        //     {
        //         wallet = new Wallet
        //         {
        //             UserId = userId,
        //             Saldo = saldo
        //         };
        //         _context.Wallets.Add(wallet);
        //     }

        //     _context.SaveChanges();

        //     return wallet;
        // }
    }
}