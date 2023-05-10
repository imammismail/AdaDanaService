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
        //[Authorize(Roles = "User")]
        [HttpPost("Topup")]
        public IActionResult TopUp(TopUpDto request)
        {
            try
            {
                // var username = User.Identity.Name;
                _walletDataService.TopUpWallet(request.Username, request.Saldo);
                return Ok("Top-up berhasil dilakukan.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "User")]
        [HttpPost("CashOut")]
        public IActionResult CashOut(TopUpDto request)
        {
            try
            {
                //var username = User.Identity.Name;
                _walletDataService.CashOutWallet(request.Username, request.Saldo);
                return Ok("Saldo Berhasil Berkurang");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "User")]
        [HttpPost("ViewBalance")]
        public IActionResult GetBalance(ReadWalletDto request)
        {
            try
            {
                //var username = User.Identity.Name;
                var saldo = _walletDataService.GetWalletBalance(request.Username);
                return Ok(new { saldo });
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