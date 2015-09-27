using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public int MaxBlock = 10; // block count

    public GameObject[] Blocks; // Blocks[0] is normal block's prefab, Blocks[1] is stone block's prefab
    public GameObject LastBlock; // last block's pointer
    public GameObject Score; // Score effect prefab

    public Text TotalScore, HighScore;
    public GameObject CatLandedBlock = null;
    public Canvas canvas;

    int MyScore = 0;
    public float LandingTimer = 0f;


    void Start()
    {
        Global.Authenticate();
    }

    void GetScore()
    {
        HighScore.text = PlayerPrefs.GetString("HighScore", "0");
    }

    void StartGame()
    {
        for (int i = 1; i < MaxBlock; i++)
        {
            Invoke("CreateNewBlock", i * 0.1f);
        }
    }

	void CreateNewBlock()
	{
        Vector3 pos = Vector3.zero;

        while (true)
        {
            int rnd = Random.Range(0, 100);

            if (rnd < 50)
            {
                pos = new Vector3(LastBlock.transform.localPosition.x, 1f, LastBlock.transform.localPosition.z + 1f);
                if (!Physics.Raycast(pos, Vector3.down, 1.5f) && !Physics.Raycast(new Vector3(pos.x, pos.y, pos.z + 1f), Vector3.down, 1.5f)) break;
            }
            else if (rnd < 70)
            {
                pos = new Vector3(LastBlock.transform.localPosition.x + 1f, 1f, LastBlock.transform.localPosition.z);
                if (!Physics.Raycast(pos, Vector3.down, 1.5f) && !Physics.Raycast(new Vector3(pos.x + 1f, pos.y, pos.z), Vector3.down, 1.5f)) break;
            }
            else if (rnd < 90)
            {
                pos = new Vector3(LastBlock.transform.localPosition.x - 1f, 1f, LastBlock.transform.localPosition.z);
                if (!Physics.Raycast(pos, Vector3.down, 1.5f) && !Physics.Raycast(new Vector3(pos.x - 1f, pos.y, pos.z), Vector3.down, 1.5f)) break;
            }
            else
            {
                pos = new Vector3(LastBlock.transform.localPosition.x, 1f, LastBlock.transform.localPosition.z - 1f);
                if (!Physics.Raycast(pos, Vector3.down, 1.5f) && !Physics.Raycast(new Vector3(pos.x, pos.y, pos.z - 1f), Vector3.down, 1.5f)) break;
            }
        }
        int num = Random.Range(0, 100) > 0 ? 0 : 1;
        GameObject temp = Instantiate(Blocks[num], new Vector3(0f, 100f, 0f), Quaternion.identity) as GameObject;
        temp.transform.parent = transform;
        temp.transform.localPosition = new Vector3(pos.x, 0f, pos.z);
        temp.SetActive(true);
        temp.name = "block";
        LastBlock = temp;
	}

    void CreateScore(int score)
    {
        GameObject temp = Instantiate(Score) as GameObject;
        temp.transform.parent = canvas.transform;
        temp.transform.localScale = new Vector3(1f, 1f, 1f);

        if (score > 0)
        {
            temp.GetComponent<Text>().text = "+" + score.ToString();
            MyScore += score;
            TotalScore.text = MyScore.ToString();

            if (MyScore > int.Parse(HighScore.text))
            {
                HighScore.text = MyScore.ToString();
                PlayerPrefs.SetString("HighScore", HighScore.text);
            }
        }
        else
        {
            temp.GetComponent<Text>().text = "MISS";
            temp.GetComponent<Text>().color = Color.red;
        }

        RectTransform rect = temp.GetComponent<RectTransform>();
        rect.anchoredPosition = WorldToCanvas(CatLandedBlock.transform.position);
        temp.transform.DOLocalMoveY(temp.transform.localPosition.y + 200f, 1.0f).SetEase(Ease.OutBack);
        temp.transform.DOLocalMoveX(temp.transform.localPosition.x + Random.Range(-100f, 100f), 1.0f);
        LeanTween.textAlpha(rect, 0f, 1.0f);

        Destroy(temp, 1.0f);
    }

    public void LeaveLandedBlock()
    {
        int score = (int)(300 - (Time.time - LandingTimer) * 1000);
        if (CatLandedBlock.GetComponent<Block>().FallDelay < 0) score = 500;
        CreateScore(score);

        CreateNewBlock();
        CatLandedBlock.SendMessage("FallBlock");
        CatLandedBlock = null;
    }

    public Vector2 WorldToCanvas(Vector3 world_position)
    {
        var viewport_position = Camera.main.WorldToViewportPoint(world_position);
        var canvas_rect = canvas.GetComponent<RectTransform>();

        return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                           (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
    }
}
