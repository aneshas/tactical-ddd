using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Tactical.DDD
{
    public abstract record ConstrainedValue<T, TE> where TE : DomainException
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

        protected ConstrainedValue(T value)
        {
            var attributes = GetType()
                .GetConstructors()
                .First()
                .GetParameters()[0]
                .GetCustomAttributes<ValidationAttribute>();

            foreach (var attr in attributes)
            {
                if (attr.IsValid(value)) continue;

                var message = $"Invalid value for {GetType().Name}: {value}";

                if (attr.ErrorMessage != null)
                {
                    message = attr.FormatErrorMessage(attr.ErrorMessage);
                }

                // ReSharper disable once PossibleNullReferenceException
                throw Activator.CreateInstance(
                    typeof(TE),
                    message
                ) as TE;
            }

            _value = value;
        }

        public static implicit operator T(ConstrainedValue<T, TE> str) => str._value;

        public override string ToString() => _value.ToString();
    }
}