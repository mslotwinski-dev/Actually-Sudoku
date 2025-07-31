namespace ActuallySudoku {

    public partial class Screen : Form {
        TextBox[,] cells = new TextBox[9, 9];
        Random rand = new Random();

        // Boolean isChecked = false;
        PictureBox hatch;


        public Screen() {
            hatch = new PictureBox();

            InitializeComponent();
            CreateSudokuGrid();
        }

        private void CreateSudokuGrid() {
            int cellSize = 40;
            int gridSize = cellSize * 9;
            int blockSpacing = 4;
            var solution = GenerateSudokuSolution();



            Label textLabel = new Label();
            textLabel.Text = "Actually Sudoku";
            textLabel.Font = new Font("Rubik", 20, FontStyle.Bold);
            textLabel.AutoSize = true;
            textLabel.ForeColor = Color.DarkSlateGray;
            this.Controls.Add(textLabel);
            textLabel.Location = new Point(
                (this.Width - textLabel.Width) / 2,
                40
            );

            Panel gridPanel = new Panel();
            gridPanel.Size = new Size(gridSize + blockSpacing * 2, gridSize + blockSpacing * 2);
            gridPanel.Location = new Point(
                (this.ClientSize.Width - gridPanel.Width) / 2,
                (this.ClientSize.Height - gridPanel.Height) / 2
            );
            gridPanel.BackColor = Color.LightSlateGray;
            this.Controls.Add(gridPanel);

            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    TextBox tb = new TextBox();
                    tb.BorderStyle = BorderStyle.None;
                    tb.TextAlign = HorizontalAlignment.Center;

                    if (col % 2 != row % 2) {
                        tb.BackColor = Color.LightGray;
                    }

                    tb.Multiline = true;

                    var r = rand.NextDouble();
                    if (r < 0.25) {
                        tb.Text = solution[row, col] == 0 ? "" : solution[row, col].ToString();
                    }


                    tb.Font = new Font("Rubik", 18, FontStyle.Regular);
                    tb.Size = new Size(cellSize, cellSize);

                    tb.Location = new Point(col * cellSize + (col - col % 3) / 3 * blockSpacing, row * cellSize + (row - row % 3) / 3 * blockSpacing);
                    tb.MaxLength = 1;
                    tb.Tag = new Point(row, col);

                    gridPanel.Controls.Add(tb);


                    cells[row, col] = tb;
                }
            }

            Panel buttonsPanel = new Panel();
            buttonsPanel.Location = new Point(10, 10);
            buttonsPanel.Size = new Size(180, 300);

            this.Controls.Add(buttonsPanel);

            Button showButton = new Button();
            showButton.Text = "Show Solution";
            showButton.Size = new Size(150, 30);
            showButton.Location = new Point(10, 10);
            showButton.Click += (sender, e) => {
                for (int row = 0; row < 9; row++) {
                    for (int col = 0; col < 9; col++) {
                        if (solution[row, col] != 0) {
                            cells[row, col].Text = solution[row, col].ToString();
                        }
                    }
                }
            };
            buttonsPanel.Controls.Add(showButton);

            Button checkButton = new Button();
            checkButton.Text = "Check";
            checkButton.Size = new Size(150, 30);
            checkButton.Location = new Point(10, 50);
            checkButton.Click += (sender, e) => {
                CheckSolution(solution);
            };
            buttonsPanel.Controls.Add(checkButton);

            hatch.Image = Image.FromFile("E:\\GitHub\\ActuallySudoku\\src\\Assets\\kula1.png");
            hatch.SizeMode = PictureBoxSizeMode.StretchImage;
            hatch.Location = new Point(35, 100);
            hatch.Size = new Size(100, 100);
            buttonsPanel.Controls.Add(hatch);
        }

        int[,] GenerateSudokuSolution() {
            int[,] grid = new int[9, 9];

            bool FillGrid(int row, int col) {
                if (row == 9)
                    return true;
                if (col == 9)
                    return FillGrid(row + 1, 0);

                List<int> numbers = Enumerable.Range(1, 9).OrderBy(n => rand.Next()).ToList();

                foreach (int num in numbers) {
                    if (IsSafe(grid, row, col, num)) {
                        grid[row, col] = num;
                        if (FillGrid(row, col + 1))
                            return true;
                        grid[row, col] = 0;
                    }
                }
                return false;
            }

            bool IsSafe(int[,] g, int row, int col, int num) {
                for (int i = 0; i < 9; i++) {
                    if (g[row, i] == num || g[i, col] == num)
                        return false;
                }

                int startRow = row / 3 * 3;
                int startCol = col / 3 * 3;
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        if (g[startRow + i, startCol + j] == num)
                            return false;
                    }
                }

                return true;
            }

            FillGrid(0, 0);
            return grid;
        }

        void CheckSolution(int[,] solution) {

            bool isCorrect = true;

            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    if (cells[row, col].Text != solution[row, col].ToString() && solution[row, col] != 0) {
                        cells[row, col].BackColor = Color.Red;
                        isCorrect = false;
                    }
                }
            }

            if (isCorrect) {
                hatch.Image = Image.FromFile("E:\\GitHub\\ActuallySudoku\\src\\Assets\\kula3.png");

                for (int row = 0; row < 9; row++) {
                    for (int col = 0; col < 9; col++) {
                        if (col % 2 != row % 2) {
                            cells[row, col].BackColor = Color.LightSeaGreen;
                        }
                        else {
                            cells[row, col].BackColor = Color.GreenYellow;
                        }

                    }
                }

                MessageBox.Show("Congratulations! The solution is correct.");

            }
            else {
                hatch.Image = Image.FromFile("E:\\GitHub\\ActuallySudoku\\src\\Assets\\kula2.png");

                MessageBox.Show("Some cells are incorrect. Please try again.");

                for (int row = 0; row < 9; row++) {
                    for (int col = 0; col < 9; col++) {
                        if (col % 2 != row % 2) {
                            cells[row, col].BackColor = Color.LightGray;
                        }
                        else {
                            cells[row, col].BackColor = Color.White;
                        }
                    }
                }

                hatch.Image = Image.FromFile("E:\\GitHub\\ActuallySudoku\\src\\Assets\\kula4.png");
            }
        }
    }


}
