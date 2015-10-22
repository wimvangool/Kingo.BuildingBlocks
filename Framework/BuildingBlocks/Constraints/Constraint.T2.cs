﻿using System;

namespace Kingo.BuildingBlocks.Constraints
{
    /// <summary>
    /// Provides a base class for the <see cref="IConstraint{T, S}"/> interface.
    /// </summary>
    /// <typeparam name="TValueIn">Type of the input (checked) value.</typeparam>
    /// <typeparam name="TValueOut">Type of the output value.</typeparam>
    public abstract class Constraint<TValueIn, TValueOut> : Constraint, IConstraintWithErrorMessage<TValueIn, TValueOut>
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint{T}" /> class.
        /// </summary>
        /// <param name="name">Name of this constraint.</param>
        /// <param name="errorMessage">Error message of this constraint.</param>  
        protected Constraint(StringTemplate errorMessage, Identifier name)
            : base(errorMessage, name) { }

        #region [====== Name & ErrorMessage ======]

        IConstraintWithErrorMessage<TValueIn> IConstraintWithErrorMessage<TValueIn>.WithName(string name)
        {
            return WithName(Identifier.ParseOrNull(name));
        }        

        IConstraintWithErrorMessage<TValueIn> IConstraintWithErrorMessage<TValueIn>.WithName(Identifier name)
        {
            return WithName(name);
        }

        IConstraintWithErrorMessage<TValueIn, TValueOut> IConstraintWithErrorMessage<TValueIn, TValueOut>.WithName(Identifier name)
        {
            return WithName(name);
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn, TValueOut> WithName(string name)
        {
            return WithName(Identifier.ParseOrNull(name));
        }

        /// <summary>
        /// Creates and returns a copy of this constraint, assigning the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">New name of the constraint.</param>
        /// <returns>A copy of this constraint with the specified <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>   
        public abstract IConstraintWithErrorMessage<TValueIn, TValueOut> WithName(Identifier name);

        IConstraintWithErrorMessage<TValueIn> IConstraintWithErrorMessage<TValueIn>.WithErrorMessage(string errorMessage)
        {
            return WithErrorMessage(StringTemplate.ParseOrNull(errorMessage));
        }        

        IConstraintWithErrorMessage<TValueIn> IConstraintWithErrorMessage<TValueIn>.WithErrorMessage(StringTemplate errorMessage)
        {
            return WithErrorMessage(errorMessage);
        }

        IConstraintWithErrorMessage<TValueIn, TValueOut> IConstraintWithErrorMessage<TValueIn, TValueOut>.WithErrorMessage(StringTemplate errorMessage)
        {
            return WithErrorMessage(errorMessage);
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn, TValueOut> WithErrorMessage(string errorMessage)
        {
            return WithErrorMessage(StringTemplate.ParseOrNull(errorMessage));
        }

        /// <summary>
        /// Creates and returns a copy of this constraint, assigning the specified <paramref name="errorMessage"/>.
        /// </summary>
        /// <param name="errorMessage">New error message of the constraint.</param>
        /// <returns>A copy of this constraint with the specified <paramref name="errorMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="errorMessage"/> is <c>null</c>.
        /// </exception>
        public abstract IConstraintWithErrorMessage<TValueIn, TValueOut> WithErrorMessage(StringTemplate errorMessage);

        #endregion

        #region [====== And, Or & Invert ======]

        /// <inheritdoc />
        public IConstraint<TValueIn> And(Func<TValueIn, bool> constraint, string errorMessage = null, string name = null)
        {
            return And(constraint, StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));
        }

        /// <inheritdoc />
        public IConstraint<TValueIn> And(Func<TValueIn, bool> constraint, StringTemplate errorMessage, Identifier name = null)
        {
            return And(new DelegateConstraint<TValueIn>(constraint, errorMessage, name));
        }

        /// <inheritdoc />
        public virtual IConstraint<TValueIn> And(IConstraint<TValueIn> constraint)
        {
            return new AndConstraint<TValueIn>(this, constraint);
        }

        /// <inheritdoc />
        public virtual IConstraint<TValueIn, TResult> And<TResult>(IConstraint<TValueOut, TResult> constraint)
        {
            return new AndConstraint<TValueIn, TValueOut, TResult>(this, constraint);
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn> Or(Func<TValueIn, bool> constraint, string errorMessage = null, string name = null)
        {
            return Or(constraint, StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn> Or(Func<TValueIn, bool> constraint, StringTemplate errorMessage, Identifier name = null)
        {
            return Or(new DelegateConstraint<TValueIn>(constraint, errorMessage, name));
        }

        /// <inheritdoc />
        public virtual IConstraintWithErrorMessage<TValueIn> Or(IConstraint<TValueIn> constraint)
        {
            return new OrConstraint<TValueIn>(this, constraint);
        }        

        IConstraint<TValueIn> IConstraint<TValueIn>.Invert()
        {
            return Invert();
        }

        IConstraint<TValueIn> IConstraint<TValueIn>.Invert(string errorMessage, string name)
        {
            return Invert(errorMessage, name);
        }

        /// <inheritdoc />
        IConstraint<TValueIn> IConstraint<TValueIn>.Invert(StringTemplate errorMessage, Identifier name)
        {
            return Invert(errorMessage, name);
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn> Invert()
        {
            return Invert(null as StringTemplate);
        }

        /// <inheritdoc />
        public IConstraintWithErrorMessage<TValueIn> Invert(string errorMessage, string name = null)
        {
            return Invert(StringTemplate.ParseOrNull(errorMessage), Identifier.ParseOrNull(name));
        }

        /// <inheritdoc />
        public virtual IConstraintWithErrorMessage<TValueIn> Invert(StringTemplate errorMessage, Identifier name = null)
        {
            return new ConstraintInverter<TValueIn>(this, errorMessage, name);
        }

        #endregion

        #region [====== MapInputToOutput ======]
       
        IConstraint<TValueIn, TValueIn> IConstraint<TValueIn>.MapInputToOutput()
        {
            return new InputToOutputMapper<TValueIn>(this);
        }

        #endregion

        #region [====== IsSatisfiedBy & IsNotSatisfiedBy ======]

        /// <inheritdoc />
        public bool IsSatisfiedBy(TValueIn value)
        {
            TValueOut valueOut;

            return IsSatisfiedBy(value, out valueOut);
        }

        /// <inheritdoc />
        public abstract bool IsSatisfiedBy(TValueIn valueIn, out TValueOut valueOut);

        /// <inheritdoc />
        public bool IsNotSatisfiedBy(TValueIn value, out IErrorMessage errorMessage)
        {
            TValueOut valueOut;

            return IsNotSatisfiedBy(value, out errorMessage, out valueOut);
        }

        /// <inheritdoc />
        public virtual bool IsNotSatisfiedBy(TValueIn valueIn, out IErrorMessage errorMessage, out TValueOut valueOut)
        {
            if (IsSatisfiedBy(valueIn, out valueOut))
            {
                errorMessage = null;
                return false;
            }
            errorMessage = new FailedConstraintMessage(this);
            return true;
        }

        #endregion
    }
}
