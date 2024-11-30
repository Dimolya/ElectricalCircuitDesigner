using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.DataBase.Dtos
{
    public class LineDataTypeDto
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public double ActiveResistance { get; set; }
        public double ReactiveResistance { get; set; }
    }
}
