using UnityEngine;


public class ColorBallTriggerCallback : MonoBehaviour
{


    public enum ColorEnum
    {
        Yellow,
        Blue,
        Green,
        Orange,
        Pink
    };
    public GameOverCheckScript gameOverController;
    public GameObject flowingColorParticleControllerRef;
    public FlowingColorParticleController flowingColorParticleController;
    public ColorEnum Color;
    // Use this for initialization
    void Start()
    {
        gameOverController = GameOverCheckScript.Instance;
        //flowingColorParticleController = Instantiate(flowingColorParticleControllerRef).GetComponent<FlowingColorParticleController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartColorFlow()
    {
        //if (flowingColorParticleController == null || gameOverController.FadeColorBallsOnShoot)
        //    return;
        //flowingColorParticleController.PlayFlowingColor();
        if (gameOverController.FadeColorBallsOnShoot)
            return;
        //SoundController.Instance.Splash.Play();
        //HealthMB.Instance.DecreaseHealth();
    }

    public void PauseColorFlow()
    {
        //if (flowingColorParticleController == null || gameOverController.FadeColorBallsOnShoot)
        //    return;
        if (gameOverController.FadeColorBallsOnShoot)
            return;
        gameOverController.DisplayPaintSplash(Color);
        SoundController.Instance.Splash.Play();
        HealthMB.Instance.DecreaseHealth();

        ////flowingColorParticleController.PauseFlowingColor();
        ////JarController jar = ColorUtilsMB.Instance.FindJar(BallColor);
        ////SyringeControllerScript syringeController = ColorUtilsMB.Instance.FindSyringe(BallColor);
        ////if (!jar.IsJarFull())
        ////    syringeController.AddToAbsorptionQueue(BallColor, flowingColorParticleController.gameObject);
        //HealthMB.Instance.DecreaseHealth();

    }

    public void DestroySplash()
    {
        Destroy(this.gameObject);
    }
}
