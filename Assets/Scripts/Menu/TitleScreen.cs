using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    public GameObject UILayer1; public GameObject UILayer2; public GameObject startButton; public GameObject clipboardCopyButton; public GameObject clipboardPasteButton;
    public TMP_Text connectionText;
    public string currentJoinCode;
    public bool isHost = false;

    public void PlayOffline()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PlayOnline()
    {
        UILayer1.SetActive(false);
        UILayer2.SetActive(true);
        startButton.SetActive(false);
        clipboardCopyButton.SetActive(false);
        clipboardPasteButton.SetActive(true);
        connectionText.text = "";
    }

    public async void StartHost()
    {
        isHost = true;
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        currentJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        NetworkManager.Singleton.StartHost();
        startButton.SetActive(false);
        clipboardCopyButton.SetActive(true);
        connectionText.text = "Host: Waiting for other player... Code: " + currentJoinCode;
    }

    public void CopyJoinCode()
    {
        GUIUtility.systemCopyBuffer = currentJoinCode;
    }

    public async void StartClient()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(currentJoinCode);
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.StartClient();
        connectionText.text = "Client: Connecting to code " + currentJoinCode + "...";
    }

    public void PasteJoinCode()
    {
        currentJoinCode = GUIUtility.systemCopyBuffer;
    }

    public void OnClientConnected(ulong clientId)
    {
        connectionText.text = "Connected: Waiting for host to start...";
    }

    public void OnPlayerJoined(ulong clientId)
    {
        connectionText.text = "Connected: Press button to start...";
        startButton.SetActive(true);
    }

    public void HostStartGame()
    {
        if (isHost) {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerJoined;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single); 
        }
    }

    public void ResetUI()
    {
        UILayer1.SetActive(true);
        UILayer2.SetActive(false);
        startButton.SetActive(false);
        clipboardCopyButton.SetActive(false);
        clipboardPasteButton.SetActive(false);
        NetworkManager.Singleton.Shutdown();
    }
}
