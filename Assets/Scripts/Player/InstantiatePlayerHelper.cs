using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class InstantiatePlayerHelper : Singleton<InstantiatePlayerHelper>
{
    public List<GameObject> characters;
    public List<GameObject> endLevelCharacters;
    //public CharacterController characterController;
    public int characterIndex = 0;
    //[SerializeField] GameObject _endLevelCharacterPos;
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

    public void SetIndex(int i)
    {
        ChoosePlayer();
        characterIndex = i;
        InstantiatePlayer();
    }

    public void InstantiatePlayer()
    {
        //instanciar o player na cena
        characters[characterIndex].SetActive(true);
        vcam.m_AnimatedTarget = characters[characterIndex].GetComponent<Animator>();
        PlayerController.Instance.characterController = GameObject.Find("=== PLAYER ===").GetComponentInChildren<CharacterController>();
        PlayerController.Instance.animator = GameObject.Find("=== PLAYER ===").GetComponentInChildren<Animator>();
        //PlayerController.Instance.characterController = characterController;
        characterIndex++;
        if (characterIndex == 2) characterIndex = 0;
        //InstantiateEndLevelCharacter();
    }

    public void InstantiateEndLevelCharacter()
    {
        //instanciar oq não foi escolhido para ficar no fim do level
        //_endLevelCharacterPos.transform.position = GameManager.Instance.femaleAnim.transform.position;
        endLevelCharacters[characterIndex].SetActive(true);
        //endLevelCharacters[characterIndex].transform.position = _endLevelCharacterPos.transform.position;
    } 
}
