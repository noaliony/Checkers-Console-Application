using System;
using System.Collections.Generic;

namespace CheckersLogicLibrary
{
    public class GameManager
    {
        private eGameState m_GameState = eGameState.NotStarted;
        private eTeamType m_TeamForCurrentTurn = eTeamType.TeamOne;
        private GameBoardModel m_GameBoard = null;
        private int m_GameBoardSize = 0;
        private string m_TeamOneName = string.Empty;
        private string m_TeamTwoName = string.Empty;
        private int m_TeamOneScore = 0;
        private int m_TeamTwoScore = 0;
        private bool m_IsSinglePlayerGame = false;
        private List<GamePiece> m_SpecificValidGamePiecesListForNextTurn = new List<GamePiece>();

        public eGameState GameState
        {
            get
            {
                return m_GameState;
            }

            private set 
            { 
                m_GameState = value;
            }
        }

        public eTeamType TeamForCurrentTurn
        {
            get 
            { 
                return m_TeamForCurrentTurn; 
            }

            private set 
            { 
                m_TeamForCurrentTurn = value;
            }
        }

        private GameBoardModel GameBoard
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

        public int GameBoardSize
        {
            get
            {
                return m_GameBoardSize;
            }

            private set
            {
                m_GameBoardSize = value;
            }
        }

        public string TeamOneName
        {
            get
            {
                return m_TeamOneName;
            }

            private set
            {
                m_TeamOneName = value;
            }
        }

        public string TeamTwoName
        {
            get
            {
                return m_TeamTwoName;
            }

            private set
            {
                m_TeamTwoName = value;
            }
        }

        public int TeamOneScore
        {
            get
            {
                return m_TeamOneScore;
            }

            private set 
            { 
                m_TeamOneScore = value;
            }
        }

        public int TeamTwoScore
        {
            get
            {
                return m_TeamTwoScore;
            }

            private set
            {
                m_TeamTwoScore = value;
            }
        }

        public bool IsSinglePlayerGame
        {
            get
            {
                return m_IsSinglePlayerGame;
            }

            private set
            {
                m_IsSinglePlayerGame = value;
            }
        }

        private List<GamePiece> SpecificValidGamePiecesListForNextTurn
        {
            get 
            { 
                return m_SpecificValidGamePiecesListForNextTurn; 
            }
        }

        public GameManager(int i_GameBoardSize, string i_TeamOneName, string i_TeamTwoName, bool i_IsSinglePlayerGame)
        {
            GameBoardSize = i_GameBoardSize;
            TeamOneName = i_TeamOneName;
            TeamTwoName = i_TeamTwoName;
            IsSinglePlayerGame = i_IsSinglePlayerGame;
            GameBoard = new GameBoardModel(GameBoardSize);
            GameState = eGameState.InProgress;
        }

        private List<GameBoardCell> createAvailableMovesListForGamePiece(GamePiece i_GamePieceToCheck)
        {
            List<GameBoardCell> listOfAvailableMoves = new List<GameBoardCell>();

            for (int i = i_GamePieceToCheck.CellOnBoard.Row - 1; i <= i_GamePieceToCheck.CellOnBoard.Row + 1; i += 2)
            {
                for (int j = i_GamePieceToCheck.CellOnBoard.Column - 1; j <= i_GamePieceToCheck.CellOnBoard.Column + 1; j += 2)
                {
                    GameBoardCell currentCellToCheck = new GameBoardCell(i, j);

                    if (i_GamePieceToCheck.IsMovementInValidDirection(currentCellToCheck) && GameBoard.IsCellInBoardBounds(currentCellToCheck))
                    {
                        GameBoardCell cellDiagonallyAcross = GameBoardCell.GetCellDiagonallyAcross(i_GamePieceToCheck.CellOnBoard, currentCellToCheck);

                        if (GameBoard.IsEmptyCellOnBoard(currentCellToCheck))
                        {
                            listOfAvailableMoves.Add(currentCellToCheck);
                        }
                        else if (canGamePieceCaptureReceivedGamePiece(i_GamePieceToCheck, GameBoard.GetGamePieceFromBoardCell(currentCellToCheck)))
                        {
                            listOfAvailableMoves.Add(cellDiagonallyAcross);
                        }
                    }
                }
            }

            return listOfAvailableMoves;
        }

