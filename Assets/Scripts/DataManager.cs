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
        DontDestroyOnLoad(gameObject);  // ���� ����� �� �ı����� �ʰڴ�
        LoadRecord();
        //todo textbestscore
        bestPlayerScoreText.text = (bestPlayer=="" ? "Unknown" : bestPlayer) + ": " + bestScore;  //�ʱⰪ�� "", �� 0�̱���

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
    }


    public void SaveRecord()  // ������ ������ �� ȣ��
    {
        SaveData data = new SaveData();
        data.scoreRecord = new Dictionary<string, int>(); // init ����� ==null ���ǹ����� body�� �۵�
        MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        int score = mainManager.m_Points;
        if (scoreRecord != null)  //ù���� �������� �ϴ� ��� �����Ͱ� �ִ°� ���� ���ϴµ�
        {
            data.scoreRecord = scoreRecord;  // load�� �ҷ����� ������  // �̰� �� �Ƿ���?
            if (scoreRecord.ContainsKey(playerName))
            {
                data.scoreRecord[playerName] = Mathf.Max(scoreRecord[playerName], score);  // ������ �̰� �ǳ�
            }
            else
            {
                scoreRecord.Add(playerName, score);
            }
            //���ھ� ����������� ����Ʈ�÷��̾ �ݵ�� ����
            if (data.bestScore < score)
            {
                data.bestScore = score;
                data.bestPlayer = playerName;
            }
        }
        else
        {
            data.scoreRecord.Add(playerName, score);  // data������ �Ƚ���� 
            data.bestPlayer = playerName;
            data.bestScore = score;
            bestPlayer = playerName;  //�����߳�
            bestScore = score; // ����
            scoreRecord = data.scoreRecord;
            //Debug.Log(playerName + mainManager.m_Points);
        }
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("save complete" + json);
    }

    public void LoadRecord()  //�޴� ùȭ����� ȣ��, ���̺����� �ٽý����Ҷ� ȣ��
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

            scoreRecord = new Dictionary<string, int>(data.scoreRecord);
            // Debug.Log(scoreRecord+"fadasdas"); ����Ƽ�� �ش� ���� ���� ���� ���ο����� �ȳ��� �ǳʶٴ°Ű����� ����׷α���°�
            //�װǾƴѵ� �����������ɺ��ϱ� �ε尡 �����̾����� ���� �ȿ��±���
            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;
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
}
