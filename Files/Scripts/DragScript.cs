using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour {
    [SerializeField] private LineRenderer _dragLine;
    [SerializeField] private Transform _playerTransform;

    [Header("Line Data")]
    [SerializeField] private float _lineExtenderMultiplyer;
    [SerializeField] private float _maxLineDrag;

    [Header("Strength Indicator")]
    [GradientUsage(false)]
    [SerializeField] private Gradient _dragStrengthIndicator;

    private Vector2 _firstClickPosition = Vector2.zero;
    private Vector2 _mousePos = Vector2.zero;
    private Vector2 _defaultPlayerPosition = Vector2.zero;
    private void Start() {
        _defaultPlayerPosition = _playerTransform.position;
    }

    void Update() {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)) {
            OnFirstClick();
        }
        if(Input.GetMouseButton(0) && _dragLine.enabled) {
            OnCurrentClick();
        }

        if(Input.GetMouseButtonUp(0)) {
            OnLastClick();
        }
    }

    void OnFirstClick() {
        bool _clickedOnPlayer = false;
        foreach(RaycastHit2D rh in Physics2D.RaycastAll(_mousePos, _mousePos, 100)) {
            if(rh.collider.CompareTag("Player")) {
                _clickedOnPlayer = true;
                _firstClickPosition = rh.collider.transform.position;
            }
        }
        if(!_clickedOnPlayer) {
            return;
        }

        _dragLine.enabled = true;
    }

    void OnCurrentClick() {
        // clamp mousepos to maxDrag
        Vector2 clampVec = _firstClickPosition - _mousePos;
        clampVec = Vector2.ClampMagnitude(clampVec, _maxLineDrag);
        Vector2 mouseClamped = _firstClickPosition - clampVec;

        // create extention/offset towards other side
        float xDiff = _firstClickPosition.x - mouseClamped.x;
        float yDiff = _firstClickPosition.y - mouseClamped.y;
        xDiff *= _lineExtenderMultiplyer;
        yDiff *= _lineExtenderMultiplyer;

        // set positions to LR
        Vector3[] positions = new Vector3[2];
        positions[0] = mouseClamped;
        positions[1] = new Vector3(_firstClickPosition.x + xDiff, _firstClickPosition.y + yDiff, 0);
        _dragLine.SetPositions(positions);

        // set player position
        _playerTransform.position = mouseClamped;

        // set color
        positions[0] = mouseClamped;
        // without diff !
        positions[1] = new Vector3(_firstClickPosition.x, _firstClickPosition.y, 0); ;

        // get the distance between start and end
        float distance = Vector2.Distance(positions[0], positions[1]);
        float res = distance / _maxLineDrag;

        // set colors depending on the position in the color gradient
        _dragLine.startColor = _dragStrengthIndicator.Evaluate(res);
        _dragLine.endColor = _dragStrengthIndicator.Evaluate(res);
    }


    void OnLastClick() {
        _dragLine.enabled = false;

        // no need for this, only resets player to default position
        _playerTransform.position = _defaultPlayerPosition;
        // shoot mechanics 
    }
}