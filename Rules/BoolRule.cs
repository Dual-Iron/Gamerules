using Gamerules;
using System;

namespace GameruleAPI.Rules
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
        protected override Result Deserialize(object jsonValue)
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
        protected override string Serialize()
        {
            return Value ? "true" : "false";
        }
    }
}