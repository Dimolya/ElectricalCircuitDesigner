using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.DataBase.Dtos
{
    public class TransformatorContainerDto
    {
        public string TypeKTP { get; set; }
        public double Uk { get; set; }
        public double Pk { get; set; }
        public int S { get; set; }
    }
}
