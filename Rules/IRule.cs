namespace Gamerules.Rules
{
    /// <summary>
    /// Defines a type of gamerule.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// The gamerule's description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The gamerule's current value.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The gamerule's fallback value.
        /// </summary>
        object DefaultValue { get; }

        /// <summary>
        /// Gets the gamerule's value from a JSON value. 
        /// <para/>Objects are represented by a <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> with <see cref="string"/> keys and <see cref="object"/> values. 
        /// <para/>Arrays are represented by a <see cref="System.Collections.Generic.List{T}"/> of <see cref="object"/> values.
        /// </summary>
        /// <param name="jsonValue">The JSON value.</param>
        Result Deserialize(object jsonValue);

        /// <summary>
        /// Gets a JSON value from the gamerule's value.
        /// </summary>
        string Serialize();
    }
}