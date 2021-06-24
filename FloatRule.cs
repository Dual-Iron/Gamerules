using System;

namespace Gamerules
{
    /// <summary>
    /// Defines a rule with a decimal value.
    /// </summary>
    public class FloatRule : Rule<float>
    {
        /// <summary>
        /// Instantiates a new rule instance with a minimum and maximum value.
        /// </summary>
        public FloatRule(float defaultValue, float min, float max) : base(defaultValue)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        public FloatRule(float defaultValue) : this(defaultValue, float.NegativeInfinity, float.PositiveInfinity)
        {
        }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public float Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public float Max { get; }

        /// <inheritdoc/>
        public override Result Deserialize(object jsonValue)
        {
            if (jsonValue is long l)
                jsonValue = (double)l;

            if (jsonValue is double d)
            {
                if (d < Min)
                {
                    Value = Min;
                    return Result.FromErr($"Value was less than {Min}.");
                }
                if (d > Max)
                {
                    Value = Max;
                    return Result.FromErr($"Value was greater than {Max}.");
                }
                Value = (float)d;
                return Result.FromOk();
            }
            Value = DefaultValue;
            return Result.FromErr("Value was not a valid decimal number.");
        }

        /// <inheritdoc/>
        public override string Serialize()
        {
            return Value.ToString();
        }
    }
}