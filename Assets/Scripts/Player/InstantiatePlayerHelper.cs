using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class InstantiatePlayerHelper : Singleton<InstantiatePlayerHelper>
{
    public List<GameObject> characters;
    public List<GameObject> endLevelCharacters;
    public GameObject currEndCharacter;
    public int characterIndex;
    public GameObject endLevelCharacterPos;
    public Cinemachine.CinemachineStateDrivenCamera vcam;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("currPlayerIndex"))
        {
            ChoosePlayer();
            characterIndex = 0;
        }

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
        var player = GameObject.Find("=== PLAYER ===").GetComponent<PlayerController>();

        //instanciar o player na cena
        characters[characterIndex].SetActive(true);
        player.characterController = GameObject.Find("=== PLAYER ===").GetComponentInChildren<CharacterController>();
        player.playerAnimation.FindAnimator();
        vcam.m_AnimatedTarget = characters[characterIndex].GetComponent<Animator>();
        PlayerPrefs.SetInt("currPlayerIndex", characterIndex);
        characterIndex++;
        if (characterIndex == 2) characterIndex = 0;
    }

    public void InstantiateEndLevelCharacter()
    {
        if (currEndCharacter != null) Destroy(currEndCharacter);

        //instanciar oq não foi escolhido para ficar no fim do level
        endLevelCharacterPos = GameObject.Find("CharacterPos");
        currEndCharacter = Instantiate(endLevelCharacters[characterIndex], endLevelCharacterPos.transform);
        currEndCharacter.transform.position = endLevelCharacterPos.transform.position;
        currEndCharacter.gameObject.SetActive(true);
        GameManager.Instance.winLevelAnim = currEndCharacter.GetComponent<Animator>();
    }
}
