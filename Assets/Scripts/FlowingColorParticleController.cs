using UnityEngine;

public class FlowingColorParticleController : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseFlowingColor()
    {
        if(GetComponent<ParticleSystem>()!=null)
        GetComponent<ParticleSystem>().Pause();
    }

    public void PlayFlowingColor()
    {
        if (GetComponent<ParticleSystem>() != null)
            GetComponent<ParticleSystem>().Play();
    }

    public  void StopFlowingColor()
    {
        if (GetComponent<ParticleSystem>() != null)
            GetComponent<ParticleSystem>().Stop();
    }
}
