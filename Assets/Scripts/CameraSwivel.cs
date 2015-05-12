using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraSwivel : MonoBehaviour
{
	private Vector3 camEulerAngles;
	private float camRotation;

	private bool animating;
	private Vector3 startPosition, targetPosition;
	private float startTime, startRotation, targetRotation;

	void Start()
	{
		camEulerAngles = transform.localEulerAngles;
	}

	void Update()
	{
		// add all horizontal inputs
		float x = Input.GetAxis("Horizontal") * 40f;
		x += Input.acceleration.x * 40f;
		x *= Time.deltaTime;
		x += Input.GetAxis("Mouse X") * 20f;

		// add all vertical inputs
		float y = Input.GetAxis("Vertical") * 40f;
		y += Input.acceleration.y * 40f;
		y *= Time.deltaTime;
		y += Input.GetAxis("Mouse Y") * 20f;

		// vertical rotation (looking up&down) is rotation.x
		y = camEulerAngles.x - y;
		//  y should be [0, 360)
		while (y < 0f)
		{
			y += 360f;
		}
		while (y >= 360f)
		{
			y -= 360f;
		}
		if (y < 180f)
		{
			y = Mathf.Clamp(y, 0f, 60f);
		}
		else if (y > 180f)
		{
			y = Mathf.Clamp(y, 300f, 359.99f);
		}
		camEulerAngles.x = y;
		// horizontal rotation (looking right&left) is rotation.y
		x += camEulerAngles.y;
		while (x < 0f)
		{
			x += 360f;
		}
		while (x >= 360f)
		{
			x -= 360f;
		}
		camEulerAngles.y = x; // Mathf.Clamp(rotation.y + x, 215f, 325f);

		// move, if the player pushed a button and if we are not already moving
		if (!animating && Input.GetButtonDown("Fire1"))
		{
			//Vector3 mousePosition = Input.mousePosition;
			//float mouseX = mousePosition.x / Screen.width;
			//float mouseY = 1f - mousePosition.y / Screen.height;
			// alternative: onmousedown on cube
			if (transform.localPosition.z == 9f &&
				camEulerAngles.y > 250f && camEulerAngles.y < 290f)
			{
				startPosition = transform.localPosition;
				targetPosition = transform.localPosition;
				targetPosition.x *= -1;
				startRotation = camRotation;
				targetRotation = (camRotation == 0f ? 180f : 0f);
				startTime = Time.time;
				animating = true;
			}
			else if (camEulerAngles.y < 260f || camEulerAngles.y > 280f)
			{
				startPosition = transform.localPosition;
				targetPosition = transform.localPosition;
				targetPosition.z += (camEulerAngles.y < 270f ? -1f : 1f) *
					(camRotation == 0f ? 1f : -1f) * 1.8f;
				targetRotation = float.NaN;
				startTime = Time.time;
				animating = true;
			}
		}

		// animate towards the next position
		if (animating)
		{
			const float duration = 1.5f;

			transform.localPosition = EaseInOut(
				this.startTime, duration, this.startPosition, this.targetPosition);

			// if targetRotation is NaN, it should not be rotated
			if (!float.IsNaN(targetRotation))
			{
				this.camRotation = EaseInOut(
					this.startTime, duration, this.startRotation, this.targetRotation);
			}

			float elapsedTime = Time.time - startTime;
			if (elapsedTime > duration)
			{
				animating = false;
			}
		}

		Vector3 angles = camEulerAngles;
		angles.y += camRotation;
		transform.localEulerAngles = angles;
	}

	private Vector3 EaseInOut(float startTime, float duration,
		Vector3 startPosition, Vector3 targetPosition)
	{
		float elapsedTime = Time.time - startTime;
		if (elapsedTime >= duration)
		{
			return targetPosition;
		}
		float distance = Vector3.Distance(startPosition, targetPosition);
		float distanceTraveled = Mathf.SmoothStep(0f, distance, elapsedTime / duration);
		Vector3 currentPosition = startPosition +
			((targetPosition - startPosition) * (distanceTraveled / distance));
		return currentPosition;
	}

	private float EaseInOut(float startTime, float duration,
		float startRotation, float targetRotation)
	{
		float elapsedTime = Time.time - startTime;
		if (elapsedTime >= duration)
		{
			return targetRotation;
		}
		float distance = targetRotation - startRotation;
		float distanceTraveled = Mathf.SmoothStep(0f,
			Mathf.Abs(distance), elapsedTime / duration);
		float currentRotation = startRotation +
			((targetRotation - startRotation) * (distanceTraveled / distance));
		return currentRotation;
	}
}