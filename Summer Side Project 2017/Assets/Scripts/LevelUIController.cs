using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUIController : MonoBehaviour {

    public GameObject airJump;
    public GameObject airDash;
    public GameObject shotCooldown;

    private int airJumpNum = 0;
    public int AirJumpNum
    {
        get { return airJumpNum; }
    }

    private int airDashNum = 0;
    public int AirDashNum
    {
        get { return airDashNum;  }
    }

    private int shotCooldownNum = 0;
    public int ShotCooldownNum
    {
        get { return shotCooldownNum;  }
    }

    public void UpdateJumpText(int newValue)
    {
        MasterUpdateText(airJump,
            "Air Jumps Left: " + newValue);
        airJumpNum = newValue;
    }
    public void UpdateDashText(int newValue)
    {
        MasterUpdateText(airDash,
            "Air Dashes Left: " + newValue);
        airDashNum = newValue;
    }
    public void UpdateShotCooldownText(int newValue)
    {
        string textInput; // What will be placed into the textbox.
        if (newValue == 0)
            textInput = "Ready";
        else
            textInput = newValue + " seconds";

        MasterUpdateText(shotCooldown,
            "Gun: " + textInput);
        shotCooldownNum = newValue;
    }

    private void MasterUpdateText(GameObject textUpdated, string message )
    {
        textUpdated.GetComponent<Text>().text = message;
    }
}
