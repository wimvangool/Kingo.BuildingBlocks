﻿using System;
using Kingo.Messaging.Domain;

namespace Kingo.Samples.Chess.Games
{
    internal sealed class King : Piece
    {
        public King(Game game, ColorOfPiece color)
        {
            EventBus = game;  
            Color = color;
        }

        protected override IDomainEventBus<Guid, int> EventBus
        {
            get;
        }

        protected override ColorOfPiece Color
        {
            get;
        }       
    }
}
