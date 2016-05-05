using Assets.Scripts.Game.Enums;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Round
    {
        public GameState GameState { get; set; }

        private static int DiceTimeMultiplier = 5;

        private float roundStart = 0;
        private float roundTime = 0;

        public bool MovementBlocked { get; internal set; }

        public int CurrentPlayer { get; internal set; }

        public void StartRound(int DiceValue)
        {
            roundStart = Time.time;
            roundTime = DiceTimeMultiplier * DiceValue;
            MovementBlocked = false;
            GameState = GameState.Move;
        }

        public void EndRound()
        {
            CurrentPlayer = (CurrentPlayer + 1) % GameLogic.Instance.Players.Count;
        }

        public float GetRoundTime()
        {
            return Time.time - roundStart;
        }

        public float GetTimeLeft()
        {
            return roundTime - GetRoundTime();
        }

        public bool CheckRoundFinished()
        {
            if (GetTimeLeft() <= 0)
            {
                GameState = GameState.Menu;
                BlockMovement();
                return true;
            }
            return false;
        }

        public Player GetCurrentPlayer()
        {
            if (CurrentPlayer < 0 || CurrentPlayer >= GameLogic.Instance.Players.Count)
            {
                return null;
            }

            return GameLogic.Instance.Players[CurrentPlayer];
        }

        // Private methods
        internal void BlockMovement()
        {
            MovementBlocked = true;
        }
    }
}
