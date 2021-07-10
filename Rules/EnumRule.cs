using System;

namespace Gamerules.Rules
{
    /// <summary>
    /// Defines a rule with an integer value.
    /// </summary>
    public class EnumRule<T> : Rule<T> where T : Enum
    {
        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        public EnumRule(T defaultValue) : base(defaultValue)
        {
        }

        /// <inheritdoc/>
        protected override Result Deserialize(object jsonValue)
        {
            var type = typeof(T);
            if (jsonValue is string s)
                try
                {
                    Value = (T)Enum.Parse(type, s.Trim(), true);
                    return Result.FromOk();
                }
                catch
                {
                    bool flags = type.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
                    return Result.FromErr($"Value is not accepted. Acceptable values can be {(flags ? "one or more" : "one")} of: [{string.Join(",", Enum.GetNames(type))}].{(flags ? " Separate multiple values with commas." : "")}");
                }
            Value = DefaultValue;
            return Result.FromErr("Value was not a valid string.");
        }

        /// <inheritdoc/>
        protected override string Serialize()
        {
            return Value.ToString();
        }
    }
}