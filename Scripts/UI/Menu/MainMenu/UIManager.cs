using UnityEngine;
using Utility;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void OnAnyKey()
    {
        BroadcastMessage("OnEnableMenu", 2, SendMessageOptions.DontRequireReceiver);
    }

    public void OnBackKey()
    {
        SendMessage("GoBack", 2, SendMessageOptions.DontRequireReceiver);
    }

}
