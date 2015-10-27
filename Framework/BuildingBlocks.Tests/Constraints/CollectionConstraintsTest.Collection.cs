﻿using System;
using System.Collections.Generic;
using Kingo.BuildingBlocks.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.BuildingBlocks.Constraints
{
    [TestClass]
    public sealed class CollectionConstraintsTest : ConstraintTestBase
    {        
        #region [====== IsNotNullOrEmpty ======]

        [TestMethod]
        public void ValidateIsNotNullOrEmpty_ReturnsExpectedError_IfCollectionIsNull()
        {
            var message = new ValidatedMessage<ICollection<object>>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNotNullOrEmpty(RandomErrorMessage);

            validator.Validate(message).AssertOneError(RandomErrorMessage);
        }

        [TestMethod]
        public void ValidateIsNotNullOrEmpty_ReturnsDefaultError_IfCollectionIsNull_And_NoErrorMessageIsSpecified()
        {
            var message = new ValidatedMessage<ICollection<object>>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNotNullOrEmpty();

            validator.Validate(message).AssertOneError("Member (<null>) must not be null and contain at least one element.");
        }

        [TestMethod]
        public void ValidateIsNotNullOrEmpty_ReturnsExpectedError_IfCollectionIsEmpty()
        {
            var message = new ValidatedMessage<ICollection<object>>(new object[0]);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNotNullOrEmpty(RandomErrorMessage);

            validator.Validate(message).AssertOneError(RandomErrorMessage);
        }

        [TestMethod]
        public void ValidateIsNotNullOrEmpty_ReturnsDefaultError_IfCollectionIsEmpty_And_NoErrorMessageIsSpecified()
        {
            var message = new ValidatedMessage<ICollection<object>>(new object[0]);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNotNullOrEmpty();

            validator.Validate(message).AssertOneError("Member (System.Object[]) must not be null and contain at least one element.");
        }

        [TestMethod]
        public void ValidateIsNotNullOrEmpty_ReturnsNoErrors_IfCollectionHasOneElement()
        {
            var message = new ValidatedMessage<ICollection<object>>(new[] { new object() });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNotNullOrEmpty(RandomErrorMessage);

            validator.Validate(message).AssertNoErrors();
        }

        #endregion

        #region [====== IsNullOrEmpty ======]

        [TestMethod]
        public void ValidateIsNullOrEmpty_ReturnsNoErrors_IfCollectionIsNull()
        {
            var message = new ValidatedMessage<ICollection<object>>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNullOrEmpty();

            validator.Validate(message).AssertNoErrors();
        }       

        [TestMethod]
        public void ValidateIsNullOrEmpty_ReturnsNoErrors_IfCollectionIsEmpty()
        {
            var message = new ValidatedMessage<ICollection<object>>(new object[0]);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNullOrEmpty();

            validator.Validate(message).AssertNoErrors();
        }        

        [TestMethod]
        public void ValidateIsNullOrEmpty_ReturnsExpectedError_IfCollectionHasOneElement()
        {
            var message = new ValidatedMessage<ICollection<object>>(new[] { new object() });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNullOrEmpty(RandomErrorMessage);

            validator.Validate(message).AssertOneError(RandomErrorMessage);
        }

        [TestMethod]
        public void ValidateIsNullOrEmpty_ReturnsDefaultError_IfCollectionHasOneElement_And_NoErrorMessageIsSpecified()
        {
            var message = new ValidatedMessage<ICollection<object>>(new[] { new object() });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).IsNullOrEmpty();

            validator.Validate(message).AssertOneError("Member (1 item(s)) must be null or empty.");
        }

        #endregion

        #region [====== ElementAt ======]

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidateElementAt_Throws_IfIndexIsNegative()
        {
            var message = new ValidatedMessage<ICollection<object>>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).ElementAt(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateElementAt_Throws_IfCollectionIsNull()
        {
            var message = new ValidatedMessage<ICollection<object>>(null);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).ElementAt(0);

            validator.Validate(message);
        }

        [TestMethod]
        public void ValidateElementAt_ReturnsExpectedError_IfCollectionIsEmpty()
        {
            var message = new ValidatedMessage<ICollection<object>>(new object[0]);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).ElementAt(0, RandomErrorMessage);

            validator.Validate(message).AssertOneError(RandomErrorMessage);
        }

        [TestMethod]
        public void ValidateElementAt_ReturnsDefaultError_IfCollectionIsEmpty_And_NoErrorMessageIsSpecified()
        {
            var message = new ValidatedMessage<ICollection<object>>(new object[0]);
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).ElementAt(0);

            validator.Validate(message).AssertOneError("Member (0 item(s)) contains no element at index 0.");
        }

        [TestMethod]
        public void ValidateElementAt_ReturnsNoErrors_IfCollectionContainsElement_At_Index0()
        {
            var value = new object();
            var message = new ValidatedMessage<ICollection<object>>(new[] { value });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member)
                .ElementAt(0, RandomErrorMessage)
                .IsSameInstanceAs(value, RandomErrorMessage);

            validator.Validate(message).AssertNoErrors();
        }

        [TestMethod]
        public void ValidateElementAt_ReturnsNoErrors_IfCollectionContainsElement_At_Index1()
        {
            var value = new object();
            var message = new ValidatedMessage<ICollection<object>>(new[] { null, value, null });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member)
                .ElementAt(1, RandomErrorMessage)
                .IsSameInstanceAs(value, RandomErrorMessage);

            validator.Validate(message).AssertNoErrors();
        }

        [TestMethod]
        public void ValidateElementAt_ChangesMemberNameAsExpected()
        {
            var value = new object();
            var message = new ValidatedMessage<ICollection<object>>(new[] { value });
            var validator = message.CreateConstraintValidator();

            validator.VerifyThat(m => m.Member).ElementAt(0, RandomErrorMessage).IsNull();

            validator.Validate(message).AssertOneError("Member[0] (System.Object) must be null.", "Member[0]");
        }

        #endregion
    }
}