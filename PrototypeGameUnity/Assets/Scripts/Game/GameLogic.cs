﻿using Assets.Scripts.Game.Enums;
using Assets.Scripts.Game.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
		private static int DiceTimeMultiplier = 5;
        public GameState CurrentGameState { get; private set; }
        private IList<PlayerController> Players { get; set; }
        private IList<Field> Fields { get; set; }

        private float roundStart = 0;
        private float roundTime = 0;

        public int CurrentPlayer { get; private set; }

        public BoardSize BoardSize { get; private set; }

        public bool MovementBlocked { get; private set; }

        // Public methods
        //***********************************
		#region players managing
        public void PlayerToNextField(PlayerController p, int FieldId)
        {
            p.ResetVelocity();
			p.Player.CurrentField = Fields[FieldId];
			p.transform.position = Fields[FieldId].StartPointPosition;
        }

        public void RegisterPlayer(PlayerController player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);

                if (Fields.Count > 0)
                {
                    PlayerToNextField(player, 0);
                }
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

		public PlayerController GetCurrentPlayer()
		{
			if(CurrentPlayer < 0 || CurrentPlayer >= Players.Count)
			{
				return null;
			}
			
			return Players[CurrentPlayer];
		}
		
		public int GetNewPlayerId()
		{
			return Players.Count;
		}
		#endregion

		#region fields managing
        public void RegisterField(Field field)
        {
            if (!Fields.Contains(field))
            {
                Fields.Add(field);
                BoardSize.Update(field);
            }
            else
            {
                Debug.Log(string.Format("RegisterFiled: {0} is already registered", field.name));
            }

            if(Fields.Count == 1)
            {
                foreach(var player in Players)
                {
                    PlayerToNextField(player, 0);
                }
            }
        }

        public void RemoveField(Field field)
        {
            if (Fields.Contains(field))
            {
                Fields.Remove(field);
                //TODO: update board size
            }
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
		
		public int GetNewFieldId()
		{
			return Fields.Count;
		}

		#region monopoly part
		public void BuyField(Player player, Field field) {
			if(player.Money >= field._price) {
				if(field._owner != null) {
					field._owner.Money += field._price;
				}
				field.SetOwner(player);
				player.Money -= field._price;
			}
		}
		#endregion

		#endregion

		#region round managing
		public void StartRound(int DiceValue) {
			roundStart = Time.time;
			roundTime = DiceTimeMultiplier * DiceValue;
			MovementBlocked = false;
			SetGameState (GameState.Move);
		}

        public void EndRound()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
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
				SetGameState(GameState.Menu);
				BlockMovement();
				return true;
			}
			return false;
		}
		#endregion

		#region game state managing
        public void SetGameState(GameState state)
        {
            CurrentGameState = state;
        }

        // Private methods
        //***********************************
        private void BlockMovement()
        {
			MovementBlocked = true;
		}
		#endregion
	
        
        private GameLogic()
        {
            Players = new List<PlayerController>();
            Fields = new List<Field>();
            BoardSize = new BoardSize();

            CurrentGameState = GameState.Move;
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
    