using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTransform : MonoBehaviour
{
	[SerializeField]
	Transform focus;

	[SerializeField]
	float speed = 0.314f;

	float angle = 0f;

	protected virtual void Update()
	{
		if (focus == null) return;
		var rot = Quaternion.LookRotation(focus.position - transform.position, transform.up);
		transform.RotateAround(focus.position, Vector3.up, speed * Time.deltaTime);
		transform.LookAt(focus);
	}
}