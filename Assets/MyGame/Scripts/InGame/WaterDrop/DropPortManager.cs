using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Drop
{
    [SerializeField, Header("栄養剤で成長完了にかかる時間")] public float ReinForceTime = 10f;
    [SerializeField, Header("与える成長量、+Size")] public float ReinForceAmount =  0.05f;
}
public class DropPortManager : SingletonMonoBehavior<DropPortManager>
{
    
    [SerializeField ,Header("発射台X移動")] private Transform _launchPort;
    [SerializeField ,Header("発射台Z移動")] private Transform _launchRail;
    [SerializeField, Header("発射口")] private Transform _dropPort;
    [SerializeField　,Header("垂らす液体")] private GameObject _dropPrefab;
    [SerializeField ,Header("移動スピード")] private float _moveSpeed;
    [SerializeField, Header("移動制限")] private Vector3 _limit;

    [Header("現在の栄養剤によって変えたいデバック用")] 
    [SerializeField] public Drop _drop;
    public void WaterDrop()
    {
        var obj = Instantiate(_dropPrefab , _dropPort.position , Quaternion.identity);
        obj.GetComponent<DropController>().SetDrop(_drop);
    }

    public void MoveLaunchPort(Vector3 input)
    {
        //移動制限
        var portPos = _launchPort.position;
        var railPos = _launchRail.position;
        
        var newPosX = Mathf.Clamp( portPos.x + input.x * _moveSpeed, -_limit.x, _limit.x);
        var newPosZ = Mathf.Clamp( railPos.z + input.z * _moveSpeed, -_limit.z, _limit.z);
        
        _launchPort.position = new Vector3(newPosX , portPos.y , portPos.z);
        _launchRail.position = new Vector3(railPos.x , railPos.y , newPosZ);
    }
}
