using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks
{
    /// <summary>
    /// Extension methods to make mocking feel more natural.
    /// </summary>
    public static class ExpectExtensions
    {
        /// <summary>
        /// Signals that a method or property call is expected on a given method.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="ignored">The method or property that is expected to be called.</param>
        public static IMethodCallOptions<TReturn> WillReturn<TReturn>(this TReturn ignored, TReturn result)
        {
            return Expect.Call(ignored).Returns(result);
        }
    }
}
