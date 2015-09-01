﻿using System.Threading.Tasks;
using Kingo.BuildingBlocks.Threading;

namespace Kingo.ChessApplication
{
    internal sealed class NullCommand : IUserCommand
    {
        public Task<bool> ExecuteWithAsync(IUserCommandProcessor processor)
        {
            return AsyncMethod.Value(true);
        }
    }
}
