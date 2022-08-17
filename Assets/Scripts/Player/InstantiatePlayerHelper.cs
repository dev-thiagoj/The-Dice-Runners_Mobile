using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class InstantiatePlayerHelper : Singleton<InstantiatePlayerHelper>
{
    public List<GameObject> characters;
    public List<GameObject> endLevelCharacters;
    public GameObject currEndCharacter;
    public int characterIndex = 0;
    public GameObject endLevelCharacterPos;
    public Cinemachine.CinemachineStateDrivenCamera vcam;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ChoosePlayer();
        characterIndex = 0;
        InstantiatePlayer();
    }

    public void ChoosePlayer()
    {
        //escolher o player que será usado no jogo
        foreach (var character in characters)
        {
            character.SetActive(false);
        }
    }

    // é chamado pelo botão de seleção do personagem no menu
    public void SetIndex(int i)
    {
        ChoosePlayer();
        characterIndex = i;
        InstantiatePlayer();
        InstantiateEndLevelCharacter();
    }

    public void InstantiatePlayer()
    {
        //instanciar o player na cena
        characters[characterIndex].SetActive(true);
        vcam.m_AnimatedTarget = characters[characterIndex].GetComponent<Animator>();
        PlayerController.Instance.characterController = GameObject.Find("=== PLAYER ===").GetComponentInChildren<CharacterController>();
        PlayerController.Instance.animator = GameObject.Find("=== PLAYER ===").GetComponentInChildren<Animator>();
        characterIndex++;
        if (characterIndex == 2) characterIndex = 0;
        //InstantiateEndLevelCharacter();
    }

    public void InstantiateEndLevelCharacter()
    {
        if(currEndCharacter != null) Destroy(currEndCharacter);
        endLevelCharacterPos = GameObject.Find("CharacterPos");
        
        //instanciar oq não foi escolhido para ficar no fim do level
        currEndCharacter = Instantiate(endLevelCharacters[characterIndex], endLevelCharacterPos.transform);
        currEndCharacter.transform.position = endLevelCharacterPos.transform.position;
        currEndCharacter.gameObject.SetActive(true);
        GameManager.Instance.winLevelAnim = currEndCharacter.GetComponent<Animator>();
    } 
}
