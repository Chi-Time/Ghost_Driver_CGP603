using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (Collider))]
public class MotherShard : MonoBehaviour
{
    [Tooltip ("What shard family does this belong to?")]
    [SerializeField] private ShardType _Type = ShardType.Alpha;
    
    [Tooltip ("The various shards that this mother will activate upon collection.")]
    [SerializeField] private List<Shard> _Shards = new List<Shard> ();

    /// <summary>Determines whether or not the mother has been activated.</summary>
    private bool _HasActivated = false;

    private void Awake ()
    {
        GetShards ();
        GetComponent<Collider> ().isTrigger = true;
    }

    private void GetShards ()
    {
        var shards = this.gameObject.FindAllObjectsOfType<Shard> ();

        for (int i = 0; i < shards.Length; i++)
        {
            if (shards[i].Type == _Type)
            {
                _Shards.Add (shards[i]);
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            ActivateMotherShard ();
            this.gameObject.SetActive (false);
        }
    }

    private void ActivateMotherShard ()
    {
        _HasActivated = !_HasActivated;

        if (_Shards.Count > 0)
        {
            for (int i = 0; i < _Shards.Count; i++)
            {
                // When the mother shard get's reset with the level.
                // We need to invert the logic of each seperate shard.
                // So we check to see if we're activated or not and then invert each one respectively.
                if (_HasActivated)
                    _Shards[i].gameObject.SetActive (!_Shards[i].ShouldHide);
                else
                    _Shards[i].gameObject.SetActive (_Shards[i].ShouldHide);
            }
        }
    }

    //private void OnEnable ()
    //{
    //    EventManager.Instance.AddListener<LevelReset> (OnLevelReset);
    //}

    //private void OnLevelReset (LevelReset e)
    //{
    //    if (this.gameObject.activeSelf == false)
    //        this.gameObject.SetActive (true);

    //    if (_HasActivated)
    //        ActivateMotherShard ();
    //}

    //private void OnDestroy ()
    //{
    //    EventManager.Instance.RemoveListener<LevelReset> (OnLevelReset);
    //}
}
