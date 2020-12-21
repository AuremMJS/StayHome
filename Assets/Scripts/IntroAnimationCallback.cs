using UnityEngine;

public class IntroAnimationCallback : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayBeep1()
    {
        SoundController.Instance.IntroBeep1.Play();
    }

    public void PlayBeep2()
    {
        SoundController.Instance.IntroBeep2.Play();
    }
}
