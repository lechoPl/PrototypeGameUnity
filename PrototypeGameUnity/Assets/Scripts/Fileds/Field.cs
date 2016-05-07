using Assets.Scripts.Common;
using Assets.Scripts.Game;
using UnityEngine;

[RequireComponent(typeof(FieldBounds))]
public class Field : MonoBehaviour, ITradeable
{
    public int Id { get; private set; }
    public Player Owner { get; private set; }
    public int Price = 100;
    public TextMesh Text;
	public Sprite FieldCard;

    private FieldBounds _bounds;
    public FieldBounds Bounds
    {
        get
        {
            if (_bounds == null)
            {
                _bounds = GetComponent<FieldBounds>();
            }

            return _bounds;
        }
    }
    private GameObject _startPoint;
    private EndPoint _endPoint;

    public Vector3 StartPointPosition
    {
        get
        {
            if (_startPoint == null)
            {
                return Vector3.zero;
            }
            else
            {
                return _startPoint.transform.position;
            }
        }
    }



    #region MonoBehaviour methods
    //***********************************

    void Awake()
    {

        Setup();
    }

    void OnDestroy()
    {
        GameLogic.Instance.RemoveField(this);
    }

    #endregion

    #region Public methods
    //***********************************
    public void SetOwner(Player player)
    {
        Owner = player;
		if(Text != null)
		{
			if(Owner != null)
			{
				Text.text = Owner.name + " owns this field.";
			}
			else
			{
				Text.text = "Press ENTER to buy this field.";
			}
		}

    }

    public void SetFieldName(string name)
    {
        this.name = name;
    }

    public void SetFieldId(int id)
    {
        Id = id;
    }

    public void SetEndPointNextFieldId(int nexFieldId)
    {
        if (_endPoint == null)
        {
            return;
        }

        _endPoint.NextFieldID = nexFieldId;
    }

    public void Setup()
    {
        SetupStartPoint();
        SetupEndPoint();
    }

    #endregion


    #region Private methods
    //***********************************
    private void SetupStartPoint()
    {
        _startPoint = Utilities.FindChildWithTag(this.gameObject, "StartPoint");
    }

    public void SetupEndPoint()
    {
        _endPoint = Utilities.FindComponentInChildWithTag<EndPoint>(this.gameObject, "EndPoint");
    }

    #endregion

	#region ITradeable methods
	public int GetAmount()
	{
		return 1;
	}

	public Sprite GetSprite()
	{
		return FieldCard;
	}

	public void GiveToPlayer(Player player, int amount)
	{
		if(Owner != null)
		{
			TakeFromPlayer(Owner, amount);
		}
		player.Items.Add (this);
		SetOwner(player);
	}
	
	public void TakeFromPlayer(Player player, int amount)
	{
		if(Owner != null)
		{
			Owner.Items.Remove(this);
			SetOwner(null);
		}
	}
	#endregion	
}
