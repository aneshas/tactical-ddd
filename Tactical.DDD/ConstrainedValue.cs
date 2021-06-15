using System;

namespace Tactical.DDD
{
    public abstract record ConstrainedValue<T, TE> where TE : Exception, new()
    {
        private readonly T _value;

        protected ConstrainedValue(T value, Func<T, (bool, string)> rule)
        {
            var (valid, error) = rule(value);

            if (!valid)
                throw new ArgumentException(error);

            _value = value;
        }

        protected ConstrainedValue(T value, Func<T, bool> rule)
        {
            if (!rule(value))
            {
                // ReSharper disable once PossibleNullReferenceException
                throw Activator.CreateInstance(
                    typeof(TE),
                    $"Invalid value for {GetType().Name}: {value}"
                ) as TE;
            }

            _value = value;
        }

        public static implicit operator T(ConstrainedValue<T, TE> str) => str._value;

        public override string ToString() => _value.ToString();
    }
}