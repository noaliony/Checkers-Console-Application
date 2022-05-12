using System;
using System.Collections.Generic;

namespace CheckersLogicLibrary
{
    public class GameBoardModel
    {
        private GamePiece[,] m_GameBoard = null;
        private List<GamePiece> m_TeamOneGamePiecesList = new List<GamePiece>();
        private List<GamePiece> m_TeamTwoGamePiecesList = new List<GamePiece>();
        private int m_SizeOfGameBoard = 0;

        private GamePiece[,] GameBoard
        {
            get 
            { 
                return m_GameBoard;
            }

            set
            { 
                m_GameBoard = value; 
            }
        }

        internal List<GamePiece> TeamOneGamePiecesList
        {
            get
            {
                return m_TeamOneGamePiecesList;
            }
        }

        internal List<GamePiece> TeamTwoGamePiecesList
        {
            get
            {
                return m_TeamTwoGamePiecesList;
            }
        }

        internal int GameBoardSize
        {
            get 
            { 
                return m_SizeOfGameBoard; 
            }
            
            set 
            { 
                m_SizeOfGameBoard = value; 
            }
        }

        internal GameBoardModel(int i_SizeOfGameBoard)
        {
            GameBoardSize = i_SizeOfGameBoard;
            GameBoard = new GamePiece[GameBoardSize, GameBoardSize];
            initializeTeamOneGamePiecesOnBoard();
            initializeTeamTwoGamePiecesOnBoard();
        }

        private void initializeTeamOneGamePiecesOnBoard()
        {
            int indexOfTeamOneGamePiecesTopRow = (GameBoardSize / 2) + 1;

            for (int i = indexOfTeamOneGamePiecesTopRow; i < GameBoardSize; i++)
            {
                for (int j = 0; j < GameBoardSize; j += 2)
                {
                    if (i % 2 == 0 && j == 0)
                    {
                        j++;
                    }

                    GameBoard[i, j] = new GamePiece(new GameBoardCell(i, j), eTeamType.TeamOne);
                    TeamOneGamePiecesList.Add(GameBoard[i, j]);
                }
            }
        }

        private void initializeTeamTwoGamePiecesOnBoard()
        {
            int amountOfGamePiecesLinesPerTeam = (GameBoardSize / 2) - 1;

            for (int i = 0; i < amountOfGamePiecesLinesPerTeam; i++)
            {
                for (int j = 0; j < GameBoardSize; j += 2)
                {
                    if (i % 2 == 0 && j == 0)
                    {
                        j++;
                    }

                    GameBoard[i, j] = new GamePiece(new GameBoardCell(i, j), eTeamType.TeamTwo);
                    TeamTwoGamePiecesList.Add(GameBoard[i, j]);
                }
            }
        }

        internal GamePiece GetGamePieceFromBoardCell(GameBoardCell i_BoardCell)
        {
            return GameBoard[i_BoardCell.Row, i_BoardCell.Column];
        }

        internal void SetGamePieceInBoardCell(GameBoardCell i_BoardCell, GamePiece i_GamePieceToSet)
        {
            GameBoard[i_BoardCell.Row, i_BoardCell.Column] = i_GamePieceToSet;
        }

        internal bool IsEmptyCellOnBoard(GameBoardCell i_BoardCellToCheck)
        {
            return GetGamePieceFromBoardCell(i_BoardCellToCheck) == null;
        }

        internal void RemoveGamePieceFromBoard(GamePiece i_GamePieceToRemove)
        {
            GameBoard[i_GamePieceToRemove.CellOnBoard.Row, i_GamePieceToRemove.CellOnBoard.Column] = null;
            switch (i_GamePieceToRemove.TeamAssignment)
            {
                case eTeamType.TeamOne:
                    TeamOneGamePiecesList.Remove(i_GamePieceToRemove);
                    break;
                case eTeamType.TeamTwo:
                    TeamTwoGamePiecesList.Remove(i_GamePieceToRemove);
                    break;
            }
        }

        internal bool IsCellInBoardBounds(GameBoardCell i_CellToCheck)
        {
            bool cellInBoardBoundsResult;

            cellInBoardBoundsResult = i_CellToCheck.Column >= 0 && i_CellToCheck.Column < GameBoardSize;
            cellInBoardBoundsResult &= i_CellToCheck.Row >= 0 && i_CellToCheck.Row < GameBoardSize;

            return cellInBoardBoundsResult;
        }
    }
}