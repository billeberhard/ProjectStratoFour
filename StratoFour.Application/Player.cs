﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application
{
    public class Player
    {
        public string Name { get; }

        public string Color { get; }

        public Player(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }
}
