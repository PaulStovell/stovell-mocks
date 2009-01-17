using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaulStovell.Mocks.MockGeneration;
using PaulStovell.Mocks.Interfaces;
using PaulStovell.Mocks.Internals;

namespace PaulStovell.Mocks
{
    /// <summary>
    /// A container for mock objects.
    /// </summary>
    public class MockRepository : IDisposable
    {
        private static MockRepository _current;
        private List<IMockRecorder> _mockRecorders;
        private IMethodCall _lastMethodCall;
        private MockRepository _previousCurrent;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRepository"/> class.
        /// </summary>
        public MockRepository()
        {
            _previousCurrent = MockRepository.Current;
            MockRepository.Current = this;
            _mockRecorders = new List<IMockRecorder>();
        }

        /// <summary>
        /// Gets or sets the current mock repository.
        /// </summary>
        /// <value>The last.</value>
        public static MockRepository Current
        {
            get { return _current; }
            private set { _current = value; }
        }

        /// <summary>
        /// Gets or sets the last method call on any mock object in this repository.
        /// </summary>
        internal IMethodCall LastMethodCall
        {
            get { return _lastMethodCall; }
            set { _lastMethodCall = value; }
        }

        /// <summary>
        /// Creates a mock object.
        /// </summary>
        /// <typeparam name="TMock">The type of object to mock.</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// This is where you'd use Reflection.Emit to generate a mock implementation
        /// </remarks>
        public TMock CreateMock<TMock>()
        {
            IMockRecorder recorder = new MockRecorder<TMock>();
            _mockRecorders.Add(recorder);

            return MockObjectFactory.CreateMock<TMock>(recorder);
        }

        /// <summary>
        /// Transitions each mock object in the repository into a replay state.
        /// </summary>
        public void ReplayAll()
        {
            foreach (IMockRecorder mock in _mockRecorders)
            {
                mock.Replay();
            }
        }

        /// <summary>
        /// Verifies all expectations were met.
        /// </summary>
        public void VerifyAll()
        {

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            MockRepository.Current = _previousCurrent;
            GC.SuppressFinalize(this);
        }
    }
}
