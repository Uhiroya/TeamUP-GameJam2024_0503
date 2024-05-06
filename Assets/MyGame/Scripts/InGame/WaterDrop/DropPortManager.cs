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
    public DropPortManagerStatus(DropPortManagerStatus status)
    {
        SupplementCoolTime = status.SupplementCoolTime;
        SupplementSize = status.SupplementSize;
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
    [SerializeField, Header("移動制限プラス方向")] private Vector3 _limitPlusVector;
    [SerializeField, Header("移動制限マイナス方向")] private Vector3 _limitMinusVector;
    
    [SerializeField , Header("デフォルトのステータス")] public DropPortManagerStatus _defaultStatus;
    
    [Header("進行によって変更するデータ")]
    [Header("現在のステータス")]
    public DropPortManagerStatus CurrentStatus;
    
    protected override void OnAwake()
    {
        CurrentStatus = new DropPortManagerStatus(_defaultStatus);
    }

    private float _coolTime;
    private void Update()
    {
        if (_coolTime < CurrentStatus.SupplementCoolTime)
        {
            _coolTime += Time.deltaTime;
            if (_coolTime > CurrentStatus.SupplementCoolTime)
            {
                AudioManager.Instance.PlaySe(SoundEffectType.ボタン1);
                ButtonAnimation.Instance.OnButtonReset();
            }
        }
    }

    public void WaterDrop()
    {
        if(_coolTime < CurrentStatus.SupplementCoolTime) return;
        AudioManager.Instance.PlaySe(SoundEffectType.ボタン2);
        ButtonAnimation.Instance.OnButtonClick();
        var obj = Instantiate(_dropPrefab , _dropPort.position , Quaternion.identity);
        obj.transform.localScale *= CurrentStatus.SupplementSize; 
        _coolTime = 0f;
    }

    public void MoveLaunchPort(Vector3 input)
    {
        //移動制限
        var portPos = _launchPort.position;
        var railPos = _launchRail.position;
        
        var newPosX = Mathf.Clamp( portPos.x + input.x * _moveSpeed, _limitMinusVector.x, _limitPlusVector.x);
        var newPosZ = Mathf.Clamp( railPos.z + input.z * _moveSpeed, _limitMinusVector.z, _limitPlusVector.z);
        
        _launchPort.position = new Vector3(newPosX , portPos.y , portPos.z);
        _launchRail.position = new Vector3(railPos.x , railPos.y , newPosZ);
    }
}
