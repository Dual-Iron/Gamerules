using System;

namespace Gamerules
{
    /// <summary>
    /// Defines a rule with a boolean value.
    /// </summary>
    public class BoolRule : Rule<bool>
    {
        /// <inheritdoc/>
        public BoolRule(bool defaultValue) : base(defaultValue)
        {
        }

        /// <inheritdoc/>
        public override Result Deserialize(object jsonValue)
        {
            if (jsonValue is bool b)
            {
                Value = b;
                return Result.FromOk();
            }
            Value = DefaultValue;
            return Result.FromErr("Value was not true or false.");
        }

        /// <inheritdoc/>
        public override string Serialize()
        {
            return Value ? "true" : "false";
        }
    }
}