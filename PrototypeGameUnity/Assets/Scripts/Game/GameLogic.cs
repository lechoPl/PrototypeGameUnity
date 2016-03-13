using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
		private static int DiceTimeMultiplier = 5;
        private IList<PlayerController> Players { get; set; }

        // Public methods
        //***********************************
        public int CurrentPlayer { get; private set; }

		private float roundStart = 0;
		private float roundTime = 0;

		public bool MovementBlocked { get; private set; }

        public void RegisterPlayer(PlayerController player)
        {
            if (!Players.Contains(player))
            {
                player.SetPlayerName(string.Format("Player {0}", Players.Count));
                Players.Add(player);
            }
            else
            {
                Debug.Log(string.Format("RegisterPlayer: {0} is already registered", player.name));
            }
        }

        public PlayerController GetCurrentPlayer()
        {
            return Players[CurrentPlayer];
        }

		public void StartRound(int DiceValue) {
			roundStart = Time.time;
			roundTime = DiceTimeMultiplier * DiceValue;
			MovementBlocked = false;
		}

        public void EndRound()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
        }

		public float GetRoundTime() {
			return Time.time - roundStart;
		}

		public float GetTimeLeft() {
			return roundTime - GetRoundTime();
		}

		public void ChechRoundFinished() {
			if (GetTimeLeft() <= 0) {
				BlockMovement();
			}
		}

		private void BlockMovement() {
			MovementBlocked = true;
		}

        // Private methods
        //***********************************
        private GameLogic()
        {
            Players = new List<PlayerController>();
        }


        #region singleton pattern 
        private static GameLogic _instance;

        public static GameLogic Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameLogic();
                }
                return _instance;
            }
        }
        #endregion
    }
}
    