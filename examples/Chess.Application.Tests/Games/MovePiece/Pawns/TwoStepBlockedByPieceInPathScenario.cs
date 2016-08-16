﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Kingo.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.Samples.Chess.Games.MovePiece.Pawns
{
    [TestClass]
    public sealed class TwoStepBlockedByPieceInPathScenario : MovePieceScenario
    {
        protected override IEnumerable<IMessageSequence> Given()
        {
            yield return base.Given().Concatenate();
            yield return WhitePlayerMove("d2", "d3");
            yield return BlackPlayerMove("e7", "e5");
            yield return WhitePlayerMove("d3", "d4");
            yield return BlackPlayerMove("e5", "e4");
            yield return WhitePlayerMove("d4", "d5");
            yield return BlackPlayerMove("e4", "e3");
        }

        protected override MessageToHandle<MovePieceCommand> When()
        {
            return WhitePlayerMove("e2", "e4");
        }

        [TestMethod]
        public override async Task ThenAsync()
        {
            await ExpectedCommandExecutionException();
        }
    }
}