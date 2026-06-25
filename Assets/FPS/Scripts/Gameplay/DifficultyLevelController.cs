using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class DifficultyLevelController : MonoBehaviour
    {
        const float k_TurretSurfaceY = 2f;

        [Serializable]
        public struct DifficultySetup
        {
            public GameDifficulty Difficulty;
            public GameObject Root;
            public int HoverbotCount;
            public int TurretCount;
            public int HealthPickupCount;
            public float EnemyHealthMultiplier;
            public float EnemyFireIntervalMultiplier;
        }

        [Header("Prefabs")]
        public GameObject HoverbotPrefab;
        public GameObject TurretPrefab;
        public GameObject HealthPickupPrefab;

        [Header("Visual Refresh")]
        public GameObject[] DecorationPrefabs;
        public Material[] DecorationMaterials;
        public bool SpawnNeonLights = true;

        [Header("Difficulty")]
        public DifficultySetup[] DifficultySetups;

        static readonly Vector3[] k_HoverbotPositions =
        {
            new Vector3(6.5f, 0.5f, 0f),
            new Vector3(16f, 0.5f, 9f),
            new Vector3(26f, 0.5f, -8f),
            new Vector3(39f, 0.5f, 5f),
            new Vector3(50f, 0.5f, -6f),
            new Vector3(57f, 0.5f, 8f),
            new Vector3(30f, 0.5f, 11.5f),
            new Vector3(45f, 0.5f, -11.5f),
            new Vector3(63f, 0.5f, -4f),
        };

        static readonly Vector3[] k_TurretPositions =
        {
            new Vector3(54.5f, k_TurretSurfaceY, 0f),
            new Vector3(34f, k_TurretSurfaceY, 9f),
            new Vector3(44f, k_TurretSurfaceY, -8f),
            new Vector3(22f, k_TurretSurfaceY, -7f),
        };

        static readonly Vector3[] k_HealthPositions =
        {
            new Vector3(12f, 0.5f, -5f),
            new Vector3(28f, 0.5f, 10f),
            new Vector3(47f, 0.5f, -10f),
            new Vector3(59f, 0.5f, 6f),
        };

        static readonly Vector3[] k_DecorationPositions =
        {
            new Vector3(10f, 0f, 12f),
            new Vector3(24f, 0f, -14f),
            new Vector3(38f, 0f, 14f),
            new Vector3(51f, 0f, -16f),
            new Vector3(60f, 0f, 12f),
        };

        void Awake()
        {
            GameDifficulty selectedDifficulty = DifficultySettings.SelectedDifficulty;

            foreach (DifficultySetup setup in DifficultySetups)
            {
                if (setup.Root)
                    setup.Root.SetActive(setup.Difficulty == selectedDifficulty);

                if (setup.Difficulty != selectedDifficulty)
                    continue;

                DisableOriginalSceneObjects();
                SpawnDifficulty(setup);
                ApplyObjectiveTarget(setup.HoverbotCount + setup.TurretCount);
                SpawnDecorations();
                return;
            }
        }

        void SpawnDifficulty(DifficultySetup setup)
        {
            GameObject runtimeRoot = new GameObject("RuntimeDifficulty_" + setup.Difficulty);

            for (int i = 0; i < setup.HoverbotCount && i < k_HoverbotPositions.Length; i++)
                Spawn(HoverbotPrefab, runtimeRoot.transform, "HoverBot_" + (i + 1), k_HoverbotPositions[i], Quaternion.Euler(0f, 180f, 0f), Vector3.one);

            for (int i = 0; i < setup.TurretCount && i < k_TurretPositions.Length; i++)
            {
                GameObject turret = Spawn(TurretPrefab, runtimeRoot.transform, "Turret_" + (i + 1),
                    k_TurretPositions[i], Quaternion.identity, Vector3.one * 4f);
                PinTurretToArenaSurface(turret);
            }

            for (int i = 0; i < setup.HealthPickupCount && i < k_HealthPositions.Length; i++)
                Spawn(HealthPickupPrefab, runtimeRoot.transform, "HealthPickup_" + (i + 1), k_HealthPositions[i], Quaternion.identity, Vector3.one);

            ApplyEnemyTuning(runtimeRoot, setup);
        }

        static void ApplyEnemyTuning(GameObject root, DifficultySetup setup)
        {
            float healthMultiplier = Mathf.Max(0.1f, setup.EnemyHealthMultiplier);
            float fireIntervalMultiplier = Mathf.Max(0.1f, setup.EnemyFireIntervalMultiplier);

            foreach (Health health in root.GetComponentsInChildren<Health>(true))
            {
                health.MaxHealth *= healthMultiplier;
            }

            foreach (WeaponController weapon in root.GetComponentsInChildren<WeaponController>(true))
            {
                weapon.DelayBetweenShots = Mathf.Max(0.02f, weapon.DelayBetweenShots * fireIntervalMultiplier);
            }
        }

        void SpawnDecorations()
        {
            GameObject runtimeRoot = new GameObject("RuntimeNeonSpaceArena");

            for (int i = 0; i < DecorationPrefabs.Length && i < k_DecorationPositions.Length; i++)
            {
                GameObject decoration = Spawn(DecorationPrefabs[i], runtimeRoot.transform, "SpaceProp_" + (i + 1),
                    k_DecorationPositions[i], Quaternion.Euler(0f, i * 37f, 0f), Vector3.one * 2.2f);

                if (DecorationMaterials.Length > 0)
                    ApplyMaterial(decoration, DecorationMaterials[i % DecorationMaterials.Length]);
            }

            if (SpawnNeonLights)
            {
                AddPointLight(runtimeRoot.transform, "NeonBlueLight", new Vector3(20f, 8f, 5f), new Color(0.1f, 0.65f, 1f), 45f, 3f);
                AddPointLight(runtimeRoot.transform, "NeonGreenLight", new Vector3(48f, 8f, -4f), new Color(0.1f, 1f, 0.45f), 45f, 2.5f);
            }
        }

        static GameObject Spawn(GameObject prefab, Transform parent, string objectName, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (!prefab)
                return null;

            GameObject instance = Instantiate(prefab, position, rotation, parent);
            instance.name = objectName;
            instance.transform.localScale = scale;
            return instance;
        }

        static void PinTurretToArenaSurface(GameObject turret)
        {
            if (!turret)
                return;

            PinnedSpawnHeight pinnedHeight = turret.GetComponent<PinnedSpawnHeight>();
            if (!pinnedHeight)
                pinnedHeight = turret.AddComponent<PinnedSpawnHeight>();

            pinnedHeight.WorldY = k_TurretSurfaceY;
            pinnedHeight.DisableNavMeshAgent = true;
            pinnedHeight.PinContinuously = true;
            pinnedHeight.StartupPinFrames = 45;
        }

        static void ApplyMaterial(GameObject instance, Material material)
        {
            if (!instance || !material)
                return;

            foreach (Renderer renderer in instance.GetComponentsInChildren<Renderer>(true))
                renderer.sharedMaterial = material;
        }

        static void AddPointLight(Transform parent, string objectName, Vector3 position, Color color, float range, float intensity)
        {
            GameObject lightObject = new GameObject(objectName);
            lightObject.transform.SetParent(parent);
            lightObject.transform.position = position;

            Light pointLight = lightObject.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.color = color;
            pointLight.range = range;
            pointLight.intensity = intensity;
        }

        static void ApplyObjectiveTarget(int enemyCount)
        {
            ObjectiveKillEnemies objective = FindAnyObjectByType<ObjectiveKillEnemies>();
            if (!objective)
                return;

            objective.KillsToCompleteObjective = enemyCount;
            objective.Description = "0 / " + enemyCount;
            objective.NotificationEnemiesRemainingThreshold = Mathf.Min(3, enemyCount);
        }

        static void DisableOriginalSceneObjects()
        {
            foreach (GameObject sceneObject in FindObjectsByType<GameObject>(FindObjectsInactive.Exclude))
            {
                if (sceneObject.transform.parent)
                    continue;

                if (sceneObject.name == "Enemy_HoverBot" || sceneObject.name == "Enemy_Turret" || sceneObject.name == "Pickup_Health")
                    sceneObject.SetActive(false);
            }
        }
    }
}
