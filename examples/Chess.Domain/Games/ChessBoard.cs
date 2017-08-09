﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kingo.Messaging.Domain;
using Kingo.Samples.Chess.Resources;

namespace Kingo.Samples.Chess.Games
{
    internal sealed class ChessBoard
    {
        internal const int Size = 8;
        
        private readonly ChessPiece[,] _pieces;
        private readonly bool _isSimulated;

        private ChessBoard(ChessPiece[,] pieces, bool isSimulated = false)
        {            
            _pieces = pieces;
            _isSimulated = isSimulated;
        }

        #region [====== ToString ======]

        public override string ToString()
        {
            var board = $"W: {CountPieces(ColorOfPiece.White)} piece(s), B: {CountPieces(ColorOfPiece.Black)} piece(s)";

            if (_isSimulated)
            {
                return board + " [Simulated]";
            }
            return board;
        }        

        private int CountPieces(ColorOfPiece color)
        {
            return EnumeratePieces(_pieces, color).Count();
        }

        private static IEnumerable<Tuple<Square, ChessPiece>> EnumeratePieces(ChessPiece[,] pieces, ColorOfPiece color)
        {
            return EnumeratePieces(pieces).Where(piece => piece.Item2.HasColor(color));
        }

        private static IEnumerable<Tuple<Square, ChessPiece>> EnumeratePieces(ChessPiece[,] pieces)
        {
            for (int fileIndex = 0; fileIndex < Size; fileIndex++)
            {
                for (int rankIndex = 0; rankIndex < Size; rankIndex++)
                {
                    var piece = pieces[fileIndex, rankIndex];
                    if (piece != null)
                    {
                        yield return new Tuple<Square, ChessPiece>(new Square(fileIndex, rankIndex), piece);
                    }
                }
            }
        }

        #endregion

        #region [====== IsEmpty & SelectPiece ======]

        public bool IsEmpty(Square square)
        {
            return SelectPiece(square) == null;
        }

        public ChessPiece SelectPiece(Square square)
        {
            return _pieces[square.FileIndex, square.RankIndex];          
        }

        #endregion

        #region [====== MovePiece ======]

        public void MovePiece(Move move, ColorOfPiece colorOfKing)
        {
            var piece = SelectPiece(move.From);
            if (piece == null)
            {
                throw NewEmptySquareException(move.From);
            }
            if (piece.HasColor(colorOfKing))
            {
                piece.Move(this, move);
                return;
            }
            throw NewWrongColorException(move.From, colorOfKing, colorOfKing.Invert());
        }

        public GameState SimulateMove(Move move, ColorOfPiece colorOfKing)
        {
            return ApplyMove(move).DetermineNewState(colorOfKing);
        }

        public GameState SimulateEnPassantMove(Move move, Square enPassantHit, ColorOfPiece colorOfKing)
        {
            return ApplyEnPassantMove(move, enPassantHit).DetermineNewState(colorOfKing);
        }

        public GameState SimulateCastlingMove(Move moveOfKing, Move moveOfRook, ColorOfPiece colorOfKing)
        {
            return ApplyCastlingMove(moveOfKing, moveOfRook).DetermineNewState(colorOfKing);
        }

        private static Exception NewEmptySquareException(Square square)
        {
            var messageFormat = ExceptionMessages.Game_EmptySquare;
            var message = string.Format(messageFormat, square);
            return new IllegalOperationException(message);
        }

        private static Exception NewWrongColorException(Square square, ColorOfPiece expectedColor, ColorOfPiece actualColor)
        {
            var messageFormat = ExceptionMessages.Game_WrongColor;
            var message = string.Format(messageFormat, square, expectedColor, actualColor);
            return new IllegalOperationException(message);
        }

        #endregion        

        #region [====== ApplyMove ======]

        public ChessBoard ApplyMove(Square from, Square to)
        {
            return ApplyMove(Move.Calculate(from, to));
        }

