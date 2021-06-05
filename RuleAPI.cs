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
        /// Registers a gamerule. Once a rule is registered, it can't be registered again. Make sure to wrap this in a try-catch.
        /// </summary>
        /// <param name="rule">The gamerule instance, or null to remove the gamerule.</param>
        /// <param name="id">Can only contain a-z, 0-9, forward slash, and underscore.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static void Register(string id, IRule rule)
        {
            if (id == null || rule == null)
                throw new ArgumentNullException();

            if (rules.ContainsKey(id))
                throw new ArgumentException($"The ID '{id}' is already taken.");

            for (int i = 0; i < id.Length; i++)
            {
                bool valid = 
                    id[i] >= 'a' && id[i] <= 'z' || 
                    id[i] >= '0' && id[i] <= '9' || 
                    id[i] == '_' || id[i] == '/';

                if (!valid)
                    throw new ArgumentException($"The name '{id}' is invalid. Rule names can only contain a-z, 0-9, forward slash, and underscore.");
            }

            rules[id] = rule;
        }

        /// <summary>
        /// An enumerable of every registered gamerule.
        /// </summary>
        public static IEnumerable<IRule> Rules => rules.Values;

        /// <summary>
        /// Tries to get a rule with the given name.
        /// </summary>
        /// <returns>True if a matching rule exists; false otherwise.</returns>
        public static bool TryGetRule(string id, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out IRule rule)
        {
            return rules.TryGetValue(id, out rule);
        }
    }
}
