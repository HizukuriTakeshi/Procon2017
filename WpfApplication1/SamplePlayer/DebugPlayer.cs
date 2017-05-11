﻿using System;
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

        FieldObject[,]  boardState;

        public DebugPlayer() : base()
        {
            SetInitialPlacement(new GhostAttribute[2, 4] { { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil },
                                                      { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good }}
                                                   );

        }

        public DebugPlayer(string name) : base()
        {
            this.name = name;
            SetInitialPlacement(new GhostAttribute[2, 4] { { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil },
                                                      { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good }}
                                                              );
        }

        public override Move GetMove()
        {
            boardState = GetBoardState();

            List<Ghost> glist = GetMyGhostList().OrderBy(j => Guid.NewGuid()).ToList();
            List<GhostMove> gmlist = new List<GhostMove>();
            gmlist.Add(GhostMove.Down);
            //gmlist.Add(GhostMove.Left);
            //gmlist.Add(GhostMove.Right);
            //gmlist.Add(GhostMove.Up);
            //gmlist.Add(GhostMove.Down);
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
                    if (p.X == 5)
                    {
                        return false;
                    }

                    if (boardState[p.X + 1, p.Y] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                else if (gm == GhostMove.Left)
                {
                    if (p.Y == 0)
                    {
                        return false;
                    }

                    if (boardState[p.X, p.Y-1] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
				else if (gm == GhostMove.Right)
				{
					if (p.Y == 5)
					{
						return false;
					}

					if (boardState[p.X, p.Y + 1] == GetMyPlayerID())
					{
						return false;
					}
				}
				else if (gm == GhostMove.Up)
				{
					if (p.X == 0)
					{
                        if(p.Y == 0 || p.Y == 5)
                        {
                            return true;
                        }
                        return false;
					}

					if (boardState[p.X-1, p.Y ] == GetMyPlayerID())
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
            foreach(Ghost g in GetMyGhostList())
            {
                if(g.P == p){
                    return true;
                }
            }

            return false;    
        }




    }
}