        private ChessBoard ApplyMove(Move move)
        {
            return ApplyPieces(current =>
            {
                if (current.Equals(move.From))
                {
                    return null;
                }
                if (current.Equals(move.To))
                {
                    return SelectPiece(move.From).ApplyMove(move);
                }
                return SelectPiece(current)?.RemainInPlace();
            });
        }

        public ChessBoard ApplyEnPassantMove(Square from, Square to, Square enPassantHit)
        {
            return ApplyEnPassantMove(Move.Calculate(from, to), enPassantHit);
        }

        internal ChessBoard ApplyEnPassantMove(Move move, Square enPassantHit)
        {
            return ApplyPieces(current =>
            {
                if (current.Equals(move.From) || current.Equals(enPassantHit))
                {
                    return null;
                }
                if (current.Equals(move.To))
                {
                    return SelectPiece(move.From).ApplyMove(move);
                }
                return SelectPiece(current)?.RemainInPlace();
            });
        }

        public ChessBoard ApplyCastlingMove(Square kingFrom, Square kingTo, Square rookFrom, Square rookTo)
        {
            return ApplyCastlingMove(Move.Calculate(kingFrom, kingTo), Move.Calculate(rookFrom, rookTo));
        }

        private ChessBoard ApplyCastlingMove(Move moveOfKing, Move moveOfRook)
        {
            return ApplyPieces(current =>
            {
                if (current.Equals(moveOfKing.From) || current.Equals(moveOfKing.From))
                {
                    return null;
                }
                if (current.Equals(moveOfKing.To))
                {
                    return SelectPiece(moveOfKing.From).ApplyMove(moveOfKing);
                }
                if (current.Equals(moveOfRook.To))
                {
                    return SelectPiece(moveOfRook.From).ApplyMove(moveOfRook);
                }
                return SelectPiece(current)?.RemainInPlace();
            });
        }

        #endregion

        #region [====== PromotePawn ======]

        public void PromotePawn(TypeOfPiece promoteTo, ColorOfPiece colorOfKing)
        {
            var pawnAtPosition = FindPawnToPromote(colorOfKing);
            var pawn = pawnAtPosition.Item2;
            var position = pawnAtPosition.Item1;

            pawn.PromoteTo(this, position, promoteTo);
        }

        private Tuple<Square, ChessPiece> FindPawnToPromote(ColorOfPiece colorOfPawn)
        {
            var pawns =
                from pieceAtPosition in EnumeratePieces(_pieces, colorOfPawn)
                where pieceAtPosition.Item1.IsEightRank(colorOfPawn)
                where pieceAtPosition.Item2.IsOfType(TypeOfPiece.Pawn)
                select pieceAtPosition;

            return pawns.Single();
        }

        public GameState SimulatePawnPromotion(Square pawnPosition, TypeOfPiece promoteTo, ColorOfPiece colorOfPawn)
        {
            return ApplyPawnPromotion(pawnPosition, promoteTo).DetermineNewState(colorOfPawn);
        }

        #endregion

        #region [====== ApplyPawnPromotion ======]

        public ChessBoard ApplyPawnPromotion(Square pawnPosition, TypeOfPiece promoteTo)
        {
            return ApplyPieces(current =>
            {
                if (current.Equals(pawnPosition))
                {
                    return SelectPiece(pawnPosition).ApplyPromotion(promoteTo);
                }
                return SelectPiece(current)?.RemainInPlace();
            });
        }

        #endregion

        #region [====== State ======]

        private GameState DetermineNewState(ColorOfPiece colorOfKing)
        {
            if (IsInCheck(colorOfKing))
            {
                return GameState.Error;
            }

            // When this board is simulated, we only check whether or not the board is in a valid state.
            // This is mainly to prevent an endless recursive simulation of moves.
            if (_isSimulated)
            {
                return GameState.NoError;
            }
            var colorOfOtherKing = colorOfKing.Invert();

            if (IsInCheck(colorOfOtherKing))
            {
                if (AnyMoveResultsInCheckOfOwnKing(colorOfOtherKing))
                {
                    return GameState.CheckMate;
                }
                return GameState.Check;
            }
            if (AnyMoveResultsInCheckOfOwnKing(colorOfOtherKing))
            {
                return GameState.StaleMate;
            }
            return GameState.Normal;
        }

