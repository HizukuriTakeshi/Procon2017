using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geister.GameSystem;
using System.Diagnostics;
using System.Threading;

namespace Geister.GameInformation
{
    public class DebugPlayer : AbstractPlayer
    {

        FieldObject[,] boardState;

        public DebugPlayer() : base()
        {
            SetInitialPlacement(new GhostAttribute[2, 4] { { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil },
                                                      { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good }}
                                                   );

        }

        public DebugPlayer(string name) : base()
        {
            this.name = name;
            SetInitialPlacement(new GhostAttribute[2, 4] {
                 { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good },
                 { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil }});
        }

        public override Move GetMove()
        {
            boardState = GetBoardState();
            List<Ghost> glist = GetMyGhostList().OrderBy(j => Guid.NewGuid()).ToList();
            List<GhostMove> gmlist = new List<GhostMove>();
            gmlist.Add(GhostMove.Down);
            gmlist.Add(GhostMove.Left);
            gmlist.Add(GhostMove.Right);
            gmlist.Add(GhostMove.Up);
            gmlist = gmlist.OrderBy(j => Guid.NewGuid()).ToList();

            Move m = null;

            for (int i = 0; i < GetMyGhostList().Count; i++)
            {
                Ghost g = glist[i];
                foreach (GhostMove gm in gmlist)
                {
                    if (IsMovable(g.P, gm))
                    {
                        m = new Move(g.P, gm);
                        break;
                    }
                }

            }
            return m;
        }

        private Boolean IsMovable(Position p, GhostMove gm)
        {

            if (Exists(p))
            {
                if (gm == GhostMove.Down)
                {
                    if (p.Row == 5)
                    {
                        return false;
                    }

                    if (boardState[p.Row + 1, p.Col] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                else if (gm == GhostMove.Left)
                {
                    if (p.Col == 0)
                    {
                        return false;
                    }

                    if (boardState[p.Row, p.Col - 1] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                else if (gm == GhostMove.Right)
                {
                    if (p.Col == 5)
                    {
                        return false;
                    }

                    if (boardState[p.Row, p.Col + 1] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                else if (gm == GhostMove.Up)
                {
                    if (p.Row == 0)
                    {
                        if (p.Col == 0 || p.Col == 5)
                        {
                            return true;
                        }
                        return false;
                    }

                    if (boardState[p.Row - 1, p.Col] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ises the exist.
        /// </summary>
        /// <returns><c>true</c>, if exist was ised, <c>false</c> otherwise.</returns>
        /// <param name="p">P.</param>
        private Boolean Exists(Position p)
        {
            foreach (Ghost g in GetMyGhostList())
            {
                if (g.P == p)
                {
                    return true;
                }
            }

            return false;
        }




    }
}
