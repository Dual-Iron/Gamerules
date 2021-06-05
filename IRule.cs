﻿namespace Gamerules
{
    /// <summary>
    /// Defines a type of gamerule.
    /// </summary>
    /// <typeparam name="T">The type of the gamerule's value.</typeparam>
    public interface IRule
    {
        /// <summary>
        /// The gamerule's display name.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The gamerule's description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The gamerule's current value.
        /// </summary>
        object Value { get; set; }

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