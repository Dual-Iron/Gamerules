using System;
using System.Collections.Generic;

namespace Gamerules
{
    public static class RuleAPI
    {
        internal static readonly Dictionary<string, IRule<object>> rules = new();

        /// <summary>
        /// Register a gamerule.
        /// </summary>
        /// <param name="rule">The gamerule instance.</param>
        public static void Register<T>(IRule<T> rule)
        {
            string name = rule.Name;
            for (int i = 0; i < name.Length; i++)
            {
                bool valid = 
                    name[i] >= 'A' && name[i] <= 'Z' || 
                    name[i] >= 'a' && name[i] <= 'z' || 
                    name[i] >= '0' && name[i] <= '9' || 
                    name[i] == ' ' || name[i] == '-' || name[i] == '_' || name[i] == ',' || name[i] == '\'';

                if (!valid)
                    throw new ArgumentException($"The name '{name}' is invalid. Rule names can only contain a-z, A-Z, 0-9, apostrophe, comma, hyphen, underscore, and space.");
            }
            rules[name] = (IRule<object>)rule;
        }

        /// <summary>
        /// An enumerable of every registered gamerule.
        /// </summary>
        public static IEnumerable<IRule<object>> Rules => rules.Values;
    }
}