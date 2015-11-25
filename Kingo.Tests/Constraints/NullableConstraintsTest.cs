﻿using Kingo.Clocks;
using Kingo.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.Constraints
{
    [TestClass]
    public sealed class NullableConstraintsTest : ConstraintTestBase
    {        
        #region [====== HasValue ======]

        [TestMethod]
        public void ValidateIsHasValue_ReturnsExpectedError_IfMemberIsNull()
        {
            var message = new ValidatedMessage<int?>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).HasValue(RandomErrorMessage);

            validator.Validate(message).AssertMemberError(RandomErrorMessage);
        }

        [TestMethod]
        public void ValidateIsHasValue_ReturnsDefaultError_IfMemberIsNull_And_NoErrorMessageIsSpecified()
        {
            var message = new ValidatedMessage<int?>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).HasValue();

            validator.Validate(message).AssertMemberError("Member (<null>) must have a value.");
        }

        [TestMethod]
        public void ValidateIsHasValue_ReturnsNoErrors_IfMemberIsHasValue()
        {
            var member = Clock.Current.UtcDateAndTime().Second;
            var message = new ValidatedMessage<int?>(member);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).HasValue(RandomErrorMessage);

            validator.Validate(message).AssertNoErrors();
        }

        [TestMethod]
        public void ValidateIsHasValue_ReturnsExpectedValue_IfMemberIsHasValue()
        {
            var member = Clock.Current.UtcDateAndTime().Second;
            var message = new ValidatedMessage<int?>(member);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member)
                .HasValue(RandomErrorMessage)
                .IsEqualTo(member, RandomErrorMessage);

            validator.Validate(message).AssertNoErrors();
        }

        #endregion
    }
}