using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using System.Text;
using System.Linq;

namespace CGber
{
    public class GUIManager : NetworkBehaviour
    {
        // InputField
        [SerializeField] private InputField _keyInputField;
        [SerializeField] private InputField _nameInputField;
        [SerializeField] private InputField _passwordInputField;

        [SerializeField] private GameObject _mainMenuGui;
        [SerializeField] private GameObject _leaveButton;

        private UserService userService = new UserService();

        private static IList<UserModel> _clientData;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

            string[] args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--server")
                {
                    _keyInputField.text = "0000";
                    _nameInputField.text = "Host";
                    Host();
                }

            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("esc");
                _leaveButton.SetActive(!_leaveButton.activeSelf);
            }

        }

        private void OnDestroy()
        {
            // Prevent error in the editor
            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void Host()
        {
            Debug.Log("This is HOST");

            // init client data
            _clientData = new List<UserModel>();

            // get input field
            string userId = _nameInputField.text;
            string userPassword = _passwordInputField.text;

            Debug.Log(userId);
            Debug.Log(userPassword);

            // read user by user id
            UserModel user = userService.ReadUserByIdAndPasswd(userId, userPassword);

            // if user not exist, return
            if (user == null) return;

            // add cliet id to user
            user.clientId = NetworkManager.Singleton.LocalClientId;

            // save user to client data
            _clientData.Add(user);

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }



        public void Client()
        {
            Debug.Log("This is Client");
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                connectKey = _keyInputField.text,
                userId = _nameInputField.text,
                userPassword = _passwordInputField.text,
            });

            byte[] playloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = playloadBytes;
            NetworkManager.Singleton.StartClient();
        }

        public void Leave()
        {
            Debug.Log("Leave");
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }
            else
            {
                Debug.Log("Quit CGber");
                Application.Quit();
            }

            _mainMenuGui.SetActive(true);
            _leaveButton.SetActive(false);

        }

        private void HandleServerStarted()
        {
            // Temporary workaround to treat host as client
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.ServerClientId);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            // Are we the client that is connecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("Client Connected");
                _mainMenuGui.SetActive(false);
                //_leaveButton.SetActive(true);
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                // search user by client id
                UserModel user = _clientData.Where(u => u.clientId == clientId).FirstOrDefault();

                // if not found user, return
                if (user == null) return;

                // remove user from clientData
                _clientData.Remove(user);
            }

            // Are we the client that is disconnecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _mainMenuGui.SetActive(true);
                _leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            // process raw payload
            string payload = Encoding.UTF8.GetString(connectionData);
            ConnectionPayload connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            // read payload data
            string inpKey = connectionPayload.connectKey;
            string userId = connectionPayload.userId;
            string userPassword = connectionPayload.userPassword;
            string key = _keyInputField.text;

            // read user by user id
            UserModel user = userService.ReadUserByIdAndPasswd(userId, userPassword);

            // check key equal to host key and user exist
            bool approveConnection = inpKey == key && user != null;

            // if successful authentication, save into client data
            if (approveConnection)
            {
                // add client id
                user.clientId = clientId;

                // add user to cliet data
                _clientData.Add(user);
            }

            callback(true, null, approveConnection, null, null);
        }

        public static UserModel GetPlayerData(ulong clientId)
        {
            return _clientData.Where(u => u.clientId == clientId).FirstOrDefault();
        }

    }
}


