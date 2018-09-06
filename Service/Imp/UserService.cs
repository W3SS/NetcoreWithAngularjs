using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Service.Interface;

namespace Service.Imp
{
    public class UserService : ServiceBase<User>, IUserService
    {
    }
}
