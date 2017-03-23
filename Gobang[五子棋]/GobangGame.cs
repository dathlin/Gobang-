using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gobang_五子棋_
{
    /// <summary>
    /// three state of the pawn
    /// 棋子的三种状态
    /// </summary>
    public enum GobangPlayer
    {
        NonePlayer = 1,
        Player1,
        Player2
    }
    

    /// <summary>
    /// On board a pawn class, indicates whether there is a move later, such as chess order information
    /// 棋盘上一个棋子的类，指示是否有落子，下棋顺序等信息
    /// </summary>
    public class GobangPoint
    {
        /// <summary>
        /// indicates that this point is null or a player
        /// </summary>
        public GobangPlayer GobangPlayer { get; set; } = GobangPlayer.NonePlayer;
        /// <summary>
        /// the order number
        /// </summary>
        public int StepNumber { get; set; } = 0;
        /// <summary>
        /// indicates the weight score
        /// </summary>
        public int WeightScore { get; set; } = 0;

        public System.Drawing.Brush GetPawnBrush
        {
            get
            {
                switch (GobangPlayer)
                {
                    case GobangPlayer.Player1: return System.Drawing.Brushes.Orange;
                    case GobangPlayer.Player2: return System.Drawing.Brushes.DimGray;
                    default: return System.Drawing.Brushes.Transparent;
                }
            }
        }

        public bool IsEmpty
        {
            get { return GobangPlayer == GobangPlayer.NonePlayer; }
        }

        public static bool operator ==(GobangPoint gp1, GobangPoint gp2)
        {
            return gp1.GobangPlayer == gp2.GobangPlayer;
        }
        public static bool operator !=(GobangPoint gp1, GobangPoint gp2)
        {
            return gp1.GobangPlayer != gp2.GobangPlayer;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
