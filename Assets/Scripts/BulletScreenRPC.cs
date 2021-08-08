using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MLAPI;
using MLAPI.Messaging;
using System.Text;

namespace CGber
{
    public class BulletScreenRPC : NetworkBehaviour
    {
        [SerializeField] private InputField M_InputField;
        [SerializeField] private GameObject BornPos;
        [SerializeField] private GameObject ToOpenButton;
        [SerializeField] private GameObject ToCloseButton;
        [SerializeField] private GameObject ChatUI;
        [SerializeField] private GameObject TextObj;

        private void Update()
        {
            if (M_InputField.text != "" && Input.GetKeyDown(KeyCode.Return)) SendMessage();
        }

        public void SendMessage()
        {
            if (M_InputField.text != "")
            {
                Color col = new Color(Random.Range(0f, 0.9f), Random.Range(0f, 0.9f), Random.Range(0f, 0.9f));
                CreateTextServerRpc(M_InputField.text, col);
                M_InputField.text = "";
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void CreateTextServerRpc(string message, Color col)
        {          
            CreateTextClientRpc(message, col);
        }

        [ClientRpc]
        void CreateTextClientRpc(string message, Color col)
        {
            GameObject obj = Instantiate(TextObj, BornPos.transform);
            Vector2 pos = BornPos.transform.position;
            pos.y += Random.Range(-100,100);
                     
            obj.transform.position = pos;
            obj.GetComponent<Text>().text = message;
            obj.GetComponent<Text>().color = col;
        }

        public void OpenChatUI()
        {
            ChatUI.SetActive(true);
            BornPos.SetActive(true);
            ToOpenButton.SetActive(false);
            ToCloseButton.SetActive(true);
        }

        public void CloseChatUI()
        {
            ChatUI.SetActive(false);
            BornPos.SetActive(false);
            ToCloseButton.SetActive(false);
            ToOpenButton.SetActive(true);
        }
    }
}