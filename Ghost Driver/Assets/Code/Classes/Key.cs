using UnityEngine;

[RequireComponent (typeof (Collider))]
public class Key : MonoBehaviour
{
    private void Awake ()
    {
        GetComponent<Collider> ().isTrigger = true;

        var gate = GameObject.FindObjectOfType<Gate> ();
        gate.Key = this;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
            this.gameObject.SetActive (false);
    }

    //private void OnEnable ()
    //{
    //    EventManager.Instance.AddListener<LevelReset> (OnLevelReset);
    //}

    //private void OnLevelReset (LevelReset e)
    //{
    //    if (this.gameObject.activeSelf == false)
    //        this.gameObject.SetActive (true);
    //}

    //private void OnDestroy ()
    //{
    //    EventManager.Instance.RemoveListener<LevelReset> (OnLevelReset);
    //}
}
