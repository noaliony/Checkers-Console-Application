using System;
using System.Text;
using CheckersLogicLibrary;

namespace CheckersUI
{
    internal class GameAPI
    {
        private const char k_TeamOnePawnGamePieceChar = 'X';
        private const char k_TeamTwoPawnGamePieceChar = 'O';
        private const char k_TeamOneKingGamePieceChar = 'K';
        private const char k_TeamTwoKingGamePieceChar = 'U';
        private const char k_EmptyCellChar = ' ';

        private static char getCharOfCellInGameBoard(GameManager i_CheckersGame, GameBoardCell i_CellToPrint)
        {
            char charToPrintInCell = k_EmptyCellChar;

            if (i_CheckersGame.IsGameBoardCellOccupied(i_CellToPrint))
            {
                bool isGamePieceInBoardCellKing = i_CheckersGame.IsGamePieceInBoardCellKing(i_CellToPrint);

                if (i_CheckersGame.GetTeamAssignmentOfGamePieceInBoardCell(i_CellToPrint) == eTeamType.TeamOne)
                {
                    charToPrintInCell = k_TeamOnePawnGamePieceChar;
                    if (isGamePieceInBoardCellKing)
                    {
                        charToPrintInCell = k_TeamOneKingGamePieceChar;
                    }
                }
                else
                {
                    charToPrintInCell = k_TeamTwoPawnGamePieceChar;
                    if (isGamePieceInBoardCellKing)
                    {
                        charToPrintInCell = k_TeamTwoKingGamePieceChar;
                    }
                }
            }

            return charToPrintInCell;
        }

        private static void printFirstLineOfBoard(GameManager i_CheckersGame)
        {
            StringBuilder lineToPrint = new StringBuilder();

            lineToPrint.Append("  ");
            for (int i = 0; i < i_CheckersGame.GameBoardSize; i++)
            {
                lineToPrint.AppendFormat(" {0}  ", (char)(i + 'A'));
            }

            Console.WriteLine(lineToPrint);
        }

        private static void printDividerBetweenLinesOfBoard(GameManager i_CheckersGame)
        {
            StringBuilder dividerToPrint = new StringBuilder();

            dividerToPrint.Append(" =");
            for (int i = 0; i < i_CheckersGame.GameBoardSize; i++)
            {
                dividerToPrint.Append("====");
            }

            Console.WriteLine(dividerToPrint);
        }

        private static void printGameBoard(GameManager i_CheckersGame)
        {
            printFirstLineOfBoard(i_CheckersGame);
            printDividerBetweenLinesOfBoard(i_CheckersGame);
            for (int i = 0; i < i_CheckersGame.GameBoardSize; i++)
            {
                StringBuilder lineToPrint = new StringBuilder();

                lineToPrint.AppendFormat("{0}|", (char)(i + 'a'));
                for (int j = 0; j < i_CheckersGame.GameBoardSize; j++)
                {
                    GameBoardCell currentCellInGameBoard = new GameBoardCell(i, j);
                    char charOfCellInGameBoard = getCharOfCellInGameBoard(i_CheckersGame, currentCellInGameBoard);

                    lineToPrint.AppendFormat(" {0} |", charOfCellInGameBoard);
                }

                Console.WriteLine(lineToPrint);
                printDividerBetweenLinesOfBoard(i_CheckersGame);
            }
        }

        private static char getCharRepresentingGamePieceOfTeam(eTeamType i_Team)
        {
            char charRepresentingGamePieceOfTeam = k_TeamOnePawnGamePieceChar;

            if (i_Team == eTeamType.TeamTwo)
            {
                charRepresentingGamePieceOfTeam = k_TeamTwoPawnGamePieceChar;
            }

            return charRepresentingGamePieceOfTeam;
        }

        private static void printCurrentTurnLine(GameManager i_CheckersGame)
        {
            string currentTurnPlayerName = i_CheckersGame.GetNameOfCurrentTeamToPlay();
            char charRepresentingGamePieceOfTeam = getCharRepresentingGamePieceOfTeam(i_CheckersGame.TeamForCurrentTurn);
            StringBuilder lineToPrint = new StringBuilder();

            lineToPrint.AppendFormat("{0}'s Turn ({1}): ", currentTurnPlayerName, charRepresentingGamePieceOfTeam);
            Console.Write(lineToPrint);
        }

