using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndLevel : MonoBehaviour {
    public int numEndConditions;
    public int IntensityMax;
    public GameObject winText;
    public GameObject scoreText;
    public GameObject MenuButton;
    public GameObject NextLevelButton;
    public GameObject[] ColorChangeObjects;
    public float requiredFinishHold = 1;  //time in seconds needed to hold 100% to finish the level

    private bool finished;
    private int currIntensity;
    private float currPercentage;
    private float finishTimer;
	// Use this for initialization
	void Start () {
        currIntensity = 0;
        currPercentage = 0;
        finished = false;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(currPercentage);
        if (currPercentage == 100)
        {
            finishTimer += Time.deltaTime;

            if (finishTimer >= requiredFinishHold)
            {
                winText.SetActive(true);
                MenuButton.SetActive(true);
                NextLevelButton.SetActive(true);
                
                foreach (GameObject obj in ColorChangeObjects)
                {
                    obj.GetComponent<ChangeMaterial>().SetChange(true);
                }
            }
        }
        else
            finishTimer = 0;   
    }

    void LateUpdate()
    {
        scoreText.GetComponent<Text>().text = "Color: " + currPercentage + "%";
    }

    public void ChangeLightIntensity(int val)
    {
        currIntensity += val;
        currPercentage = ((float)currIntensity / (float)IntensityMax) * 100;
        //scoreText.GetComponent<Text>().text = "Color: " + currPercentage + "%";
    }

    public void ChangeLevel(string levelName)
    {
         SceneManager.LoadScene(levelName);
    }
}
