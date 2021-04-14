using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public SceneFader sceneFader;

    //Class that lets us change scenes using the Unity editor
    public void LoadScene(string sceneName)
    {
        //check that scene name has been assigned
        if (sceneName == null)
            Debug.Log("<color=orange>" + gameObject.name + ": No Scene Name Was given for LoadScene function!</color>");
        //load the scene linked in editor
        SceneManager.LoadScene(sceneName); //load a scene
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }
}
