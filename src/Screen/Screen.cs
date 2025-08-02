using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ActuallySudoku {
    public partial class Screen : Form {

        // --- Constants ---
        private const int CellSize = 40;
        private const int BlockSpacing = 4;
        private const int GridDimension = 9;
        private const int TotalGridSize = CellSize * GridDimension + BlockSpacing * 2;

        // --- Fields ---
        private readonly TextBox[,] _cells = new TextBox[GridDimension, GridDimension];
        private readonly SudokuGame _game;
        private readonly PictureBox _hatchPictureBox;

        public Screen() {
            InitializeComponent();

            _game = new SudokuGame();
            _hatchPictureBox = new PictureBox();

            CreateSudokuUI();

            StartNewGame();
        }

        private void StartNewGame() {
            _game.GenerateNewPuzzle(30);
            PopulateGrid();
            ResetCellColors();
            _hatchPictureBox.Image = Image.FromFile("src/Assets/kula1.png");
        }

        private void CreateSudokuUI() {
            CreateTitleLabel();
            CreateGridPanel();
            CreateControlsPanel();
        }

        private void CreateTitleLabel() {
            Label textLabel = new Label {
                Text = "Actually Sudoku",
                Font = new Font("Rubik", 20, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkSlateGray
            };
            textLabel.Location = new Point((this.Width - textLabel.Width) / 2, 40);
            this.Controls.Add(textLabel);
        }

        private void CreateGridPanel() {
            Panel gridPanel = new Panel {
                Size = new Size(TotalGridSize, TotalGridSize),
                Location = new Point((this.ClientSize.Width - TotalGridSize) / 2, (this.ClientSize.Height - TotalGridSize) / 2),
                BackColor = Color.LightSlateGray
            };
            this.Controls.Add(gridPanel);

            for (int row = 0; row < GridDimension; row++) {
                for (int col = 0; col < GridDimension; col++) {
                    TextBox tb = new TextBox {
                        BorderStyle = BorderStyle.None,
                        TextAlign = HorizontalAlignment.Center,
                        Multiline = true,
                        Font = new Font("Rubik", 18, FontStyle.Regular),
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(col * CellSize + (col / 3) * BlockSpacing, row * CellSize + (row / 3) * BlockSpacing),
                        MaxLength = 1,
                        Tag = new Point(row, col)
                    };

                    gridPanel.Controls.Add(tb);
                    _cells[row, col] = tb;
                }
            }
        }

        private void CreateControlsPanel() {
            Panel buttonsPanel = new Panel { Location = new Point(10, 10), Size = new Size(180, 300) };
            this.Controls.Add(buttonsPanel);

            Button showButton = new Button { Text = "Show Solution", Size = new Size(150, 30), Location = new Point(10, 10) };
            showButton.Click += ShowButton_Click;
            buttonsPanel.Controls.Add(showButton);

            Button checkButton = new Button { Text = "Check", Size = new Size(150, 30), Location = new Point(10, 50) };
            checkButton.Click += CheckButton_Click;
            buttonsPanel.Controls.Add(checkButton);

            _hatchPictureBox.Image = Image.FromFile("src/Assets/kula1.png");
            _hatchPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _hatchPictureBox.Location = new Point(35, 100);
            _hatchPictureBox.Size = new Size(100, 100);
            buttonsPanel.Controls.Add(_hatchPictureBox);
        }

        private void PopulateGrid() {
            for (int row = 0; row < GridDimension; row++) {
                for (int col = 0; col < GridDimension; col++) {
                    if (_game.Puzzle[row, col] != 0) {
                        _cells[row, col].Text = _game.Puzzle[row, col].ToString();
                        _cells[row, col].ReadOnly = true;
                        _cells[row, col].Font = new Font(_cells[row, col].Font, FontStyle.Bold);
                    }
                    else {
                        _cells[row, col].Text = "";
                        _cells[row, col].ReadOnly = false;
                        _cells[row, col].Font = new Font(_cells[row, col].Font, FontStyle.Regular);
                    }
                }
            }
        }

        private void ResetCellColors() {
            for (int row = 0; row < GridDimension; row++) {
                for (int col = 0; col < GridDimension; col++) {
                    bool isLight = (col % 2 != row % 2);
                    _cells[row, col].BackColor = isLight ? Color.LightGray : Color.WhiteSmoke;
                }
            }
        }

        private void ShowButton_Click(object sender, EventArgs e) {
            for (int row = 0; row < GridDimension; row++) {
                for (int col = 0; col < GridDimension; col++) {
                    _cells[row, col].Text = _game.Solution[row, col].ToString();
                    bool isLight = (col % 2 != row % 2);
                    _cells[row, col].BackColor = isLight ? Color.LightSkyBlue : Color.LightBlue;
                }
            }
        }

        private void CheckButton_Click(object sender, EventArgs e) {
            ResetCellColors();
            var userGrid = new int[GridDimension, GridDimension];
            // bool isComplete = true;

            for (int row = 0; row < GridDimension; row++) {
                for (int col = 0; col < GridDimension; col++) {
                    if (int.TryParse(_cells[row, col].Text, out int value)) {
                        userGrid[row, col] = value;
                    }
                    else {
                        userGrid[row, col] = 0;
                        // isComplete = false;
                    }
                }
            }

            // if (!isComplete) {
            //     MessageBox.Show("Please fill all cells before checking.", "Incomplete Puzzle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     return;
            // }

            var incorrectCells = _game.CheckSolution(userGrid);

            if (incorrectCells.Count == 0) {
                _hatchPictureBox.Image = Image.FromFile("src/Assets/kula3.png");
                for (int row = 0; row < GridDimension; row++) {
                    for (int col = 0; col < GridDimension; col++) {
                        bool isLight = (col % 2 != row % 2);
                        _cells[row, col].BackColor = isLight ? Color.LightSeaGreen : Color.LightGreen;
                    }
                }
                MessageBox.Show("Congratulations! The solution is correct.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                _hatchPictureBox.Image = Image.FromFile("src/Assets/kula2.png");
                foreach (var point in incorrectCells) {
                    if (!_cells[point.X, point.Y].ReadOnly) {
                        _cells[point.X, point.Y].BackColor = Color.Salmon;
                    }
                }
                MessageBox.Show($"Found {incorrectCells.Count} incorrect cells. Please try again.", "Mistakes Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _hatchPictureBox.Image = Image.FromFile("src/Assets/kula1.png");
                ResetCellColors();
            }
        }
    }
}