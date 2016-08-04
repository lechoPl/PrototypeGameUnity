using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    public float BulletMaxDistance = 25;
    public float BulletSpeed = 75;
    public ParticleSystem ExplosionParticleSystem;

    private float BulletTraveledDistance = 0;

    private Vector3 LastBulletPosition;

	// Use this for initialization
	void Start () {
        LastBulletPosition = transform.position;
        ExplosionParticleSystem.Emit(100);

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        GetComponent<Rigidbody>().velocity = BulletSpeed * transform.up;
    }
	
	// Update is called once per frame
	void Update () {
        BulletTraveledDistance += Vector3.Distance(transform.position, LastBulletPosition);
        LastBulletPosition = transform.position;
        if(BulletTraveledDistance >= BulletMaxDistance)
        {
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision col)
    {
        print("Collision");
        Destroy(this.gameObject);
    }
}
