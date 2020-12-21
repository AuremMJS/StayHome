using System.Collections;
using TMPro;
using UnityEngine;

public class HealthMB : MonoBehaviour
{
    public static HealthMB Instance;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value <= 0)
                GameOverCheckScript.Instance.EndGame();
            Color c = Color.Lerp(Color.red, Color.green, value / 100.0f);
            Fluid.material.color = c;
            HealthText.text = value.ToString() + "%";
            health = value;
        }
    }
    public GameObject FluidScaler;
    public TextMeshPro HealthText;
    public MeshRenderer Fluid;
    public bool isShieldActive;
    private float timer;
    // Use this for initialization
    void Start()
    {
        Health = 100;
        Instance = this;
        //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //var x = -pos.x;// - 0.5f;
        //transform.position = new Vector3(x, transform.position.y, transform.position.z);
        this.Fluid.material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReviveHealth()
    {
        Health = 100;
        this.FluidScaler.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        this.Fluid.material.color = Color.green;
        //ScaleFluidToPercentage(100);
    }

    public void DecreaseHealth()
    {
        if (isShieldActive)
        {
            ShieldStoreObject.Instance.DecreaseHealth();
            return;
        }
        if (Health <= 0)
        {
            return;
        }
        Health -= 10;
        if (Health >= 0)
            ScaleFluidToPercentage(Health);
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
            timer += Time.deltaTime;
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
