using UnityEngine;
using UnityEngine.Networking;

public class ServerMenu : MonoBehaviour {

    NetworkManager manager;

    void Awake()
	{
        manager = GetComponent<NetworkManager>();
    }


	public void CreateInternet()
	{
        manager.StartMatchMaker();
        manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
    }

    public void FindInternet()
    {

    }

    public void CreateLAN()
    {

    }

    public void JoinLAN()
    {

    }
}
