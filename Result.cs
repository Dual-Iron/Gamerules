namespace Gamerules
{
    /// <summary>
    /// Defines a potential error.
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// The error message, or null if there was no error.
        /// </summary>
        public string? Err { get; }

        /// <summary>
        /// True if there was an error.
        /// </summary>
        public bool IsOk => Err == null;

        private Result(string err)
        {
            Err = err;
        }

        /// <summary>
        /// Gets a result that indicates no errors occurred.
        /// </summary>
        public static Result FromOk() => default;

        /// <summary>
        /// Gets a result that indicates an error occurred.
        /// </summary>
        /// <param name="err">The error message.</param>
        public static Result FromErr(string err) => new(err);
    }
}
