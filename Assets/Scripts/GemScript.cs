using UnityEngine;

public class GemScript : MonoBehaviour {


    public int TotalGems { get; set; }

    public static GemScript Instance;

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TotalGems"))
        {
            TotalGems = PlayerPrefs.GetInt("TotalGems");
        }
        else
        {
            TotalGems = 0;
            PlayerPrefs.SetInt("TotalGems", TotalGems);
            PlayerPrefs.Save();
        }
    }

    // Use this for initialization
    void Start () {
        


        //GemText.text = TotalGems.ToString();
        Debug.Log("CSU Total Gems = " + TotalGems);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitGame()
    {
        PlayerPrefs.SetInt("TotalGems", TotalGems);
        PlayerPrefs.Save();
    }

}
