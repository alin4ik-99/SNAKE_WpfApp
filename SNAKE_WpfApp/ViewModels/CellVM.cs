using Prism.Mvvm;
using SNAKE_WpfApp.Models;

namespace SNAKE_WpfApp.ViewModels
{
    internal class CellVM : BindableBase
    {
        public int Row { get; }
        public int Column { get; }

        private CellType _cellType = CellType.None;

        public CellType CellType
        {
            get { return _cellType; }
            set 
            {
                _cellType = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column;
            CellType = cellType;
        }
    }
}
