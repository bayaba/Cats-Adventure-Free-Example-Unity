using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using DG.Tweening;

using GooglePlayGames;
using GooglePlayGames.OurUtils;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;



public class CatMove : MonoBehaviour
{
    Animator anim;

    float speed = 0.3f, height = 0.5f;

    public GameObject Sea;
    public GameObject WaterEffect;
    public BlockManager Manager;

    public GameObject OverPanel, JoypadPanel, TitlePanel;
    public AudioClip CatDie;

    bool isDead = false;

    ArrayList KeyArray = new ArrayList();
    float StartTime = 0f, GrayBlockTime = 0f;
    bool GrayBlock = false;


    void Awake()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("73464");
        }
        else
        {
            Debug.Log("Platform not supported");
        }
    }

    void ShowAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(null, new ShowOptions
            {
                resultCallback = result =>
                {
                    Debug.Log(result.ToString());
                }
            });
        }
        else
        {
            Debug.Log("Advertisement is not ready");
        }
    }

    void ScoreLeaderBoard()
    {
        if (Global.Authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI7cCOyswOEAIQCw");
        }
    }

    void BlockLeaderBoard()
    {
        if (Global.Authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI7cCOyswOEAIQCg");
        }
    }

    void TimeLeaderBoard()
    {
        if (Global.Authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI7cCOyswOEAIQDA");
        }
    }

    void Achivement()
    {
        if (Global.Authenticated)
        {
            Global.ShowAchievementsUI();
        }
    }

    void Start()
	{
        Global.KillCount = PlayerPrefs.GetInt("KillCount");

        if (Time.time - Global.AdsTime >= 300f)
        {
            Global.AdsTime = Time.time;
            ShowAds();
        }
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        Manager.SendMessage("GetScore");
        anim = GetComponentInChildren<Animator>();
	}

    void GameOver()
    {
        Global.KillCount++;
        PlayerPrefs.SetInt("KillCount", Global.KillCount);

        Global.SaveScore(long.Parse(Manager.TotalScore.text), (long)Global.BlockCount, (long)((Time.time - StartTime) * 1000f));
        Global.BlockCount = 0;

        OverPanel.SetActive(true);
    }

    void StartGame()
    {
        if (Global.Authenticated || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Manager.SendMessage("StartGame");
            TitlePanel.SetActive(false);
            JoypadPanel.SetActive(true);
            StartTime = Time.time;
        }
    }

    void Update()
	{
        if (TitlePanel.activeSelf)
        {
            if (!Global.Authenticated && Application.platform == RuntimePlatform.WindowsEditor)
            {
                Global.Authenticate();
            }
        }
        else
        {
            Sea.transform.position = new Vector3(transform.position.x, Sea.transform.position.y, transform.position.z);

            if (transform.position.y < 0f)
            {
                if (!isDead)
                {
                    GameOver();
                    audio.clip = CatDie;
                    audio.Play();
                    LeanTween.rotateAroundLocal(gameObject, Vector3.left, 90f, 0.5f);
                    WaterEffect.transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
                    WaterEffect.SetActive(true);
                    Invoke("Restart", 3.0f);
                    isDead = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) Front();
            if (Input.GetKeyDown(KeyCode.DownArrow)) Back();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Left();
            if (Input.GetKeyDown(KeyCode.RightArrow)) Right();

            if (!isDead && Manager.CatLandedBlock != null)
            {
                if (KeyArray.Count > 0)
                {
                    KeyCode key = (KeyCode)KeyArray[0];

                    if (key == KeyCode.UpArrow) FrontMove();
                    if (key == KeyCode.DownArrow) BackMove();
                    if (key == KeyCode.LeftArrow) LeftMove();
                    if (key == KeyCode.RightArrow) RightMove();

                    KeyArray.RemoveAt(0);
                }
            }
        }
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    void Front()
    {
        KeyArray.Add(KeyCode.UpArrow);
    }

    void Back()
    {
        KeyArray.Add(KeyCode.DownArrow);
    }

    void Left()
    {
        KeyArray.Add(KeyCode.LeftArrow);
    }

    void Right()
    {
        KeyArray.Add(KeyCode.RightArrow);
    }

    void FrontMove()
    {
        audio.Play();
        anim.Play("ready");

        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
        transform.DOJump(pos, height, 1, speed);

        Manager.LeaveLandedBlock();
        anim.SetTrigger("jump");
    }

    void BackMove()
    {
        audio.Play();
        anim.Play("ready");

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
        transform.DOJump(pos, height, 1, speed);

        Manager.LeaveLandedBlock();
        anim.SetTrigger("jump");
    }

    void LeftMove()
    {
        audio.Play();
        anim.Play("ready");

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        Vector3 pos = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        transform.DOJump(pos, height, 1, speed);

        Manager.LeaveLandedBlock();
        anim.SetTrigger("jump");
    }

    void RightMove()
    {
        audio.Play();
        anim.Play("ready");

        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        Vector3 pos = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        transform.DOJump(pos, height, 1, speed);

        Manager.LeaveLandedBlock();
        anim.SetTrigger("jump");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "block")
        {
            if (col.gameObject.GetComponent<Block>().FallDelay < 0f)
            {
                GrayBlock = true;
                GrayBlockTime = Time.time;
            }
            else
            {
                if (GrayBlock)
                {
                    StartTime += Time.time - GrayBlockTime;
                    GrayBlock = false;
                }
            }
            Manager.LandingTimer = Time.time;
            Manager.CatLandedBlock = col.gameObject;

            Global.SendAchievement(++Global.BlockCount);
        }
    }

    void Restart()
    {
        Application.LoadLevel(0);
    }
}
