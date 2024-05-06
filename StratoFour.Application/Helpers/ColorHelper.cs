using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.Helpers
{
    public class ColorHelper
    {
        public static string GetRandomColor()
        {
            var random = new Random();
            return $"#{random.Next(0x1000000):X6}";
        }
    }
}
