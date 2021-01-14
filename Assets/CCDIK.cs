using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDIK : MonoBehaviour
{
    // 関節がrootから遠い方から順に格納されている。
    public Transform[] joints;
    public Transform end;
    public Transform destination;

    void Update()
    {
        //if (Input.GetKeyDown("a"))
        {
            CCD(200);
        }
    }

    void CCD(int maxItr)
    {
        float dist = (destination.position - end.position).magnitude;

        for(int i=0; i < maxItr && dist > Mathf.Epsilon; i++)
        {
            //末端に近いほうから
            for(int k = 0; k < joints.Length; k++) 
            {
                Transform j = joints[k];
                
                Vector3 v1 = (end.position - j.position).normalized;
                Vector3 v2 = (destination.position - j.position).normalized;

                Vector3 axis = Vector3.Cross(v1, v2);
                float dot = Vector3.Dot(v1, v2);
                dot = Mathf.Clamp(dot, -1f, 1f); //|dot|が１を超える場合がある。おそらく誤差
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                j.RotateAround(j.position, axis, angle);

            }
            dist = (destination.position - end.position).magnitude;
        }
    }
}
