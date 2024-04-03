using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SzamKirakoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KiRajzol(3);
        }
        public void KiRajzol(int meret)
        {
            int[,] puzzleGrid = new int[meret, meret];
            Button[,] buttons = new Button[meret, meret];
            Grid DynamicGrid = new Grid();
            DynamicGrid.Width = 360;
            DynamicGrid.Height = 360;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
            Grid.SetRow(DynamicGrid, 1);
            grKulso.Children.Add(DynamicGrid);

            for (int i = 0; i < meret; i++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(columnDef);
                RowDefinition rowDef = new RowDefinition();
                DynamicGrid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < meret; j++)
                {
                    Button button = new Button();
                    int number = i * meret + j + 1;
                    puzzleGrid[i, j] = number;
                    buttons[i, j] = button;
                    button.Content = number == meret * meret ? "" : number.ToString();
                    button.Width = 120;
                    button.Height = 120;
                    button.Click += (sender, e) => Button_Click(sender, e, button, puzzleGrid, buttons, meret);
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    DynamicGrid.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e, Button clickedButton, int[,] puzzleGrid, Button[,] buttons, int meret)
        {
            int clickedRow = Grid.GetRow(clickedButton);
            int clickedCol = Grid.GetColumn(clickedButton);

            if ((clickedRow > 0 && puzzleGrid[clickedRow - 1, clickedCol] == meret * meret) ||
                (clickedRow < meret - 1 && puzzleGrid[clickedRow + 1, clickedCol] == meret * meret) ||
                (clickedCol > 0 && puzzleGrid[clickedRow, clickedCol - 1] == meret * meret) ||
                (clickedCol < meret - 1 && puzzleGrid[clickedRow, clickedCol + 1] == meret * meret))
            {
                int emptyRow = -1, emptyCol = -1;
                for (int i = 0; i < meret; i++)
                {
                    for (int j = 0; j < meret; j++)
                    {
                        if (puzzleGrid[i, j] == meret * meret)
                        {
                            emptyRow = i;
                            emptyCol = j;
                            break;
                        }
                    }
                    if (emptyRow != -1) break;
                }
                if (!string.IsNullOrEmpty(clickedButton.Content.ToString()))
                {
                    buttons[emptyRow, emptyCol].Content = clickedButton.Content;
                    buttons[emptyRow, emptyCol].IsEnabled = true;
                    clickedButton.Content = "";
                    clickedButton.IsEnabled = false;

                    puzzleGrid[emptyRow, emptyCol] = int.Parse(buttons[emptyRow, emptyCol].Content.ToString());
                    puzzleGrid[clickedRow, clickedCol] = meret * meret;
                }
            }
        }
    }
}