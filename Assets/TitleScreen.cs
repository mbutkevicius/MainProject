using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public float startDelay;
    public float fadeDelay;

    public Text scoreText;

    [HideInInspector]
    private bool isInputLocked = true;

    public Animator transition;
    public Animator keyPrompt;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isInputLocked)
        {
            StartCoroutine(PlayGame());
        }
    }

    IEnumerator PlayGame()
    {
        transition.SetTrigger("Death");

        FindObjectOfType<AudioManager>().Play("TitleScreenKeyPress");

        keyPrompt.SetTrigger("PressedKey");

        yield return new WaitForSeconds(fadeDelay);

        transition.SetTrigger("Out");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(1);
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);

        isInputLocked = false;
    }
}
