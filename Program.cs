using System;
using System.ComponentModel;
using System.Threading;

namespace TicTacToe
{
    class Program
    {
        private static Player Player1 = new Player(PlayerName.Player1, "X");
        private static Player Player2 = new Player(PlayerName.Player2, "O");

        static void Main()
        {
            var random = new Random();
            var isGameOver = false;
            var isTie = false;
            var grid = new GridCell[3, 3]
            {
                {new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, ""))},
                {new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, ""))},
                {new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, "")), new GridCell(0,0, new Player(PlayerName.Undecided, ""))}
            };
            
            ShowGameTitle();
            ShowRules();
            var chosenPlayMode = DecidePlayMode();
            Console.WriteLine($"PLAY MODE: {chosenPlayMode}");
            var currentPlayer = DecideFirstToGo(random);
            DrawGrid(grid);
            
            while (!isGameOver && !isTie)
            {
                ReportStatus(currentPlayer);
                var placement = GetPlacement(chosenPlayMode, currentPlayer, random, grid);
                WriteInGrid(grid, placement);
                DrawGrid(grid);
                isGameOver = CheckIsGameOver(grid, currentPlayer);
                if (!isGameOver) isTie = CheckIsAllCellTaken(grid);
                if (!isGameOver && !isTie) currentPlayer = SwitchPlayer(currentPlayer);
            }

            Console.WriteLine(isTie ? "Tie!" : $"{currentPlayer.PlayerName} won!");
            ShowCredits();
        }
        
        private static void ShowGameTitle()
        {
            Console.WriteLine(@"
  _______ _          _______             _______         
 |__   __(_)        |__   __|           |__   __|        
    | |   _  ___ ______| | __ _  ___ ______| | ___   ___ 
    | |  | |/ __|______| |/ _` |/ __|______| |/ _ \ / _ \
    | |  | | (__       | | (_| | (__       | | (_) |  __/
    |_|  |_|\___|      |_|\__,_|\___|      |_|\___/ \___|
                                                         
                                                         
");
        }
        
        private static void ShowRules()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Player1: X ");
            Console.WriteLine("Player2: O ");
            Console.WriteLine("(Computer will assume Player2 in the Against computer mode) ");
            Console.ResetColor();
        }
        
        private static PlayMode DecidePlayMode()
        {
            var choice = "";
            Console.WriteLine("Choose a play mode:");
            Console.WriteLine("(1) 2 players (2) against computer");
            choice = Console.ReadLine();
            while (choice != "1" && choice != "2")
            {
                Console.WriteLine("Invalid input. Try again :)");
                choice = Console.ReadLine();
            }
            return (PlayMode) int.Parse(choice);
        }
        
        private static Player DecideFirstToGo(Random random)
        {
            var result = random.Next(0, 2);
            return result == 0 ? Player1 : Player2;
        }
        
        private static void DrawGrid(GridCell[,] grid)
        {
            var totalRows = grid.GetLength(0);
            var totalColumns = grid.GetLength(1);
            for (var rowIdx = 0; rowIdx < totalRows; rowIdx++)
            {
                var tmp = "";
                for (var colIdx = 0; colIdx < totalColumns; colIdx++)
                {
                    var cell = grid[colIdx, rowIdx];
                    if (cell.Player.PlayerName != PlayerName.Undecided) tmp += $"_{cell.Player.Token}|";
                    else tmp += "__|";
                }
                Console.WriteLine(tmp);
            }
        }

        private static bool CheckIsGameOver(GridCell[,] grid, Player currentPlayer)
        {
            var isGameOver = false;
            var totalRows = grid.GetLength(0);
            var totalColumns = grid.GetLength(1);

            for (int row = 0; row < totalRows; row++)
            {
                // win condition 1: same row that got all three columns
                if (grid[row, 0].Player.Token == currentPlayer.Token &&
                    grid[row, 1].Player.Token == currentPlayer.Token &&
                    grid[row, 2].Player.Token == currentPlayer.Token)
                {
                    isGameOver = true;
                    break;
                }
            }

            if (!isGameOver)
            {
                for (int col = 0; col < totalColumns; col++)
                {
                    // win condition 2: same column that got all three rows
                    if (grid[0, col].Player.Token == currentPlayer.Token &&
                        grid[1, col].Player.Token == currentPlayer.Token &&
                        grid[2, col].Player.Token == currentPlayer.Token)
                    {
                        isGameOver = true;
                        break;
                    }
                }
            }

            if (!isGameOver)
            {
                isGameOver = (grid[0,0].Player.Token == currentPlayer.Token && grid[1,1].Player.Token == currentPlayer.Token && grid[2,2].Player.Token == currentPlayer.Token) ||
                             (grid[0,2].Player.Token == currentPlayer.Token && grid[1,1].Player.Token == currentPlayer.Token && grid[2,0].Player.Token == currentPlayer.Token);
            }

            return isGameOver;
        }
        
        private static bool CheckIsAllCellTaken(GridCell[,] grid)
        {
            var totalRows = grid.GetLength(0);
            var totalColumns = grid.GetLength(1);
            var undecidedCells = 0;
            for (var row = 0; row < totalRows; row++)
            {
                for (var col = 0; col < totalColumns; col++)
                {
                    if (grid[row, col].Player.PlayerName == PlayerName.Undecided) undecidedCells++;
                }
            }

            return undecidedCells == 1;
        }

        private static Player SwitchPlayer(Player currentPlayer)
        {
            return currentPlayer.PlayerName == PlayerName.Player1 ? Player2 : Player1;
        }

        private static void WriteInGrid(GridCell[,] grid, GridCell targetCell)
        {
            grid[targetCell.Column, targetCell.Row] = targetCell;
        }

        private static void ReportStatus(Player currentPlayer)
        {
            Console.WriteLine($"{currentPlayer}'s turn.");
        }

        private static void ShowCredits()
        {
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("A game made by");
            Console.WriteLine(@"
  _____                _____  _        
 |_   _|              / ____|| |       
   | |  _ __   __ _  | (___  | |       
   | | | '_ \ / _` |  \___ \ | |       
  _| |_| | | | (_| |  ____) || |____ _ 
 |_____|_| |_|\__,_| |_____(_)______(_)
                                       
                                       
");
        }
        
        private static GridCell GetPlacement(PlayMode chosenPlayMode, Player currentPlayer, Random random, GridCell[,] grid)
        {
            return (chosenPlayMode == PlayMode.PvC && currentPlayer.PlayerName == PlayerName.Player2) ? ComputerChoose(random, grid) : PlayerChoose(currentPlayer, grid);
        }
        
        private static GridCell ComputerChoose(Random random, GridCell[,] grid)
        {
            var check = false;
            var colInput = 0;
            var rowInput = 0;
            while (!check)
            {
                colInput = random.Next(0, 3);
                rowInput = random.Next(0, 3);
                check = CellIsAvailable(colInput, rowInput, grid);
            }
            Thread.Sleep(3000);
            return new GridCell(colInput, rowInput, Player2);
        }

        private static bool CellIsAvailable(int colIdx, int rowIdx, GridCell[,] grid)
        {
            return grid[colIdx, rowIdx].Player.PlayerName == PlayerName.Undecided;
        }

        private static GridCell PlayerChoose(Player currentPlayer, GridCell[,] grid)
        {
            var colInput = "";
            var rowInput = "";
            var currentToken = currentPlayer.Token;
            var check = false;
            while (!check)
            {
                Console.WriteLine($"Choose a column to set your {currentToken} (1 - 3)");
                colInput = Console.ReadLine();
                while (colInput != "1" && colInput != "2" && colInput != "3")
                {
                    Console.WriteLine("Invalid input. Try again :)");
                    colInput = Console.ReadLine();
                }

                Console.WriteLine($"Choose a row to set your {currentToken} (1 - 3)");
                rowInput = Console.ReadLine();
                while (rowInput != "1" && rowInput != "2" && rowInput != "3")
                {
                    Console.WriteLine("Invalid input. Try again :)");
                    rowInput = Console.ReadLine();
                }
                check = CellIsAvailable(Convert.ToInt32(colInput) - 1, Convert.ToInt32(rowInput) - 1, grid);
                if (!check) Console.WriteLine("Position already taken! >:(");
            }

            var chosenColumn = int.Parse(colInput) - 1;
            var chosenRow = int.Parse(rowInput) - 1;

            return new GridCell(chosenColumn, chosenRow, currentPlayer);
        }
    }
    
    struct GridCell
    {
        public GridCell(int column, int row, Player player)
        {
            Column = column;
            Row = row;
            Player = player;
        }

        public int Column { get; }
        public int Row { get; }
        public Player Player { get; }
    }

    struct Player
    {
        public Player(PlayerName playerName, string token)
        {
            PlayerName = playerName;
            Token = token;
        }

        public PlayerName PlayerName { get; }
        public string Token { get; }
        public override string ToString()
        {
            return $"Player {(Token == "X" ? 1 : 2)}";
        }
    }
    
    internal enum PlayerName
    {
        [Description("Player 1")]
        Player1,
        [Description("Player 2")] // acting as AI player in PvC and Impossible play modes
        Player2,
        [Description("Undecided")]
        Undecided
    }
    
    internal enum PlayMode
    {
        [Description("Player 1 vs Player 2")]
        PvP = 1,
        [Description("Player vs Computer")]
        PvC = 2
    }
}
