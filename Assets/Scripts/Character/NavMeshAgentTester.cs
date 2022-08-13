using UnityEngine;
using UnityEngine.AI;

namespace LeMinhHuy.Character
{
	public class NavMeshAgentTester : MonoBehaviour
	{
		[SerializeField] LayerMask groundLayer;

		Camera cam;
		NavMeshAgent agent;

		void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
		}
		void Start()
		{
			cam = Camera.main;
		}
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				// var mousePoint = Input.mousePosition;
				var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, groundLayer))
				{
					agent.SetDestination(hit.point);
				}
			}
		}
	}
}
