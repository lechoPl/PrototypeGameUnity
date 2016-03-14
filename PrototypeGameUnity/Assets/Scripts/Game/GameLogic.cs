using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
		private static int DiceTimeMultiplier = 5;
        private IList<PlayerController> Players { get; set; }
        private IList<Field> Fields { get; set; }

        // Public methods
        //***********************************
        public void PlayerToNextField(PlayerController p, int FieldId)
        {
            p.transform.position = Fields[FieldId].SartPointPosition;
        }

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

        public void RemovePlayer(PlayerController player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
            }
        }

        public void RegisterFiled(Field field)
        {
            if (!Fields.Contains(field))
            {
                field.SetFieldName(string.Format("Field {0}", Fields.Count));
                Fields.Add(field);
            }
            else
            {
                Debug.Log(string.Format("RegisterFiled: {0} is already registered", field.name));
            }
        }

        public void RemoveField(Field field)
        {
            if (Fields.Contains(field))
            {
                Fields.Remove(field);
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

        public IList<PlayerController> GetPlayersOnField(int fieldNumber)
        {
            if(fieldNumber < 0 || fieldNumber >= Fields.Count)
            {
                return null;
            }

            var field = Fields[fieldNumber];

            var top = field.transform.position.y + field.Height / 2f;
            var down = field.transform.position.y - field.Height / 2f;
            var left = field.transform.position.x - field.Width / 2f;
            var right = field.transform.position.x + field.Width / 2f;

            return Players.Where(p => 
                top > p.transform.position.y &&
                down < p.transform.position.y &&
                left < p.transform.position.x &&
                right > p.transform.position.x).ToList();
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
            Fields = new List<Field>();
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
    