using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

namespace CGber
{
    public class Answer : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) { return; }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DestroyAnswerServerRpc();
            }
        }

        [ServerRpc]
        public void DestroyAnswerServerRpc()
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }
}

