using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdaDanaService.Data;
using AdaDanaService.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace AdaDanaService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "User")]
    public class WalletController : ControllerBase
    {
        private readonly WalletDataService _walletDataService;

        public WalletController(WalletDataService walletDataService)
        {
            _walletDataService = walletDataService;
        }

        /* [HttpPost("topup")]*/
        /*        public async Task<IActionResult> TopUp(TopUpDto topUpDto)
                {
                    // Cek apakah pengguna memiliki role 'user'
                    var userRole = _walletDataService.GetUserRole(topUpDto.Username);
                    if (userRole != "user")
                    {
                        return BadRequest("Anda bukan user");
                    }

                    // Lakukan top up saldo
                    var wallet = _walletDataService.GetWallet(topUpDto.Saldo);
                    wallet.Saldo += topUpDto.Saldo;
                    _walletDataService.UpdateWallet(wallet);

                    return Ok("Saldo berhasil ditambahkan");
                }*/

        [HttpPost("topup")]
        public IActionResult TopUp(TopUpDto request)
        {
            try
            {
                _walletDataService.TopUpWallet(request.Username, request.Saldo);
                return Ok("Top-up berhasil dilakukan.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [HttpPost("topup")]
        // public IActionResult TopUpBalance([FromBody] TopUpDto topUpDto)
        // {
        //     var userId = int.Parse(User.Identity.Name);

        //     var wallet = _walletDataService.TopUpBalance(userId, topUpDto.Saldo);

        //     return Ok(wallet);
        // }
    }
}