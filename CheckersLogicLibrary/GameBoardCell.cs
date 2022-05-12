namespace CheckersLogicLibrary
{
    public struct GameBoardCell
    {
        private ushort m_Row;
        private ushort m_Column;

        public ushort Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public ushort Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }

        public GameBoardCell(int i_Row, int i_Column)
        {
            m_Row = (ushort)i_Row;
            m_Column = (ushort)i_Column;
        }

        internal static GameBoardCell GetCellDiagonallyAcross(GameBoardCell i_SourceCell, GameBoardCell i_CellToHopOver)
        {
            int cellDiagonallyAcrossRow = i_CellToHopOver.Row + i_CellToHopOver.CalculateDeltaOfRows(i_SourceCell);
            int cellDiagonallyAcrossColumn = i_CellToHopOver.Column + i_CellToHopOver.CalculateDeltaOfColumns(i_SourceCell);
            GameBoardCell cellDiagonallyAcross = new GameBoardCell(cellDiagonallyAcrossRow, cellDiagonallyAcrossColumn);

            return cellDiagonallyAcross;
        }

        internal static GameBoardCell GetCellBetweenTwoCellsOnADiagonal(GameBoardCell i_CellInCorner1, GameBoardCell i_CellInCorner2)
        {
            int maxRowOfTwoCells = System.Math.Max(i_CellInCorner1.Row, i_CellInCorner2.Row);
            int maxColumnOfTwoCells = System.Math.Max(i_CellInCorner1.Column, i_CellInCorner2.Column);
            int cellBetweenTwoCellsRow = maxRowOfTwoCells - 1;
            int cellBetweenTwoCellsColumn = maxColumnOfTwoCells - 1;
            GameBoardCell cellBetweenTwoCells = new GameBoardCell(cellBetweenTwoCellsRow, cellBetweenTwoCellsColumn);

            return cellBetweenTwoCells;
        }

        internal bool IsAboveReceivedCellOnBoard(GameBoardCell i_CellToCompare)
        {
            return Row < i_CellToCompare.Row;
        }

        internal bool IsBelowReceivedCellOnBoard(GameBoardCell i_CellToCompare)
        {
            return Row > i_CellToCompare.Row;
        }

        internal int CalculateDeltaOfColumns(GameBoardCell i_CellToCompare)
        {
            return Column - i_CellToCompare.Column;
        }

        internal int CalculateDeltaOfRows(GameBoardCell i_CellToCompare)
        {
            return Row - i_CellToCompare.Row;
        }
    }
}