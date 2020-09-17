using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SectionData
    {
        public IParticpant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticpant Right { get; set; }
        public int DistanceRight { get; set; }
    }
}
