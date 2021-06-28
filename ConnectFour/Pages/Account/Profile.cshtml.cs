using ConnectFour.Data;
using ConnectFour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConnectFour.Pages.Account.LoginModel;

namespace ConnectFour.Pages.Account
{
    [AllowAnonymous]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(
            ApplicationDbContext context)
        {
            _context = context;
        }



        public string PlayerNickName { get; set; }
        public int NumberDefeats { get; set; }
        public int NumberVictories { get; set; }
        public int NumberDraw { get; set; }
        public int Record { get; set; }
        public int TotalGames { get; set; }
        public string VictoryRatio { get; set; }


        public void OnGet()
        {
            Player player = _context.Player.Where(x => x.PlayerEmail == User.Identity.Name).FirstOrDefault();

            int totalGames = player.NumberVictories + player.NumberDefeats + player.NumberDraw;

            string victoryRatio;
            if (player.NumberDraw + player.NumberDefeats == 0 && player.NumberVictories != 0)
            {
                victoryRatio = Convert.ToString(player.NumberVictories);
            }
            else if(player.NumberDraw + player.NumberDefeats != 0 && player.NumberVictories != 0)
            {
                victoryRatio = Convert.ToString(Convert.ToDouble(player.NumberVictories) / Convert.ToDouble((player.NumberDraw + player.NumberDefeats)));
            }
            else
            {
                victoryRatio = "-";
            }

            NumberDefeats = player.NumberDefeats;
            NumberDraw = player.NumberDraw;
            NumberVictories = player.NumberVictories;
            PlayerNickName = player.PlayerNickName;
            Record = player.Record;
            TotalGames = totalGames;
            VictoryRatio = victoryRatio;


        }

    }
}