        private bool IsInCheck(ColorOfPiece colorOfKing)
        {
            return CanAnyPieceMoveTo(FindSquareOfKing(colorOfKing), colorOfKing.Invert());
        }

        private Square FindSquareOfKing(ColorOfPiece colorOfKing)
        {
            var piecesOnBoard =
                from pieceOnBoard in EnumeratePieces(_pieces, colorOfKing)
                where pieceOnBoard.Item2.IsOfType(TypeOfPiece.King)
                select pieceOnBoard.Item1;

            return piecesOnBoard.Single();            
        }

        internal bool CanAnyPieceMoveTo(Square squareOfKing, ColorOfPiece colorOfPiece)
        {           
            var board = new ChessBoard(_pieces, true);

            foreach (var pieceOnBoard in EnumeratePieces(_pieces, colorOfPiece))
            {                
                var from = pieceOnBoard.Item1;
                var piece = pieceOnBoard.Item2;

                if (piece.IsSupportedMove(board, Move.Calculate(@from, squareOfKing)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool AnyMoveResultsInCheckOfOwnKing(ColorOfPiece colorOfKing)
        {
            var board = new ChessBoard(_pieces, true);

            foreach (var pieceOnBoard in EnumeratePieces(_pieces, colorOfKing))
            {
                var from = pieceOnBoard.Item1;
                var piece = pieceOnBoard.Item2;

                if (piece.CanMove(board, @from))
                {
                    return false;
                }
            }
            return true;
        }

        private ChessBoard ApplyPieces(Func<Square, ChessPiece> pieceFactory)
        {
            var piecesAfterMove = new ChessPiece[Size, Size];

            for (int fileIndex = 0; fileIndex < Size; fileIndex++)
            {
                for (int rankIndex = 0; rankIndex < Size; rankIndex++)
                {
                    piecesAfterMove[fileIndex, rankIndex] = pieceFactory.Invoke(new Square(fileIndex, rankIndex));
                }
            }
            return new ChessBoard(piecesAfterMove, _isSimulated);
        }

        #endregion

        #region [====== New Game ======]

        public static ChessBoard SetupNewGame(Game game)
        {
            var pieces = new ChessPiece[Size, Size];

            AddWhitePiecesTo(game, pieces);
            AddBlackPiecesTo(game, pieces);

            return new ChessBoard(pieces);
        } 

        private static void AddWhitePiecesTo(Game game, ChessPiece[,] pieces)
        {            
            AddRegularPiecesTo(game, pieces, ColorOfPiece.White, 0);
            AddPawnsTo(game, pieces, ColorOfPiece.White, 1);           
        }

        private static void AddBlackPiecesTo(Game game, ChessPiece[,] pieces)
        {
            AddRegularPiecesTo(game, pieces, ColorOfPiece.Black, 7);
            AddPawnsTo(game, pieces, ColorOfPiece.Black, 6);
        }

        private static void AddPawnsTo(Game game, ChessPiece[,] pieces, ColorOfPiece color, int rankIndex)
        {
            for (int fileIndex = 0; fileIndex < Size; fileIndex++)
            {
                pieces[fileIndex, rankIndex] = new Pawn(game, color);
            }
        }
        
        private static void AddRegularPiecesTo(Game game, ChessPiece[,] pieces, ColorOfPiece color, int rankIndex)
        {
            pieces[0, rankIndex] = new Rook(game, color);
            pieces[1, rankIndex] = new Knight(game, color);
            pieces[2, rankIndex] = new Bishop(game, color);
            pieces[3, rankIndex] = new Queen(game, color);
            pieces[4, rankIndex] = new King(game, color);
            pieces[5, rankIndex] = new Bishop(game, color);
            pieces[6, rankIndex] = new Knight(game, color);
            pieces[7, rankIndex] = new Rook(game, color);
        }

        #endregion
    }
}
