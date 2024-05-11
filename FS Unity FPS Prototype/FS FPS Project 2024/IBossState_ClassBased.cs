using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBossState
{
    IBossState DoState(BossSearch boss);
}
