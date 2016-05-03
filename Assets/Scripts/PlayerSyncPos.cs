using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPos : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncScale;

    void FixedUpdate()
    {
        LerpPosition();
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            transform.localScale = syncScale;
        }
    }

    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdSendPosition(transform.localScale);
        }
    }

    [Command]
    void CmdSendPosition(Vector3 client_scale)
    {
        syncScale = client_scale;
    }

}