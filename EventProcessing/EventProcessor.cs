using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AdaDanaService.Data;
using AdaDanaService.Dtos;
using AdaDanaService.Models;

namespace AdaDanaService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly AdaDanaContext _context;

        public EventProcessor(AdaDanaContext context, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _scopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _context = context;
        }

        public void ProccessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.TopupWalletPublished:
                    cashoutWallet(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "TopupWallet_NewPublished":
                    Console.WriteLine("--> TopupWallet_NewPublished Event Detected");
                    return EventType.TopupWalletPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }


        private void cashoutWallet(string topupWalletMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IWalletService>();
                var walletPublishedDto = JsonSerializer.Deserialize<TopupWalletPublishDto>(topupWalletMessage);
                try
                {
                    var wallet = _mapper.Map<Wallet>(walletPublishedDto);
                    var getWalletId = _context.Users.FirstOrDefault(u => u.Username == walletPublishedDto.Username);
                    if (!repo.WalletExists(getWalletId.Id))
                    {
                        Console.WriteLine("--> Wallet doesn't exist in database");
                    }
                    else
                    {
                        repo.CashoutOtherService(getWalletId.Id, walletPublishedDto.Saldo);
                        Console.WriteLine("--> Topup cash added to wallet order");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        TopupWalletPublished,
        Undetermined
    }
}