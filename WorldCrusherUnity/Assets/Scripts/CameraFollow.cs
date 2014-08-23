using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraFollow : MonoBehaviour {

	public float speed = 4.0f;

	public Transform _target = null;

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

		_transform.position = Vector3.Lerp(_transform.position, new Vector3(_target.position.x, _target.position.y, -10.0f), Time.deltaTime * speed);
	}

}