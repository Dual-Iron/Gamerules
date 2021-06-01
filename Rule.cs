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
        protected Rule(string name, string displayName, T defaultValue, string description)
        {
            Name = name;
            Description = description;
            DisplayName = displayName;
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        /// <summary>
        /// Instantiates a new rule instance with an inferred display name.
        /// </summary>
        protected Rule(string name, T defaultValue, string description)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
            Value = defaultValue;

            StringBuilder sb = new();

            for (int i = 0; i < name.Length - 1; i++)
            {
                if (name[i] == '/')
                    sb.Append(" -> ");
                else if (name[i] == '_')
                    sb.Append(" ");
                else if (i == 0 || name[i - 1] == '_' || name[i - 1] == '/')
                    sb.Append(" " + char.ToUpper(name[i]));
                else
                    sb.Append(name[i]);
            }

            DisplayName = sb.ToString();
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public string DisplayName { get; }

        /// <inheritdoc/>
        public T DefaultValue { get; }

        /// <inheritdoc/>
        public virtual T Value { get; set; }

        object IRule.Value { get => Value; set => Value = (T)value; }

        object IRule.DefaultValue => DefaultValue;

        /// <inheritdoc/>
        public abstract Result Deserialize(string jsonValue);

        /// <inheritdoc/>
        public abstract string Serialize();
    }
}