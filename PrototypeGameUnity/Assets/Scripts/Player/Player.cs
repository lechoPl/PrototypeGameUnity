using Assets.Scripts.Game;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public int Id { get; private set; }

    public PlayerController Controller { get; private set; }
    private PlayerMark playerMark;

    public int Money = 500;
    public Field CurrentField { get; set; }


    #region MonoBehavior methods
    //***********************************

    void Start()
    {
        playerMark = GetComponentInChildren<PlayerMark>();

        Controller = GetComponent<PlayerController>();

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

	public void ChangeMoney(int amount)
	{
		Money += amount;
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

