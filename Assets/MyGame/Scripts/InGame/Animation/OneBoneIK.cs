using UnityEngine;

public class OneBoneIK : MonoBehaviour
{
    public Transform bone;     // 回転させたいボーン始点
    public Transform boneEnd;  // 回転させたいボーン終点
    public Transform target;   // ターゲットとするオブジェクト

    void Update()
    {
        var beforeDirection = boneEnd.position - bone.position;
        var afterDirection = target.position - bone.position;
        var delta = Quaternion.FromToRotation(beforeDirection, afterDirection);
        bone.rotation = delta * bone.rotation;
    }
}
