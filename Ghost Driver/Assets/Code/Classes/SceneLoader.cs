using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TODO: MAke scene loader work better as currently having people needing to find it is stupid and messy.

class SceneLoader : MonoBehaviour, IWakeable
{
    public static SceneLoader Instance = null;
    
    [Tooltip ("The progress bar to update.")]
    [SerializeField] private Image _ProgressBar = null;

    private string _Scene = "SC_Main_Menu";

    public void Waken ()
    {
        if (Instance != null && Instance != this)
            Destroy (this.gameObject);
        else
            Instance = this;

        //Unity hack because it never calls ondestory on objects that are inactive for the whole scene.
        // It also doesn't fire off many other basic functions because it's a useless engine.
        this.gameObject.SetActive (true);
        this.gameObject.SetActive (false);
    }

    public void Load (string scene)
    {
        this.gameObject.SetActive (true);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive (true);
        }

        _Scene = scene;

        StartCoroutine (LoadNewScene ());
    }

    // The coroutine runs on its own at the same time as Update() and takes a string indicating which scene to load.
    IEnumerator LoadNewScene ()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync (_Scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            _ProgressBar.fillAmount = async.progress;
            yield return new WaitForEndOfFrame ();
        }
    }
}
