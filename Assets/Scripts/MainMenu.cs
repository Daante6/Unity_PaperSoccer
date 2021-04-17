using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button instructionsButton;
    public Button backButton;
    public Button vsPlayerButton;
    public Button vsAIButton;
    public Button nextButton;
    public Text instructionText;
    public Text instructionText2;
    public void vsPlayer()
    {
        StaticNameController.aiActive = false;
        SceneManager.LoadScene(1);
    }
    public void vsAI()
    {
        StaticNameController.aiActive = true;
        SceneManager.LoadScene(1);
    }
    public void instructions()
    {
        vsAIButton.gameObject.SetActive(false);
        vsPlayerButton.gameObject.SetActive(false);
        instructionsButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }
    public void back()
    {
        vsAIButton.gameObject.SetActive(true);
        vsPlayerButton.gameObject.SetActive(true);
        instructionsButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);
        instructionText2.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
    public void next()
    {
        instructionText.gameObject.SetActive(false);
        instructionText2.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }
}
