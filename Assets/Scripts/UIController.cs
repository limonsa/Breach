using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private TMP_Text _waterLeftValue;
    [SerializeField] private TMP_Text _aliensLeftValue;
    [SerializeField] private TMP_Text _algaeLeftValue;
    [SerializeField] private GameObject _gameOverCanvas;

    private void Start()
    {
        WaterSource.UpdatingHealth += ShowWaterResources;
        _gameOverCanvas.SetActive(false);
    }
    public void ShowWaterResources()
    {
        _waterLeftValue.text = _gm.CalculateWaterLeft().ToString();
    }

    public void ShowAliensLeft(int value)
    {
        _aliensLeftValue.text = value.ToString();
    }

    public void ShowAlgaeLeft(int value)
    {
        _algaeLeftValue.text = value.ToString();
    }

    public void EnableButtons()
    {
        _gameOverCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        WaterSource.UpdatingHealth -= ShowWaterResources;
    }
}