        private bool canGamePieceCaptureReceivedGamePiece(GamePiece i_GamePieceToCapture, GamePiece i_GamePieceToBeCaptured)
        {
            bool canGamePieceCaptureResult;
            GameBoardCell cellDiagonallyAcross = GameBoardCell.GetCellDiagonallyAcross(i_GamePieceToCapture.CellOnBoard, i_GamePieceToBeCaptured.CellOnBoard);

            canGamePieceCaptureResult = GameBoard.IsCellInBoardBounds(cellDiagonallyAcross) && !i_GamePieceToCapture.CheckIfSameTeamPieces(i_GamePieceToBeCaptured) && GameBoard.IsEmptyCellOnBoard(cellDiagonallyAcross);

            return canGamePieceCaptureResult;
        }

        private List<GameBoardCell> createAvailableCapturesListForGamePiece(GamePiece i_GamePieceToCheck)
        {
            List<GameBoardCell> listOfAvailableMovesForGamePiece = createAvailableMovesListForGamePiece(i_GamePieceToCheck);
            List<GameBoardCell> listOfAvailableCapturesForGamePiece = new List<GameBoardCell>();

            foreach (GameBoardCell currentCellToCheck in listOfAvailableMovesForGamePiece)
            {
                if (System.Math.Abs(i_GamePieceToCheck.CellOnBoard.CalculateDeltaOfColumns(currentCellToCheck)) > 1)
                {
                    listOfAvailableCapturesForGamePiece.Add(currentCellToCheck);
                }
            }

            return listOfAvailableCapturesForGamePiece;
        }

        public bool IsGameBoardCellOccupied(GameBoardCell i_SourceCell)
        {
            return !GameBoard.IsEmptyCellOnBoard(i_SourceCell);
        }

        public eTeamType GetTeamAssignmentOfGamePieceInBoardCell(GameBoardCell i_SourceCell)
        {
            return GameBoard.GetGamePieceFromBoardCell(i_SourceCell).TeamAssignment;
        }

        public bool IsGamePieceInBoardCellKing(GameBoardCell i_SourceCell)
        {
            return GameBoard.GetGamePieceFromBoardCell(i_SourceCell).IsKing;
        }

        private bool isGamePieceSpecificValidGamePieceForNextTurn(GamePiece i_GamePieceToCheck)
        {
            bool isGamePieceSpecificValidGamePieceForNextTurnResult = false;

            foreach (GamePiece currentGamePieceToCheckInValidPiecesForNextTurn in SpecificValidGamePiecesListForNextTurn)
            {
                if (i_GamePieceToCheck == currentGamePieceToCheckInValidPiecesForNextTurn)
                {
                    isGamePieceSpecificValidGamePieceForNextTurnResult = true;
                    break;
                }
            }
            
            return isGamePieceSpecificValidGamePieceForNextTurnResult;
        }

