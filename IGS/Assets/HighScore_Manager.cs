using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore_Manager : MonoBehaviour {

    public GameObject DeathScreen;
    public GameObject Highscore_screen;

    private Text deathScore;

    private InputField group_name;
    private Text highScore_groupName;
    private GameObject highScore_groupNameObj;
    private Text highScore_score;
    private GameObject highScore_scoreObj;

    private string cur_group;
    private int score;
    private bool on_deathScreen = false;
    private bool on_highScoreScreen = false;

    private int[] HighScoreRecords = new int[15];

    private int[] distance = { 10, 50, 90, 130, 170, 210, 250, 290, 330, 370};

    bool setObjs = false;

    void Start()
    {
        DeathScreen.SetActive(false);
        Highscore_screen.SetActive(false);

        group_name = DeathScreen.GetComponentInChildren<InputField>();

        foreach(Transform child in Highscore_screen.transform)
        {
            if(child.name == "GroupName_text")
            {
                highScore_groupName = child.GetComponent<Text>();
                highScore_groupNameObj = child.gameObject;
            }
            else if(child.name == "Score_text")
            {
                highScore_score = child.GetComponent<Text>();
                highScore_scoreObj = child.gameObject;
            }
        }

        foreach(Transform child in DeathScreen.transform)
        {
            if(child.name == "Score_Text")
            {
                deathScore = child.GetComponent<Text>();
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            Death(1);

        if (on_highScoreScreen)
        {
            if (!setObjs)
            {
                for (int i = 0; i <= 9; i++)
                {

                    GameObject temp = Instantiate(highScore_groupNameObj, new Vector3(highScore_groupNameObj.transform.position.x,
                        highScore_groupNameObj.transform.position.y - distance[i]
                        , highScore_groupNameObj.transform.position.z), Quaternion.identity);

                    temp.transform.SetParent(Highscore_screen.transform);
                    temp.GetComponent<Text>().text = PlayerPrefs.GetString("Group Name", i.ToString());

                    GameObject temp2 = Instantiate(highScore_scoreObj, new Vector3(highScore_scoreObj.transform.position.x,
                         highScore_scoreObj.transform.position.y - distance[i]
                        , highScore_scoreObj.transform.position.z), Quaternion.identity);

                    temp2.transform.SetParent(Highscore_screen.transform);
                    temp2.GetComponent<Text>().text = PlayerPrefs.GetString("Player_Score - " + cur_group, i.ToString());

                    if (i == 9)
                    {
                        Time.timeScale = 1;
                        setObjs = true;
                    }
                }

                highScore_groupNameObj.SetActive(false);
                highScore_scoreObj.SetActive(false);
            }
            highScore_groupName.text = cur_group;
            highScore_score.text = score.ToString();
            StartCoroutine("closeScore", 3);
        }
    }

    IEnumerator closeScore(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Highscore_screen.SetActive(false);
    }

    public void Death(int highscore)
    {
        Time.timeScale = 0;
        DeathScreen.SetActive(true);
        on_deathScreen = true;
        deathScore.text = "Score: " + highscore.ToString();

        score = highscore;
        PlayerPrefs.SetInt("Player_Score - " + cur_group, score);
    }

    public void onSubmit()
    {
        if (on_deathScreen)
        {
            cur_group = group_name.text;
            PlayerPrefs.SetString("Group Name" , cur_group);

            Debug.Log(cur_group);
            on_deathScreen = false;
            DeathScreen.SetActive(false);
            Highscore_screen.SetActive(true);
            on_highScoreScreen = true;
        }
    }

}
