using System;

namespace UptimeRobotDotnet.Exceptions
{
    /// <summary>
    /// Base exception for all UptimeRobot client errors.
    /// </summary>
    public class UptimeRobotException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotException"/> class.
        /// </summary>
        public UptimeRobotException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UptimeRobotException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotException"/> class with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UptimeRobotException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

