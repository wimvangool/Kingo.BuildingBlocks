﻿using System;
using Kingo.Resources;

namespace Kingo.Messaging.Validation
{
    /// <summary>
    /// Serves as a base-class for all validation tests for a specific request message.
    /// </summary>
    public abstract class RequestMessageTestBase : AutomatedTest
    {
        #region [====== ValidationResult ======]

        private sealed class ValidationResult : IValidationResult
        {
            private readonly RequestMessageTestBase _test;
            private readonly ErrorInfo _errorInfo;

            public ValidationResult(RequestMessageTestBase test, ErrorInfo errorInfo)
            {
                _test = test;
                _errorInfo = errorInfo;
            }

            public IValidationResult AssertErrorCountIs(int expectedErrorCount)
            {
                if (_errorInfo.ErrorCount == expectedErrorCount)
                {
                    return this;
                }
                throw NewUnexpectedErrorCountException(expectedErrorCount, _errorInfo.ErrorCount);
            }            

            public IValidationResult AssertInstanceError(string expectedErrorMessage = null, StringComparison comparison = StringComparison.Ordinal)
            {                
                return AssertInstanceError(actualErrorMessage =>
                {
                    if (AreEqual(expectedErrorMessage, actualErrorMessage, comparison))
                    {
                        return;
                    }
                    throw NewUnexpectedInstanceErrorException(expectedErrorMessage, _errorInfo.Error, comparison);
                });                
            }            

            public IValidationResult AssertInstanceError(Action<string> assertCallback)
            {
                if (assertCallback == null)
                {
                    throw new ArgumentNullException(nameof(assertCallback));
                }
                if (string.IsNullOrEmpty(_errorInfo.Error))
                {
                    throw NewNoInstanceErrorException();
                }
                assertCallback.Invoke(_errorInfo.Error);
                return this;
            }

            public IValidationResult AssertMemberError(string memberName, string expectedErrorMessage = null, StringComparison comparison = StringComparison.Ordinal)
            {
                return AssertMemberError(memberName, actualErrorMessage =>
                {
                    if (AreEqual(expectedErrorMessage, actualErrorMessage, comparison))
                    {
                        return;
                    }
                    throw NewUnexpectedMemberErrorException(memberName, expectedErrorMessage, actualErrorMessage, comparison);
                });
            }           

            public IValidationResult AssertMemberError(string memberName, Action<string> assertCallback)
            {
                if (memberName == null)
                {
                    throw new ArgumentNullException(nameof(memberName));
                }
                if (assertCallback == null)
                {
                    throw new ArgumentNullException(nameof(assertCallback));
                }
                string errorMessage;

                if (_errorInfo.MemberErrors.TryGetValue(memberName, out errorMessage))
                {
                    assertCallback.Invoke(errorMessage);
                    return this;
                }
                throw NewNoMemberErrorException(memberName);
            }            

            private static bool AreEqual(string expectedErrorMessage, string actualErrorMessage, StringComparison comparison) =>
                expectedErrorMessage == null || string.Compare(expectedErrorMessage, actualErrorMessage, comparison) == 0;

            private Exception NewUnexpectedErrorCountException(int expectedErrorCount, int actualErrorCount)
            {
                var messageFormat = ExceptionMessages.RequestMessageTest_UnexpectedErrorCount;
                var message = string.Format(messageFormat, expectedErrorCount, actualErrorCount);
                return _test.NewAssertFailedException(message);
            }

            private Exception NewNoInstanceErrorException() =>
                _test.NewAssertFailedException(ExceptionMessages.RequestMessageTest_NoInstanceError);

            private Exception NewNoMemberErrorException(string memberName)
            {
                var messageFormat = ExceptionMessages.RequestMessageTest_NoMemberError;
                var message = string.Format(messageFormat, memberName);
                return _test.NewAssertFailedException(message);
            }

            private Exception NewUnexpectedInstanceErrorException(string expectedErrorMessage, string actualErrorMessage, StringComparison comparison)
            {
                var messageFormat = ExceptionMessages.RequestMessageTest_UnexpectedInstanceError;
                var message = string.Format(messageFormat, expectedErrorMessage, actualErrorMessage, comparison);
                return _test.NewAssertFailedException(message);
            }

            private Exception NewUnexpectedMemberErrorException(string memberName, string expectedErrorMessage, string actualErrorMessage, StringComparison comparison)
            {
                var messageFormat = ExceptionMessages.RequestMessageTest_UnexpectedMemberError;
                var message = string.Format(messageFormat, memberName, expectedErrorMessage, actualErrorMessage, comparison);
                return _test.NewAssertFailedException(message);
            }            
        }

        #endregion

        /// <summary>
        /// Validates the specified <paramref name="message"/> and asserts that the message is valid.
        /// </summary>
        /// <param name="message">The message to validate.</param>
        /// <returns>The result of the validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        protected virtual void AssertIsValid(IRequestMessage message) =>
            Validate(message).AssertErrorCountIs(0);

        /// <summary>
        /// Validates the specified <paramref name="message"/> and asserts that the result contains a certain amount of errors.
        /// </summary>
        /// <param name="message">The message to validate.</param>
        /// <param name="expectedErrorCount">
        /// The expected amount of errors.
        /// </param>
        /// <returns>The result of the validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>  
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="expectedErrorCount" /> is a negative number.
        /// </exception>
        protected virtual IValidationResult AssertIsNotValid(IRequestMessage message, int expectedErrorCount)
        {
            if (expectedErrorCount <= 0)
            {
                throw NewInvalidErrorCountException(expectedErrorCount);
            }
            return Validate(message).AssertErrorCountIs(expectedErrorCount);
        }               

        private ValidationResult Validate(IRequestMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return new ValidationResult(this, message.Validate());
        }

        private static Exception NewInvalidErrorCountException(int expectedErrorCount)
        {
            var messageFormat = ExceptionMessages.RequestMessageTest_InvalidErrorCount;
            var message = string.Format(messageFormat, expectedErrorCount);
            return new ArgumentOutOfRangeException(nameof(expectedErrorCount), message);
        }
    }
}
