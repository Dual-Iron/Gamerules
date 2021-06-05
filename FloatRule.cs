using System;

namespace Gamerules
{
    /// <summary>
    /// Defines a rule with a decimal value.
    /// </summary>
    public class FloatRule : Rule<double>
    {
        /// <summary>
        /// Instantiates a new rule instance with a minimum and maximum value.
        /// </summary>
        public FloatRule(double defaultValue, double min, double max) : base(defaultValue)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        public FloatRule(double defaultValue) : this(defaultValue, double.MinValue, double.MaxValue)
        {
        }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public double Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public double Max { get; }

        /// <inheritdoc/>
        public override Result Deserialize(object jsonValue)
        {
            if (jsonValue is long l)
                jsonValue = (double)l;

            if (jsonValue is double f)
            {
                if (f < Min)
                {
                    Value = Min;
                    return Result.FromErr($"Value was less than {Min}.");
                }
                if (f > Max)
                {
                    Value = Max;
                    return Result.FromErr($"Value was greater than {Max}.");
                }
                Value = f;
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