using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class DollyTrackSetting : MonoBehaviour
{
    public List<Transform> trans = new List<Transform>();
    void Start()
    {
        var path = GetComponent<CinemachineSmoothPath>();
        
        if (path != null && trans != null)
        {
            //��������Ʈ ������ ��ǥ�� �����ŭ ReSize
            path.m_Waypoints = new CinemachineSmoothPath.Waypoint[trans.Count];

            for (int i = 0; i < trans.Count; i++)
            {
                //�� �־�� �༮
                CinemachineSmoothPath.Waypoint waypoint = new CinemachineSmoothPath.Waypoint();
                waypoint.position = trans[i].position;//��ǥ ����
                waypoint.roll = 0; //ȸ���� �ʿ��� 0���� �ʱ�ȭ �ؼ� �־���. ������ ���߿� Ÿ�� ���� ������ ������
                path.m_Waypoints[i] = waypoint;

            }
            //InvalidateDistanceCache �޼���� Ʈ�������� �Ÿ��� �ٽ� ����ϵ��� CinemachineSmoothPath�� �����ϴ� ����
            path.InvalidateDistanceCache();
        }

        //List<Vector3> pos = new List<Vector3>();

        //foreach (var t in trans)
        //{
        //    pos.Add(t.position);
        //}
        //m_Waypoints ��� vec3 �迭�̾ƴϿ���....��
        //waypointsPos = pos;
    }


}
