using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaulStovell.Mocks.Interfaces;
using PaulStovell.Mocks.Internals;

namespace PaulStovell.Mocks
{
    /// <summary>
    /// This class sets up expectations on methods.
    /// </summary>
    public static class Expect
    {
        /// <summary>
        /// Signals that a method or property call is expected on a given method.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="ignored">The method or property that is expected to be called.</param>
        public static IMethodCallOptions<TReturn> Call<TReturn>(TReturn ignored)
        {
            IMethodCall methodCall = MockRepository.Current.LastMethodCall;
            return (MethodCall<TReturn>)methodCall;
        }
    }
}
