namespace CheckersLogicLibrary
{
    internal class GamePiece
    {
        private GameBoardCell m_CellOnBoard;
        private eTeamType m_TeamAssignment;
        private bool m_IsKing = false;

        internal GameBoardCell CellOnBoard
        {
            get
            { 
                return m_CellOnBoard;
            }

            set
            { 
                m_CellOnBoard = value;
            }
        }

        internal eTeamType TeamAssignment
        {
            get
            {
                return m_TeamAssignment;
            }

            set
            { 
                m_TeamAssignment = value;
            }
        }

        internal bool IsKing
        {
            get 
            { 
                return m_IsKing; 
            }
            
            set 
            { 
                m_IsKing = value; 
            }
        }

        internal GamePiece(GameBoardCell i_CellOnBoard, eTeamType i_TeamAssignment)
        {
            m_CellOnBoard = i_CellOnBoard;
            m_TeamAssignment = i_TeamAssignment;
        }

        internal bool IsMovementInValidDirection(GameBoardCell i_AttemptedMovementDestinationCell)
        {
            bool validDirectionResult = true;

            if (!IsKing)
            {
                switch (TeamAssignment)
                {
                    case eTeamType.TeamOne:
                        validDirectionResult = CellOnBoard.IsBelowReceivedCellOnBoard(i_AttemptedMovementDestinationCell);
                        break;
                    case eTeamType.TeamTwo:
                        validDirectionResult = CellOnBoard.IsAboveReceivedCellOnBoard(i_AttemptedMovementDestinationCell);
                        break;
                }
            }

            return validDirectionResult;
        }

        internal bool CheckIfSameTeamPieces(GamePiece i_GamePieceToCheck)
        {
            return TeamAssignment == i_GamePieceToCheck.TeamAssignment;
        }
    }
}