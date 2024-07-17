using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative() // 호스트가 아닌 클라이언트들도 움직임 동기화 해주기 위해서
    {
        //서버인증을 false로 놓고
        return false;
    }
}
