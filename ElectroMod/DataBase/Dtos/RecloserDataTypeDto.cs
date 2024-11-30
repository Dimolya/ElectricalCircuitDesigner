using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.DataBase.Dtos
{
    public class RecloserDataTypeDto
    {
        public List<InternalPropRecloser> TypeRecloser { get; set; }
    }
    public class InternalPropRecloser    
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> TypeTT { get; set; }
    }

}
