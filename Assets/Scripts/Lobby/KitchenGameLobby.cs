using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby instance { get; private set; }

    private Lobby joinedLobby;
    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }
    private async void InitializeUnityAuthentication()
    {
       if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();

            options.SetProfile(Random.Range(0,10000).ToString());

            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }
    public async void CreateLobby(string lobbyName,bool isPrivate)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KitchenObjectNetworkManager.MAX_PLAYER, new CreateLobbyOptions
            {
                IsPrivate = isPrivate
            });
            KitchenObjectNetworkManager.instance.StartHost();
            Loader.LoadNetworkScene(Loader.Scene.CharacterScene);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            KitchenObjectNetworkManager.instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);

        }
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
