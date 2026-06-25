using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FPS.Gameplay
{
    public class PinnedSpawnHeight : MonoBehaviour
    {
        public float WorldY = 2f;
        public bool DisableNavMeshAgent = true;
        public bool PinContinuously = true;
        public int StartupPinFrames = 20;

        NavMeshAgent m_NavMeshAgent;

        void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            PinHeight();
        }

        IEnumerator Start()
        {
            yield return null;

            if (DisableNavMeshAgent && m_NavMeshAgent)
                m_NavMeshAgent.enabled = false;

            for (int i = 0; i < StartupPinFrames; i++)
            {
                PinHeight();
                yield return null;
            }
        }

        void LateUpdate()
        {
            if (PinContinuously)
                PinHeight();
        }

        void PinHeight()
        {
            Vector3 position = transform.position;
            if (Mathf.Abs(position.y - WorldY) <= 0.001f)
                return;

            position.y = WorldY;
            transform.position = position;
        }
    }
}