        private static bool isCharValidColumnLetter(GameManager i_CheckersGame, char i_CharToCheck)
        {
            bool isCharValidColumnLetterResult = false;

            if (i_CharToCheck >= 'A')
            {
                isCharValidColumnLetterResult = i_CharToCheck <= 'F' && i_CheckersGame.GameBoardSize == 6;
                isCharValidColumnLetterResult |= i_CharToCheck <= 'H' && i_CheckersGame.GameBoardSize == 8;
                isCharValidColumnLetterResult |= i_CharToCheck <= 'J' && i_CheckersGame.GameBoardSize == 10;
            }

            return isCharValidColumnLetterResult;
        }

        private static bool isCharValidRowLetter(GameManager i_CheckersGame, char i_CharToCheck)
        {
            bool isCharValidRowLetterResult = false;

            if (i_CharToCheck >= 'a')
            {
                isCharValidRowLetterResult = i_CharToCheck <= 'f' && i_CheckersGame.GameBoardSize == 6;
                isCharValidRowLetterResult |= i_CharToCheck <= 'h' && i_CheckersGame.GameBoardSize == 8;
                isCharValidRowLetterResult |= i_CharToCheck <= 'j' && i_CheckersGame.GameBoardSize == 10;
            }

            return isCharValidRowLetterResult;
        }

        private static bool isInputStringValidMovementString(GameManager i_CheckersGame, string i_UserInputStringToCheck)
        {
            bool movementInputStringIsValidResult = false;

            if (i_UserInputStringToCheck.Length == 5)
            {
                movementInputStringIsValidResult = isCharValidColumnLetter(i_CheckersGame, i_UserInputStringToCheck[0]) && isCharValidColumnLetter(i_CheckersGame, i_UserInputStringToCheck[3]);
                movementInputStringIsValidResult &= isCharValidRowLetter(i_CheckersGame, i_UserInputStringToCheck[1]) && isCharValidRowLetter(i_CheckersGame, i_UserInputStringToCheck[4]);
                movementInputStringIsValidResult &= i_UserInputStringToCheck[2] == '>';
            }

            return movementInputStringIsValidResult;
        }

        private static bool isInputStringValidQuittingMessage(string i_UserInputStringToCheck)
        {
            return i_UserInputStringToCheck == "Q";
        }

        private static bool isInputStringValidMovementOnBoard(GameManager i_CheckersGame, string i_UserInputStringToCheck)
        {
            bool movementInputStringIsValidMovementOnBoardResult;
            GameBoardCell currentMovementSourceCell;
            GameBoardCell currentMovementDestinationCell;

            currentMovementSourceCell = getMovementSourceCellFromUserInput(i_UserInputStringToCheck);
            currentMovementDestinationCell = getMovementDestinationCellFromUserInput(i_UserInputStringToCheck);
            movementInputStringIsValidMovementOnBoardResult = i_CheckersGame.IsAttemptedMovementValid(currentMovementSourceCell, currentMovementDestinationCell);

            return movementInputStringIsValidMovementOnBoardResult;
        }

        private static void printInvalidMovementInputMessage()
        {
            Console.WriteLine("Invalid input entered. Enter a valid movement (Example: Af>Bf) or Q to quit.");
            Console.Write("Please enter your selection: ");
        }

        private static string getMovementInputFromUser(GameManager i_CheckersGame)
        {
            string movementInputFromUser;

            movementInputFromUser = Console.ReadLine();
            while (!isInputStringValidQuittingMessage(movementInputFromUser) && (!isInputStringValidMovementString(i_CheckersGame, movementInputFromUser) || !isInputStringValidMovementOnBoard(i_CheckersGame, movementInputFromUser)))
            {
                printInvalidMovementInputMessage();
                movementInputFromUser = Console.ReadLine();
            }

            return movementInputFromUser;
        }

