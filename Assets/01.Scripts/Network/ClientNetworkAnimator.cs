using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative() //ClientNetworkTransform ���� ����
    {
        //���������� false�� ����
        return false;
    }
}
