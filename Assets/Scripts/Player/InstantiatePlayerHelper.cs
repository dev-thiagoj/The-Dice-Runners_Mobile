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

    //escolher o player que será usado no jogo
    public void ChoosePlayer()
    {
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
        PlayerController.Instance.characterController = GameObject.Find("=== PLAYER ===").GetComponentInChildren<CharacterController>();
        PlayerController.Instance.playerAnimation.FindAnimator();
        vcam.m_AnimatedTarget = characters[characterIndex].GetComponent<Animator>();
        characterIndex++;
        if (characterIndex == characters.Count) characterIndex = 0;
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
