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
        //  datamanager�� ��ǲ���ִ� �̸������ͼ� �־��ֱ�
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity �÷��̾ �����ϴ� ���� �ڵ�
#endif
    }
}
