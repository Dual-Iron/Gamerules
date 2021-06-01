using System;

namespace Gamerules
{
    /// <summary>
    /// Defines a rule with a boolean value.
    /// </summary>
    public class BoolRule : Rule<bool>
    {
        /// <inheritdoc/>
        public BoolRule(string name, string displayName, bool defaultValue, string description) : base(name, displayName, defaultValue, description)
        {
        }

        /// <inheritdoc/>
        public override Result Deserialize(string fromString)
        {
            if (fromString == "true")
            {
                Value = true;
                return Result.FromOk();
            }
            if (fromString == "false")
            {
                Value = false;
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