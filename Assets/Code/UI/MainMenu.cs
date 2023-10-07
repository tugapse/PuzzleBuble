using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        StartCoroutine(this.FadeOut());
    }
    private IEnumerator FadeOut()
    {
        this.animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        this.levelManager.StartSinglePlayerGame();
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
