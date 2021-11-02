﻿using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Atomic.Utils
{
    [DebuggerStepThrough]
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value,
            [InvokerParameterName] string parameterName
        )
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}