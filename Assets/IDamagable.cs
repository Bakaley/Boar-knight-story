using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //����� �� ����� �������� ������ ������ ������ �� ����, � ��� ���� �� ����� ����� ���� ������
    void sufferDamage(int damage);

    void die(float time);
}
