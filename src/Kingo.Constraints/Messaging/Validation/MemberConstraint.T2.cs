﻿namespace Kingo.Messaging.Validation
{
    internal sealed class MemberConstraint<TValueIn, TValueOut> : IMemberConstraint<TValueIn, TValueOut>
    {        
        private readonly IFilter<TValueIn, TValueOut> _constraint;
        private readonly MemberTransformer _transformer;      

        internal MemberConstraint(IFilter<TValueIn, TValueOut> constraint)
            : this(constraint, new MemberTransformer()) { }

        internal MemberConstraint(IFilter<TValueIn, TValueOut> constraint, MemberTransformer transformer)
        {            
            _constraint = constraint;
            _transformer = transformer;
        }

        public IMemberConstraint<TValueIn, TOther> And<TOther>(IMemberConstraint<TValueOut, TOther> constraint) =>
            new AndMemberConstraint<TValueIn, TValueOut, TOther>(this, constraint);

        public bool IsNotSatisfiedBy(Member<TValueIn> member, IErrorMessageCollection reader, out Member<TValueOut> transformedMember)
        {
            if (_constraint.IsNotSatisfiedBy(member.Value, out var errorMessage, out var valueOut))
            {
                errorMessage.Put(ErrorMessageBuilder.MemberIdentifier, member);
                member.WriteErrorMessageTo(reader, errorMessage);
                transformedMember = null;
                return true;
            }
            transformedMember = _transformer.Transform(member, valueOut);
            return false;
        }                     
    }
}
