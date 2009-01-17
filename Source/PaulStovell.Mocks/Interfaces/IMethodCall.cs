using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PaulStovell.Mocks.Interfaces
{
    /// <summary>
    /// Implemented by classes which represent a recording of a call to a specific method 
    /// on a mock object.
    /// </summary>
    internal interface IMethodCall
    {
    }

    /// <summary>
    /// Implemented by classes which represent a recording of a call to a specific method 
    /// on a mock object.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return.</typeparam>
    internal interface IMethodCall<TReturn> : 
        IMethodCall, 
        IMethodCallOptions<TReturn>
    {
        /// <summary>
        /// Gets a value indicating whether the mock call is dependent on the parameters used to call it.
        /// </summary>
        bool ShouldIgnoreParameters { get; }

        /// <summary>
        /// Gets the expected result.
        /// </summary>
        TReturn ExpectedResult { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        MethodBase Method { get; }

        /// <summary>
        /// Gets the parameters used in the method call.
        /// </summary>
        IEnumerable<object> Parameters { get; }
    }
}
