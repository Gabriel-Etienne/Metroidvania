using System;
using UnityEngine;

public class CamLimitSetter : MonoBehaviour
{
    [SerializeField] private bool _drawGizmo = true;
    
    [SerializeField] private Transform _xMin, _xMax, _yMin, _yMax;
    
    private Vector2 _xVec => new Vector2(_xMin.position.x, _xMax.position.x);
    private Vector2 _yVec => new Vector2(_yMin.position.y, _yMax.position.y);
    
    private Vector2 _actualX = Vector2.zero;
    private Vector2 _actualY = Vector2.zero;

    private void OnEnable()
    {
        CameraManager.UpdateMinMaxCamPosEvent += GetActualValue;
    }
    private void OnDisable()
    {
        CameraManager.UpdateMinMaxCamPosEvent -= GetActualValue;
    }

    void GetActualValue(Vector2 newX,Vector2 newY)
    {
        _actualX =  newX;
        _actualY = newY;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (_actualX != _xVec || _actualY != _yVec)
            {
                CameraManager.UpdateMinMaxCamPosEvent?.Invoke(new Vector2(_xMin.position.x, _xMax.position.x), new Vector2(_yMin.position.y, _yMax.position.y));
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!_drawGizmo) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(
            new Vector3(
                (_xMin.position.x + _xMax.position.x) / 2,
                (_yMin.position.y + _yMax.position.y) / 2,
                0
            ),
            new Vector3(
                _xMin.position.x - _xMax.position.x,
                _yMin.position.y - _yMax.position.y, 
                0.25F
            )
        );
    }
}
