using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdaDanaService.Data;
using AdaDanaService.Dtos;
using Microsoft.AspNetCore.Authorization;
using AdaDanaService.Models;
using System.Security.Claims;

namespace AdaDanaService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly AdaDanaContext _context;
        private readonly IWalletService _walletService;

        public WalletController(AdaDanaContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("topup")]
        public async Task<IActionResult> TopUp(TopUpDto topUpDto)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = int.Parse(userIdClaim.Value);

                await _walletService.TopUp(userId, topUpDto.Saldo);
                return Ok("Saldo berhasil ditambahkan");
            }
            catch (Exception ex)
            {
                return BadRequest($"Terjadi kesalahan: {ex.Message}");
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("cashout")]
        public async Task<IActionResult> CashOut(CashOutDto cashOutDto)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = int.Parse(userIdClaim.Value);

                await _walletService.CashOut(userId, cashOutDto.Saldo);
                return Ok("Saldo berhasil ditarik");
            }
            catch (Exception ex)
            {
                return BadRequest($"Terjadi kesalahan: {ex.Message}");
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance(ReadWalletDto readWalletDto)
        {
            try
            {
                var balance = await _walletService.GetBalance();
                return Ok(new { Saldo = balance });
            }
            catch (Exception ex)
            {
                return BadRequest($"Terjadi kesalahan: {ex.Message}");
            }
        }
    }
}