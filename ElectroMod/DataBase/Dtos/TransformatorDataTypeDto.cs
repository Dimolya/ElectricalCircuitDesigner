using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroMod.DataBase.Dtos
{
    public class TransformatorDataTypeDto
    {
        public List<TypeProp> TypesKTP { get; set; }
        public List<ShemesProp> SchemesConnectingWinding { get; set; }
    }

    public class TypeProp
    {
        public string Type { get; set; }
        public double ResistanceOne { get; set; }
        public double ResistanceTwo { get; set; }
    }

    public class ShemesProp
    {
        public string Scheme { get; set; }
    }

}
