using System;
using System.Reflection;

namespace PaulStovell.Mocks.Interfaces
{
    /// <summary>
    /// This interface is implemented by an object supplied to mock objects for them to record and replay method calls.
    /// </summary>
    public interface IMockRecorder
    {
        /// <summary>
        /// Records or invokes the specified call, depending on the state of the recorder.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value of the method.</typeparam>
        /// <param name="methodInfo">The method being invoked.</param>
        /// <param name="arguments">Any arguments.</param>
        TReturn MethodCall<TReturn>(MethodBase methodInfo, params object[] arguments);

        /// <summary>
        /// Replays the mock recording.
        /// </summary>
        void Replay();
    }
}
