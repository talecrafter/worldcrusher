using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NodeCamera : MonoBehaviour {

	public float speed = 4.0f;

	private Vector3? _target = null;
	private Transform _transform;

	void Awake()
	{
		_transform = transform;
	}

    void Update() {
		if (_target != null)
		{
			FollowTarget();
		}
    }

	private void FollowTarget()
	{
		_transform.position = Vector3.Lerp(_transform.position, _target.Value, Time.deltaTime * speed);
	}

	public void Show(NodeDisplay node)
	{
		if (_target == null)
		{
			_target = new Vector3(node.transform.position.x, node.transform.position.y, -10.0f);
			_transform.position = _target.Value;
		}
		else
		{
			_target = new Vector3(node.transform.position.x, node.transform.position.y, -10.0f);
		}
	}

}