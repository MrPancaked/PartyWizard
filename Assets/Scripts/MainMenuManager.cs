using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject controlsUI;
    
    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PauseGameAction.performed += ToggleMenu;
        }
        OpenMenu();
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PauseGameAction.performed -= ToggleMenu;
        }
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!mainMenuUI.activeInHierarchy)
            {
                OpenMenu();
            }
            else 
            {
                CloseMenu();
            }
        }
    }
    private void OpenMenu()
    {
        Time.timeScale = 0;
        controlsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        controlsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
