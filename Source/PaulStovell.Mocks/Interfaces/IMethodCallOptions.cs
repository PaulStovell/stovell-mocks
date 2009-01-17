using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaulStovell.Mocks.Interfaces
{
    /// <summary>
    /// Represents the various operations the mock object user can use to set options on a recording.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMethodCallOptions<T>
    {
        /// <summary>
        /// Sets the expected value of a mock method call.
        /// </summary>
        /// <param name="expectedReturnValue">The expected return value.</param>
        /// <returns></returns>
        IMethodCallOptions<T> Returns(T expectedReturnValue);
        /// <summary>
        /// Indicates that this method call is required for the tests to proceed.
        /// </summary>
        /// <returns></returns>
        IMethodCallOptions<T> Required();
        /// <summary>
        /// Indicates that the call should ignore the values of parameters.
        /// </summary>
        /// <returns></returns>
        IMethodCallOptions<T> IgnoreParameters();
    }
}