        public bool IsAttemptedMovementValid(GameBoardCell i_SourceCell, GameBoardCell i_DestinationCell)
        {
            bool isAttemptedMovementValidResult = false;
            GamePiece gamePieceToMove = GameBoard.GetGamePieceFromBoardCell(i_SourceCell);

            if (gamePieceToMove != null && gamePieceToMove.TeamAssignment == TeamForCurrentTurn && (SpecificValidGamePiecesListForNextTurn.Count == 0 || isGamePieceSpecificValidGamePieceForNextTurn(gamePieceToMove)))
            {
                List<GameBoardCell> listOfAvailableMovesForGamePiece = createAvailableMovesListForGamePiece(gamePieceToMove);

                if (listOfAvailableMovesForGamePiece.Count > 0)
                {
                    bool canGamePieceCapture = canGamePieceCaptureAnyGamePieces(gamePieceToMove);

                    if (!canGamePieceCapture || isAttemptedCapture(gamePieceToMove, i_DestinationCell))
                    {
                        foreach (GameBoardCell currentCellToCheck in listOfAvailableMovesForGamePiece)
                        {
                            if (currentCellToCheck.Equals(i_DestinationCell))
                            {
                                isAttemptedMovementValidResult = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isAttemptedMovementValidResult;
        }

        private bool isAttemptedCapture(GamePiece i_GamePieceToMove, GameBoardCell i_DestinationCell)
        {
            List<GameBoardCell> listOfAvailableCapturesForGamePiece = createAvailableCapturesListForGamePiece(i_GamePieceToMove);
            bool isAttemptedCaptureResult = false;

            foreach (GameBoardCell currentCellToCheck in listOfAvailableCapturesForGamePiece)
            {
                if (currentCellToCheck.Equals(i_DestinationCell))
                {
                    isAttemptedCaptureResult = true;
                    break;
                }
            }

            return isAttemptedCaptureResult;
        }

        private bool canGamePieceCaptureAnyGamePieces(GamePiece i_GamePieceToCheck)
        {
            List<GameBoardCell> listOfAvailableMovesForGamePiece = createAvailableMovesListForGamePiece(i_GamePieceToCheck);
            bool canGamePieceCaptureAgainResult = false;

            foreach (GameBoardCell currentCellToCheck in listOfAvailableMovesForGamePiece)
            {
                if (isAttemptedCapture(i_GamePieceToCheck, currentCellToCheck))
                {
                    canGamePieceCaptureAgainResult = true;
                    break;
                }
            }

            return canGamePieceCaptureAgainResult;
        }

        private eTeamType getOtherTeam(eTeamType i_Team)
        {
            eTeamType otherTeam;

            switch (i_Team)
            {
                case eTeamType.TeamOne:
                    otherTeam = eTeamType.TeamTwo;
                    break;
                case eTeamType.TeamTwo:
                    otherTeam = eTeamType.TeamOne;
                    break;
                default:
                    otherTeam = i_Team;
                    break;
            }

            return otherTeam;
        }

        public string GetWinningTeamName()
        {
            string winningTeamNameResult;

            switch (GameState)
            {
                case eGameState.TeamOneWon:
                    winningTeamNameResult = TeamOneName;
                    break;
                case eGameState.TeamTwoWon:
                    winningTeamNameResult = TeamTwoName;
                    break;
                default:
                    winningTeamNameResult = string.Empty;
                    break;
            }

            return winningTeamNameResult;
        }

        private bool doesTeamHaveAnyAvailableMoves(eTeamType i_TeamToCheck)
        {
            bool teamHasAvailableMovesResult = false;
            List<GamePiece> listOfGamePiecesToGoOver = new List<GamePiece>();
            
            switch (i_TeamToCheck)
            {
                case eTeamType.TeamOne:
                    listOfGamePiecesToGoOver = GameBoard.TeamOneGamePiecesList;
                    break;
                case eTeamType.TeamTwo:
                    listOfGamePiecesToGoOver = GameBoard.TeamTwoGamePiecesList;
                    break;
            }

            foreach (GamePiece currentPieceToCheck in listOfGamePiecesToGoOver)
            {
                if (createAvailableMovesListForGamePiece(currentPieceToCheck).Count > 0)
                {
                    teamHasAvailableMovesResult = true;
                    break;
                }
            }

            return teamHasAvailableMovesResult;
        }

        private int getAmountOfGamePiecesRemainingInTeam(eTeamType i_TeamToCheck)
        {
            int amountOfGamePiecesRemainingInTeam = 0;

            switch (i_TeamToCheck)
            {
                case eTeamType.TeamOne:
                    amountOfGamePiecesRemainingInTeam = GameBoard.TeamOneGamePiecesList.Count;
                    break;
                case eTeamType.TeamTwo:
                    amountOfGamePiecesRemainingInTeam = GameBoard.TeamTwoGamePiecesList.Count;
                    break;
            }

            return amountOfGamePiecesRemainingInTeam;
        }

        private int calculatePointsOfWinningTeam(eTeamType i_TeamThatWon)
        {
            int pointsOfWinningTeam = 0;
            List<GamePiece> listOfWinningTeamGamePiecesToGoOver = new List<GamePiece>();
            List<GamePiece> listOfLosingTeamGamePiecesToGoOver = new List<GamePiece>();

            switch (i_TeamThatWon)
            {
                case eTeamType.TeamOne:
                    listOfWinningTeamGamePiecesToGoOver = GameBoard.TeamOneGamePiecesList;
                    listOfLosingTeamGamePiecesToGoOver = GameBoard.TeamTwoGamePiecesList;
                    break;
                case eTeamType.TeamTwo:
                    listOfWinningTeamGamePiecesToGoOver = GameBoard.TeamTwoGamePiecesList;
                    listOfLosingTeamGamePiecesToGoOver = GameBoard.TeamOneGamePiecesList;
                    break;
            }

            foreach (GamePiece currentGamePieceToCheck in listOfWinningTeamGamePiecesToGoOver)
            {
                if (currentGamePieceToCheck.IsKing)
                {
                    pointsOfWinningTeam += 4;
                }
                else
                {
                    pointsOfWinningTeam++;
                }
            }

            return pointsOfWinningTeam - listOfLosingTeamGamePiecesToGoOver.Count;
        }

        private void updateGameDetailsForTeamThatWon(eTeamType i_TeamThatWon)
        {
            switch (i_TeamThatWon)
            {
                case eTeamType.TeamOne:
                    GameState = eGameState.TeamOneWon;
                    TeamOneScore += calculatePointsOfWinningTeam(eTeamType.TeamOne);
                    break;
                case eTeamType.TeamTwo:
                    GameState = eGameState.TeamTwoWon;
                    TeamTwoScore += calculatePointsOfWinningTeam(eTeamType.TeamTwo);
                    break;
            }
        }

        private void checkAndHandleVictoryOrTie()
        {
            eTeamType enemyTeam = getOtherTeam(TeamForCurrentTurn);

            if (getAmountOfGamePiecesRemainingInTeam(enemyTeam) == 0 || (!doesTeamHaveAnyAvailableMoves(enemyTeam) && doesTeamHaveAnyAvailableMoves(TeamForCurrentTurn)))
            {
                updateGameDetailsForTeamThatWon(TeamForCurrentTurn);
            }
            else if (!doesTeamHaveAnyAvailableMoves(enemyTeam) && !doesTeamHaveAnyAvailableMoves(TeamForCurrentTurn))
            {
                GameState = eGameState.Tie;
            }
        }

        private void convertGamePieceToKingIfReachedFarthestLine(GamePiece i_GamePieceToCheck)
        {
            int topLineOnBoard = 0;
            int bottomLineOnBoard = GameBoard.GameBoardSize - 1;
            int lineNeededToReach = 0;

            switch (i_GamePieceToCheck.TeamAssignment)
            {
                case eTeamType.TeamOne:
                    lineNeededToReach = topLineOnBoard;
                    break;
                case eTeamType.TeamTwo:
                    lineNeededToReach = bottomLineOnBoard;
                    break;
            }

            if (i_GamePieceToCheck.CellOnBoard.Row == lineNeededToReach && !i_GamePieceToCheck.IsKing)
            {
                i_GamePieceToCheck.IsKing = true;
            }
        }

        public string GetNameOfCurrentTeamToPlay()
        {
            string nameOfCurrentTeamToPlay = TeamOneName;

            if (TeamForCurrentTurn == eTeamType.TeamTwo)
            {
                nameOfCurrentTeamToPlay = TeamTwoName;
            }

            return nameOfCurrentTeamToPlay;
        }

        private void checkAndHandleIfTeamHasAnyCapturesAvailable(eTeamType i_TeamToCheck)
        {
            List<GamePiece> listOfGamePiecesToGoOver = new List<GamePiece>();

            switch (i_TeamToCheck)
            {
                case eTeamType.TeamOne:
                    listOfGamePiecesToGoOver = GameBoard.TeamOneGamePiecesList;
                    break;
                case eTeamType.TeamTwo:
                    listOfGamePiecesToGoOver = GameBoard.TeamTwoGamePiecesList;
                    break;
            }

            foreach (GamePiece currentPieceToCheck in listOfGamePiecesToGoOver)
            {
                if (createAvailableCapturesListForGamePiece(currentPieceToCheck).Count > 0)
                {
                    SpecificValidGamePiecesListForNextTurn.Add(currentPieceToCheck);
                }
            }
        }

        public void MoveGamePiece(GameBoardCell i_SourceCell, GameBoardCell i_DestinationCell)
        {
            GamePiece gamePieceToMove = GameBoard.GetGamePieceFromBoardCell(i_SourceCell);
            eTeamType teamForNextTurn = getOtherTeam(TeamForCurrentTurn);
            bool justCapturedEnemyPiece = false;

            if (isAttemptedCapture(gamePieceToMove, i_DestinationCell))
            {
                GameBoardCell cellBetween = GameBoardCell.GetCellBetweenTwoCellsOnADiagonal(i_SourceCell, i_DestinationCell);
                GamePiece capturedGamePiece = GameBoard.GetGamePieceFromBoardCell(cellBetween);

                GameBoard.RemoveGamePieceFromBoard(capturedGamePiece);
                justCapturedEnemyPiece = true;
            }

            SpecificValidGamePiecesListForNextTurn.Clear();
            gamePieceToMove.CellOnBoard = i_DestinationCell;
            GameBoard.SetGamePieceInBoardCell(i_SourceCell, null);
            GameBoard.SetGamePieceInBoardCell(i_DestinationCell, gamePieceToMove);
            convertGamePieceToKingIfReachedFarthestLine(gamePieceToMove);
            if (justCapturedEnemyPiece && canGamePieceCaptureAnyGamePieces(gamePieceToMove))
            { 
                teamForNextTurn = TeamForCurrentTurn;
            }

            checkAndHandleVictoryOrTie();
            checkAndHandleIfTeamHasAnyCapturesAvailable(teamForNextTurn);
            TeamForCurrentTurn = teamForNextTurn;
        }

        private List<GamePiece> createListOfComputerGamePiecesWithAvailableMoves()
        {
            List<GamePiece> computerGamePiecesWithAvailableMoves = new List<GamePiece>();

            foreach (GamePiece currentPieceToCheck in GameBoard.TeamTwoGamePiecesList)
            {
                if (createAvailableMovesListForGamePiece(currentPieceToCheck).Count > 0)
                {
                    computerGamePiecesWithAvailableMoves.Add(currentPieceToCheck);
                }
            }

            return computerGamePiecesWithAvailableMoves;
        }

        public void PrepareGameForRematch()
        {
            GameState = eGameState.InProgress;
            TeamForCurrentTurn = eTeamType.TeamOne;
            GameBoard = new GameBoardModel(GameBoardSize);
            SpecificValidGamePiecesListForNextTurn.Clear();
        }

        public void PerformComputerMovement(out GameBoardCell io_ComputerMovementSourceCell, out GameBoardCell io_ComputerMovementDestinationCell)
        {
            GamePiece gamePieceToMove;
            List<GameBoardCell> availableMovesListForGamePiece;
            List<GamePiece> listOfComputerGamePiecesWithAvailableMoves;
            Random randomNumberGenerator = new Random();

            if (SpecificValidGamePiecesListForNextTurn.Count > 0)
            {
                gamePieceToMove = SpecificValidGamePiecesListForNextTurn[randomNumberGenerator.Next(SpecificValidGamePiecesListForNextTurn.Count)];
                availableMovesListForGamePiece = createAvailableCapturesListForGamePiece(gamePieceToMove);
            }
            else
            {
                listOfComputerGamePiecesWithAvailableMoves = createListOfComputerGamePiecesWithAvailableMoves();
                gamePieceToMove = listOfComputerGamePiecesWithAvailableMoves[randomNumberGenerator.Next(listOfComputerGamePiecesWithAvailableMoves.Count)];
                availableMovesListForGamePiece = createAvailableMovesListForGamePiece(gamePieceToMove);
            }

            io_ComputerMovementSourceCell = gamePieceToMove.CellOnBoard;
            io_ComputerMovementDestinationCell = availableMovesListForGamePiece[randomNumberGenerator.Next(availableMovesListForGamePiece.Count)];
            MoveGamePiece(io_ComputerMovementSourceCell, io_ComputerMovementDestinationCell);
        }

        public void HandleCurrentTeamQuittingRequest()
        {
            eTeamType enemyTeam = getOtherTeam(TeamForCurrentTurn);

            switch (TeamForCurrentTurn)
            {
                case eTeamType.TeamOne:
                    GameBoard.TeamOneGamePiecesList.Clear();
                    break;
                case eTeamType.TeamTwo:
                    GameBoard.TeamTwoGamePiecesList.Clear();
                    break;
            }

            updateGameDetailsForTeamThatWon(enemyTeam);
        }
    }
}