using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using TMPro;

namespace CGber
{
    public class CharacterSelect : NetworkBehaviour
    {
        [SerializeField] private GameObject characterSelectDisplay = default;
        [SerializeField] private Transform characterPreviewParent = default;
        [SerializeField] private TMP_Text characterNameText = default;
        [SerializeField] private Character[] characters = default;

        private int currentCharacterIndex = 0;
        private List<GameObject> characterInstances = new List<GameObject>();


        public override void NetworkStart()
        {
            if (characterPreviewParent.childCount == 0)
            {
                foreach (var character in characters)
                {
                    GameObject characterInstance =
                        Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);

                    characterInstance.SetActive(false);

                    characterInstances.Add(characterInstance);
                }
            }
            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;

            characterSelectDisplay.SetActive(true);
        }

        public void Select()
        {
            SpawnServerRpc(NetworkManager.Singleton.LocalClientId, currentCharacterIndex);
            characterSelectDisplay.SetActive(false);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnServerRpc(ulong clientId, int characterIndex)
        {
            GameObject characterInstance = Instantiate(characters[characterIndex].GameplayCharacterPrefab);
            characterInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
        }

        public void Right()
        {
            characterInstances[currentCharacterIndex].SetActive(false);

            currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;

            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;
        }

        public void Left()
        {
            characterInstances[currentCharacterIndex].SetActive(false);

            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
            {
                currentCharacterIndex += characterInstances.Count;
            }

            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;
        }
    }
}
