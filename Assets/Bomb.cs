using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    
    //������������ ����� �� ������ ����������� ������ �� ������, ���� �� �� ������ �� ������ ���
    [SerializeField]
    GameObject defaultCollder;

    //����� ������ �� ������ � ������������ ������ �������� �������� � ���� ������
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player")) defaultCollder.gameObject.layer = 6;
    }


}
