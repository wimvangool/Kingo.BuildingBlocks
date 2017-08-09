﻿namespace Kingo.Messaging.Validation
{
    internal sealed class CompositeValidator : IRequestMessageValidator
    {
        private readonly IRequestMessageValidator _leftValidator;
        private readonly IRequestMessageValidator _rightValidator;

        public CompositeValidator(IRequestMessageValidator leftValidator, IRequestMessageValidator rightValidator)
        {
            _leftValidator = leftValidator;
            _rightValidator = rightValidator;
        }

        public override string ToString() =>
            $"{_leftValidator}-{_rightValidator}";

        public IRequestMessageValidator Append<TMessage>(IRequestMessageValidator<TMessage> validator) =>
            new CompositeValidator(this, new RequestRequestMessageValidator<TMessage>(validator));

        public ErrorInfo Validate(object message, bool haltOnFirstError = false)
        {
            var errorInfo = _leftValidator.Validate(message);
            if (errorInfo.ErrorCount > 0 && haltOnFirstError)
            {
                return errorInfo;
            }
            return ErrorInfo.Merge(errorInfo, _rightValidator.Validate(message, haltOnFirstError));
        }
    }
}
