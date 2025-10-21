using System;

namespace UptimeRobotDotnet.Exceptions
{
    /// <summary>
    /// Exception thrown when the UptimeRobot API returns an error response.
    /// </summary>
    public class UptimeRobotApiException : UptimeRobotException
    {
        /// <summary>
        /// Gets the error type returned by the API.
        /// </summary>
        public string? ErrorType { get; }

        /// <summary>
        /// Gets the error message returned by the API.
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Gets the parameter name that caused the error, if applicable.
        /// </summary>
        public string? ParameterName { get; }

        /// <summary>
        /// Gets the raw error object returned by the API.
        /// </summary>
        public object? RawError { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotApiException"/> class.
        /// </summary>
        public UptimeRobotApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotApiException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UptimeRobotApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotApiException"/> class with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UptimeRobotApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotApiException"/> class with detailed error information.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="errorType">The error type returned by the API.</param>
        /// <param name="errorMessage">The error message returned by the API.</param>
        /// <param name="parameterName">The parameter name that caused the error.</param>
        /// <param name="rawError">The raw error object from the API response.</param>
        public UptimeRobotApiException(string message, string? errorType, string? errorMessage, string? parameterName, object? rawError)
            : base(message)
        {
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            ParameterName = parameterName;
            RawError = rawError;
        }
    }
}

