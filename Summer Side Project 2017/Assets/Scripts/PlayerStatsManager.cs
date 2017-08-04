using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour {

    #region Public Access

    /*
    public int level;
    public int hp;
    public int batteryCharge; // It might be implemented, still considering.
    */

    private int maxJumps;
    public int MaxJumps
    {
        get { return MaxJumps; }
        set { maxJumps = value; }
    }
    private string maxJumpsKey = "maxJumps";

    private int maxDashes;
    public int MaxDashes
    {
        get { return MaxDashes; }
        set { maxDashes = value; }
    }
    private string maxDashesKey = "maxDashes";

    #endregion

    #region Default Values
    private int maxJumpsDefault = 2;
    private int maxDashesDefault = 2;
    #endregion


    // Use this for initialization
    void Start () {
        CheckForKeys();
        LoadSave();
	}

    private void CheckForKeys()
    {
        //If there are no values, set to default.
        if (!PlayerPrefs.HasKey(maxJumpsKey)) PlayerPrefs.SetInt(maxJumpsKey, maxJumpsDefault);
        if (!PlayerPrefs.HasKey(maxDashesKey)) PlayerPrefs.SetInt(maxDashesKey, maxDashesDefault);
    }

    public void LoadSave()
    {
        maxJumps = PlayerPrefs.GetInt("maxJumps");
        maxDashes = PlayerPrefs.GetInt("maxDashes");
    }
}
