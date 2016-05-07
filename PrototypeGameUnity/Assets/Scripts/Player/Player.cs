using Assets.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public int Id { get; private set; }

    public PlayerController Controller { get; private set; }
    private PlayerMark playerMark;

	public Money Money;
	public IList<ITradeable> Items;
	public IList<Message> Messages;

    public Field CurrentField { get; set; } 
	
    #region MonoBehavior methods
    //***********************************

    void Start()
    {
        playerMark = GetComponentInChildren<PlayerMark>();

        Controller = GetComponent<PlayerController>();

		Money = new Money(500);
		
		Items = new List<ITradeable>();

		Messages = new List<Message>();

		/*for(int i=0; i<10; i++) {
			
			Messages.Add (new Message("Title " + i, "Description " + i));
		}*/

        GameLogic.Instance.RegisterPlayer(this);
    }

    void OnDestroy()
    {
        GameLogic.Instance.RemovePlayer(this);
    }

    void Update()
    {
        playerMark.CheckActive();
    }

    #endregion

    #region Public methods
    //***********************************

    public void SetPlayerName(string name)
    {
        this.name = name;
    }

    public void SetId(int id)
    {
        Id = id;
        SetupPlayerMark();
    }

	public bool Has(Money money)
	{
		return Money.GetAmount() >= money.GetAmount();
	}

	public bool Has(ITradeable item)
	{
		if(item is Money) return Has (item as Money);

		return Items.Contains(item);
	}

    #endregion


    #region Private methods
    //***********************************

    private void SetupPlayerMark()
    {
        if (playerMark != null)
        {
            playerMark.SetPlayerText(Id);

            var render = GetComponent<Renderer>();
            playerMark.SetColor(render.material);
        }
    }

    #endregion
}

