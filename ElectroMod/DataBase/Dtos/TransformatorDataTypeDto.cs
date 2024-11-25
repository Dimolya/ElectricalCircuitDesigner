using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.DataBase.Dtos
{
    public class TransformatorDataTypeDto
    {
        public List<InternalPropTransformator> TypeKTP { get; set; }
        public List<InternalPropTransformator> ShemesConnectingWinding { get; set; }
    }

    public class InternalPropTransformator
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
