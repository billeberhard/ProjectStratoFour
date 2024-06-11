using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching
{
    public class MatchInfo
    {
        public UserModel Player1 { get; set; }
        public UserModel Player2 { get; set; }
        public RobotModel Robot { get; set; }
        public bool AcceptedByPlayer1 { get; set; } = false;
        public bool AcceptedByPlayer2 { get; set; } = false;
    }
}
