using UnityEngine;
using System.Collections;

public class Money : ITradeable
{
	private int Amount;

	public Money(int amount)
	{
		Amount = amount;
	}

	public void Merge(Money money)
	{
		Amount += money.Amount;
		money.Amount = 0;
	}

	public int GetAmount()
	{
		return Amount;
	}
	
	public Sprite GetSprite()
	{
		return null;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;

		Money m = obj as Money ;
		if ((System.Object)m == null)
			return false;

		return m.Amount == Amount;
	}

	public void GiveToPlayer(Player player, int amount)
	{
		if(player != null && amount <= Amount) {
			Amount -= amount;
			player.Money.Amount += amount;
		}
	}

	public void TakeFromPlayer(Player player, int amount)
	{
		if(player != null && player.Money.Amount >= amount)
		{
			player.Money.Amount -= amount;
		}
	}
}

