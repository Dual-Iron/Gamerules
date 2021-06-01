namespace Gamerules
{
    /// <summary>
    /// An abstract implementation of <see cref="IRule{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type parameter for <see cref="IRule{T}"/>.</typeparam>
    public abstract class Rule<T> : IRule<T>
    {
        /// <summary>
        /// Instantiates a new rule instance.
        /// </summary>
        protected Rule(string name, string description, T defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public T DefaultValue { get; }

        /// <inheritdoc/>
        public T Value { get; set; }

        /// <inheritdoc/>
        public abstract Result Deserialize(string jsonValue);

        /// <inheritdoc/>
        public abstract string Serialize();
    }
}