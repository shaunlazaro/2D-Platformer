using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUIController : MonoBehaviour {

    public GameObject airJump;
    public GameObject airDash;

    private Text airJumpText;
    private Text airDashText;

    public int airJumpNum = 0;
    public int airDashNum = 0;

	// Use this for initialization
	void Start () {
        airJumpText = airJump.GetComponent<Text>();
        airDashText = airDash.GetComponent<Text>();
	}


    public void UpdateJumpText(int newValue)
    {
        MasterUpdateText(airJumpText,
            "Air Jumps Left: " + newValue);
        airJumpNum = newValue;
    }
    public void UpdateDashText(int newValue)
    {
        MasterUpdateText(airDashText,
            "Air Dashes Left: " + newValue);
        airDashNum = newValue;
    }
    private void MasterUpdateText(Text textUpdated, string message )
    {
        textUpdated.text = message;
    }
}
