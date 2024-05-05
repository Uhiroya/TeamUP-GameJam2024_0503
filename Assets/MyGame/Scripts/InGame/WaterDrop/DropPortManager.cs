using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropPortManagerStatus
{
    [SerializeField, Header("栄養剤クールタイム")] public float SupplementCoolTime;
    [SerializeField, Header("栄養を吸収する時間")] public float ReinForceTime = 10f;
    [SerializeField, Header("与える成長量、+Size")] public float ReinForceAmount =  0.05f;
    [SerializeField, Header("栄養剤の大きさ(複数ヒット可能にする)")] public float SupplementSize;
    [SerializeField, Header("栄養剤の個数（シャワーみたいにする？）")] public float SupplementCount;
    
    

    public DropPortManagerStatus(DropPortManagerStatus status)
    {
        SupplementCoolTime = status.SupplementCoolTime;
        SupplementSize = status.SupplementSize;
        SupplementCount = status.SupplementCount;
        ReinForceTime = status.ReinForceTime;
        ReinForceAmount = status.ReinForceAmount;
    }
}
public class DropPortManager : SingletonMonoBehavior<DropPortManager>
{
    
    [SerializeField ,Header("発射台X移動")] private Transform _launchPort;
    [SerializeField ,Header("発射台Z移動")] private Transform _launchRail;
    [SerializeField, Header("発射口")] private Transform _dropPort;
    [SerializeField　,Header("垂らす液体")] private GameObject _dropPrefab;
    [SerializeField ,Header("移動スピード")] private float _moveSpeed;
    [SerializeField, Header("移動制限")] private Vector3 _limit;
    
    [SerializeField , Header("デフォルトのステータス")] public DropPortManagerStatus _defaultStatus;
    
    [Header("進行によって変更するデータ")]
    [Header("現在のステータス")]
    public DropPortManagerStatus CurrentStatus;
    
    protected override void OnAwake()
    {
        CurrentStatus = new DropPortManagerStatus(_defaultStatus);
    }
    
    public void WaterDrop()
    {
        var obj = Instantiate(_dropPrefab , _dropPort.position , Quaternion.identity);
        obj.GetComponent<DropController>().SetDrop(_defaultStatus);
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
