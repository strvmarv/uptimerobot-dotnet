using System;

namespace UptimeRobotDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when parameter validation fails before making an API request.
    /// </summary>
    public class UptimeRobotValidationException : UptimeRobotException
    {
        /// <summary>
        /// Gets the name of the parameter that failed validation.
        /// </summary>
        public string? ParameterName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotValidationException"/> class.
        /// </summary>
        public UptimeRobotValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotValidationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UptimeRobotValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotValidationException"/> class with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UptimeRobotValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotValidationException"/> class with a specified error message and parameter name.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="parameterName">The name of the parameter that failed validation.</param>
        public UptimeRobotValidationException(string message, string parameterName) : base(message)
        {
            ParameterName = parameterName;
        }
    }
}

