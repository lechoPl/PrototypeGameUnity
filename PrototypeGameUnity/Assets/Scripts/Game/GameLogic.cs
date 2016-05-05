﻿using Assets.Scripts.Game.Enums;
using Assets.Scripts.Game.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameLogic
    {
        public bool DebugMode { get; set; }

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

            return Players.Where(p => field.Bounds.IsIn(p.transform.position)).ToList();
        }

        #endregion


        #region monopoly part
        public void BuyField(Player player, Field field)
        {
            if (player.Money >= field.Price)
            {
                if (field.Ovner != null)
                {
                    field.Ovner.Money += field.Price;
                }
                field.SetOwner(player);
                player.Money -= field.Price;
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
