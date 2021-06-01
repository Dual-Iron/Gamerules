using System;
using System.Collections.Generic;

namespace Gamerules
{
    /// <summary>
    /// Start here. Provides methods for registering, enumerating, and getting gamerules.
    /// </summary>
    public static class RuleAPI
    {
        internal static readonly Dictionary<string, IRule> rules = new();

        /// <summary>
        /// Registers a new gamerule or replaces an existing one with the same name.
        /// </summary>
        /// <param name="rule">The gamerule instance.</param>
        public static void Register(IRule rule)
        {
            string name = rule.Name;
            for (int i = 0; i < name.Length; i++)
            {
                bool valid = 
                    name[i] >= 'a' && name[i] <= 'z' || 
                    name[i] >= '0' && name[i] <= '9' || 
                    name[i] == '_';

                if (!valid)
                    throw new ArgumentException($"The name '{name}' is invalid. Rule names can only contain a-z, A-Z, 0-9, apostrophe, comma, hyphen, underscore, and space.");
            }
            rules[name] = rule;
        }

        /// <summary>
        /// An enumerable of every registered gamerule.
        /// </summary>
        public static IEnumerable<IRule> Rules => rules.Values;

        /// <summary>
        /// Tries to get a rule with the given name.
        /// </summary>
        /// <returns>True if a matching rule exists; false otherwise.</returns>
        public static bool TryGetRule(string name, [MaybeNullWhen(false)] out IRule rule)
        {
            return rules.TryGetValue(name, out rule);
        }

        private class MaybeNullWhenAttribute : Attribute
        {
            public readonly bool when;

            public MaybeNullWhenAttribute(bool when)
            {
                this.when = when;
            }
        }
    }
}