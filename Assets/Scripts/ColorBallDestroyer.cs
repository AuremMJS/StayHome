using UnityEngine;

public class ColorBallDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("ColorBall"))
        {
            Destroy(collision.collider.gameObject);
        }
    }
}
