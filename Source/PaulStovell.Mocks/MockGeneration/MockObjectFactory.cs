using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks.MockGeneration
{
    /// <summary>
    /// The mock object factory provides helper methods for creating mock objects that implement a given interface.
    /// </summary>
    internal static class MockObjectFactory
    {
        /// <summary>
        /// Creates a mock object implementing the <typeparamref name="TMock"/> interface.
        /// </summary>
        /// <typeparam name="TMock">The type of the mock.</typeparam>
        /// <param name="mockRecorder">A recorder passed to the mock.</param>
        /// <returns></returns>
        public static TMock CreateMock<TMock>(IMockRecorder mockRecorder)
        {
            return (TMock)CreateMock(typeof(TMock), mockRecorder);
        }

        /// <summary>
        /// Creates a mock object implementing the <paramref name="typeToMock"/> interface.
        /// </summary>
        /// <param name="typeToMock">The type of the mock.</param>
        /// <param name="mockRecorder">A recorder passed to the mock.</param>
        /// <returns></returns>
        public static object CreateMock(Type typeToMock, IMockRecorder mockRecorder)
        {
            MockObjectBuilder builder = new MockObjectBuilder(typeToMock);
            Type mockType = builder.GenerateType();
            object mockInstance = Activator.CreateInstance(mockType, mockRecorder);
            return mockInstance;
        }
    }
}
