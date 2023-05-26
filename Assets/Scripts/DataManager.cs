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




    public void SaveRecord()  // ������ ������ �� ȣ��
    {
        SaveData tmpData = new SaveData();
        tmpData.scoreRecord = new Dictionary<string, int>(); //tmp init
        MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();  // ���� ����Ȯ��������
        int curScore = mainManager.m_Points;
        if (scoreRecord != null)
        {

            // �÷��̾� ���α�� ����
            if (scoreRecord.ContainsKey(playerName))
            {
                scoreRecord[playerName] = Mathf.Max(scoreRecord[playerName],curScore);
            }
            else
            {
                scoreRecord.Add(playerName,curScore);
            }

            // bestplayer ��� ����
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
        // data ����
        tmpData.bestPlayer = bestPlayer;
        tmpData.bestScore = bestScore;
        tmpData.scoreRecord = scoreRecord;

        string json = JsonConvert.SerializeObject(tmpData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        //Debug.Log("save complete" + json);


        //data.scoreRecord = new Dictionary<string, int>(); // init ����� ==null ���ǹ����� body�� �۵�
        //MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();  // ���� ����Ȯ��������
        /*int score = mainManager.m_Points;
        if (scoreRecord != null)  //ù���� �������� �ϴ� ��� �����Ͱ� �ִ°� ���� ���ϴµ�
        {
            Debug.Log("�ι�°����");
            data.scoreRecord = scoreRecord;  // load�� �ҷ����� ������  // �̰� �� �Ƿ���?
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
            //���ھ� ����������� ����Ʈ�÷��̾ �ݵ�� ����
            if (data.bestScore < score)
            {
                data.bestPlayer = playerName;
                data.bestScore = score;
            }
        }
        else  // ù ������ ���
        {
            Debug.Log("�ѹ���?");
            data.scoreRecord.Add(playerName, score);  // data������ �Ƚ���� 
            data.bestPlayer = playerName;
            data.bestScore = score;
            bestPlayer = data.bestPlayer;  //�����߳�
            bestScore = data.bestScore; // ����
            scoreRecord = data.scoreRecord;
            //Debug.Log(playerName + mainManager.m_Points);
        }*/
    }

    public void LoadRecord()  //�޴� ùȭ����� ȣ��, ���̺����� �ٽý����Ҷ� ȣ��
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
            //scoreRecord = new Dictionary<string, int>(data.scoreRecord);  // dictionary����
            // Debug.Log(scoreRecord+"fadasdas"); ����Ƽ�� �ش� ���� ���� ���� ���ο����� �ȳ��� �ǳʶٴ°Ű����� ����׷α���°�
            //�װǾƴѵ� �����������ɺ��ϱ� �ε尡 �����̾����� ���� �ȿ��±���
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
        bestPlayerScoreText.text = "Best Score : " + (bestPlayer == "" ? "Unknown" : bestPlayer) + " : " + bestScore;  //�ʱⰪ�� "", �� 0�̱���
    }
}
