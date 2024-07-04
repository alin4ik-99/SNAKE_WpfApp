using Prism.Commands;
using Prism.Mvvm;
using SNAKE_WpfApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SNAKE_WpfApp.ViewModels
{
    internal class MainWindVM : BindableBase 
    {
		private bool _continueGame = false;

		public bool ContinueGame
		{
			get => _continueGame;
            private set
            { 
				_continueGame = value;
				RaisePropertyChanged(nameof(ContinueGame));

				if (_continueGame) SnakeGo(); 
			}
		}

        private double _cellD = 50;
        public double CellD
        {
            get => _cellD;
            set
            {
                _cellD = value;
                RaisePropertyChanged(nameof(CellD));
            }
        }

        public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

		public DelegateCommand StartStopCommand { get; }
		private MoveDirection _currentMoveDirection = MoveDirection.Right;

		private int _rowCount = 20;
		private int _columnCount = 20;
		private const int speed = 400;
        private int _speed = speed;

        private Snake _snake;
		private MainWindow _mainWnd;
        private CellVM _lastFood;

        public MainWindVM(MainWindow mainWnd)
        {
            _mainWnd = mainWnd;

            StartStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);

			for (int row = 0; row < _rowCount; row++)
			{ 
				var rowList = new List<CellVM>();
				for(int column = 0; column < _columnCount; column++)
				{
					var cell = new CellVM(row, column, Models.CellType.None);
					rowList.Add(cell);
				}
				
				AllCells.Add(rowList);
			}

            _snake = new Snake(AllCells, AllCells[_rowCount / 2][_columnCount / 2], CreateFood);
			CreateFood();

             _mainWnd.KeyDown += UserKeyDown;
            _mainWnd.Loaded += (s, e) => UpdateCell();
            _mainWnd.SizeChanged += (s, e) => UpdateCell();
        }

        private void UpdateCell()
        {
            if (_mainWnd.IsLoaded)
                CellD = (_mainWnd.Width - 150) / _columnCount;
        }


        private async Task SnakeGo()
		{
			while (ContinueGame)
			{
				await Task.Delay(_speed);
				try
				{
					_snake.Move(_currentMoveDirection);

				}
				catch (Exception ex) 
				{
                    ContinueGame = false;
					MessageBox.Show(ex.Message);
					_speed = speed;
					_snake.Restart();
					_lastFood.CellType = CellType.None;
					CreateFood();
                }
			}
		}

		private void UserKeyDown(object sender, KeyEventArgs e) 
		{
			switch (e.Key)
			{
				case Key.A:
					if (_currentMoveDirection != MoveDirection.Right)
					_currentMoveDirection = MoveDirection.Left; 
					break;
				case Key.D:
					if (_currentMoveDirection != MoveDirection.Left)
					_currentMoveDirection = MoveDirection.Right;
					break;
				case Key.W:
                    if (_currentMoveDirection != MoveDirection.Down)
                        _currentMoveDirection = MoveDirection.Up;
					break;
				case Key.S:
                    if (_currentMoveDirection != MoveDirection.Up)
                        _currentMoveDirection = MoveDirection.Down;
					break;
					default:
					break;

			}
		}


        private void CreateRandomFoodOld()
        {
            var rn = new Random();
            int foodRow = rn.Next(_rowCount);
            int foodColumn = rn.Next(_columnCount);

            _lastFood = AllCells[foodRow][foodColumn];

            if (_snake.SnakeCells.Contains(_lastFood))
            {
                CreateRandomFoodOld();
            }

            _lastFood.CellType = CellType.Food;
            _speed = (int)(_speed * 0.95);
        }

        private void CreateFood()
		{

            var noneCells = AllCells
                .SelectMany(x => x.Where(c => c.CellType == CellType.None))
                .ToArray();
            var rn = new Random();
            int randomIndex = rn.Next(noneCells.Count());

            _lastFood = noneCells[randomIndex];

            _lastFood.CellType = CellType.Food;
            _speed = (int)(_speed * 0.95);



        }

    }
}
