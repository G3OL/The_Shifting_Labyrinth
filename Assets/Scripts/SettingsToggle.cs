using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public GameObject settingsMenu; // Assign the Panel in the Inspector
    public GameObject gameBoard; // Assign your game board GameObject here in the Inspector
    public GameObject character;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject rightButton;
    public GameObject leftButton;

    public void ToggleSettingsMenu()
    {
        bool isMenuActive = settingsMenu.activeSelf;
        settingsMenu.SetActive(!isMenuActive);
        gameBoard.SetActive(isMenuActive); // Toggle the game board's active state opposite to the menu's
        character.SetActive(isMenuActive);
        upButton.SetActive(isMenuActive);
        downButton.SetActive(isMenuActive);
        rightButton.SetActive(isMenuActive);
        leftButton.SetActive(isMenuActive);
    }
}
