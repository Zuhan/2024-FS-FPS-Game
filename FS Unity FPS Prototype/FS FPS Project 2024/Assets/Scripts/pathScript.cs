using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathScript : MonoBehaviour
{
    //read me
    //you need a path to attach to the moving platform
    //the path prefab has this script attached
    //attach the path object to the serialize field on the moving platform
    //you can move the path anywhere and change how many points in the path there are
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
