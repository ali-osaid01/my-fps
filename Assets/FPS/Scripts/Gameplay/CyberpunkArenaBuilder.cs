using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class CyberpunkArenaBuilder : MonoBehaviour
    {
        [Header("Build")]
        public bool BuildOnAwake = true;
        public bool ReplaceOriginalLevelGeometry = true;

        Material m_FloorMaterial;
        Material m_WallMaterial;
        Material m_DarkTrimMaterial;
        Material m_CyanNeonMaterial;
        Material m_MagentaNeonMaterial;
        Material m_AmberNeonMaterial;
        Material m_GlassMaterial;

        void Awake()
        {
            if (!BuildOnAwake)
                return;

            BuildArena();
        }

        public void BuildArena()
        {
            if (ReplaceOriginalLevelGeometry)
                DisableOriginalLevelGeometry();

            CreateMaterials();
            TuneAtmosphere();

            Transform root = new GameObject("RuntimeCyberpunkArena").transform;
            BuildFloor(root);
            BuildWalls(root);
            BuildCeilingFrames(root);
            BuildCoverAndPlatforms(root);
            BuildSignsAndProps(root);
            BuildLighting(root);
        }

        void CreateMaterials()
        {
            m_FloorMaterial = CreateMaterial("CyberFloor", new Color(0.055f, 0.065f, 0.085f), 0f);
            m_WallMaterial = CreateMaterial("CyberWall", new Color(0.065f, 0.075f, 0.105f), 0f);
            m_DarkTrimMaterial = CreateMaterial("CyberTrim", new Color(0.018f, 0.022f, 0.032f), 0f);
            m_CyanNeonMaterial = CreateMaterial("NeonCyan", new Color(0.08f, 0.95f, 1f), 3.4f);
            m_MagentaNeonMaterial = CreateMaterial("NeonMagenta", new Color(1f, 0.08f, 0.78f), 3.4f);
            m_AmberNeonMaterial = CreateMaterial("NeonAmber", new Color(1f, 0.62f, 0.08f), 2.8f);
            m_GlassMaterial = CreateMaterial("CyberGlass", new Color(0.08f, 0.24f, 0.32f, 0.6f), 0.65f);
        }

        static Material CreateMaterial(string materialName, Color color, float emissionIntensity)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (!shader)
                shader = Shader.Find("Standard");

            Material material = new Material(shader) { name = materialName };
            material.color = color;

            if (emissionIntensity > 0f)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * emissionIntensity);
            }

            return material;
        }

        static void TuneAtmosphere()
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.006f;
            RenderSettings.fogColor = new Color(0.035f, 0.04f, 0.065f);
            RenderSettings.ambientLight = new Color(0.08f, 0.095f, 0.13f);
            RenderSettings.ambientIntensity = 0.65f;

            foreach (Light sceneLight in FindObjectsByType<Light>(FindObjectsInactive.Exclude))
            {
                if (sceneLight.type != LightType.Directional)
                    continue;

                sceneLight.color = new Color(0.55f, 0.7f, 1f);
                sceneLight.intensity = 0.8f;
            }
        }

        void BuildFloor(Transform root)
        {
            for (int i = 0; i < 11; i++)
            {
                float x = -55f + i * 12f;
                AddBlock(root, "FloorPanel_" + (i + 1), new Vector3(x, -0.32f, 0f), new Vector3(11.5f, 0.25f, 32f), m_FloorMaterial, true);
                AddBlock(root, "FloorCyanLine_" + (i + 1), new Vector3(x, -0.17f, -7.5f), new Vector3(10.5f, 0.04f, 0.12f), m_CyanNeonMaterial, false);
                AddBlock(root, "FloorMagentaLine_" + (i + 1), new Vector3(x, -0.165f, 7.5f), new Vector3(10.5f, 0.04f, 0.12f), m_MagentaNeonMaterial, false);
                AddBlock(root, "FloorCenterTrim_" + (i + 1), new Vector3(x, -0.18f, 0f), new Vector3(10.5f, 0.04f, 0.08f), i % 2 == 0 ? m_CyanNeonMaterial : m_MagentaNeonMaterial, false);
            }

            AddBlock(root, "SpawnSafetyDeck", new Vector3(-28.5f, -0.34f, -7.25f), new Vector3(18f, 0.3f, 14f), m_FloorMaterial, true);
            AddBlock(root, "SpawnGuideLine", new Vector3(-20f, -0.14f, -7.25f), new Vector3(28f, 0.05f, 0.16f), m_CyanNeonMaterial, false);
        }

        void BuildWalls(Transform root)
        {
            AddBlock(root, "NorthWall", new Vector3(5f, 2.6f, 16f), new Vector3(134f, 5.4f, 0.8f), m_WallMaterial, true);
            AddBlock(root, "SouthWall", new Vector3(5f, 2.6f, -16f), new Vector3(134f, 5.4f, 0.8f), m_WallMaterial, true);
            AddBlock(root, "WestWall", new Vector3(-62f, 2.6f, 0f), new Vector3(0.8f, 5.4f, 32f), m_WallMaterial, true);
            AddBlock(root, "EastWall", new Vector3(72f, 2.6f, 0f), new Vector3(0.8f, 5.4f, 32f), m_WallMaterial, true);

            for (int i = 0; i < 18; i++)
            {
                float x = -61f + i * 8f;
                Material stripMaterial = i % 2 == 0 ? m_CyanNeonMaterial : m_MagentaNeonMaterial;
                AddBlock(root, "NorthNeonRib_" + i, new Vector3(x, 2.8f, 15.54f), new Vector3(0.25f, 4.2f, 0.14f), stripMaterial, false);
                AddBlock(root, "SouthNeonRib_" + i, new Vector3(x, 2.8f, -15.54f), new Vector3(0.25f, 4.2f, 0.14f), stripMaterial, false);
                AddBlock(root, "NorthPanel_" + i, new Vector3(x + 3.5f, 3.7f, 15.48f), new Vector3(3.6f, 1.2f, 0.12f), m_GlassMaterial, false);
                AddBlock(root, "SouthPanel_" + i, new Vector3(x + 3.5f, 3.7f, -15.48f), new Vector3(3.6f, 1.2f, 0.12f), m_GlassMaterial, false);
            }

            for (int i = 0; i < 5; i++)
            {
                float z = -12f + i * 6f;
                AddBlock(root, "WestNeonRib_" + i, new Vector3(-1.54f, 2.8f, z), new Vector3(0.14f, 4.2f, 0.25f), i % 2 == 0 ? m_MagentaNeonMaterial : m_CyanNeonMaterial, false);
                AddBlock(root, "EastNeonRib_" + i, new Vector3(71.54f, 2.8f, z), new Vector3(0.14f, 4.2f, 0.25f), i % 2 == 0 ? m_MagentaNeonMaterial : m_CyanNeonMaterial, false);
            }
        }

        void BuildCeilingFrames(Transform root)
        {
            for (int i = 0; i < 12; i++)
            {
                float x = -61f + i * 12f;
                AddBlock(root, "CeilingBeam_" + i, new Vector3(x, 5.4f, 0f), new Vector3(0.45f, 0.35f, 31f), m_DarkTrimMaterial, false);
                AddBlock(root, "CeilingNeon_" + i, new Vector3(x, 5.22f, 0f), new Vector3(0.16f, 0.12f, 28f), i % 2 == 0 ? m_CyanNeonMaterial : m_MagentaNeonMaterial, false);
            }
        }

        void BuildCoverAndPlatforms(Transform root)
        {
            Vector3[] coverPositions =
            {
                new Vector3(13f, 0.55f, -10f),
                new Vector3(21f, 0.55f, 11f),
                new Vector3(32f, 0.55f, -11f),
                new Vector3(43f, 0.55f, 10f),
                new Vector3(55f, 0.55f, -9f),
                new Vector3(63f, 0.55f, 7f),
            };

            for (int i = 0; i < coverPositions.Length; i++)
            {
                AddBlock(root, "CoverBase_" + i, coverPositions[i], new Vector3(4.5f, 1.1f, 1.2f), m_DarkTrimMaterial, true);
                AddBlock(root, "CoverNeon_" + i, coverPositions[i] + Vector3.up * 0.62f, new Vector3(4.3f, 0.12f, 0.16f), i % 2 == 0 ? m_CyanNeonMaterial : m_MagentaNeonMaterial, false);
            }

            AddBlock(root, "RaisedPlatform_Left", new Vector3(34f, 0.35f, -6f), new Vector3(9f, 0.7f, 5f), m_FloorMaterial, true);
            AddBlock(root, "RaisedPlatform_Right", new Vector3(47f, 0.35f, 6f), new Vector3(9f, 0.7f, 5f), m_FloorMaterial, true);
            AddBlock(root, "PlatformGlow_Left", new Vector3(34f, 0.75f, -3.4f), new Vector3(8.5f, 0.12f, 0.18f), m_AmberNeonMaterial, false);
            AddBlock(root, "PlatformGlow_Right", new Vector3(47f, 0.75f, 3.4f), new Vector3(8.5f, 0.12f, 0.18f), m_AmberNeonMaterial, false);
        }

        void BuildSignsAndProps(Transform root)
        {
            for (int i = 0; i < 6; i++)
            {
                float x = 7f + i * 11f;
                AddBlock(root, "HoloBillboard_" + i, new Vector3(x, 3.15f, i % 2 == 0 ? 15.3f : -15.3f), new Vector3(4.6f, 1.8f, 0.1f), i % 2 == 0 ? m_MagentaNeonMaterial : m_CyanNeonMaterial, false);
                AddBlock(root, "BillboardFrame_" + i, new Vector3(x, 3.15f, i % 2 == 0 ? 15.15f : -15.15f), new Vector3(5f, 2.15f, 0.12f), m_DarkTrimMaterial, false);
            }

            for (int i = 0; i < 5; i++)
            {
                float x = 10f + i * 13f;
                AddCylinder(root, "DataCore_" + i, new Vector3(x, 1.2f, i % 2 == 0 ? -13.5f : 13.5f), new Vector3(0.9f, 2.4f, 0.9f), i % 2 == 0 ? m_CyanNeonMaterial : m_MagentaNeonMaterial, true);
            }
        }

        void BuildLighting(Transform root)
        {
            AddPointLight(root, "CyberKey_Cyan", new Vector3(18f, 5f, -8f), new Color(0.05f, 0.85f, 1f), 30f, 6f);
            AddPointLight(root, "CyberKey_Magenta", new Vector3(42f, 5f, 8f), new Color(1f, 0.05f, 0.75f), 30f, 6.2f);
            AddPointLight(root, "CyberBack_Amber", new Vector3(62f, 4f, 0f), new Color(1f, 0.45f, 0.08f), 24f, 4f);
            AddPointLight(root, "SpawnFill_Cyan", new Vector3(-28f, 4.4f, -7f), new Color(0.2f, 0.9f, 1f), 22f, 3.5f);

            for (int i = 0; i < 6; i++)
            {
                AddPointLight(root, "LaneLight_" + i, new Vector3(7f + i * 11f, 4.4f, i % 2 == 0 ? -11f : 11f),
                    i % 2 == 0 ? new Color(0.05f, 0.8f, 1f) : new Color(1f, 0.05f, 0.7f), 15f, 2.6f);
            }
        }

        GameObject AddBlock(Transform parent, string objectName, Vector3 position, Vector3 scale, Material material, bool hasCollider)
        {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = objectName;
            block.transform.SetParent(parent);
            block.transform.position = position;
            block.transform.localScale = scale;

            Renderer renderer = block.GetComponent<Renderer>();
            renderer.sharedMaterial = material;

            Collider collider = block.GetComponent<Collider>();
            collider.enabled = hasCollider;

            return block;
        }

        GameObject AddCylinder(Transform parent, string objectName, Vector3 position, Vector3 scale, Material material, bool hasCollider)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.name = objectName;
            cylinder.transform.SetParent(parent);
            cylinder.transform.position = position;
            cylinder.transform.localScale = scale;

            Renderer renderer = cylinder.GetComponent<Renderer>();
            renderer.sharedMaterial = material;

            Collider collider = cylinder.GetComponent<Collider>();
            collider.enabled = hasCollider;

            return cylinder;
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
            pointLight.shadows = LightShadows.None;
        }

        static void DisableOriginalLevelGeometry()
        {
            foreach (Renderer renderer in FindObjectsByType<Renderer>(FindObjectsInactive.Exclude))
            {
                if (!ShouldReplace(renderer.transform))
                    continue;

                renderer.enabled = false;
            }

            foreach (Collider collider in FindObjectsByType<Collider>(FindObjectsInactive.Exclude))
            {
                if (!ShouldReplace(collider.transform))
                    continue;

                collider.enabled = false;
            }
        }

        static bool ShouldReplace(Transform transform)
        {
            Transform current = transform;
            while (current)
            {
                string objectName = current.name;

                if (objectName.StartsWith("Runtime") ||
                    objectName.Contains("Enemy") ||
                    objectName.Contains("Pickup") ||
                    objectName.Contains("Player") ||
                    objectName.Contains("Weapon") ||
                    objectName.Contains("Objective") ||
                    objectName.Contains("GameManager") ||
                    objectName.Contains("HUD") ||
                    objectName.Contains("Canvas"))
                {
                    return false;
                }

                if (objectName.StartsWith("Room_") ||
                    objectName.StartsWith("Dun_") ||
                    objectName.StartsWith("Wall_") ||
                    objectName.StartsWith("Floor_") ||
                    objectName.StartsWith("Ramp_") ||
                    objectName.StartsWith("Stairs") ||
                    objectName.StartsWith("Stairwell") ||
                    objectName.StartsWith("ShortSteps") ||
                    objectName.StartsWith("HighStep") ||
                    objectName.StartsWith("Basic_Floor") ||
                    objectName.StartsWith("Cat_") ||
                    objectName.StartsWith("Observatory") ||
                    objectName.StartsWith("Entry") ||
                    objectName.StartsWith("Balcony") ||
                    objectName.StartsWith("Pit_") ||
                    objectName == "===== LEVEL =====")
                {
                    return true;
                }

                current = current.parent;
            }

            return false;
        }
    }
}
