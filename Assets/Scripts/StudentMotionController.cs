using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

namespace CGber
{
    public class StudentMotionController : NetworkBehaviour
    {
        [SerializeField] private GameObject AnswerPos;
        [SerializeField] private NetworkObject O; // Answer  true 
        [SerializeField] private NetworkObject X; //Answer  false 

        private Animator m_animator;
        private bool _isAnswer = false;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public override void NetworkStart()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) { return; }

            if (Input.GetKeyDown(KeyCode.O) && !_isAnswer)
            {
                // Network Animator motion
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 1);
                }

                Vector3 spawnPos = AnswerPos.transform.position;

                SpawnAnswereServerRpc('O', spawnPos);
                _isAnswer = true;
            }

            if (Input.GetKeyDown(KeyCode.X) && !_isAnswer)
            {
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 2);
                }

                Vector3 spawnPos = AnswerPos.transform.position;

                SpawnAnswereServerRpc('X', spawnPos);
                _isAnswer = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 0);
                }
                _isAnswer = false;
            }
        }

        [ServerRpc]
        public void SpawnAnswereServerRpc(char cmd, Vector3 spawnPos)
        {
            switch (cmd)
            {
                case 'O':
                    NetworkObject CorrectAnswerInstance = Instantiate(O, spawnPos, Quaternion.identity);
                    CorrectAnswerInstance.transform.parent = AnswerPos.transform;
                    CorrectAnswerInstance.SpawnWithOwnership(OwnerClientId);
                    break;
                case 'X':
                    NetworkObject WrongAnswerInstance = Instantiate(X, spawnPos, Quaternion.identity);
                    WrongAnswerInstance.transform.parent = AnswerPos.transform;
                    WrongAnswerInstance.SpawnWithOwnership(OwnerClientId);
                    break;
                default:
                    break;
            }
        }

    }
}