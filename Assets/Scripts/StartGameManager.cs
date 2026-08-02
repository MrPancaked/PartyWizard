using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    [FMODUnity.BankRef] public List<string> banks;

    private void Start()
    {
        LoadBanks();
    }
    
    public void LoadBanks()
    {
        foreach (string bank in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(bank, true);
            Debug.Log("Loaded bank " + bank);
        }
        /*
            For Chrome / Safari browsers / WebGL.  Reset audio on response to user interaction (LoadBanks is called from a button press), to allow audio to be heard.
        */
        FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
        FMODUnity.RuntimeManager.CoreSystem.mixerResume();
        StartCoroutine(CheckBanksLoaded());
    }

    private IEnumerator CheckBanksLoaded()
    {
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
