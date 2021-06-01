using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamerules
{
    public struct Result
    {
        public string? Err { get; }

        public bool Success => Err == null;

        private Result(string err)
        {
            Err = err;
        }

        public static Result FromOk() => default;
        public static Result FromErr(string err) => new(err);
    }

    public struct Result<T>
    {
        private readonly T? ok;
        public T Ok => ok ?? throw new InvalidOperationException("Tried to get Ok despite result being a failure.");

        private readonly string? err;
        public string Err
        {
            get
            {
                if (ok == null)
                    return err ?? "An error occurred.";
                throw new InvalidOperationException("Tried to get Err despite result being a success.");
            }
        }

        public bool Success => err == null;

        private Result(T ok)
        {
            this.ok = ok;
            err = null;
        }

        private Result(string err)
        {
            ok = default;
            this.err = err;
        }

        public static Result<T> FromOk(T ok) => new(ok);
        public static Result<T> FromErr(string err) => new(err);
    }
}
