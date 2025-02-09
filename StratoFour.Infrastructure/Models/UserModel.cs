﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Infrastructure.Models;

public class UserModel
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailVerification { get; set; }
    public string ConnectionId { get; set; } // SignalR Connection ID
}
