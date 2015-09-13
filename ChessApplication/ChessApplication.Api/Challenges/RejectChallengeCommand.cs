﻿using System;
using Kingo.BuildingBlocks;
using Kingo.BuildingBlocks.Messaging;
using Kingo.BuildingBlocks.Messaging.Constraints;

namespace Kingo.ChessApplication.Challenges
{
    /// <summary>
    /// Represents a request to reject a challenge.
    /// </summary>
    public sealed class RejectChallengeCommand : Message<RejectChallengeCommand>
    {
        public readonly Guid ChallengeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RejectChallengeCommand" /> class.
        /// </summary>
        /// <param name="challengeId">Identifier of a challenge.</param>
        public RejectChallengeCommand(Guid challengeId)
        {
            ChallengeId = challengeId;
        }

        #region [====== Copy ======]

        private RejectChallengeCommand(RejectChallengeCommand message)
            : base(message)
        {
            ChallengeId = message.ChallengeId;
        }

        /// <inheritdoc />
        public override RejectChallengeCommand Copy()
        {
            return new RejectChallengeCommand(this);
        }

        #endregion

        #region [====== Equals & GetHashCode ======]

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as RejectChallengeCommand);
        }

        /// <inheritdoc />
        public bool Equals(RejectChallengeCommand other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            return ChallengeId.Equals(other.ChallengeId);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Of(ChallengeId);
        }

        #endregion

        #region [====== Validation ======]

        /// <inheritdoc />
        protected override IMessageValidator<RejectChallengeCommand> CreateValidator()
        {
            var validator = new ConstraintValidator<RejectChallengeCommand>();

            validator.VerifyThat(message => message.ChallengeId).IsNotEmpty();

            return validator;
        }

        #endregion
    }
}