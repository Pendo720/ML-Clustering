using System;
using System.Text.Json;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Field(string name, float value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => String.Format("{0}:{1}", Name, (float)Value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        public override bool Equals(object obj) =>
            ((obj == null || GetType() != obj.GetType()) ? false :
            Name.Equals((obj as Field).Name) && Value.Equals((obj as Field).Value));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => base.GetHashCode();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string asJson() => JsonSerializer.Serialize(this);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fmin"></param>
        /// <param name="fmax"></param>
        /// <returns></returns>
        public float Normalise(float fmin, float fmax)
        {
            float f = Value - fmin;
            Value = (float)((double)f / fmax);
            return Value;
        }
    }
}
