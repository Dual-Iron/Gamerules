namespace Gamerules
{
    /// <summary>
    /// Defines a type of gamerule.
    /// </summary>
    /// <typeparam name="T">The type of the gamerule's value.</typeparam>
    public interface IRule
    {
        /// <summary>
        /// The gamerule's name. This should be immutable and can only contain a-z, A-Z, 0-9, apostrophe, comma, hyphen, underscore, and space.
        /// </summary>
        string Name { get; }

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
        /// </summary>
        /// <param name="jsonValue">The JSON value.</param>
        Result Deserialize(string jsonValue);

        /// <summary>
        /// Gets a JSON value from the gamerule's value.
        /// </summary>
        string Serialize();
    }
}