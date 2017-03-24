using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gobang_五子棋_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initializatize the chess
            //初始化棋盘
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    main_chess[i, j] = new GobangPoint();
                }
            }

            Richard_Hu = new ComputerAI_Richard_Hu(main_chess);
        }

        #region Property of Chess [棋盘的相关属性]

        /// <summary>
        /// when mouse move over pictureBox control 
        /// 鼠标移动过后时提示的位置
        /// </summary>
        private Point MouseActivePosition { get; set; } = new Point(-1, -1);
        /// <summary>
        /// last position of the pawn
        /// 上次落子的位置
        /// </summary>
        private Point LastPressPosition { get; set; } = new Point(-1, -1);
        /// <summary>
        /// means the count of the pawns
        /// 所有落子的数量
        /// </summary>
        private int PawnIndex { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        private bool IsMouseHoverOnChess { get; set; } = true;

        private StringFormat StringFormatCenter { get; set; } = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };

        private bool IsGamePlaying { get; set; } = false;
        

        private GobangPlayer CurrentPlayer { get; set; } = GobangPlayer.Player1;

        private Point LastPointOfPlayerUser { get; set; } = new Point(-1, -1);

        private bool IsGameOver(int x, int y, GobangPlayer player)
        {
            int m_lenght = 1;
            int m_x = x;
            int m_y = y;
            GobangPlayer m_play = player;
            for (int j = 0; j < 4; j++)
            {
                m_lenght = 1;
                Point m_Point_1;

                for (int i = 1; i < 5; i++)
                {
                    if (j == 0) m_Point_1 = new Point(m_x - i, m_y);
                    else if (j == 1) m_Point_1 = new Point(m_x, m_y - i);
                    else if (j == 2) m_Point_1 = new Point(m_x - i, m_y + i);
                    else m_Point_1 = new Point(m_x - i, m_y - i);
                    if (IsPositionHasPawn(m_Point_1, player))
                    {
                        m_lenght++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 1; i < 5; i++)
                {
                    if (j == 0) m_Point_1 = new Point(m_x + i, m_y);
                    else if (j == 1) m_Point_1 = new Point(m_x, m_y + i);
                    else if (j == 2) m_Point_1 = new Point(m_x + i, m_y - i);
                    else m_Point_1 = new Point(m_x + i, m_y + i);
                    if (IsPositionHasPawn(m_Point_1, player))
                    {
                        m_lenght++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (m_lenght >= 5) return true;
                m_lenght = 1;
            }
            return false;
        }

        #endregion

        private GobangPoint[,] main_chess = new GobangPoint[21, 21];


        
        
        private void StartGame()
        {
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    main_chess[i, j].GobangPlayer = GobangPlayer.NonePlayer;
                    main_chess[i, j].StepNumber = 0;
                    main_chess[i, j].WeightScore = 0;
                }
            }
            IsGamePlaying = true;
            CurrentPlayer = GobangPlayer.NonePlayer;
            PawnIndex = 1;
            
            if (checkBox2.Checked)
            {
                SetPointPawn(new Point(10, 10), GobangPlayer.Player1);
            }
            else
            {

            }

            label3.Text = "Waitting";

            pictureBox1.Refresh();
        }

        private Bitmap GetBackgroundImage()
        {
            Bitmap bitmap = new Bitmap(525, 525);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            Pen m_pen = Pens.LightGray;

            for (int i = 10; i <= 510; i += 25)
            {
                g.DrawLine(m_pen, new Point(10, i), new Point(510, i));
                g.DrawLine(m_pen, new Point(i, 10), new Point(i, 510));
            }
            return bitmap;
        }

        private bool IsPositionHasPawn(int x,int y)
        {
            if (x >= 0 && x < 21 && y >= 0 && y < 21)
            {
                return main_chess[x, y].GobangPlayer != GobangPlayer.NonePlayer;
            }
            else
            {
                return false;
            }
        }
        private bool IsPositionHasPawn(int x, int y, GobangPlayer player)
        {
            if (x >= 0 && x < 21 && y >= 0 && y < 21)
            {
                return main_chess[x, y].GobangPlayer == player;
            }
            else
            {
                return false;
            }
        }
        private bool IsPositionHasPawn(Point point, GobangPlayer player)
        {
            return IsPositionHasPawn(point.X, point.Y, player);
        }

        private bool IsPositionHasPawn(Point point)
        {
            return IsPositionHasPawn(point.X, point.Y);
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //paint the whole chess [绘制棋盘]
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    int m_x = i * 25 + 10;
                    int m_y = (20 - j) * 25 + 10;

                    if (main_chess[i, j].GobangPlayer == GobangPlayer.NonePlayer)
                    {
                        if(checkBox3.Checked)
                        {
                            Rectangle m_rect = new Rectangle(m_x - 20, m_y - 10, 40, 20);
                            using (Font fontSmall = new Font("Microsoft YaHei UI", 8))
                            {
                                g.DrawString(main_chess[i, j].WeightScore.ToString(), fontSmall, Brushes.Gray, m_rect, StringFormatCenter);
                            }
                        }
                    }
                    else
                    {
                        Rectangle m_rect = new Rectangle(m_x - 10, m_y - 10, 20, 20);
                        g.FillEllipse(main_chess[i, j].GetPawnBrush, m_rect);
                        g.DrawEllipse(Pens.DimGray, m_rect);

                        //m_rect.Offset(1, 1);
                        m_rect.Offset(-10, 0);
                        m_rect.Width += 20;
                        if(checkBox1.Checked)
                        {
                            using (Font fontSmall = new Font("Microsoft YaHei UI", 8))
                            {
                                g.DrawString(main_chess[i, j].StepNumber.ToString(),fontSmall, Brushes.White, m_rect, StringFormatCenter);
                            }
                        }
                    }
                }
            }

            //paint the active position
            if(IsMouseHoverOnChess)
            {
                PaintMarkPoint(g, MouseActivePosition);
            }
            if (!checkBox1.Checked)
            {
                //paint the active pawn
                PaintMarkPoint(g, LastPressPosition);
            }
        }

        /// <summary>
        /// translate the mouse position into the chess position
        /// </summary>
        /// <param name="picture_x">mouse point x</param>
        /// <param name="picture_y">mouse point y</param>
        /// <returns></returns>
        private Point MouseMovePoint(int picture_x, int picture_y)
        {
            //0~20  25~45  50~70
            int m_x = picture_x / 25;
            int m_y = picture_y / 25;
            m_y = 20 - m_y;
            return new Point(m_x, m_y);
        }

        private void PaintMarkPoint(Graphics g, Point point)
        {
            if (point.X >= 0)
            {
                int m_x = point.X * 25 + 10;
                int m_y = (20 - point.Y) * 25 + 10;
                Pen pen = Pens.LightGray;
                

                if (main_chess[point.X, point.Y].GobangPlayer == GobangPlayer.Player1)
                {
                    pen = Pens.Red;
                }
                g.DrawLines(pen, new Point[]
                {
                    new Point(m_x-3,m_y-9),new Point(m_x-3,m_y-3),new Point(m_x-9,m_y-3)
                });
                g.DrawLines(pen, new Point[]
                {
                    new Point(m_x-3,m_y+9),new Point(m_x-3,m_y+3),new Point(m_x-9,m_y+3)
                });
                g.DrawLines(pen, new Point[]
                {
                    new Point(m_x+3,m_y-9),new Point(m_x+3,m_y-3),new Point(m_x+9,m_y-3)
                });
                g.DrawLines(pen, new Point[]
                {
                    new Point(m_x+3,m_y+9),new Point(m_x+3,m_y+3),new Point(m_x+9,m_y+3)
                });
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureBox1.Image = GetBackgroundImage();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            MouseActivePosition = new Point(-1, -1);
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X % 25 > 20 || e.X > 520 ||
                e.Y % 25 > 20 || e.Y > 520)
            {
                IsMouseHoverOnChess = false;
                pictureBox1.Refresh();
                return;
            }
            MouseActivePosition = MouseMovePoint(e.X, e.Y);

            IsMouseHoverOnChess = !IsPositionHasPawn(MouseActivePosition);

            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void SetPointPawn(Point point, GobangPlayer player)
        {
            main_chess[point.X, point.Y].GobangPlayer = player;
            main_chess[point.X, point.Y].StepNumber = PawnIndex++;
            LastPressPosition = point;
            CurrentPlayer = player;

            if (player == GobangPlayer.Player1)
            {
                Richard_Hu.CalculateAllPoints();
            }
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            //press the chess board
            if (!IsGamePlaying) return;

            Point point_origin = pictureBox1.PointToClient(Cursor.Position);

            //check the position is correct
            if (point_origin.X % 25 > 20 || point_origin.X > 520)
            {
                return;
            }
            if (point_origin.Y % 25 > 20 || point_origin.Y > 520)
            {
                return;
            }
            Point point = MouseMovePoint(point_origin.X, point_origin.Y);
            // already has pawn
            if (IsPositionHasPawn(point)) return;

            if (CurrentPlayer != GobangPlayer.Player2)
            {
                SetPointPawn(point, GobangPlayer.Player2);

                LastPointOfPlayerUser = point;

                if (IsGameOver(point.X, point.Y, GobangPlayer.Player2))
                {
                    IsGamePlaying = false;
                    label3.Text = "You Win";
                    label_score_you.Text = (Convert.ToInt32(label_score_you.Text) + 1).ToString();
                }
                else
                {
                    //waitting for computer to calculate
                    label3.Text = "Thinking";

                    Point computer = await Richard_Hu.CalculateComputerAI();

                    SetPointPawn(computer, GobangPlayer.Player1);

                    if (IsGameOver(computer.X, computer.Y, GobangPlayer.Player1))
                    {
                        label3.Text = "You lose";
                        IsGamePlaying = false;
                        label_score_computer.Text = (Convert.ToInt32(label_score_computer.Text) + 1).ToString();
                    }
                    else
                    {
                        label3.Text = "waitting";
                    }
                    pictureBox1.Refresh();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }


        #region Computer AI

        ComputerAI_Richard_Hu Richard_Hu = null;




        #endregion

        
    }
}
