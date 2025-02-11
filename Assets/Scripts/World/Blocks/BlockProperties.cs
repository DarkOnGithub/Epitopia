using System;

namespace World.Blocks
{
    public class BlockProperties : ICloneable
    {
        public bool IsTransparent = false;
        public bool IsSolid = true;
        public string SpritePath = null;
        public bool MergeWithDirt = false;
        public byte LightEmission = 0;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}