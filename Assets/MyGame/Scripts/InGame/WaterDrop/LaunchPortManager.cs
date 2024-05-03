using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPortManager : MonoBehaviour
{
    
    [SerializeField ,Header("発射台")] private Transform _launchPort;
    [SerializeField, Header("発射口")] private Transform _dropPort;
    [SerializeField　,Header("垂らす液体")] private GameObject _dropPrefab;
    [SerializeField ,Header("移動スピード")] private float _moveSpeed;
    [SerializeField, Header("移動制限")] private Vector3 _limit;
    public void WaterDrop()
    {
        Instantiate(_dropPrefab , _dropPort.position , Quaternion.identity);
    }

    public void MoveLaunchPort(Vector3 input)
    {
        //移動制限
        var position = _launchPort.position;
        var newPosition = position + input * _moveSpeed;
        var newPosX = Mathf.Clamp(newPosition.x, -_limit.x, _limit.x);
        var newPosZ = Mathf.Clamp(newPosition.z, -_limit.z, _limit.z);
        position = new Vector3(newPosX, position.y, newPosZ);
        
        _launchPort.position = position;
    }
}
