using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
        private IList<PlayerController> Players { get; set; }

        // Public methods
        //***********************************
        public int CurrentPlayer { get; private set; }

		private float RoundStart = -1;
		private float RoundTime = 15;

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

		public void StartRound() {
			RoundStart = Time.time;
		}

        public void EndRound()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
        }

		public float GetRoundTime() {
			return Time.time - RoundStart;
		}

		public float GetTimeLeft() {
			return RoundTime - GetRoundTime();
		}

		public void ChechRoundFinished() {
			if (Time.time - RoundStart >= RoundTime) {
				EndRound ();
			}
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
    