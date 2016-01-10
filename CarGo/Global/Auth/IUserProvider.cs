using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarGo.Model;

namespace CarGo.Global.Auth
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}
