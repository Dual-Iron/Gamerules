using Gamerules;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GameruleAPI.Rules
{
    /// <summary>
    /// An abstract, generic implementation of <see cref="IRule"/>.
    /// </summary>
    /// <typeparam name="T">The type parameter for <see cref="IRule"/>.</typeparam>
    public abstract class Rule<T> : IRule where T : notnull
    {
        /// <summary>
        /// Used for <see cref="Rule{T}.OnDeserialize"/>.
        /// </summary>
        public delegate void DeserializeDelegate(T value);

        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        protected Rule(T defaultValue)
        {
            DefaultValue = Value = defaultValue;
        }

        /// <summary>
        /// Registers this gamerule.
        /// </summary>
        /// <param name="id">The ID to register this rule under. Can only contain a-z, 0-9, forward slash, and underscore.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public void Register(string id)
        {
            RuleAPI.Register(id, this);
            // Have to explicitly set ID after calling Register
            // to prevent setting ID if an exception is thrown.
            ID = id;
        }

        /// <summary>
        /// The rule's ID. When set, registers the gamerule through <see cref="RuleAPI.Register(string, IRule)"/>. See the documentation on that method.
        /// </summary>
        public string? ID { get; private set; }

        /// <inheritdoc/>
        public string Description { get; set; } = "";

        /// <inheritdoc/>
        public T DefaultValue { get; }

        /// <inheritdoc/>
        public T Value { get; set; }

        object IRule.Value { get => Value; set => Value = (T)value; }

        object IRule.DefaultValue => DefaultValue;

        /// <inheritdoc/>
        protected abstract Result Deserialize(object jsonValue);

        /// <inheritdoc/>
        protected abstract string Serialize();

        /// <summary>
        /// Fired after the rule is deserialized.
        /// </summary>
        public event DeserializeDelegate? OnDeserialize;

        Result IRule.Deserialize(object jsonValue)
        {
            var result = Deserialize(jsonValue);
            OnDeserialize?.Invoke(Value);
            return result;
        }

        string IRule.Serialize()
        {
            return Serialize();
        }

        /// <summary>
        /// Gets the string representation of this rule's value.
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Implicit cast to <typeparamref name="T"/>.
        /// </summary>
        public static implicit operator T(Rule<T> t) => t.Value;
    }
}