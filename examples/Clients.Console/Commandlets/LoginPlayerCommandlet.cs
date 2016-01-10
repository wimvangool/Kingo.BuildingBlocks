﻿using System;
using System.Collections.Generic;
using Clients.ConsoleApp.Proxies;
using Clients.ConsoleApp.States;
using Kingo.Samples.Chess;
using Kingo.Samples.Chess.Players;

namespace Clients.ConsoleApp.Commandlets
{
    internal sealed class LoginPlayerCommandlet : Commandlet
    {
        private const string _PlayerArgument = "<player>";
        private readonly PlayerServiceProxy _playerService;
        private readonly ChessApplication _application;

        internal LoginPlayerCommandlet(ChessApplication application)
            : base("Login-Player", "Log in as a player.")
        {
            _playerService = new PlayerServiceProxy();
            _application = application;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, _PlayerArgument);
        }

        protected override IEnumerable<string> Arguments()
        {
            yield return _PlayerArgument;
        }

        internal override void Execute(IReadOnlyList<string> arguments)
        {            
            Execute(arguments[0]);
        }

        internal void Execute(string enteredName)
        {
            RegisteredPlayer player;

            if (_playerService.TryGetRegisteredPlayer(enteredName, out player))
            {
                var session = Session.CreateSessionScope(player.Id, player.Name);
                var loggedInState = new LoggedInState(_application, session);

                _application.SwitchTo(loggedInState);
            }
            else
            {
                using (ChessApplication.UseColor(ConsoleColor.Red))
                {
                    Console.WriteLine("Cannot log in as player '{0}' because this player has not been registered.", enteredName);
                }
            }
        }        
    }
}
