using System;
using System.Text;
using CheckersLogicLibrary;

namespace CheckersUI
{
    internal class Menu
    {
        private static void printWelcomeMessage()
        {
            StringBuilder messageToPrint = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            messageToPrint.AppendLine("********************************");
            messageToPrint.AppendLine("*     Welcome to Checkers!     *");
            messageToPrint.AppendLine("********************************");
            Console.WriteLine(messageToPrint);
        }

        private static void printGoodbyeMessage()
        {
            StringBuilder messageToPrint = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            messageToPrint.AppendLine("********************************");
            messageToPrint.AppendLine("*           Goodbye!           *");
            messageToPrint.AppendLine("********************************");
            Console.WriteLine(messageToPrint);
            System.Threading.Thread.Sleep(2000);
        }

        private static void printMenu()
        {
            StringBuilder menuToPrint = new StringBuilder();

            menuToPrint.AppendLine("Please choose one of the following options:");
            menuToPrint.AppendLine("1. Start Game");
            menuToPrint.AppendLine("2. Exit");
            Console.WriteLine(menuToPrint);
        }

        private static void printBoardSizeSelection()
        {
            StringBuilder menuToPrint = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            menuToPrint.AppendLine("Please choose one of the following board size options:");
            menuToPrint.AppendLine("1. Size 6X6");
            menuToPrint.AppendLine("2. Size 8X8");
            menuToPrint.AppendLine("3. Size 10X10");
            Console.WriteLine(menuToPrint);
        }

        private static void printGameModeSelection()
        {
            StringBuilder menuToPrint = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            menuToPrint.AppendLine("Please choose one of the following game mode options:");
            menuToPrint.AppendLine("1. Single player");
            menuToPrint.AppendLine("2. Two players");
            Console.WriteLine(menuToPrint);
        }

        private static eMenuOptions getUserMenuSelection()
        {
            eMenuOptions menuOptionsSelection = eMenuOptions.NoSelection;
            string userInputString;
            bool isUserInputValidMenuSelection;

            Console.Write("Enter your selection and then press Enter (Example: 1): ");
            userInputString = Console.ReadLine();
            isUserInputValidMenuSelection = Enum.TryParse(userInputString, out menuOptionsSelection);
            while (!isUserInputValidMenuSelection || menuOptionsSelection == eMenuOptions.NoSelection || userInputString.Length != 1)
            {
                Console.Write("Invalid input. Please enter your selection: ");
                userInputString = Console.ReadLine();
                isUserInputValidMenuSelection = Enum.TryParse(userInputString, out menuOptionsSelection);
            }
            
            return menuOptionsSelection;
        }

        private static bool isBoardSizeSelectionFromUserValid(string i_BoardSizeSelectionUserInputString)
        {
            bool isBoardSizeSelectionValid = false;

            isBoardSizeSelectionValid |= i_BoardSizeSelectionUserInputString == "1";
            isBoardSizeSelectionValid |= i_BoardSizeSelectionUserInputString == "2";
            isBoardSizeSelectionValid |= i_BoardSizeSelectionUserInputString == "3";

            return isBoardSizeSelectionValid;
        }

        private static eBoardSizeOptions getBoardSizeSelectionFromUser()
        {
            eBoardSizeOptions boardSizeSelection = eBoardSizeOptions.Size8X8;
            string userInputString;
            int boardSizeSelectionFromUser;

            Console.Write("Enter your selection and then press Enter (Example: 1): ");
            userInputString = Console.ReadLine();
            while (!isBoardSizeSelectionFromUserValid(userInputString))
            {
                Console.Write("Invalid input. Please enter your selection: ");
                userInputString = Console.ReadLine();
            }

            int.TryParse(userInputString, out boardSizeSelectionFromUser);
            switch (boardSizeSelectionFromUser)
            {
                case 1:
                    boardSizeSelection = eBoardSizeOptions.Size6X6;
                    break;
                case 2:
                    boardSizeSelection = eBoardSizeOptions.Size8X8;
                    break;
                case 3:
                    boardSizeSelection = eBoardSizeOptions.Size10X10;
                    break;
            }

            return boardSizeSelection;
        }

        private static string getTeamOneNameFromUser()
        {
            string teamOneName;

            Console.Write("Please enter your team's name: ");
            teamOneName = Console.ReadLine();

            return teamOneName;
        }

        private static eGameModeOptions getGameModeSelectionFromUser()
        {
            eGameModeOptions gameModeSelection;
            string userInputString;
            bool isUserInputValidGameModeSelection;

            Console.Write("Enter your selection and then press Enter (Example: 1): ");
            userInputString = Console.ReadLine();
            isUserInputValidGameModeSelection = Enum.TryParse(userInputString, out gameModeSelection);
            while (!isUserInputValidGameModeSelection || userInputString.Length != 1)
            {
                Console.Write("Invalid input. Please enter your selection: ");
                userInputString = Console.ReadLine();
                isUserInputValidGameModeSelection = Enum.TryParse(userInputString, out gameModeSelection);
            }

            return gameModeSelection;
        }

