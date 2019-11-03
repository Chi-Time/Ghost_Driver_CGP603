using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//TODO: Implement relic text system.

class UIRelicScreenController : MonoBehaviour, IWakeable
{
    [Tooltip ("The label to use for displaying the relic's name.")]
    [SerializeField] private Text _RelicNameLabel = null;
    [Tooltip ("The label to use for displaying the relic's description.")]
    [SerializeField] private Text _RelicTextLabel = null;

    //TODO: Consider creating signal states that define when something new has happened.

    public void Waken ()
    {
        ExplorationSignals.OnRelicCollected += OnRelicCollected;

        //Unity hack because it never calls ondestory on objects that are inactive for the whole scene.
        // It also doesn't fire off many other basic functions because it's a useless engine.
        this.gameObject.SetActive (true);
        this.gameObject.SetActive (false);
    }

    private void OnRelicCollected (RelicInfo relic)
    {
        this.gameObject.SetActive (true);
        _RelicNameLabel.text = relic.Name;
        _RelicTextLabel.text = relic.Description;
    }

    private void OnDestroy ()
    {
        ExplorationSignals.OnRelicCollected -= OnRelicCollected;
    }

    public void Close ()
    {
        this.gameObject.SetActive (false);
        ExplorationSignals.QuitRelic ();
    }
}
