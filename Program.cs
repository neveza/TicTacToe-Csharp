using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{ 


    class Program
    { 
        class Grid
        {
            string verticalLine = "|";
            string horizontalLine = "-";
            int _Width;
            int _Height;

            public int getWidth
            {
                get { return _Width; }
            }
            public int getHeight
            {
                get { return _Height; }
            }

            public string[,] gameBoard;

            public Grid()
            {
                CreatetGrid();
                PrintGrid();
            }

            public void CreatetGrid()
            {
                _Width = 5;
                _Height = 5;
                gameBoard = new string[_Width, _Height];
                int i = 1;

                for (int x = 0; x < _Width; x++ )
                {

                    for (int y = 0; y < _Height; y++)
                    {

                        if (x == 1 || x == 3)
                        {

                            gameBoard[x, y] = verticalLine;
                        }
                        else if (x == 0 || x == 2 || x == 4)
                        {
                            if (y == 1 || y == 3)
                            {
                                gameBoard[x, y] = horizontalLine;
                            }

                            if ( y == 0 || y == 2 || y == 4)
                            {

                                gameBoard[x, y] = i.ToString();

                                i++;
                            }
                        }


                        
                    }
                }

            }

            public void UpdateGrid(int Selection, string marker)
            {
                for (int x = 0; x < _Width; x++)
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        if (gameBoard[x, y] == Selection.ToString())
                        {
                            gameBoard[x, y] = marker;

                        }
                    }
                }
                
            }

            public void PrintGrid()
            {
                Console.Clear();
                for (int y = 0; y < _Height; y++)
                {
                    for (int x = 0; x < _Width; x++)
                    {

                        if (x == 4)
                        {
                            Console.WriteLine(gameBoard[x, y]);
                        }
                        else
                        {
                            Console.Write(gameBoard[x, y]);
                        } 

                    }
                }

            }

        }

        abstract class Player
        {
            string _choice;
            int _choiceAsInt;

            string _playerMarker;
            public virtual string getPlayerMarker
            {
                get { return _playerMarker; }
            }


            public virtual string getChoice
            {
                get { return _choice; }
            }
            public virtual int getChoiceAsInt
            {
                get { return _choiceAsInt; }
            }

            public Player(string marker)
            {
                _choice = " ";
                _choiceAsInt = 0;
                _playerMarker = marker;
            }

            public abstract void Input();
        }

        class Human: Player
        {
            string _choice;
            public override string getChoice
            {
                get { return _choice; }
            }

            string _playerMarker;
            public override string getPlayerMarker
            {
                get { return _playerMarker; }
            }

            public Human(string marker): base(marker)
            {
                _playerMarker = marker;
            }

            public override void Input()
            {
                _choice = Console.ReadLine();
         
            }

        }

        class Computer : Player
        {

            Grid GameBoard;

            List<string> moves;
            Dictionary<string, List<string>> BestMoves;

            List<List<string>> memoryBlock;
            Dictionary<string, List<List<string>>> memoryBlocks = new Dictionary<string, List<List<string>>>();

            string _choice;
            public override string getChoice
            {
                get { return _choice; }
            }

            string _playerMarker;
            public override string getPlayerMarker
            {
                get { return _playerMarker; }
            }

            public Computer(string marker, Grid grid) : base(marker)
            {
                _playerMarker = marker;
                _choice = " ";
                GameBoard = grid;

                memoryBlocks.Add("Row", memoryBlock = new List<List<string>>());
                memoryBlocks.Add("Column", memoryBlock = new List<List<string>>());
                memoryBlocks.Add("DiagonalA", memoryBlock = new List<List<string>>());
                memoryBlocks.Add("DiagonalB", memoryBlock = new List<List<string>>());

            }

            
            private void DetermineMoves()
            {
                foreach (var key in memoryBlocks.Keys)
                {
                    foreach (var block in memoryBlocks[key])
                    {
                        if(block.Count() != 0)
                        {
                            block.Clear();
                        }
                    }
                }

                Console.Write("I am deciding!");


                //checks down the grid
                for (int x = 0; x < GameBoard.getWidth; x += 2)
                {
                    moves = new List<string>();

                    for (int y = 0; y < GameBoard.getHeight; y += 2)
                    {
                        if(GameBoard.gameBoard[x, y] != "X" && GameBoard.gameBoard[x, y] != "O")
                        {

                            moves.Add(GameBoard.gameBoard[x, y]);
                        }

                    }

                        memoryBlocks["Row"].Add(moves);


                }
                

                //checks across the grid
                for (int y = 0; y < GameBoard.getHeight; y += 2)
                {
                    moves = new List<string>();

                    for (int x = 0; x < GameBoard.getWidth; x += 2)
                    {
                        if (GameBoard.gameBoard[x, y] != "X" && GameBoard.gameBoard[x, y] != "O")
                        {
                            moves.Add(GameBoard.gameBoard[x, y]);
                        }

                    }

                    memoryBlocks["Column"].Add(moves);

                }

                //checks crossways
                for (int x = 0; x < 5;)
                {
                    moves = new List<string>();

                    for (int y = 0; y < 5; y += 2)
                    {
                        if (GameBoard.gameBoard[x, y] != "X" && GameBoard.gameBoard[x, y] != "O")
                        {
                            moves.Add(GameBoard.gameBoard[x, y]);
                        }

                        x += 2;
                    }

                    memoryBlocks["DiagonalA"].Add(moves);
                }


                for (int x = 4; x > 0;)
                {                   
                    moves = new List<string>();

                    for (int y = 0; y < 5; y += 2)
                    {
                        if (GameBoard.gameBoard[x, y] != "X" && GameBoard.gameBoard[x, y] != "O")
                        {
                            moves.Add(GameBoard.gameBoard[x, y]);
                        }

                        x -= 2;
                    }

                    memoryBlocks["DiagonalB"].Add(moves);

                }


            }

            private void SeekBestMoves()
            {
                int rank = 0; // 0 is best, first choice
                BestMoves = new Dictionary<string, List<string>>();

                foreach (var key in memoryBlocks.Keys)
                {
                    var TopMoves =  memoryBlocks[key].Where(x => x.Count() == 1);

                        if (TopMoves.Count() != 0)
                        {
                            foreach (var moves in TopMoves)
                            {
                                BestMoves.Add(rank.ToString(), moves);
                                rank++;
                            }

                        }

                }

                foreach (var key in memoryBlocks.Keys)
                {
                    var BottomMoves = memoryBlocks[key].Where(x => x.Count() == 2);

                    if (BottomMoves.Count() != 0)
                    {
                        foreach (var moves in BottomMoves)
                        {
                            BestMoves.Add(rank.ToString(), moves);
                            rank++;
                        }
                    }
                }

            }

            public override void Input()
            {
                Random random = new Random();
                DetermineMoves();
                SeekBestMoves();

                var key = random.Next(0, BestMoves.Keys.Count()); // counts keys

                if (key > 2)
                {
                    key -= 2;
                }

                var maxMoves = BestMoves[key.ToString()].Count(); //counts list

                if (maxMoves > 2)
                {
                    maxMoves -= 2;
                }

                _choice = BestMoves[key.ToString()][random.Next(0, maxMoves)];


                Console.WriteLine("I am choosing:" + _choice);
                Console.ReadLine();

            }
        }

        class GameControl
        {

            Grid GameBoard;
            public bool isRunning;

            int _gridSelection;
            public int getGridSelection
            {
                get { return _gridSelection; }
            }

            public GameControl(Grid grid)
            {
                GameBoard = grid;
                isRunning = true;
            }

            public bool CheckCondition() {


                //checks down the grid
                for (int x = 0; x < GameBoard.getWidth; x++)
                {
                    int player = 0;
                    int computer = 0;
                    for (int y = 0; y < GameBoard.getHeight; y++)
                    {

                        if (GameBoard.gameBoard[x, y] == "X")
                        {
                            player++;   
                        }
                        else if (GameBoard.gameBoard[x, y] == "O")
                        {
                            computer++;
                        }

                        if (player == 3)
                        {
                            Console.WriteLine("Player wins!");
                            Console.ReadLine();
                            return true;
                        }
                        if (computer == 3)
                        {
                            Console.WriteLine("Computer wins!");
                            Console.ReadLine();
                            return true;
                        }
                    }
                }

                //checks across the grid
                for (int y = 0; y < GameBoard.getHeight; y++)
                {
                    int player = 0;
                    int computer = 0;

                    for (int x = 0; x < GameBoard.getWidth; x++)
                    {

                        if (GameBoard.gameBoard[x, y] == "X")
                        {
                            player++;
                        }
                        else if (GameBoard.gameBoard[x, y] == "O")
                        {
                            computer++;
                        }

                        if (player == 3)
                        {
                            Console.WriteLine("Player wins!");
                            Console.ReadLine();
                            return true;
                        }
                        if (computer == 3)
                        {
                            Console.WriteLine("Computer wins!");
                            Console.ReadLine();
                            return true;
                        }
                    }
                }

                //checks crossways

                for (int x = 0; x < 5;)
                {
                    int player = 0;
                    int computer = 0;

                    for (int y = 0; y < 5; y += 2)
                    {

                        if (GameBoard.gameBoard[x, y] == "X")
                        {
                            player++;
                        }
                        else if (GameBoard.gameBoard[x, y] == "O")
                        {
                            computer++;
                        }

                        if (player == 3)
                        {
                            Console.WriteLine("Player wins!");
                            Console.ReadLine();
                            return true;
                        }
                        if (computer == 3)
                        {
                            Console.WriteLine("Computer wins!");
                            Console.ReadLine();
                            return true;
                        }
                        x += 2;
                    }
                }


                for (int x = 4; x > 0;)
                {
                    int player = 0;
                    int computer = 0;
                    for (int y = 0; y < 5; y += 2)
                    {
                        //Console.Write(x);
                        //Console.WriteLine(y);
                        if (GameBoard.gameBoard[x, y] == "X")
                        {
                            player++;
                        }
                        else if (GameBoard.gameBoard[x, y] == "O")
                        {
                            computer++;
                        }

                        if (player == 3)
                        {
                            Console.WriteLine("Player wins!");
                            Console.ReadLine();
                            return true;
                        }
                        if (computer == 3)
                        {
                            Console.WriteLine("Computer wins!");
                            Console.ReadLine();
                            return true;
                        }
                        x -= 2;
                    }


                }

                int counter = 0;

                //checks for draw
                for (int x = 0; x < GameBoard.getWidth; x++)
                {

                    for (int y = 0; y < GameBoard.getHeight; y++)
                    {

                        if (GameBoard.gameBoard[x, y] == "X" || GameBoard.gameBoard[x, y] == "Y")
                        {
                            counter++;
                        }

                    }

                    if (counter == GameBoard.getWidth)
                    {
                        Console.WriteLine("Draw!");
                        Console.ReadLine();
                        return true;
                    }
                }

                return false;
            }
            public bool CheckInput(string choice)
            {
                Console.WriteLine(choice);
                for (int x = 0; x < GameBoard.getWidth; x++)
                {
                    for (int y = 0; y < GameBoard.getHeight; y++)
                    {
                        //Console.WriteLine(GameBoard.gameBoard[x, y]);

                        if (GameBoard.gameBoard[x, y] == choice)
                        {
                            _gridSelection = Convert.ToInt32(choice);
                            

                            Console.Write("returning true");
                            return true;
                        }
                        else if (choice == "Quit")
                        {
                            return true;
                        }
                        
                    }
                }

                Console.Write("Please enter a valid selection: ");
                return false;
            }

        }

        static void Main(string[] args)
        {
            Grid gameGrid = new Grid();
            GameControl Game = new GameControl(gameGrid);
            Human human = new Human("X");
            Computer computer = new Computer("O", gameGrid);
            //Human human2 = new Human("O");

            Queue<Player> Turn = new Queue<Player>();
            Turn.Enqueue(human);
            //Turn.Enqueue(human2);
            Turn.Enqueue(computer);

            while (Game.isRunning == true)
            {
                if (Turn.Count == 0)
                {
                    Turn.Enqueue(human);
                    //Turn.Enqueue(human2);
                    Turn.Enqueue(computer);
                }

                var activePlayer = Turn.Dequeue();

                while (!Game.CheckInput(activePlayer.getChoice))
                {
                    activePlayer.Input();
                }

                gameGrid.UpdateGrid(Game.getGridSelection, activePlayer.getPlayerMarker);


                gameGrid.PrintGrid();
                Console.WriteLine(activePlayer.getChoice);

                if (Game.CheckCondition())
                {
                    Game.isRunning = false;
                }
                else if (human.getChoice == "Quit")
                {
                    Game.isRunning = false;
                }


            }

            Console.ReadLine();



        } 

  
    }
}