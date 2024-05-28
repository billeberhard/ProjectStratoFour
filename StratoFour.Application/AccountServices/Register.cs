using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.AccountServices
{
    public class Register
    {
        private UserModel CreateUser()
        {
            try
            {
                //new UserData().InsertUser
                //return 
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserModel)}'. " +
                    $"Ensure that '{nameof(UserModel)}' is not an abstract class and has a parameterless constructor.");
            }
            return new UserModel();
        }
    }
}
