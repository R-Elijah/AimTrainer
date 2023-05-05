using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStatDisplay : MonoBehaviour
{
    public Text hitsDisplay;
    public Text accDisplay;
    // Start is called before the first frame update
    void Start()
    {
        hitsDisplay.text = "Hits: " + PlayerStats.hitsTracker;
        accDisplay.text = "Accuracy: " + string.Format("{0:0.##}", ((double)PlayerStats.hitsTracker / PlayerStats.shotsTracker) * 100) + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
