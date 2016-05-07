using System;
using UnityEngine;

public interface ITradeable
{
	int GetAmount();
	Sprite GetSprite();
	void GiveToPlayer(Player player, int amount);
	void TakeFromPlayer(Player player, int amount);
}

