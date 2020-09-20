using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// シネマシーン反転script
/// </summary>
public class CameraCon : MonoBehaviour
{
	void Start()
	{
		Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
	}

	public float GetAxisCustom(string axisName)
	{
		if (axisName == "Mouse X")
		{
			return Input.GetAxis(axisName) * -1f;
		}
		else if (axisName == "Mouse Y")
		{
			return Input.GetAxis(axisName) * -1f;
		}

		return 0;
	}
}