        private static GameBoardCell getMovementSourceCellFromUserInput(string i_MovementInputFromUserString)
        {
            return new GameBoardCell(i_MovementInputFromUserString[1] - 'a', i_MovementInputFromUserString[0] - 'A');
        }

        private static GameBoardCell getMovementDestinationCellFromUserInput(string i_MovementInputFromUserString)
        {
            return new GameBoardCell(i_MovementInputFromUserString[4] - 'a', i_MovementInputFromUserString[3] - 'A');
        }

        private static void printPreviousMovementLine(string i_NameOfTeamForPreviousTurn, char i_CharRepresentingGamePieceOfTeamForPreviousTurn, string i_MovementInputStringForPreviousTurn)
        {
            string lineToPrint;

            lineToPrint = string.Format("{0}'s move was ({1}): {2}", i_NameOfTeamForPreviousTurn, i_CharRepresentingGamePieceOfTeamForPreviousTurn, i_MovementInputStringForPreviousTurn);
            Console.WriteLine(lineToPrint);
        }

        private static string convertSourceAndDestinationCellsToMovementString(GameBoardCell i_MovementSourceCell, GameBoardCell i_MovementDestinationCell)
        {
            string movementStringResult;
            string movementSourceCellString;
            string movementDestinationCellString;

            movementSourceCellString = convertCellToRowAndColumnString(i_MovementSourceCell);
            movementDestinationCellString = convertCellToRowAndColumnString(i_MovementDestinationCell);
            movementStringResult = string.Format("{0}>{1}", movementSourceCellString, movementDestinationCellString);

            return movementStringResult;
        }

        private static string convertCellToRowAndColumnString(GameBoardCell i_CellToConvert)
        {
            string rowAndColumnStringResult;
            char rowOfCellChar;
            char columnOfCellChar;

            rowOfCellChar = (char)(i_CellToConvert.Row + 'a');
            columnOfCellChar = (char)(i_CellToConvert.Column + 'A');
            rowAndColumnStringResult = string.Format("{0}{1}", columnOfCellChar, rowOfCellChar);

            return rowAndColumnStringResult;
        }

        internal static void StartGame(GameManager i_CheckersGameManager)
        {
            string currentMovementString;
            string nameOfTeamForCurrentTurn;
            GameBoardCell currentMovementSourceCell;
            GameBoardCell currentMovementDestinationCell;
            eTeamType teamForCurrentTurn;

            printGameBoard(i_CheckersGameManager);
            printCurrentTurnLine(i_CheckersGameManager);
            while (i_CheckersGameManager.GameState == eGameState.InProgress)
            {
                nameOfTeamForCurrentTurn = i_CheckersGameManager.GetNameOfCurrentTeamToPlay();
                teamForCurrentTurn = i_CheckersGameManager.TeamForCurrentTurn;
                if (i_CheckersGameManager.IsSinglePlayerGame && teamForCurrentTurn == eTeamType.TeamTwo)
                {
                    System.Threading.Thread.Sleep(1000);
                    i_CheckersGameManager.PerformComputerMovement(out currentMovementSourceCell, out currentMovementDestinationCell);
                    currentMovementString = convertSourceAndDestinationCellsToMovementString(currentMovementSourceCell, currentMovementDestinationCell);
                }
                else
                {
                    currentMovementString = getMovementInputFromUser(i_CheckersGameManager);
                    if (isInputStringValidQuittingMessage(currentMovementString))
                    {
                        i_CheckersGameManager.HandleCurrentTeamQuittingRequest();
                    }
                    else
                    {
                        currentMovementSourceCell = getMovementSourceCellFromUserInput(currentMovementString);
                        currentMovementDestinationCell = getMovementDestinationCellFromUserInput(currentMovementString);
                        i_CheckersGameManager.MoveGamePiece(currentMovementSourceCell, currentMovementDestinationCell);
                    }
                }

                if (!isInputStringValidQuittingMessage(currentMovementString))
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    printGameBoard(i_CheckersGameManager);
                    printPreviousMovementLine(nameOfTeamForCurrentTurn, getCharRepresentingGamePieceOfTeam(teamForCurrentTurn), currentMovementString);
                    printCurrentTurnLine(i_CheckersGameManager);
                }
            }
        }
    }
}