using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liftoff.VelopackDemo.Layout
{
    internal class NavMenuViewModel
    {
        public string Version => $"v{GetType().Assembly.GetName().Version}";
    }
}
