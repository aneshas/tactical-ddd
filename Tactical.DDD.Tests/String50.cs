﻿using System.ComponentModel.DataAnnotations;

namespace Tactical.DDD.Tests
{
    public record String50 : ConstrainedValue<string, DomainException>
    {
        public String50([MaxLength(50)] string value) : base(value)
        {
        }
    }
}