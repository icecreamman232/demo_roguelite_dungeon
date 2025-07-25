namespace SGGames.Script.PathFindings
{
    public class GridController
    {
        private int m_width;
        private int m_height;
        private bool[,] m_grid;

        public bool[,] Grid => m_grid;
    
        public GridController(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_grid = new bool[width, height];
        }

        public void SetWalkable(int x, int y, bool isWalkable)
        {
            Grid[x, y] = isWalkable;
        }
    }
}
