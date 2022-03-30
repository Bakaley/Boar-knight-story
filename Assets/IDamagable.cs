using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //игрок не может потерять больше одного сердца за удар, а вот урон от бомбы может быть разным
    void sufferDamage(int damage);

    void die(float time);
}
