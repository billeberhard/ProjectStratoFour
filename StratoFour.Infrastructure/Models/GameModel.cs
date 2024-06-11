using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Infrastructure.Models;

public class GameModel
{
    public int GameId { get; set; }
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }
    public int RobotId { get; set; }
    public DateTime StartTime { get; set; }
    public string GameMode {  get; set; }
    public int? WinnerId { get; set; }
    public bool IsActive { get; set; }


    public UserModel Player1 { get; set; } 
    public UserModel Player2 { get; set; } 
    public UserModel Winner { get; set; }
    public RobotModel Robot { get; set; } 
}
