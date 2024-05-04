using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public GameObject target;
    public Vector2 rotationSpeed;
    public bool reverse;

    [SerializeField]  private float _threshold;
    private Vector2 lastMousePosition;
    private Vector2 newAngle = new Vector2(0, 0);

    // ゲーム実行中の繰り返し処理
    void Update()
    {
        // 左クリックした時
        if (Input.GetMouseButtonDown(1))
        {
            newAngle = target.transform.localEulerAngles;
            lastMousePosition = Input.mousePosition;
        }
        // 左ドラッグしている間
        else if (Input.GetMouseButton(1))
        {
            if (((Vector2)Input.mousePosition - lastMousePosition).magnitude < _threshold) return;
            if (!reverse)
            {
                
                newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.x;
                newAngle.y -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.y;

                
                target.transform.localEulerAngles = newAngle;
                lastMousePosition = Input.mousePosition;
            }
            else if (reverse)
            {
                newAngle.y -= (Input.mousePosition.x - lastMousePosition.x) * rotationSpeed.y;
                newAngle.x -= (lastMousePosition.y - Input.mousePosition.y) * rotationSpeed.x;
                target.transform.localEulerAngles = newAngle;
                lastMousePosition = Input.mousePosition;
            }
        }
    }

    // マウスドラッグ方向と視点回転方向を反転する処理
    public void DirectionChange()
    {
        // 判定フラグ変数"reverse"が"false"であれば
        if (!reverse)
        {
            // 判定フラグ変数"reverse"に"true"を代入
            reverse = true;
        }
        // でなければ（判定フラグ変数"reverse"が"true"であれば）
        else
        {
            // 判定フラグ変数"reverse"に"false"を代入
            reverse = false;
        }
    }
}
