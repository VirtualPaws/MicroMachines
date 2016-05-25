using UnityEngine;
using System.Collections;

public class SceneLoadChecker : MonoBehaviour {
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            //GetComponent<ClientSidePlayer>().CmdSpawn();
            GetComponent<ClientSidePlayer>().OnLoad();
        }
    }
}
