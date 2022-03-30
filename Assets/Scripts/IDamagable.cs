using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //����� �� ����� �������� ������ ������ ������ �� ����, � ��� ���� �� ����� ����� ���� ������
    void sufferDamage();

    void die(float time);
}
