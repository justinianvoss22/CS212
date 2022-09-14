using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mankalah
{
    // rename me
    public class JustinVossPlayer : Player // class must be public
    {
        public int timeLimit;
        Position position;

        // first set of variables is if the player is first
        int first_position_p1 = 0;
        int last_position_p1 = 0;


        // second set of variables is if the player is second
        int first_position_p2 = 0;
        int last_position_p2 = 0;


        public JustinVossPlayer(Position pos, int maxTimePerMove) // constructor must match this signature
            : base(pos, "JustinVossPlayer", maxTimePerMove) // choose a string other than "MyPlayer"
        {
            timeLimit = maxTimePerMove;
            position = pos;
            // if our person's position is bottom, then the first set of variables are assigned to bottom, while the player 2's are assigned to top.
            if (position == Position.Bottom)
            {
                first_position_p1 = 0;
                last_position_p1 = 5;

                first_position_p2 = 7;
                last_position_p2 = 12;
            }
            // otherwise, p2 has bottom and p1 has top
            else
            {
                first_position_p1 = 7;
                last_position_p1 = 12;

                first_position_p2 = 0;
                last_position_p2 = 5;
            }



        }

        public override int chooseMove(Board b) 
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            int count = 3;
            Result move = new Result(first_position_p1, -1);

            while(timer.ElapsedMilliseconds < getTimePerMove())
            {
                move = minimax(b, count, int.MinValue, int.MaxValue);
                
                // each time, the depth goes farther
                count++;
            }
            //returns the move
            return move.move;

        }
        public Result minimax(Board b, int d, int alpha, int beta)
        {
            int bestMove = first_position_p1;
            int bestValue = 0;
            //Result result;
           // Position opponent;

            if (b.gameOver() || d == 0)
            {
                return new Result(0, evaluate(b));
            }
            if(b.whoseMove() == Position.Bottom)
            {
                bestValue = int.MaxValue;
                // this for loop contains informatino about alpha and beta as well.
                // As long as alpha is smaller than beta, then this loop excecutes. 
                // This makes the parts where alpha is bigger than beta cut off in the tree.
                // Because we are trying to only have moves that maximize our turn the best and minimize the opponents the best, pruning them off will help.
                for (int move = 0; move <= 5 && alpha < beta; move++)
                {
                    if (b.legalMove(move))
                    {
                        Board newBoard = new Board(b);
                        newBoard.makeMove(move, false);
                        Result value = minimax(newBoard, d - 1, alpha, beta);

                        if (value.score < bestValue) // remembers if its the best value and move
                        {
                            bestMove = move;
                            bestValue = value.score;
                        }
                        // beta assignent, assigns the beta value if its the best (worst) value.
                        if (bestValue < beta)
                        {
                            beta = bestValue;
                        }
                    }
                }
                return new Result(bestMove, bestValue);
            }

            else // if it is in Top position
            {
                bestValue = int.MinValue;
                // same pruning as before
                for(int move = 7; move <= 12 && alpha < beta; move++)
                {
                    if (b.legalMove(move))
                    {
                        Board newBoard = new Board(b);
                        newBoard.makeMove(move, false);
                        Result value = minimax(newBoard, d - 1, alpha, beta);
                        
                        if (value.score > bestValue) // remembers if its the best value and move
                        {
                            bestValue = value.score;
                            bestMove = move;
                        }
                        // assigns the value to alpha if it is better. This gives alpha the best value it can be. 
                        if (bestValue > alpha)
                        {
                            alpha = bestValue;
                        }
                    }
                }
            }
            return new Result(bestMove, bestValue);
        }




        public override int evaluate(Board b)
        {
            int score =  b.stonesAt(13) - b.stonesAt(6);
            int total_stones = 0;
            int go_again = 0;
            int capture = 0;
            int last_hole = 0;

            if (b.whoseMove() == Position.Top)
            {
                // looking at go agains
                for (int i = 7; i < 13; i++)
                {
                    // adds up each stone each time the for loop goes, to see the total stones on your side.k
                    total_stones += b.stonesAt(i);

                    // if the amount of stones gets you to the last hole
                    if (b.stonesAt(i) == (13 - i))
                    {
                        go_again++;
                    }
                    // sets the last hole at a certain location as the current location plus the amount of stones
                    last_hole = (i + b.stonesAt(i)) % 12 ;
                    int corresponding_hole = 12 - i;

                    // if there is a stone on the other side of the board, then it adds it to the capture value.
                    if (b.stonesAt(last_hole) == 0 && b.stonesAt(corresponding_hole) > 0 && last_hole < 13)
                    {
                        capture++;
                    } 
                }
            }
            // now we have to look at the other side of the board, and subtract the values that the other person has on the board
            else
            {
                // looking at go agains
                for (int i = 0; i < 6; i++)
                {
                    // subracts the total stones on the other side of the board
                    total_stones -= b.stonesAt(i);

                    // if the amount of stones gets you to the last hole
                    if (b.stonesAt(i) == (13 - i))
                    {
                        // subtracts this time, because its the other person
                        go_again--;
                    }
                    // sets the last hole at a certain location as the current location plus the amount of stones
                    last_hole = (i + b.stonesAt(i)) % 12;

                    int corresponding_hole = 12 - i;

                    // if there is a stone on the other side of the board, then it adds it to the capture value.
                    if (b.stonesAt(last_hole) == 0 && b.stonesAt(corresponding_hole) > 0 && last_hole < 13)
                    {
                        capture--;
                    }
                    
                }
            }
            int final_score = score + go_again + capture + total_stones;

            return final_score;

        }


        public override string gloat()
        {
            return "HeeHeeHeeHa!! HeeHeeHeeHa!!" ;
        }


        public override String getImage() { return "heheheha.jpg"; }
    }



    public class Result
    {
        public int move;
        public int score;
        public Result(int setMove, int setScore)
        {
            move = setMove;
            score = setScore;
        }

        public Result(Result r)
        {
            move = r.move;
            score = r.score;
        }
        
    }


   


}