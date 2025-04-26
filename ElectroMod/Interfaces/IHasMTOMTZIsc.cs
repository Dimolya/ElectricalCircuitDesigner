using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.Interfaces
{
    public interface IHasMTOMTZIsc
    {
        double MTO { get; set; }
        double MTZ { get; set; }
        double IszMTZ { get; set; }
        double IszMTO { get; set; }
        double TableMTZ { get; set; }
        double TableMTO { get; set; }

    }
}
