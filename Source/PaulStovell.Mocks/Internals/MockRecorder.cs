using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks.Internals
{
    /// <summary>
    /// This class is used to record and replay calls to methods on the mock object.
    /// </summary>
    /// <typeparam name="TObjectToMock">The type of the object to mock.</typeparam>
    internal class MockRecorder<TObjectToMock> : IMockRecorder
    {
        private MockRepository _mockRepository;
        private MethodCallList _methodCalls;
        private bool _isRecording = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRecorder&lt;TObjectToMock&gt;"/> class.
        /// </summary>
        public MockRecorder()
        {
            _mockRepository = MockRepository.Current;
            _methodCalls = new MethodCallList();
        }

        /// <summary>
        /// Records or invokes the specified call, depending on the state of the recorder.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value of the method.</typeparam>
        /// <param name="methodInfo">The method being invoked.</param>
        /// <param name="arguments">Any arguments.</param>
        public TReturn MethodCall<TReturn>(MethodBase methodInfo, params object[] arguments)
        {
            TReturn result = default(TReturn);
            if (_isRecording)
            {
                IMethodCall methodCall = new MethodCall<TReturn>(methodInfo, arguments);
                _methodCalls.Add(methodCall);
                _mockRepository.LastMethodCall = methodCall;
            }
            else
            {
                IMethodCall<TReturn> call = _methodCalls.FindAppropriateCall<TReturn>(methodInfo, arguments);
                if (call != null)
                {
                    result = call.ExpectedResult;
                }
            }
            return result;
        }

        /// <summary>
        /// Replays the mock recording.
        /// </summary>
        public void Replay()
        {
            _isRecording = false;
        }
    }
}
