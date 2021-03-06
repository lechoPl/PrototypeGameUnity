﻿using Assets.Scripts.Game.Enums;
using Assets.Scripts.Game.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
        internal IList<Player> Players { get; set; }
        private IList<Field> Fields { get; set; }

		public Round CurrentRound;

        public BoardSize BoardSize { get; private set; }

        // Public methods
        //***********************************

        #region players managing

        public void PlayerToNextField(Player p, int FieldId)
        {
            p.Controller.ResetVelocity();
            p.CurrentField = Fields[FieldId];
            p.transform.position = Fields[FieldId].StartPointPosition;
        }

        public void RegisterPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
                player.SetPlayerName(string.Format("Player {0}", Players.Count));
                player.SetId(Players.Count);

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

        public void RemovePlayer(Player player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
            }
        }

		public Player GetPlayer(int id)
		{
			return Players[id];
		}

        #endregion

        #region fields managing

        public void RegisterField(Field field)
        {
            if (!Fields.Contains(field))
            {
                Fields.Add(field);
                field.SetFieldName(string.Format("Field {0}", Fields.Count));
                field.SetFieldId(Fields.Count);

                BoardSize.Update(field);
            }
            else
            {
                Debug.Log(string.Format("RegisterFiled: {0} is already registered", field.name));
            }

            if (Fields.Count == 1)
            {
                foreach (var player in Players)
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

        public IList<Player> GetPlayersOnField(int fieldNumber)
        {
            if (fieldNumber < 0 || fieldNumber >= Fields.Count)
            {
                return null;
            }

            var field = Fields[fieldNumber];

            var top = field.transform.position.y + field.Bounds.Height / 2f;
            var down = field.transform.position.y - field.Bounds.Height / 2f;
            var left = field.transform.position.x - field.Bounds.Width / 2f;
            var right = field.transform.position.x + field.Bounds.Width / 2f;

            return Players.Where(p =>
                                 top > p.transform.position.y &&
                                 down < p.transform.position.y &&
                                 left < p.transform.position.x &&
                                 right > p.transform.position.x).ToList();
        }

        #endregion


        #region monopoly part
		public void FinalizeTrade(Player playerA, ITradeable itemA, Player playerB, ITradeable itemB)
		{
			if((playerA == null || playerA.Has (itemA)) && (playerB == null || playerB.Has (itemB)))
		    {
				itemA.TakeFromPlayer(playerA, itemA.GetAmount());
				itemB.TakeFromPlayer (playerB, itemB.GetAmount());
				itemA.GiveToPlayer(playerB, itemA.GetAmount());
				itemB.GiveToPlayer(playerA, itemB.GetAmount());
			}
		}

		public void RequestTrade(Player playerA, ITradeable itemA, Player playerB, ITradeable itemB)
		{
			if(playerA == playerB)
			{
				return;
			}

			if(playerB == null)
			{
				Money money = itemA as Money;
				Field field = itemB as Field;
				if(money != null && field != null && money.GetAmount () >= field.Price)
				{
					FinalizeTrade(playerA, itemA, playerB, itemB);
				}
				return;
			}
			
			TradeMessage tradeMessage = new TradeMessage(playerA, itemA, playerB, itemB);

			if(playerA != null)
			{
				playerA.Messages.Add (tradeMessage);
				playerB.Messages.Add (tradeMessage);
			}
		}

        #endregion


        #region round managing

		public class Round
		{
			public GameState GameState { get; internal set; }
			
			private static int DiceTimeMultiplier = 3;

			private float roundStart = 0;
			private float roundPause = 0;
			private float roundTime = 0;
			
			public bool MovementBlocked { get; internal set; }
			
			internal int CurrentPlayer;

			public void StartRound(int DiceValue)
			{
				roundStart = Time.time;
				roundTime = DiceTimeMultiplier * DiceValue;
				MovementBlocked = false;
				SetGameState(GameState.Move);
			}
			
			internal void PauseRound()
			{
				GameState = GameState.Pause;
				roundPause = Time.time;
			}
			
			internal void ResumeRound() 
			{
				GameState = GameState.Move;
				roundStart = Time.time - (roundPause - roundStart);
			}
			
			public void EndRound()
			{
				CurrentPlayer = (CurrentPlayer + 1) % GameLogic.Instance.Players.Count;
			}
			
			public float GetRoundTime()
			{
				if(GameState == GameState.Pause) {
					return roundPause - roundStart;
				} else {
					return Time.time - roundStart;
				}
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

			public Player GetCurrentPlayer()
			{
				if (CurrentPlayer < 0 || CurrentPlayer >= GameLogic.Instance.Players.Count)
				{
					return null;
				}
				
				return GameLogic.Instance.Players[CurrentPlayer];
			}

			public int GetPlayersCount()
			{
				return GameLogic.Instance.Players.Count;
			}

			public void SetGameState(GameState state)
			{
				this.GameState = state;
			}
			
			// Private methods
			internal void BlockMovement()
			{
				MovementBlocked = true;
			}
		}
		#endregion
		
		
		private GameLogic()
        {
            Players = new List<Player>();
            Fields = new List<Field>();
            BoardSize = new BoardSize();
			CurrentRound = new Round();

			CurrentRound.GameState = GameState.Move;
        }

        #region singleton pattern 
        private static GameLogic _instance;

        public static GameLogic Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameLogic();
                }
                return _instance;
            }
        }
        #endregion
    }
}
