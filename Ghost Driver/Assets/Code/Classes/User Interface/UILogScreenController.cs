using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UILogScreenController : MonoBehaviour
{
    [Tooltip ("The button to use for entries in the log book.")]
    [SerializeField] private Button _EntryButton = null;
    [Tooltip ("The holder of the relic entries.")]
    [SerializeField] private GameObject _RelicEntryHolder = null;
    [Tooltip ("The label used for displaying the current relic's name.")]
    [SerializeField] private Text _RelicNameLabel = null;
    [Tooltip ("The label used for the current relic's description.")]
    [SerializeField] private Text _RelicDescriptionLabel = null;

    private Dictionary<Button, RelicInfo> _RelicEntries = new Dictionary<Button, RelicInfo> ();

    private void OnEnable ()
    {
        //for (int i = 0; i < 20; i++)
        //{
        //    var relicInfo = new RelicInfo
        //    {
        //        Name = "Test: " + i,
        //        Description = "Funny Text Here"
        //    };

        //    LogBook.AddNewRelic (relicInfo);
        //}

        BindRelicEntries ();
    }

    private void BindRelicEntries ()
    {
        foreach (RelicInfo relic in LogBook.Relics)
        {
            var button = Instantiate (_EntryButton);
            button.name = relic.Name;
            button.transform.SetParent (_RelicEntryHolder.transform);
            // Scale goes weird when creating for some reason so we reset it here.
            button.transform.localScale = new Vector3 (1, 1, 1);

            var buttonLabel = button.GetComponentInChildren<Text> ();
            buttonLabel.text = relic.Name;

            _RelicEntries.Add (button, relic);

            button.onClick.AddListener (delegate { OnRelicEntryClicked (button); });
        }
    }

    public void OnRelicEntryClicked (Button button)
    {
        var relicInfo = _RelicEntries[button];

        _RelicNameLabel.text = relicInfo.Name;
        _RelicDescriptionLabel.text = relicInfo.Description;
    }

    public void BackToPreviousMenu (GameObject menu)
    {
        this.gameObject.SetActive (false);
        menu.gameObject.SetActive (true);
    }

    private void OnDisable ()
    {
        // Destroy all entry buttons so that they can be rebuilt next time.
        foreach (Button button in _RelicEntries.Keys)
        {
            Destroy (button.gameObject);
        }

        _RelicNameLabel.text = "";
        _RelicDescriptionLabel.text = "";

        // Flush the entries from memory so that we can rebuild any changes that may occur next time player opens.
        _RelicEntries.Clear ();
    }
}
