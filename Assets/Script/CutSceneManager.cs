using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CutSceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Scene_Main_Menu_01");
    }
}
