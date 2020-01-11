using UnityEngine;

public class DragTarget : MonoBehaviour
{
	public LayerMask m_DragLayers;

	[Range (0.0f, 100.0f)]
	public float m_Damping = 1.0f;

	[Range (0.0f, 100.0f)]
	public float m_Frequency = 5.0f;

	public float maxForce;
	public float breakForce;

	public bool m_DrawDragLine = true;
	public Color m_Color = Color.cyan;

	public float maxMassMultiplier;

	private TargetJoint2D m_TargetJoint;

	private Rigidbody2D body;

	void Update ()
	{
		// Calculate the world position for the mouse.
		var worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if(Input.touchCount == 1)
		{
			worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		}
		if (Input.GetMouseButtonDown (0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			// Fetch the first collider.
			// NOTE: We could do this for multiple colliders.
			var collider = Physics2D.OverlapPoint (worldPos, m_DragLayers);
			if (!collider)
				return;

			// Fetch the collider body.
			body = collider.attachedRigidbody;
			if (!body)
				return;

			// Add a target joint to the Rigidbody2D GameObject.
			m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D> ();
			m_TargetJoint.dampingRatio = m_Damping;
			m_TargetJoint.frequency = m_Frequency;
			m_TargetJoint.maxForce = maxForce * Mathf.Min(body.mass, maxMassMultiplier);
			m_TargetJoint.breakForce = breakForce * Mathf.Min(body.mass, maxMassMultiplier);

			// Attach the anchor to the local-point where we clicked.
			m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint (worldPos);		
		}
		else 
		if (Input.GetMouseButtonUp (0) || (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)))
		{
			Destroy (m_TargetJoint);
			m_TargetJoint = null;
			return;
		}

		// Update the joint target.
		if (m_TargetJoint)
		{
			m_TargetJoint.maxForce = maxForce * Mathf.Min(body.mass, maxMassMultiplier);
			m_TargetJoint.breakForce = breakForce * Mathf.Min(body.mass, maxMassMultiplier);
			m_TargetJoint.target = worldPos;

			// Draw the line between the target and the joint anchor.
			if (m_DrawDragLine)
				Debug.DrawLine (m_TargetJoint.transform.TransformPoint (m_TargetJoint.anchor), worldPos, m_Color);
		}
	}
}