using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static DataManager Instance;
    public string playerName;
    public Dictionary<string, int> scoreRecord;
    public string bestPlayer;
    public int bestScore;
    public TextMeshProUGUI bestPlayerScoreText;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // 씬이 변경될 때 파괴하지 않겠다
        LoadRecord();
        //todo textbestscore
        writeMenuBestPlayerScore();
    }
    public void inputName(string name)
    {
        playerName = name;
    }

    [System.Serializable]
    class SaveData
    {
        public Dictionary<string,int> scoreRecord;
        public string bestPlayer;
        public int bestScore;
        public SaveData DeepCopy()
        {
            SaveData newCopy = new SaveData();
            newCopy.scoreRecord = this.scoreRecord;
            newCopy.bestPlayer = this.bestPlayer;
            newCopy.bestScore = this.bestScore;
            return newCopy;
        }
    }




    public void SaveRecord()  // 게임이 끝났을 때 호출
    {
        SaveData tmpData = new SaveData();
        tmpData.scoreRecord = new Dictionary<string, int>(); //tmp init
        MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();  // 현재 점수확인을위함
        int curScore = mainManager.m_Points;
        if (scoreRecord != null)
        {

            // 플레이어 개인기록 갱신
            if (scoreRecord.ContainsKey(playerName))
            {
                scoreRecord[playerName] = Mathf.Max(scoreRecord[playerName],curScore);
            }
            else
            {
                scoreRecord.Add(playerName,curScore);
            }

            // bestplayer 기록 갱신
            if (bestScore < curScore)
            {
                bestPlayer = playerName;
                bestScore = curScore;
            }
        }
        else
        {
            scoreRecord = new Dictionary<string, int>();
            scoreRecord.Add(playerName, curScore);
            bestPlayer = playerName;
            bestScore = curScore;
        }
        // data 갱신
        tmpData.bestPlayer = bestPlayer;
        tmpData.bestScore = bestScore;
        tmpData.scoreRecord = scoreRecord;

        string json = JsonConvert.SerializeObject(tmpData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        //Debug.Log("save complete" + json);


        //data.scoreRecord = new Dictionary<string, int>(); // init 해줘야 ==null 조건문안의 body가 작동
        //MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();  // 현재 점수확인을위함
        /*int score = mainManager.m_Points;
        if (scoreRecord != null)  //첫판을 연속으로 하는 경우 데이터가 있는걸 인지 못하는듯
        {
            Debug.Log("두번째부터");
            data.scoreRecord = scoreRecord;  // load로 불러왔을 데이터  // 이게 잘 되려나?
            data.bestScore = bestScore;
            data.bestPlayer = bestPlayer;
            if (scoreRecord.ContainsKey(playerName))
            {
                data.scoreRecord[playerName] = Mathf.Max(scoreRecord[playerName], score);
            }
            else
            {
                scoreRecord.Add(playerName, score);
            }
            //스코어 기록이있으면 베스트플레이어도 반드시 있음
            if (data.bestScore < score)
            {
                data.bestPlayer = playerName;
                data.bestScore = score;
            }
        }
        else  // 첫 게임인 경우
        {
            Debug.Log("한번만?");
            data.scoreRecord.Add(playerName, score);  // data참조를 안썼었네 
            data.bestPlayer = playerName;
            data.bestScore = score;
            bestPlayer = data.bestPlayer;  //누락했네
            bestScore = data.bestScore; // 누락
            scoreRecord = data.scoreRecord;
            //Debug.Log(playerName + mainManager.m_Points);
        }*/
    }

    public void LoadRecord()  //메뉴 첫화면부터 호출, 세이브이후 다시시작할때 호출
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData loadData = JsonConvert.DeserializeObject<SaveData>(json);
            scoreRecord = new Dictionary<string, int>();
            scoreRecord = loadData.scoreRecord;
            bestScore= loadData.bestScore;
            bestPlayer = loadData.bestPlayer;
            //Debug.Log("load file found");
            //scoreRecord = new Dictionary<string, int>(data.scoreRecord);  // dictionary복사
            // Debug.Log(scoreRecord+"fadasdas"); 유니티는 해당 값이 없는 경우는 라인에러가 안나고 건너뛰는거같더라 디버그로그찍는건
            //그건아닌듯 위에서찍힌걸보니까 로드가 파일이없으니 여길 안오는구나
            /*bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;*/
            // Debug.Log("load file existed");
        }
    }

    /*public Dictionary<string,int> WhoIsBestPlayer()
    {
        //bestPlayerScoreText.text = (bestPlayer == "") ? "Unknown: " + bestScore : bestPlayer + ": " + bestScore;
        Dictionary<string, int> rankers = new Dictionary<string, int>();
        rankers.Add((bestPlayer=="") ? "Unknown" : bestPlayer, bestScore);
        return rankers;
    }*/

    public void writeMenuBestPlayerScore()
    {
        bestPlayerScoreText.text = "Best Score : " + (bestPlayer == "" ? "Unknown" : bestPlayer) + " : " + bestScore;  //초기값이 "", 랑 0이구나
    }
}
