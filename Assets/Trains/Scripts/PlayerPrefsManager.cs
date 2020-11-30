using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("steel");
        PlayerPrefs.DeleteKey("passengers");
        PlayerPrefs.DeleteKey("mapSize");
        PlayerPrefs.DeleteKey("totalPoints");
    }
}
