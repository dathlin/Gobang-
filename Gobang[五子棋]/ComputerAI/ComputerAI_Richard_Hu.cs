using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gobang_五子棋_
{
    public class ComputerAI_Richard_Hu
    {
        public ComputerAI_Richard_Hu(GobangPoint[,] chess)
        {
            main_chess = chess;
        }



        public Task<Point> CalculateComputerAI()
        {
            return Task.Run<Point>(() =>
            {
                //电脑玩家所有的权重集合
                ChessWeight[,] m_Weights_Computer = new ChessWeight[21, 21];
                //玩家一的所有的权重集合
                ChessWeight[,] m_Weights_Player1 = new ChessWeight[21, 21];

                //初始化所有的权重子
                for (int i = 0; i <= 20; i++)
                {
                    for (int j = 0; j <= 20; j++)
                    {
                        if (main_chess[i, j].GobangPlayer != GobangPlayer.NonePlayer)
                        {
                            m_Weights_Computer[i, j] = new ChessWeight(new int[] { -1, -1, -1, -1 }, GobangPlayer.Player1);
                            m_Weights_Player1[i, j] = new ChessWeight(new int[] { -1, -1, -1, -1 }, GobangPlayer.Player2);
                            continue;
                        }
                        m_Weights_Computer[i, j] = GetWeight(i, j, GobangPlayer.Player1);
                        m_Weights_Player1[i, j] = GetWeight(i, j, GobangPlayer.Player2);
                        m_Weights_Computer[i, j].WeightOpponent =
                            m_Weights_Player1[i, j].WeightMax;
                        m_Weights_Player1[i, j].WeightOpponent =
                            m_Weights_Computer[i, j].WeightMax;
                    }
                }
                //to get the computer highest score point
                //获取电脑综合评定分最高的点
                List<Point> MaxPointComputer = new List<Point>();
                int MaxComputer = 0;
                int MaxPlayer1 = 0;
                List<Point> MaxPointPlayer1 = new List<Point>();
                for (int i = 0; i <= 20; i++)
                {
                    for (int j = 0; j <= 20; j++)
                    {
                        if (m_Weights_Computer[i, j].TotleScore > MaxComputer)
                        {
                            MaxComputer = m_Weights_Computer[i, j].TotleScore;
                            MaxPointComputer.Clear();
                            MaxPointComputer.Add(new Point(i, j));
                        }
                        else if (m_Weights_Computer[i, j].TotleScore == MaxComputer)
                        {
                            MaxPointComputer.Add(new Point(i, j));
                        }
                        if (m_Weights_Player1[i, j].TotleScore > MaxPlayer1)
                        {
                            MaxPlayer1 = m_Weights_Player1[i, j].TotleScore;
                            MaxPointPlayer1.Clear();
                            MaxPointPlayer1.Add(new Point(i, j));
                        }
                        else if (m_Weights_Player1[i, j].TotleScore == MaxPlayer1)
                        {
                            MaxPointPlayer1.Add(new Point(i, j));
                        }
                    }
                }
                if (MaxComputer > 49 && MaxPlayer1 < 200
                    || (MaxComputer >= 200 && MaxPlayer1 >= 200))
                {
                    int MaxTemp = 0;
                    Point MaxPoint = new Point();
                    for (int i = 0; i < MaxPointComputer.Count; i++)
                    {
                        if (m_Weights_Computer[MaxPointComputer[i].X,
                            MaxPointComputer[i].Y].WeightOpponent > MaxTemp)
                        {
                            MaxTemp = m_Weights_Computer[MaxPointComputer[i].X,
                            MaxPointComputer[i].Y].WeightOpponent;
                            MaxPoint = MaxPointComputer[i];
                        }
                    }
                    return MaxPoint;
                }

                if (MaxComputer >= MaxPlayer1)
                {
                    int MaxTemp = 0;
                    Point MaxPoint = new Point();
                    for (int i = 0; i < MaxPointComputer.Count; i++)
                    {
                        if (m_Weights_Computer[MaxPointComputer[i].X,
                            MaxPointComputer[i].Y].WeightOpponent > MaxTemp)
                        {
                            MaxTemp = m_Weights_Computer[MaxPointComputer[i].X,
                            MaxPointComputer[i].Y].WeightOpponent;
                            MaxPoint = MaxPointComputer[i];
                        }
                    }
                    return MaxPoint;
                }
                else
                {
                    int MaxTemp = 0;
                    Point MaxPoint = new Point();
                    for (int i = 0; i < MaxPointPlayer1.Count; i++)
                    {
                        if (m_Weights_Player1[MaxPointPlayer1[i].X,
                            MaxPointPlayer1[i].Y].WeightOpponent > MaxTemp)
                        {
                            MaxTemp = m_Weights_Player1[MaxPointPlayer1[i].X,
                            MaxPointPlayer1[i].Y].WeightOpponent;
                            MaxPoint = MaxPointPlayer1[i];
                        }
                    }
                    return MaxPoint;
                }
            });
        }



        private GobangPoint[,] main_chess = null;

        private GobangPoint GetClassPoint(int x, int y, GobangPlayer m_play)
        {
            if (x >= 0 && x < 21 && y >= 0 && y < 21)
            {
                return main_chess[x, y];
            }
            else
            {
                if (m_play == GobangPlayer.Player1)
                {
                    return new GobangPoint()
                    {
                        GobangPlayer = GobangPlayer.Player2
                    };
                }
                else
                {
                    return new GobangPoint()
                    {
                        GobangPlayer = GobangPlayer.Player1
                    };
                }
            }
        }

        private ChessWeight GetWeight(int m_x, int m_y, GobangPlayer m_play)
        {
            int[] m_Weight = new int[4];

            for (int i = 0; i < 4; i++)
            {
                m_Weight[i] = GetWeightSingle(i, m_x, m_y, m_play);
            }
            //return CalculateWright1(m_Weight, 4, m_play);
            ChessWeight CW = new ChessWeight(m_Weight, m_play);
            //if(m_play==Player.Player1)
            //{
            //if (CW.WeightMax == 7 && CW.WeightLarge != 5)
            //{
            //CW.WeightMax = 5;
            //}
            //}
            return CW;
        }

        private int GetWeightSingle(int Direction, int x, int y, GobangPlayer player)
        {
            //定义九个端子
            GobangPoint[] LEFT_TO_RIGHT = new GobangPoint[9];
            if (Direction == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    LEFT_TO_RIGHT[i] = GetClassPoint(x + i - 4, y, player);
                }
            }
            else if (Direction == 1)
            {
                for (int i = 0; i < 9; i++)
                {
                    LEFT_TO_RIGHT[i] = GetClassPoint(x, y + i - 4, player);
                }
            }
            else if (Direction == 2)
            {
                for (int i = 0; i < 9; i++)
                {
                    LEFT_TO_RIGHT[i] = GetClassPoint(x + i - 4, y + i - 4, player);
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    LEFT_TO_RIGHT[i] = GetClassPoint(x - (i - 4), y + i - 4, player);
                }
            }
            GobangPlayer player_enemy = GobangPlayer.Player1;
            if (player == GobangPlayer.Player1) player_enemy = GobangPlayer.Player2;

            GobangPoint OwnPawn = new GobangPoint() { GobangPlayer = player };
            GobangPoint OtherPawn = new GobangPoint() { GobangPlayer = player_enemy };

            //综合考虑所有的情况  
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                LEFT_TO_RIGHT[2].IsEmpty &&
                LEFT_TO_RIGHT[3].IsEmpty &&

                LEFT_TO_RIGHT[5].IsEmpty &&
                LEFT_TO_RIGHT[6].IsEmpty &&
                LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 1;
            }
            //五子
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn)
            {
                return 200;
            }
            if (LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn)
            {
                return 200;
            }
            if (LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn)
            {
                return 200;
            }
            if (LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn)
            {
                return 200;
            }
            if (LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                LEFT_TO_RIGHT[8] == OwnPawn)
            {
                return 200;
            }

            //四子两头空

            if (LEFT_TO_RIGHT[0].IsEmpty &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 50;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 50;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 50;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 50;
            }
            //o*o*o*o
            if (LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2].IsEmpty &&
                    LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6].IsEmpty &&
                    LEFT_TO_RIGHT[7] == OwnPawn)
            {
                return 50;
            }
            //四子一头空
            if (LEFT_TO_RIGHT[0] == OtherPawn &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[1] == OtherPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 12;
            }

            if (LEFT_TO_RIGHT[0].IsEmpty &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 12;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                LEFT_TO_RIGHT[8] == OtherPawn)
            {
                return 12;
            }


            //1
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                    LEFT_TO_RIGHT[1].IsEmpty &&
                    LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                    LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2].IsEmpty &&
                    LEFT_TO_RIGHT[3] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2].IsEmpty &&
                    LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                    LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3].IsEmpty)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3].IsEmpty &&

                    LEFT_TO_RIGHT[5] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3].IsEmpty &&

                    LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[5].IsEmpty &&
                    LEFT_TO_RIGHT[6] == OwnPawn &&
                    LEFT_TO_RIGHT[7] == OwnPawn &&
                    LEFT_TO_RIGHT[8] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5].IsEmpty &&
                    LEFT_TO_RIGHT[6] == OwnPawn &&
                    LEFT_TO_RIGHT[7] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5].IsEmpty &&
                    LEFT_TO_RIGHT[6] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6].IsEmpty &&
                    LEFT_TO_RIGHT[7] == OwnPawn &&
                    LEFT_TO_RIGHT[8] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6].IsEmpty &&
                    LEFT_TO_RIGHT[7] == OwnPawn)
            {
                return 11;
            }
            if (LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6] == OwnPawn &&
                    LEFT_TO_RIGHT[7].IsEmpty &&
                    LEFT_TO_RIGHT[8] == OwnPawn)
            {
                return 11;
            }
            //四子两头堵
            if (LEFT_TO_RIGHT[0] == OtherPawn &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[1] == OtherPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                LEFT_TO_RIGHT[8] == OtherPawn)
            {
                return 0;
            }

            //三子两头空=============================================
            //以下三种情况削减得分
            if (LEFT_TO_RIGHT[0] == OtherPawn &&
                LEFT_TO_RIGHT[1].IsEmpty &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5].IsEmpty &&
                LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 7;
            }
            if (
                LEFT_TO_RIGHT[1] == OtherPawn &&
                LEFT_TO_RIGHT[2].IsEmpty &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6].IsEmpty &&
                LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                LEFT_TO_RIGHT[3].IsEmpty &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7].IsEmpty &&
                LEFT_TO_RIGHT[8] == OtherPawn)
            {
                return 7;
            }

            if (LEFT_TO_RIGHT[1].IsEmpty &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 10;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 10;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 10;
            }

            //特殊处理
            if (LEFT_TO_RIGHT[0].IsEmpty &&
                    LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3].IsEmpty &&
                    LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 9;
            }
            if (LEFT_TO_RIGHT[0].IsEmpty &&
                    LEFT_TO_RIGHT[1] == OwnPawn &&
                    LEFT_TO_RIGHT[2].IsEmpty &&
                    LEFT_TO_RIGHT[3] == OwnPawn &&

                    LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 9;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                    LEFT_TO_RIGHT[2] == OwnPawn &&
                    LEFT_TO_RIGHT[3].IsEmpty &&

                    LEFT_TO_RIGHT[5] == OwnPawn &&
                    LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 9;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&

                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 9;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 9;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 9;
            }
            //三子一头空==================================================
            if (LEFT_TO_RIGHT[1] == OtherPawn &&
              LEFT_TO_RIGHT[2] == OwnPawn &&
              LEFT_TO_RIGHT[3] == OwnPawn &&

              LEFT_TO_RIGHT[5].IsEmpty &&
              LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 8;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
              LEFT_TO_RIGHT[3] == OwnPawn &&

              LEFT_TO_RIGHT[5] == OwnPawn &&
              LEFT_TO_RIGHT[6].IsEmpty &&
              LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 8;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&

              LEFT_TO_RIGHT[5] == OwnPawn &&
              LEFT_TO_RIGHT[6] == OwnPawn &&
              LEFT_TO_RIGHT[7].IsEmpty &&
              LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 8;
            }
            if (LEFT_TO_RIGHT[0].IsEmpty &&
              LEFT_TO_RIGHT[1].IsEmpty &&
              LEFT_TO_RIGHT[2] == OwnPawn &&
              LEFT_TO_RIGHT[3] == OwnPawn &&

              LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 8;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
              LEFT_TO_RIGHT[2].IsEmpty &&
              LEFT_TO_RIGHT[3] == OwnPawn &&

              LEFT_TO_RIGHT[5] == OwnPawn &&
              LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 8;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
              LEFT_TO_RIGHT[3].IsEmpty &&

              LEFT_TO_RIGHT[5] == OwnPawn &&
              LEFT_TO_RIGHT[6] == OwnPawn &&
              LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 8;
            }
            //特殊情况
            if (LEFT_TO_RIGHT[0] == OtherPawn &&
                  LEFT_TO_RIGHT[1] == OwnPawn &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&

                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[0] == OtherPawn &&
                 LEFT_TO_RIGHT[1] == OwnPawn &&
                 LEFT_TO_RIGHT[2].IsEmpty &&
                 LEFT_TO_RIGHT[3] == OwnPawn &&

                 LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[1] == OtherPawn &&
                 LEFT_TO_RIGHT[2] == OwnPawn &&
                 LEFT_TO_RIGHT[3].IsEmpty &&

                 LEFT_TO_RIGHT[5] == OwnPawn &&
                 LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&

                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&

                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&

                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 7;
            }


            if (LEFT_TO_RIGHT[0].IsEmpty &&
                  LEFT_TO_RIGHT[1] == OwnPawn &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[0].IsEmpty &&
                  LEFT_TO_RIGHT[1] == OwnPawn &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[8] == OtherPawn)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty &&
                  LEFT_TO_RIGHT[8] == OtherPawn)
            {
                return 7;
            }
            //特殊状态的冲三
            if (LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[8] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 7;
            }
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                  LEFT_TO_RIGHT[1] == OwnPawn &&
                  LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3].IsEmpty)
            {
                return 7;
            }
            //三子两头堵====================================

            if (LEFT_TO_RIGHT[1] == OtherPawn &&
                LEFT_TO_RIGHT[2] == OwnPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                LEFT_TO_RIGHT[3] == OwnPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&

                LEFT_TO_RIGHT[5] == OwnPawn &&
                LEFT_TO_RIGHT[6] == OwnPawn &&
                LEFT_TO_RIGHT[7] == OtherPawn)
            {
                return 0;
            }

            //两子两头空==========================================
            if (LEFT_TO_RIGHT[0] == OwnPawn &&
                 LEFT_TO_RIGHT[1].IsEmpty &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 6;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[7].IsEmpty &&
                  LEFT_TO_RIGHT[8] == OwnPawn)
            {
                return 6;
            }

            if (LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 5;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 5;
            }
            if (LEFT_TO_RIGHT[1].IsEmpty &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 5;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 5;
            }
            if (LEFT_TO_RIGHT[0].IsEmpty &&
                LEFT_TO_RIGHT[1] == OwnPawn &&
                  LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 5;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                LEFT_TO_RIGHT[7] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[8].IsEmpty)
            {
                return 5;
            }
            //两子一头空==============================================
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 4;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6].IsEmpty)
            {
                return 4;
            }
            if (LEFT_TO_RIGHT[2].IsEmpty &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 4;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 4;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[7].IsEmpty)
            {
                return 3;
            }
            if (LEFT_TO_RIGHT[1] == OtherPawn &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 3;
            }
            if (LEFT_TO_RIGHT[5] == OtherPawn &&
                  LEFT_TO_RIGHT[2] == OwnPawn &&
                  LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[1].IsEmpty)
            {
                return 3;
            }
            if (LEFT_TO_RIGHT[7] == OtherPawn &&
                  LEFT_TO_RIGHT[6] == OwnPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty &&
                  LEFT_TO_RIGHT[3].IsEmpty)
            {
                return 3;
            }
            //两子两头堵
            if (LEFT_TO_RIGHT[2] == OtherPawn &&
                  LEFT_TO_RIGHT[3] == OwnPawn &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 0;
            }
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[5] == OwnPawn &&
                  LEFT_TO_RIGHT[6] == OtherPawn)
            {
                return 0;
            }
            //一子两头空
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 1;
            }
            //一子一头空
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[5].IsEmpty)
            {
                return 2;
            }
            if (LEFT_TO_RIGHT[3].IsEmpty &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 2;
            }
            //一子两头堵
            if (LEFT_TO_RIGHT[3] == OtherPawn &&
                  LEFT_TO_RIGHT[5] == OtherPawn)
            {
                return 0;
            }
            return 1;
        }

    }

    public class ChessWeight
    {

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public ChessWeight()
        {

        }
        public ChessWeight(int[] data, GobangPlayer m_play)
        {
            ValuePlayer = m_play;
            Array.Sort(data);
            Array.Reverse(data);
            
            WeightMax = data[0];
            WeightLarge = data[1];
            WeightSmall = data[2];
            WeightMin = data[3];

            if ((WeightMax == 11 || WeightMax == 12) &&
                (WeightLarge == 11 || WeightLarge == 12))
            {
                WeightMax = 40;
            }
            if ((WeightMax == 11 || WeightMax == 12) &&
                (WeightLarge == 9 || WeightLarge == 10))
            {
                WeightMax = 30;
            }
            if (ValuePlayer == GobangPlayer.Player2)
            {
                if ((WeightMax == 11 || WeightMax == 12) && WeightLarge < 7)
                {
                    WeightMax = 7;
                }
            }
        }

        private GobangPlayer ValuePlayer = GobangPlayer.Player1;
        private int ValueOpponent = 0;

        /// <summary>
        /// user large weight
        /// 对方的最大权重的落子
        /// </summary>
        public int WeightOpponent
        {
            get { return ValueOpponent; }
            set
            {
                if (ValuePlayer == GobangPlayer.Player1)
                {
                    ValueOpponent = value;
                    //if (ValueMax >= 10) return;
                    //if (ValueOpponent == 5 || ValueOpponent == 6 || ValueOpponent == 8)
                    //{
                    //if (ValueOpponent > ValueLarge)
                    //{
                    //ValueLarge = ValueOpponent;
                    //}
                    //}
                }
                else
                {
                    ValueOpponent = value;
                    if (value == 12 || value == 13)
                    {
                        WeightMax += 5;
                    }
                    //if (ValueOpponent > 4)
                    //{
                    //if(ValueOpponent==7&&ValueMax==6)
                    //{
                    //ValueMax = 9;
                    //}
                    //if (ValueOpponent > ValueLarge)
                    //{
                    //ValueLarge = ValueOpponent + 1;
                    //}
                    //}
                }
            }
        }
        public int WeightMax { get; set; } = 0;
        public int WeightLarge { get; set; } = 0;
        public int WeightSmall { get; set; } = 0;
        public int WeightMin { get; set; } = 0;
        public int TotleWeight
        {
            get
            {
                int totle = 0;
                if (WeightMax > 4) totle += WeightMax;
                if (WeightLarge > 4) totle += WeightLarge;
                if (WeightSmall > 4) totle += WeightSmall;
                if (WeightMin > 4) totle += WeightMin;

                if (totle < 7)
                {
                    totle = WeightMax + WeightLarge + WeightSmall + WeightMin;
                }
                return totle;
            }
        }
        public int TotleScore
        {
            get
            {
                return WeightMax + WeightLarge + WeightSmall + WeightMin;
            }
        }

        public static bool operator >(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightMax > cw2.WeightMax) return true;
            if (cw1.WeightMax < cw2.WeightMax) return false;
            if (cw1.WeightLarge > cw2.WeightLarge) return true;
            if (cw1.WeightLarge < cw2.WeightLarge) return false;
            if (cw1.WeightSmall > cw2.WeightSmall) return true;
            if (cw1.WeightSmall < cw2.WeightSmall) return false;
            if (cw1.WeightMin > cw2.WeightMin) return true;
            return false;
        }
        public static bool operator <(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightMax < cw2.WeightMax) return true;
            if (cw1.WeightMax > cw2.WeightMax) return false;
            if (cw1.WeightLarge < cw2.WeightLarge) return true;
            if (cw1.WeightLarge > cw2.WeightLarge) return false;
            if (cw1.WeightSmall < cw2.WeightSmall) return true;
            if (cw1.WeightSmall > cw2.WeightSmall) return false;
            if (cw1.WeightMin < cw2.WeightMin) return true;
            return false;
        }
        public static bool operator >=(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightMax > cw2.WeightMax) return true;
            if (cw1.WeightMax < cw2.WeightMax) return false;
            if (cw1.WeightLarge > cw2.WeightLarge) return true;
            if (cw1.WeightLarge < cw2.WeightLarge) return false;
            if (cw1.WeightSmall > cw2.WeightSmall) return true;
            if (cw1.WeightSmall < cw2.WeightSmall) return false;
            if (cw1.WeightMin > cw2.WeightMin) return true;
            if (cw1.WeightMin < cw2.WeightMin) return false;
            return true;
        }
        public static bool operator <=(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightMax < cw2.WeightMax) return true;
            if (cw1.WeightMax > cw2.WeightMax) return false;
            if (cw1.WeightLarge < cw2.WeightLarge) return true;
            if (cw1.WeightLarge > cw2.WeightLarge) return false;
            if (cw1.WeightSmall < cw2.WeightSmall) return true;
            if (cw1.WeightSmall > cw2.WeightSmall) return false;
            if (cw1.WeightMin < cw2.WeightMin) return true;
            if (cw1.WeightMin > cw2.WeightMin) return false;
            return true;
        }
        public static bool operator ==(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightLarge == cw2.WeightLarge &&
                cw1.WeightMax == cw2.WeightMax &&
                cw1.WeightSmall == cw2.WeightSmall &&
                cw1.WeightMin == cw2.WeightMin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(ChessWeight cw1, ChessWeight cw2)
        {
            if (cw1.WeightLarge == cw2.WeightLarge &&
                cw1.WeightMax == cw2.WeightMax &&
                cw1.WeightSmall == cw2.WeightSmall &&
                cw1.WeightMin == cw2.WeightMin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
