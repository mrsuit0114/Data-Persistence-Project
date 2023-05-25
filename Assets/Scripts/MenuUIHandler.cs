using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField inputFiled_PlayerName;

    public void StartNew()
    {
        DataManager.Instance.inputName(inputFiled_PlayerName.text);
        //  datamanager에 인풋에있는 이름가져와서 넣어주기
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity 플레이어를 종료하는 원본 코드
#endif
    }
    /*public void WriteBestPlayer()
    {
        DataManager.Instance.bestPlayerScoreText.text = (DataManager.Instance.bestPlayer == "") ? "Unknown: " + DataManager.Instance.bestScore : DataManager.Instance.bestPlayer + ": " + DataManager.Instance.bestScore;
    }*/  // brick도 보면 UI에서 안하고 자체적으로 처리하는거보니 규모가 작아서그러나 아직 어디까지 역할을 나눠야할지 모르겠음
}
