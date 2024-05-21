using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace spaceRace
{
    public partial class Form1 : Form
    {
        //brushes
        SolidBrush p2Brush = new SolidBrush(Color.Red);
        SolidBrush p1Brush = new SolidBrush(Color.Blue);
        SolidBrush asteroidBrush = new SolidBrush(Color.Khaki);
        SolidBrush explodeBrush = new SolidBrush(Color.White);

        SoundPlayer score = new SoundPlayer(Properties.Resources.Robot_blip_Marianne_Gagnon_120342607);

        Random rando = new Random();
        int randVal = 0;

        bool upPressed = false;
        bool downPressed = false;
        bool wPressed = false;
        bool sPressed = false;

        int playerSpeed = 6;
        int startY = 478;
        int p1Score = 0;
        int p2Score = 0;

        //players
        Rectangle player1 = new Rectangle(141, 478, 18, 22);
        Rectangle player2 = new Rectangle(441, 478, 18, 22);

        List<Rectangle> asteroidList = new List<Rectangle>();
        List<int> speedList = new List<int>();
        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeGame()
        {
            gameTimer.Enabled = true;
           
            p1Score = 0;
            p2Score = 0;

            asteroidList.Clear();
            speedList.Clear();

            player1.Y = startY;
            player2.Y = startY;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Space:
                    InitializeGame();
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }
                    break;
            }

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameTimer.Enabled == false && p1Score == 0 && p2Score == 0)
            {
                titleLabel.Text = "Space  Race";
                subtitleLabel.Text = "Sace to start or Esc to exit";
            }
            else if (gameTimer.Enabled == false && (p1Score == 4 || p2Score == 4))
            {
                titleLabel.Text = "Game  Over";
            }
            else if (gameTimer.Enabled == true)
            {
                titleLabel.Text = "";
                subtitleLabel.Text = "";
                e.Graphics.FillRectangle(p1Brush, player1);
                e.Graphics.FillRectangle(p2Brush, player2);
                for (int i = 0; i < asteroidList.Count; i++)
                {
                    e.Graphics.FillRectangle(asteroidBrush, asteroidList[i]);
                }
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
           //move players
           if(upPressed == true && player2.Y >= 0)
           {
                player2.Y -= playerSpeed;
           }
           if (downPressed == true && player2.Y <= startY)
           {
                player2.Y += playerSpeed;
           }

           if (wPressed == true && player1.Y >= 0)
           {
                player1.Y -= playerSpeed;
           }
           if (sPressed == true && player1.Y <= startY)
           {
                player1.Y += playerSpeed;
           }

            //makin dem rox
           randVal = rando.Next(0, 2);
           if (randVal == 1)
           {
               randVal = rando.Next(1, 100);
               if (randVal < 35)
               {
                    randVal = rando.Next(10, 470);

                    Rectangle leftrock = new Rectangle(0, randVal, 20, 6);
                    asteroidList.Add(leftrock);
                    speedList.Add(rando.Next(6, 10));
               }
           }
           else
           {
               randVal = rando.Next(1, 100);
               if (randVal < 35)
               {
                   randVal = rando.Next(10, 470);
                   
                   Rectangle rightrock = new Rectangle(this.Width, randVal, 20, 6);
                   asteroidList.Add(rightrock);

                    speedList.Add(rando.Next(-10, -6));
               }
           }

           //move the asteroids
           for (int i = 0; i < asteroidList.Count; i++)
           {
                int x = asteroidList[i].X + speedList[i];

                asteroidList[i] = new Rectangle(x, asteroidList[i].Y, 20, 6);
           }

            //remove the asteroids
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (asteroidList[i].X == 0 || asteroidList[i].X == this.Width - 19)
                {
                    asteroidList.RemoveAt(i);
                    speedList.RemoveAt(i);
                }
            }

            //collisions between player and rocks
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (player1.IntersectsWith(asteroidList[i]))
                {
                    player1.Y = startY;
                }
                if (player2.IntersectsWith(asteroidList[i]))
                {
                    player2.Y = startY;
                }
            }

            //scoring
            if (player1.Y < 0)
            {
                score.Play();
                p1Score++;
                player1.Y = startY;
                p1ScoreLabel.Text = p1Score.ToString();
            }
            if (player2.Y < 0)
            {
                score.Play();
                p2Score++;
                player2.Y = startY;
                p2ScoreLabel.Text = p2Score.ToString();
            }

            if (p1Score == 4)
            {
                p1ScoreLabel.ForeColor = Color.DeepSkyBlue;
                p2ScoreLabel.Text = "X";
                gameTimer.Enabled = false;
            }
           else if (p2Score == 4)
           {
               p2ScoreLabel.ForeColor = Color.OrangeRed;
               p1ScoreLabel.Text = "X";
               gameTimer.Enabled = false;
            }


            //keep at bottom
            Refresh();
        }
    }
}
