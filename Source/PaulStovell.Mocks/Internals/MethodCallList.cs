using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks.Internals
{
    /// <summary>
    /// Manages a list of recorded method calls for a mock object instance.
    /// </summary>
    internal class MethodCallList
    {
        private List<IMethodCall> _calls;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCallList"/> class.
        /// </summary>
        public MethodCallList()
        {
            _calls = new List<IMethodCall>();
        }

        /// <summary>
        /// Adds the specified mock object method call.
        /// </summary>
        /// <param name="call">The call.</param>
        public void Add(IMethodCall call)
        {
            _calls.Add(call);
        }

        /// <summary>
        /// Finds the call information for a given method and parameters.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IMethodCall<TReturn> FindAppropriateCall<TReturn>(MethodBase methodInfo, IEnumerable<object> parameters)
        {
            // Find the first recorded call where the parameters completely match
            IMethodCall<TReturn> result = _calls
                .Where(c => c is IMethodCall<TReturn>)
                .Cast<IMethodCall<TReturn>>()
                .Where(c => c.Method == methodInfo && c.Parameters.SequenceEqual(parameters))
                .FirstOrDefault();

            if (result == null)
            {
                // We couldn't find an exact match, so drop back to the first recorded call for the method
                // which doesn't care about parameters
                result = _calls
                    .Where(c => c is IMethodCall<TReturn>)
                    .Cast<IMethodCall<TReturn>>()
                    .Where(c => c.Method == methodInfo && c.ShouldIgnoreParameters)
                    .FirstOrDefault();
            }
            return result;
        }
    }
}
