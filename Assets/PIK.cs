using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIK : MonoBehaviour
{
    
    public Transform[] joints;
    public Transform end;
    public Transform destination;
    float[] constraints; //関節間の距離
    float constraintSum; //距離の総和(稼働半径)


    private void Start()
    {
        
        constraints = new float[joints.Length];
        constraints[0] = Vector3.Magnitude(end.position - joints[0].position);
        constraintSum = constraints[0];
        for (int i = 1; i < joints.Length; i++)
        {
            constraints[i] = Vector3.Magnitude(joints[i - 1].position - joints[i].position);
            constraintSum += constraints[i];
        }
    }
    void Update()
    {
        //if (Input.GetKeyDown("b"))
        {
            //Debug.Log("pik in");
            ParticleIK(200);
        }

    }

    
    void ParticleIK(int maxItr)
    {


        for (int i = 0; i < maxItr; i++)
        {
            end.position += destination.position - end.position;
            Vector3 dv;

            // 稼働範囲外にいかないように。
            Vector3 root2end = end.position - joints[joints.Length - 1].position;
            if (Vector3.Magnitude(root2end) > constraintSum)
            {
                end.position = joints[joints.Length - 1].position + root2end.normalized * constraintSum; 
            }

            dv = end.position - joints[0].position;
            dv *= 0.5f * (1 - constraints[0] / dv.magnitude);
            end.position -= dv;
            joints[0].position += dv;

            //末端から
            for (int k = 0; k < joints.Length - 2; k++)
            {
                dv = joints[k+1].position - joints[k].position;
                dv *= 0.5f * (1 - constraints[k]/dv.magnitude);
                joints[k].position += dv;
                joints[k+1].position -= dv;

            }
            // rootは動かさない
            dv = joints[joints.Length-2].position - joints[joints.Length - 1].position;
            dv *= 1 - constraints[joints.Length - 1]/dv.magnitude;
            joints[joints.Length - 2].position -= dv;

        }
        // 階層構造だとうまくいかない。
        for (int i = joints.Length - 1; i > 0; i--)
        {
            joints[i].LookAt(joints[i - 1].position);
        }
        joints[0].LookAt(end.position);

    }
}
