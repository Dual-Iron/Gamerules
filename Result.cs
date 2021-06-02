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
        /// Gets a result that indicates no errors occurred.
        /// </summary>
        internal static Result<T> FromOk<T>(T ok) => Result<T>.FromOk(ok);

        /// <summary>
        /// Gets a result that indicates an error occurred.
        /// </summary>
        /// <param name="err">The error message.</param>
        public static Result FromErr(string err) => new(err);
    }

    /// <summary>
    /// Defines a potential error.
    /// </summary>
    internal struct Result<T>
    {
        /// <summary>
        /// The error message, or null if there was no error.
        /// </summary>
        public string? Err { get; }

        /// <summary>
        /// The ok value.
        /// </summary>
        public T? Ok { get; }

        /// <summary>
        /// True if there was an error.
        /// </summary>
        public bool IsOk { get; }

        private Result(string? err, T? ok)
        {
            Err = err;
            Ok = ok;
            IsOk = err == null;
        }

        /// <summary>
        /// Gets a result that indicates no errors occurred.
        /// </summary>
        /// <param name="ok">The ok value.</param>
        public static Result<T> FromOk(T ok) => new(null, ok);

        /// <summary>
        /// Gets a result that indicates an error occurred.
        /// </summary>
        /// <param name="err">The error message.</param>
        public static Result<T> FromErr(string err) => new(err, default);
    }
}
