using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    //Unique game name for registering
    const string typeName = "MetropoliaCarGame2014";

    //Port for the server
    int port = 7777;
    int maxPlayers = 10;

    //Lobby name
    string gameName = "Host1";
    string comment = "Ebin";

    //Password
    string serverPassword = "12345";
    string password = "12345";

    //All the host data
    HostData[] hostData;
    HostData selectedHost;

    // Use this for initialization
    void Awake()
    {
        //Check that only 1 Network manager exist!?
    }

    // Update is called once per frame
    void Update()
    {

    }

    //GUIButtons
    void OnGUI()
    {
        if (GameManager.CheckForState(State.StartMenu))
        {
            //You are not on server
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                //StartServer Button
                if (GUI.Button(new Rect(100, 100, 100, 100), "Start Server"))
                    StartServer(maxPlayers, password, comment);

                //GetHostList Button
                if (hostData == null)
                {
                    if (GUI.Button(new Rect(100, 250, 100, 100), "Get Host List"))
                        GetHostList();
                }
                else
                {
                    if (GUI.Button(new Rect(100, 250, 100, 100), "Refresh hosts"))
                        RefreshHostList();
                }

                //JoinServer Buutton
                if (GUI.Button(new Rect(100, 400, 100, 100), "Join Server") && selectedHost != null)
                    Connect(selectedHost, password);

                //HostList
                if (hostData != null)
                {
                    for (int i = 0; i < hostData.Length; i++)
                    {
                        if (GUI.Button(new Rect(200,100+i*100,100,100), hostData[i].gameName))
                            selectedHost = hostData[i];
                    }
                }
            }
        }
    }

    /// <summary>
    /// Start Server
    /// </summary>
    /// <param name="maxPlayers">Number of max players</param>
    /// <param name="pass">Password for server (optional)</param>
    /// <param name="comment">Comment of the lobby</param>
    private void StartServer(int maxPlayers, string pass, string comment)
    {
        Network.incomingPassword = pass;
        bool useNat = !Network.HavePublicAddress();

        //Initialize Server
        Network.InitializeServer(maxPlayers, port, useNat);

        //Register Host for Unity Master Server
        MasterServer.RegisterHost(typeName, gameName, comment);
    }

    /// <summary>
    /// Get hosts
    /// </summary>
    private void GetHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    /// <summary>
    /// Refresh hosts
    /// </summary>
    private void RefreshHostList()
    {
        MasterServer.ClearHostList();
        GetHostList();
    }

    /// <summary>
    /// Connect to server
    /// </summary>
    /// <param name="hostData">Selected Server</param>
    /// <param name="pass">Password for the server</param>
    private void Connect(HostData hostData, string pass)
    {
        if (hostData.passwordProtected)
            Network.Connect(hostData, pass);
        else
            Network.Connect(hostData);
    }

    /// <summary>
    /// Disconnect from Server
    /// </summary>
    private void Disconnect()
    {
        Network.Disconnect();
    }

    //MESSAGES
    //Server is initialized
    void OnServerInitialized()
    {
        Debug.Log("Server initialized");
    }

    //Called on server
    void OnPlayerConnected()
    {
        Debug.Log("Player Connected");
    }

    //Called on client
    void OnConnectedToServer()
    {
        Debug.Log("I connected to server");
    }

    //Called on Server when player disconnetes
    void OnPlayerDisconnected()
    {

    }

    //Called on Client when disconnecetd
    void OnDisconnectedFromServer()
    {

    }

    //MasterServerEvents
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Server Registeration Succeeded");
        if (msEvent == MasterServerEvent.RegistrationFailedGameName)
            Debug.Log("GameName is invalid");
        if (msEvent == MasterServerEvent.RegistrationFailedGameType)
            Debug.Log("GameType is invalid");
        if (msEvent == MasterServerEvent.RegistrationFailedNoServer)
            Debug.Log("Server not found");

        if (msEvent == MasterServerEvent.HostListReceived)
            hostData = MasterServer.PollHostList();
    }

}
