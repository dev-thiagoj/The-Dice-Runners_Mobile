using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMagneticBtnManager : MonoBehaviour
{
    public Image image;
    [SerializeField] Color disabled;
    [SerializeField] Color activated;

    [Header("Particle System")]
    [SerializeField] ParticleSystem particleSystem;

    [Header("Text")]
    public TextMeshProUGUI valueText;

    private void OnEnable()
    {
        Magnetic_PowerUp.onChangeAmount += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        Magnetic_PowerUp.onChangeAmount -= UpdateUI;
    }

    private void OnValidate()
    {
        image = GetComponent<Image>();
    }

    private void Awake()
    {
        valueText = GameObject.Find("ValueText").GetComponent<TextMeshProUGUI>();
    }

    public void HasMagnetic()
    {
        image.color = activated;
        particleSystem.Play();
    }

    void HasntMagnetic()
    {
        image.color = disabled;
        particleSystem.Stop();
    }

    void UpdateUI()
    {
        int value = GameObject.Find("=== PLAYER ===").GetComponent<Magnetic_PowerUp>().magneticValue;
        valueText.text = value.ToString();

        if(value > 0)
        {
            HasMagnetic();
            return;
        }
        HasntMagnetic();
    }
}
