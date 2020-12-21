using System.Collections;
using TMPro;
using UnityEngine;

public class ShieldStoreObject : StoreObject {

    public static ShieldStoreObject Instance;

    public GameObject ShieldBar;

    public int ShieldHealth { get; private set; }

    public GameObject FluidScaler;
    public TextMeshPro ShieldHealthText;
    private float timer;

    // Use this for initialization
    protected override void Start () {
        Instance = this;
        base.Start();
        ShieldHealth = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        base.ApplyStoreObjectToGamePlay(storeObjectsController);
        ShieldBar.SetActive(true);
        HealthMB.Instance.isShieldActive = true;
        HealthMB.Instance.ReviveHealth();
        ShieldHealth = 100;
        FluidScaler.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void RemoveStoreObjectFromGamePlay()
    {
        base.RemoveStoreObjectFromGamePlay();
        HealthMB.Instance.isShieldActive = false;
        ShieldBar.SetActive(false);
    }


    public void DecreaseHealth()
    {
        ShieldHealth -= 34;
        if (ShieldHealth > 0)
        {
            ScaleFluidToPercentage(ShieldHealth);
            ShieldHealthText.text = ShieldHealth.ToString() + "%";
        }
        else
        {
            RemoveStoreObjectFromGamePlay();
        }
    }
    public void ScaleFluidToPercentage(float percentage)
    {
        float YScaleFactor = percentage / 100.0f;
        Vector3 targetScale = new Vector3(1, YScaleFactor, 1);
        StartCoroutine(ScaleFluidCoroutine(targetScale));
    }

    private IEnumerator ScaleFluidCoroutine(Vector3 targetScale)
    {
        Vector3 CurrentScale = FluidScaler.transform.localScale;
        timer = 0.0f;
        while (timer <= 5.0f && CurrentScale.y != targetScale.y)
        {
            timer += UnityEngine.Time.deltaTime;
            if (targetScale.y > 0.9f)
                targetScale.y = 0.9f;
            Vector3 scaleVector = Vector3.Lerp(FluidScaler.transform.localScale, targetScale, timer / 0.5f);
            this.FluidScaler.GetComponent<Transform>().localScale = new Vector3(1, scaleVector.y, 1);
            yield return new WaitForEndOfFrame();
            CurrentScale = FluidScaler.transform.localScale;

        }
        yield break;
    }
}
