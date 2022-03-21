using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fedyaev_Petrov_Namordik.EF
{
    class AppData
    {
        public static Entities Context { get; } = new Entities();
    }
}
