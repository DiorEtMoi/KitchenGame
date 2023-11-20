using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public static Scene LoadSceneTarget;

    public enum Scene
    {
        GameScene,
        LoadingScene,
        MainMenu,
        LobbyScene,
        CharacterScene
    }
    public static void Load(Scene scene)
    {
        LoadSceneTarget = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());

    }
    public static void LoadNetworkScene(Scene scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }
    public static void LoadCallback()
    {
        SceneManager.LoadScene(LoadSceneTarget.ToString());
    }
}
