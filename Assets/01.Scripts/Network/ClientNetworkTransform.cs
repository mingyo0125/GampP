using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative() // ȣ��Ʈ�� �ƴ� Ŭ���̾�Ʈ�鵵 ������ ����ȭ ���ֱ� ���ؼ�
    {
        //���������� false�� ����
        return false;
    }
}