        private static string getTeamTwoNameFromUser()
        {
            string teamOneName;

            Console.Write("Please enter the second team's name: ");
            teamOneName = Console.ReadLine();

            return teamOneName;
        }

        private static bool isUserSelectionSinglePlayerGameMode(eGameModeOptions i_GameModeSelectionFromUser)
        {
            return i_GameModeSelectionFromUser == eGameModeOptions.SinglePlayer;
        }

        private static GameManager createGameManagerFromUserInput()
        {
            GameManager checkersGameManagerResult;
            eBoardSizeOptions boardSizeSelection;
            string teamOneName;
            eGameModeOptions gameModeSelection;
            string teamTwoName = "Computer";
            bool isGameModeSinglePlayer;

            Ex02.ConsoleUtils.Screen.Clear();
            teamOneName = getTeamOneNameFromUser();
            printBoardSizeSelection();
            boardSizeSelection = getBoardSizeSelectionFromUser();
            printGameModeSelection();
            gameModeSelection = getGameModeSelectionFromUser();
            isGameModeSinglePlayer = isUserSelectionSinglePlayerGameMode(gameModeSelection);
            if (!isGameModeSinglePlayer)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                teamTwoName = getTeamTwoNameFromUser();
            }

            checkersGameManagerResult = new GameManager((int)boardSizeSelection, teamOneName, teamTwoName, isGameModeSinglePlayer);
            
            return checkersGameManagerResult;
        }

        private static void printWinnerOrTieAnnouncement(GameManager i_CheckersGame)
        {
            StringBuilder winnerOrTieMessageToPrint = new StringBuilder();

            if (i_CheckersGame.GameState == eGameState.Tie)
            {
                winnerOrTieMessageToPrint.AppendLine("*****| It's a tie! |*****");
            }
            else
            {
                string winningTeamName = i_CheckersGame.GetWinningTeamName();
                winnerOrTieMessageToPrint.AppendFormat("*****| {0} Wins! |*****", winningTeamName).AppendLine();
            }

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(winnerOrTieMessageToPrint);
        }

        private static void printBothTeamsScores(GameManager i_CheckersGame)
        {
            StringBuilder scoresMessageToPrint = new StringBuilder();

            scoresMessageToPrint.AppendFormat("{0}'s score: {1}", i_CheckersGame.TeamOneName, i_CheckersGame.TeamOneScore).AppendLine();
            scoresMessageToPrint.AppendFormat("{0}'s score: {1}", i_CheckersGame.TeamTwoName, i_CheckersGame.TeamTwoScore).AppendLine();
            Console.WriteLine(scoresMessageToPrint);
        }

        private static void printGameResults(GameManager i_CheckersGame)
        {
            System.Threading.Thread.Sleep(1000);
            printWinnerOrTieAnnouncement(i_CheckersGame);
            printBothTeamsScores(i_CheckersGame);
        }

        private static void printRematchOptions()
        {
            StringBuilder questionToPrint = new StringBuilder();

            questionToPrint.AppendLine("Would you like a rematch?");
            questionToPrint.AppendLine("Please choose one of the following options:");
            questionToPrint.AppendLine("1. Yes");
            questionToPrint.AppendLine("2. No");
            questionToPrint.Append("Enter your selection and then press Enter (Example: 1): ");
            Console.Write(questionToPrint);
        }

        private static bool isRematchSelectionFromUserValid(string i_UserInputString)
        {
            return i_UserInputString == "1" || i_UserInputString == "2";
        }

        private static bool getRematchSelectionFromUser()
        {
            bool userWantsRematch;
            string userInputString;

            printRematchOptions();
            userInputString = Console.ReadLine();
            while (!isRematchSelectionFromUserValid(userInputString))
            {
                Console.Write("Invalid input. Please enter your selection: ");
                userInputString = Console.ReadLine();
            }

            userWantsRematch = userInputString == "1";

            return userWantsRematch;
        }

        internal static void ShowOptions()
        {
            eMenuOptions userMenuSelection = eMenuOptions.NoSelection;

            while (userMenuSelection != eMenuOptions.Exit)
            {
                printWelcomeMessage();
                printMenu();
                userMenuSelection = getUserMenuSelection();
                switch (userMenuSelection)
                {
                    case eMenuOptions.StartGame:
                        GameManager checkersGameManager = createGameManagerFromUserInput();
                        bool isOngoingGameSession = true;

                        while (isOngoingGameSession)
                        {
                            Ex02.ConsoleUtils.Screen.Clear();
                            GameAPI.StartGame(checkersGameManager);
                            printGameResults(checkersGameManager);
                            checkersGameManager.PrepareGameForRematch();
                            isOngoingGameSession = getRematchSelectionFromUser();
                        }

                        userMenuSelection = eMenuOptions.NoSelection;
                        break;
                    case eMenuOptions.Exit:
                        printGoodbyeMessage();
                        break;
                }
            }
        }
    }
}