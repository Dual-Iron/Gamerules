using System;

namespace Gamerules
{
    /// <summary>
    /// Defines a rule with an integer value.
    /// </summary>
    public class IntRule : Rule<int>
    {
        /// <summary>
        /// Instantiates a new rule instance with a minimum and maximum value.
        /// </summary>
        public IntRule(int defaultValue, int min, int max) : base(defaultValue)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        public IntRule(int defaultValue) : this(defaultValue, int.MinValue, int.MaxValue)
        {
        }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public int Max { get; }

        /// <inheritdoc/>
        public override Result Deserialize(string fromString)
        {
            if (int.TryParse(fromString, out var i))
            {
                if (i < Min)
                {
                    Value = Min;
                    return Result.FromErr($"Value was less than {Min}.");
                }
                if (i > Max)
                {
                    Value = Max;
                    return Result.FromErr($"Value was greater than {Max}.");
                }
                Value = i;
                return Result.FromOk();
            }
            Value = DefaultValue;
            return Result.FromErr("Value was not a valid integer.");
        }

        /// <inheritdoc/>
        public override string Serialize()
        {
            return Value.ToString();
        }
    }
}