using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static DataManager Instance;
    public string playerName;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // ���� ����� �� �ı����� �ʰڴ�
    }

    public void inputName(string name)
    {
        playerName = name;
    }

}
