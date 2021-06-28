using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectFour.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerNickName { get; set; }
        public string PlayerProfilePic { get; set; }
        public string PlayerEmail { get; set; }
        public string PlayerPassword { get; set; }
        public int NumberDefeats { get; set; }
        public int NumberVictories { get; set; }
        public int NumberDraw { get; set; }
        public int Record { get; set; }
    }
}
