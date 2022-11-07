using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public CharacterController2D Left;
    public CharacterController2D Right;
	[Space]
	public float Speed = 0.25f;

	private bool tapLeft;
	private bool tapRight;
	private DeviceType deviceType;


	void Start()
    {
		Application.targetFrameRate = 80;
		deviceType = SystemInfo.deviceType;
    }


	void Update()
    {
		if (deviceType == DeviceType.Handheld)
		{
			tapLeft = tapRight = false;

			if (Input.touches.Length != 0)
			{
				// "touches[0]" - first finger to touch
				CheckTouchSide(Input.touches[0]);

				if (Input.touches.Length > 1)
				{
					// "touches[1]" - second finger to touch
					CheckTouchSide(Input.touches[1]);
				}
			}
		}
		else if (deviceType == DeviceType.Desktop)
		{
			tapLeft = Input.GetKey(KeyCode.A);
			tapRight = Input.GetKey(KeyCode.D);
		}


		// Process the inputs
		if (tapLeft)
		{
			Left.Move(Speed, false, false);
		}
		if (tapRight)
		{
			Right.Move(Speed, false, false);
		}
	}

	private void CheckTouchSide(Touch touch)
	{
		if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
		{
			if (touch.position.x < Screen.width / 2)
			{
				tapLeft = true;
			}
			else if (touch.position.x > Screen.width / 2)
			{
				tapRight = true;
			}
		}
	}
}
