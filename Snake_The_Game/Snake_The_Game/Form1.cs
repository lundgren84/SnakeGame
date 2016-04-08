using Snake_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_The_Game
{
    public partial class Form1 : Form
    {
        List<Circle> Snake;
        Circle food;
        bool GameDone = true;
        List<Score> HighScore;
        string name = "";



        public Form1()
        {
            InitializeComponent();
            timerGame.Tick += TimerGame_Tick;
            timerMove.Tick += TimerMove_Tick;
            pbCanvas.Paint += PbCanvas_Paint;
              HighScore = LoadAndSaveGame.LoadG();
             SetScoresOnBoard();
        }

        private void SetScoresOnBoard()
        {
            for (int i = 0; i < HighScore.Count; i++)
            {
                lblName1.Text = HighScore[0]._name;
                lblScore1.Text = HighScore[0]._score.ToString();
                lblName2.Text = HighScore[1]._name;
                lblScore2.Text = HighScore[1]._score.ToString();
                lblName3.Text = HighScore[2]._name;
                lblScore3.Text = HighScore[2]._score.ToString();
            }
        }

        private void TimerMove_Tick(object sender, EventArgs e)
        {
            this.KeyDown += new KeyEventHandler(ChangeDirection);
        }

        private void TimerGame_Tick(object sender, EventArgs e)
        {
            MovePlayer();
            pbCanvas.Invalidate();
        }

        private void StartGame()
        {
            pbCanvas.Image = Properties.Resources.untitled;
            Snake = new List<Circle>();
            new Settings();
            GameDone = false;
            //Creat a new Player object
            Circle head = new Circle();
            head.x = 15;
            head.y = 12;
            Snake.Add(head);
            lblScore.Text = Settings.score.ToString();
            GenerateFood();

            timerGame.Start();
            timerMove.Start();
        }
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / 15;
            int maxYPos = pbCanvas.Size.Height / 15;

            Random random = new Random();
            food = new Circle();
            food.x = random.Next(0, maxXPos);
            food.y = random.Next(0, maxYPos);
        }

        private void PbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!GameDone)
            {

                //    Sett colour of snake
                Brush snakeColor;
                //Draw snake
                for (int i = Snake.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Black;
                    }
                    else
                        snakeColor = Brushes.Green;


                    //Paint Snake
                    canvas.FillEllipse(snakeColor,
                         new Rectangle(Snake[i].x * Settings.Width,
                                       Snake[i].y * Settings.Height,
                                       Settings.Width, Settings.Height));


                    //Paint food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.x * Settings.Width,
                        food.y * Settings.Height, Settings.Width, Settings.Height));
                }

            }
        }

        private void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // handle up/down/left/right
                case Keys.Up:
                    if (Settings.direction != Direction.Down)
                        Settings.direction = Direction.Up;
                    break;
                case Keys.Left:
                    if (Settings.direction != Direction.Right)
                        Settings.direction = Direction.Left;
                    break;
                case Keys.Right:
                    if (Settings.direction != Direction.Left)
                        Settings.direction = Direction.Right;
                    break;
                case Keys.Down:
                    if (Settings.direction != Direction.Up)
                        Settings.direction = Direction.Down;
                    break;
                case Keys.Space:
                    Settings.direction = Direction.Stop;
                    break;
                default: return;  // ignore other keys
            }

            //if (Inputs.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
            //    Settings.direction = Direction.Right;
            //else if (Inputs.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
            //    Settings.direction = Direction.Left;
            //else if (Inputs.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
            //    Settings.direction = Direction.Up;
            //else if (Inputs.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
            //    Settings.direction = Direction.Down;
        }
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //MoveHead
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Left:
                            Snake[i].x--;
                            break;
                        case Direction.Right:
                            Snake[i].x++;
                            break;
                        case Direction.Up:
                            Snake[i].y--;
                            break;
                        case Direction.Down:
                            Snake[i].y++;
                            break;
                        default:
                            break;
                    }

                    var MaxX = pbCanvas.Size.Width / Settings.Width;
                    var MaxY = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].x == MaxX || Snake[i].y == MaxY || Snake[i].x < 0 || Snake[i].y < 0)
                    {
                        Die();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            Die();
                        }
                    }
                    if (Snake[0].x == food.x && Snake[0].y == food.y)
                    {
                        Eat();
                    }


                }
                else
                {
                    //Move body
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }

        }

        private void Eat()
        {
            Circle food = new Circle();
            food.x = Snake[Snake.Count - 1].x;
            food.y = Snake[Snake.Count - 1].y;
            Snake.Add(food);
            Settings.score += Settings.points;
            lblScore.Text = Settings.score.ToString();
            GenerateFood();
        }

        private void Die()
        {
            GameDone = true;
            pbCanvas.Image = Properties.Resources.Snake;

            Score score = new Score(name, Settings.score);
            HighScore = LoadAndSaveGame.CheckHighScore(HighScore, score);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes ==

            MessageBox.Show("Are you sure?", "Important", MessageBoxButtons.YesNo))
            {
                this.Close();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
            pbCanvas.Invalidate();
        }

        private void changeSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pbSave.Visible = true;
            //pbSave.BringToFront();
            //btnSave.Visible = true;
            //btnSave.BringToFront();
            //tbSave.Visible = true;
            //tbSave.BringToFront();
            //lblSave.Visible = true;
            //lblSave.BringToFront();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //name = tbSave.Text;
            //Score score = new Score(name, Settings.score);
            //HighScore = LoadAndSaveGame.CheckHighScore(HighScore,score);       
            //pbSave.Visible = false;
            //btnSave.Visible = false;      
            //tbSave.Visible = false;        
            //lblSave.Visible = false;
            

        }
    }
}
