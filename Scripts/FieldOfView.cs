using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	public bool isEnabled = true;

	public LayerMask groundMask;
	public LayerMask unitsMask;

	public float viewRadius;

	[Range(0, 360)]
	public float viewAngle;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public float meshDrawResolution;
	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	private void Start() {
		if (isEnabled) {
			viewMesh = new Mesh();
			viewMesh.name = "View Mesh";

			viewMeshFilter.mesh = viewMesh;

			StartCoroutine("findTargetsWithDelay", 0.35f);
		}
	}

	// LateUpdate called after main update
	private void LateUpdate() {
		if (isEnabled) drawFieldOfView();
	}

	// getting view cast, basically raycast :) 
	ViewCastInfo getViewCast(float _globalAngle) {
		Vector3 dirFromAngle = getDirFromAngle(_globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dirFromAngle, out hit, viewRadius, groundMask)) {
			return new ViewCastInfo(true, hit.point, hit.distance, _globalAngle);
		} else {
			return new ViewCastInfo(false, transform.position + dirFromAngle * viewRadius, viewRadius, _globalAngle);
		}
	}

	void drawFieldOfView() {
		int stepCount = Mathf.RoundToInt(viewAngle * meshDrawResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();

		for (int i = 0; i <= stepCount; i++) {
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			//Debug.DrawLine(transform.position, transform.position + getDirFromAngle(angle, true) * viewRadius, Color.red);
			ViewCastInfo viewCastInfo = getViewCast(angle);
			viewPoints.Add(viewCastInfo.hitPoint);
		}
		// field of view draws from sets of triangles (like in OpenGL)
		// count of vertex that we need
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			// transform to local pos
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2) {
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

	// just call findVisibleTargets function with custom delay
	IEnumerator findTargetsWithDelay(float _delay) {
		while(true) {
			yield return new WaitForSeconds(_delay);
			findVisibleTargets();
		}
	}

	// find all visible units in range sector
	public void findVisibleTargets() {
		visibleTargets.Clear();

		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, unitsMask);

		for(int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
		
			// if target in view sector radius
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
				float distToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, groundMask)) {
					visibleTargets.Add(target);
				}
			}
		}
	}
	
	// in unity degree system start from up : 0 and next values moving clockwise
	// so we need set: 90 - x to start from right
	// sin(90 - x) = cos(x)
	public Vector3 getDirFromAngle(float _angleInDegrees, bool _isAngleGlobal) {
		float angleInDegrees = _angleInDegrees;

		if (!_isAngleGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		// Mathf.Deg2Rad is CONSTANT, not method!
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	// contains cast in view sector main information
	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 hitPoint;
		public float distance;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _hitPoint, float _distance, float _angle) {
			hit = _hit;
			hitPoint = _hitPoint;
			distance = _distance;
			angle = _angle;
		}
	}
}
