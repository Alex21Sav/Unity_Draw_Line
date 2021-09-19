using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesDrawer : MonoBehaviour
{
	private int _cantDrawOverLayerIndex;
	private Line _currentLine;
	private Camera _camera;

	[SerializeField] private GameObject _linePrefab;
	[SerializeField] private LayerMask _cantDrawOverLayer;

	[Space(30f)]
	[SerializeField] private Gradient _lineColor;
	[SerializeField] private float _linePointsMinDistance;
	[SerializeField] private float _lineWidth;

	private void Start()
	{
		_camera = Camera.main;
		_cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
			BeginDraw();
		}		

		if (_currentLine != null)
        {
			Draw();
		}	

		if (Input.GetMouseButtonUp(0))
        {
			EndDraw();
		}			
	}
	private void BeginDraw()
	{
		_currentLine = Instantiate(_linePrefab, this.transform).GetComponent<Line>();
				
		_currentLine.UsePhysics(false);
		_currentLine.SetLineColor(_lineColor);
		_currentLine.SetPointsMinDistance(_linePointsMinDistance);
		_currentLine.SetLineWidth(_lineWidth);
	}
	private void Draw()
	{
		Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.CircleCast(mousePosition, _lineWidth / 3f, Vector2.zero, 1f, _cantDrawOverLayer);

		if (hit)
        {
			EndDraw();
		}
        else
		{
			_currentLine.AddPoint(mousePosition);
		}			
	}
	private void EndDraw()
	{
		if (_currentLine != null)
		{
			if (_currentLine.PointsCount < 2)
			{
				Destroy(_currentLine.gameObject);
			}
			else
			{
				_currentLine.gameObject.layer = _cantDrawOverLayerIndex;

				_currentLine.UsePhysics(true);

				_currentLine = null;
			}
		}
	}
}
