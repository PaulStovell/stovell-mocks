using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaulStovell.Mocks
{
    /// <summary>
    /// Used to assert as part of the tests.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Checks that two values are equal.
        /// </summary>
        public static void AreEqual(object expected, object actual, string message)
        {
            RecordAssert(expected, actual, expected.Equals(actual), message);
        }

        private static void RecordAssert(object expected, object actual, bool assertion, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Assert: " + message.PadRight(50));
            if (assertion)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Passed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed");
                Console.WriteLine("   Expected value: {0}", expected);
                Console.WriteLine("   Actual value:   {0}", actual);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
