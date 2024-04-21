using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathScript : MonoBehaviour
{
    public Transform getPoint(int index)
    {
        return transform.GetChild(index);
    }
    public int getNext(int index)
    {
        int next = index + 1;
        if(next == transform.childCount)
        {
            next = 0;
        }
        return next;
    }
}
