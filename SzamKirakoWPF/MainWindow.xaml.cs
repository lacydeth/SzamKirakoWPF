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
        Grid DynamicGrid;
        Button[,] buttonGrid;
        int[,] puzzleGrid;
        public MainWindow()
        {
            InitializeComponent();
            KiRajzol(3);
        }
        public void KiRajzol(int size)
        {
            puzzleGrid = new int[size, size];
            buttonGrid = new Button[size, size];
            DynamicGrid = new Grid();
            
            if (cbMekkora.SelectedItem == cb4)
            {
                DynamicGrid.Width = 480;
                DynamicGrid.Height = 480;
            }
            else if (cbMekkora.SelectedItem == cb3)
            {
                DynamicGrid.Width = 360;
                DynamicGrid.Height = 360;
            }
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
            Grid.SetRow(DynamicGrid, 1);
            grKulso.Children.Add(DynamicGrid);

            for (int i = 0; i < size; i++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(columnDef);
                RowDefinition rowDef = new RowDefinition();
                DynamicGrid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();
                    int number = i * size + j + 1;
                    puzzleGrid[i, j] = number;
                    buttonGrid[i, j] = button;
                    button.Content = number == size * size ? "" : number.ToString();
                    button.IsEnabled = number == size * size ? false : true;
                    button.Width = 120;
                    button.Height = 120;
                    button.Click += (sender, e) => Button_Click(sender, e, button, puzzleGrid, buttonGrid, size);
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    DynamicGrid.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e, Button clickedButton, int[,] puzzleGrid, Button[,] buttonGrid, int size)
        {
            int clickedRow = Grid.GetRow(clickedButton);
            int clickedCol = Grid.GetColumn(clickedButton);

            if ((clickedRow > 0 && puzzleGrid[clickedRow - 1, clickedCol] == size * size) ||
                (clickedRow < size - 1 && puzzleGrid[clickedRow + 1, clickedCol] == size * size) ||
                (clickedCol > 0 && puzzleGrid[clickedRow, clickedCol - 1] == size * size) ||
                (clickedCol < size - 1 && puzzleGrid[clickedRow, clickedCol + 1] == size * size))
            {
                int emptyRow = -1, emptyCol = -1;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (puzzleGrid[i, j] == size * size)
                        {
                            emptyRow = i;
                            emptyCol = j;
                            break;
                        }
                    }
                    if (emptyRow != -1)
                    {
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(clickedButton.Content.ToString()))
                {
                    buttonGrid[emptyRow, emptyCol].Content = clickedButton.Content;
                    buttonGrid[emptyRow, emptyCol].IsEnabled = true;
                    clickedButton.Content = "";
                    clickedButton.IsEnabled = false;

                    puzzleGrid[emptyRow, emptyCol] = int.Parse(buttonGrid[emptyRow, emptyCol].Content.ToString());
                    puzzleGrid[clickedRow, clickedCol] = size * size;
                }
            }
        }

        private void btnKeveres_Click(object sender, RoutedEventArgs e)
        {
            Keveres(buttonGrid, puzzleGrid);
        }

        private void Keveres(Button[,] buttonGrid, int[,] puzzleGrid)
        {
            Random random = new Random();
            int rows = buttonGrid.GetLength(0);
            int cols = buttonGrid.GetLength(1);

            List<Button> buttonList = [.. buttonGrid];

            int n = buttonList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Button value = buttonList[k];
                buttonList[k] = buttonList[n];
                buttonList[n] = value;
            }

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    buttonGrid[i, j] = buttonList[index];
                    index++;

                    if (buttonGrid[i, j].Content.ToString() == "")
                    {
                        puzzleGrid[i, j] = rows * cols;
                    }
                    else
                    {
                        puzzleGrid[i, j] = int.Parse(buttonGrid[i, j].Content.ToString());
                    }
                }
            }

            DynamicGrid.Children.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Grid.SetColumn(buttonGrid[i, j], j);
                    Grid.SetRow(buttonGrid[i, j], i);
                    DynamicGrid.Children.Add(buttonGrid[i, j]);
                }
            }
        }

        private void cbMekkora_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DynamicGrid != null)
            {
                DynamicGrid.Children.Clear();

                if (cbMekkora.SelectedItem == cb3)
                {
                    KiRajzol(3);
                }
                else if (cbMekkora.SelectedItem == cb4)
                {
                    KiRajzol(4);
                }
            }
        }

        private void btnKesz_Click(object sender, RoutedEventArgs e)
        {
           bool order =  HelyesSorrend(buttonGrid, puzzleGrid);

            if (order) {
                MessageBox.Show("Sikeresen rakta ki a számkirakót!", "Helyes megoldás", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("A megoldása még nem a helyes sorrend szerint történt!", "Helytelen megoldás", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool HelyesSorrend(Button[,] buttonGrid, int[,] puzzleGrid)
        {
            int rows = buttonGrid.GetLength(0);
            int cols = buttonGrid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int correctValue = i * cols + j + 1;
                    if (puzzleGrid[i, j] != correctValue)
                    {
                        return false;
                    }
                }
            }

            if (puzzleGrid[rows - 1, cols - 1] != rows * cols)
            {
                return false;
            }

            return true;
        }
    }
}