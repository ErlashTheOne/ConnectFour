using ConnectFour.Data;
using ConnectFour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ConnectFour.Hubs
{
    public class PlayerHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public PlayerHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public void ReceiveGameData(int win, int lose, int tie, int movementCont)
        {
            if (!String.IsNullOrEmpty(Context.User.Identity.Name)) { 
                string email = Context.User.Identity.Name;

                Player player = _context.Player.Where(x => x.PlayerEmail == email).FirstOrDefault();
                if (player.Record < movementCont)
                {
                    player.Record = movementCont;
                }
                player.NumberDefeats = player.NumberDefeats + lose;
                player.NumberDraw = player.NumberDraw + tie;
                player.NumberVictories = player.NumberVictories + win;

                try
                {
                    _context.Update(player);
                    _context.SaveChanges();
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }
    }
}
