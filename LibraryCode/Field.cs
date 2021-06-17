using System;

namespace KmsLibraryCode
{
    public sealed class Field
    {
        public string Name { get; set; }
        public float Value { get; set; }

        public Field()
        {
            Name = "None";
            Value = float.MinValue;
        }
        

        public Field(string name, float value)
        {
            Name = name;
            Value = value;
        }
        
        public override string ToString() => String.Format("{0}:{1}", Name, (float)Value);
        
        public float Normalise(float fmin, float fmax)
        {
            float f = Value - fmin;
            Value = (float)( f / fmax);
            return Value;
        }
    }
}
