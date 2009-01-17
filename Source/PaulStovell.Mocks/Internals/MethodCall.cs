using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks.Internals
{
    /// <summary>
    /// Represents a recording of a call to a specific method on a mock object.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return.</typeparam>
    internal class MethodCall<TReturn> : IMethodCall<TReturn>
    {
        private MethodBase _method;
        private IEnumerable<object> _parameters;
        private bool _isRequired;
        private bool _shouldIgnoreParameters;
        private TReturn _expectedResult = default(TReturn);

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCall&lt;TReturn&gt;"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        public MethodCall(MethodBase method, params object[] parameters)
        {
            _method = method;
            _parameters = parameters;
        }

        /// <summary>
        /// Gets a value indicating whether the mock call is dependent on the parameters used to call it.
        /// </summary>
        /// <value></value>
        public bool ShouldIgnoreParameters
        {
            get { return _shouldIgnoreParameters; }
        }

        /// <summary>
        /// Gets the parameters used in the method call.
        /// </summary>
        /// <value></value>
        public IEnumerable<object> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value></value>
        public MethodBase Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Gets the expected result.
        /// </summary>
        /// <value></value>
        public TReturn ExpectedResult
        {
            get { return _expectedResult; }
        }

        /// <summary>
        /// Returnses the specified expected result.
        /// </summary>
        /// <param name="expectedResult">The expected result.</param>
        /// <returns></returns>
        public IMethodCallOptions<TReturn> Returns(TReturn expectedResult)
        {
            _expectedResult = expectedResult;
            return this;
        }

        /// <summary>
        /// Indicates that this method call is required for the tests to proceed.
        /// </summary>
        /// <returns></returns>
        public IMethodCallOptions<TReturn> Required()
        {
            _isRequired = true;
            return this;
        }

        /// <summary>
        /// Indicates that the call should ignore the values of parameters.
        /// </summary>
        /// <returns></returns>
        public IMethodCallOptions<TReturn> IgnoreParameters()
        {
            _shouldIgnoreParameters = true;
            return this;
        }
    }
}
