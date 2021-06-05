using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gamerules
{
    /// <summary>
    /// An abstract, generic implementation of <see cref="IRule"/>.
    /// </summary>
    /// <typeparam name="T">The type parameter for <see cref="IRule"/>.</typeparam>
    public abstract class Rule<T> : IRule where T : notnull
    {
        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        protected Rule(T defaultValue)
        {
            DefaultValue = Value = defaultValue;
        }

        private string? defaultDisplayName;
        private string? displayName;
        private string? id;

        /// <summary>
        /// The rule's ID. When set, registers the gamerule through <see cref="RuleAPI.Register(string, IRule)"/>. See the documentation on that method.
        /// </summary>
        public string? ID
        {
            get => id; 
            set
            {
                if (value == null)
                    return;
                if (id != null || RuleAPI.TryGetRule(value, out _))
                    throw new InvalidOperationException("Can't change a gamerule's ID once already registered.");
                RuleAPI.Register(id = value, this);
            }
        }

        /// <inheritdoc/>
        public string Description { get; set; } = "";

        /// <summary>
        /// The gamerule's display name. This is automatically determined by <see cref="ID"/>, but you can override it by setting this property.
        /// </summary>
        public string DisplayName { get => displayName ?? (defaultDisplayName ??= GetDefaultDisplayName()); set => displayName = value; }

        private string GetDefaultDisplayName()
        {
            if (id == null)
                return "";

            StringBuilder sb = new();

            for (int i = 0; i < id.Length - 1; i++)
            {
                if (id[i] == '/')
                    sb.Append(" -> ");
                else if (id[i] == '_')
                    sb.Append(" ");
                else if (i == 0 || id[i - 1] == '_' || id[i - 1] == '/')
                    sb.Append(" " + char.ToUpper(id[i]));
                else
                    sb.Append(id[i]);
            }

            return sb.ToString();
        }

        /// <inheritdoc/>
        public T DefaultValue { get; }

        /// <inheritdoc/>
        public virtual T Value { get; set; }

        object IRule.Value { get => Value; set => Value = (T)value; }

        object IRule.DefaultValue => DefaultValue;

        /// <inheritdoc/>
        public abstract Result Deserialize(object jsonValue);

        /// <inheritdoc/>
        public abstract string Serialize();

        /// <summary>
        /// Implicit cast to <typeparamref name="T"/>.
        /// </summary>
        public static implicit operator T(Rule<T> t) => t.Value;
    }
}