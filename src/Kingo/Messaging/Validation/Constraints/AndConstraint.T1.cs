﻿using System;
using System.Linq;

namespace Kingo.Messaging.Validation.Constraints
{
    internal sealed class AndConstraint<TValue> : IConstraint<TValue>
    {        
        private readonly IConstraint<TValue>[] _constraints;

        internal AndConstraint(IConstraint<TValue> left, IConstraint<TValue> constraint)
        {            
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }
            _constraints = new [] { left, constraint };
        }        

        private AndConstraint(AndConstraint<TValue> left, IConstraint<TValue> constraint)            
        {
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }
            _constraints = left._constraints.Add(constraint);
        }

        public void AcceptVisitor(IConstraintVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }
            visitor.VisitAnd(this, _constraints);
        }

        #region [====== And, Or & Invert ======]

        public IConstraint<TValue> And(Predicate<TValue> constraint, string errorMessage = null, string name = null) =>
            And(constraint, StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));

        public IConstraint<TValue> And(Predicate<TValue> constraint, StringTemplate errorMessage, Identifier name = null) =>
            And(new DelegateConstraint<TValue>(constraint).WithErrorMessage(errorMessage).WithName(name));

        public IConstraint<TValue> And(IConstraint<TValue> constraint) =>
            new AndConstraint<TValue>(this, constraint);

        public IConstraintWithErrorMessage<TValue> Or(Predicate<TValue> constraint, string errorMessage = null, string name = null) =>
            Or(constraint, StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));

        public IConstraintWithErrorMessage<TValue> Or(Predicate<TValue> constraint, StringTemplate errorMessage, Identifier name = null) =>
            Or(new DelegateConstraint<TValue>(constraint).WithErrorMessage(errorMessage).WithName(name));

        public IConstraintWithErrorMessage<TValue> Or(IConstraint<TValue> constraint) =>
            new OrConstraint<TValue>(this, constraint);

        public IConstraint<TValue> Invert() =>
            Invert(null as StringTemplate);

        public IConstraint<TValue> Invert(string errorMessage, string name = null) =>
            Invert(StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));

        public IConstraint<TValue> Invert(StringTemplate errorMessage, Identifier name = null)
        {
            return new OrConstraint<TValue>(_constraints.Select(constraint => constraint.Invert()))
                .WithErrorMessage(errorMessage)
                .WithName(name);
        }

        #endregion

        #region [====== Conversion ======]

        public IFilter<TValue, TValue> MapInputToOutput() =>
            new InputToOutputMapper<TValue>(this);

        public Predicate<TValue> ToDelegate() =>
            IsSatisfiedBy;

        #endregion

        #region [====== IsSatisfiedBy & IsNotSatisfiedBy ======]

        public bool IsSatisfiedBy(TValue value)
        {
            return _constraints.All(constraint => constraint.IsSatisfiedBy(value));
        }

        public bool IsNotSatisfiedBy(TValue value, out IErrorMessageBuilder errorMessage)
        {
            foreach (var constraint in _constraints)
            {
                if (constraint.IsNotSatisfiedBy(value, out errorMessage))
                {
                    return true;
                }
            }
            errorMessage = null;
            return false;
        }        

        #endregion        
    }
}