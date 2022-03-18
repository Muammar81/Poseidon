using PanettoneGames.Poseidon.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PanettoneGames.Poseidon.Menu
{
    public class PoolMenu : ScriptableObject
    {
        private static string myPubID = "46749";
        private static GameObjectPool _playerPool;
        private static GameObjectPool _enemyPool;
        private static InputActionReference _inputActionReference;
        private const string resourcesPath = @"Assets/Plugins/PanettoneGames/Poseidon/Assets/Resources/";
        private const string playerPoolAssetPath = resourcesPath + "PlayerPool.asset";
        private const string enemyAIPoolAssetPath = resourcesPath + "EnemyAIPool.asset";
        private const string inputActionAssetPath = resourcesPath + "PoolGameControls.inputactions";

        private static void InitializeResources()
        {
            _playerPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(playerPoolAssetPath, typeof(GameObjectPool));
            _enemyPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(enemyAIPoolAssetPath, typeof(GameObjectPool));

            InputActionAsset _inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionAssetPath);
            var _inputAction = _inputAsset.FindAction("Fire");
            _inputActionReference = InputActionReference.Create(_inputAction);
        }

        [MenuItem("Tools/Poseidon/Standard Player Setup", false, 11)]
        public static void PlayerPoolSetup()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length < 1) return;
            InitializeResources();

            foreach (var obj in selectedObjects)
            {
                if (HasBehaviour(obj)) continue;
                obj.AddComponent<PlayerShooting>();
                var comp = obj.GetComponent<PlayerShooting>();
                comp.SetFirePoint();

                if(_playerPool == null)
                {
                    Debug.LogError("pool asset is missing");
                }
                else
                {
                    comp.SetPool(_playerPool);
                }

                if (_inputActionReference == null)
                {
                    Debug.LogError("Fire action is missing");
                }
                else
                {
                    comp.SetFireButton(_inputActionReference);
                }
            }
        }
        [MenuItem("Tools/Poseidon/Standard Player Setup", true, 11)]
        static bool Validate_PlayerPoolSetup() => Selection.gameObjects.Length > 0;

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", false, 11)]
        public static void EnemyAIPoolSetup()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length < 1) return;
            InitializeResources();

            foreach (var obj in selectedObjects)
            {
                if (HasBehaviour(obj)) continue;
                obj.AddComponent<EnemyAIShooting>();
                var comp = obj.GetComponent<EnemyAIShooting>();
                comp.SetFirePoint();

                if (_enemyPool == null)
                {
                    Debug.LogError("pool asset is missing");
                }
                else
                {
                    comp.SetPool(_enemyPool);
                }
            }
        }

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", true, 11)]
        static bool Validate_EnemyAIPoolSetup() => Selection.gameObjects.Length > 0;
        private static bool HasBehaviour(GameObject activeObject)
        {
            var found = activeObject.GetComponent<PooledShootingBehaviour>() != null;
            if (found)
            {
                Debug.LogWarning($"{activeObject.name} already contains a shooting behaviour");
            }
            return found;
        }

        [MenuItem("Tools/Poseidon/Rate Please :)", false, 30)]
        public static void MenuRate() => Application.OpenURL($"https://assetstore.unity.com/packages/tools/utilities/poseidon-simple-pooling-system-201537?aid=1011lds77&utm_source=aff#reviews");

        [MenuItem("Tools/Poseidon/Help", false, 31)]
        public static void MenuHelp()
        {
            Application.OpenURL(@"https://panettonegames.com/");

            string helpFilePath = Application.dataPath + @"/Plugins/PanettoneGames/Poseidon/Poseidon Read Me.pdf";
            Debug.Log($"Help file is in: {helpFilePath}");
            Application.OpenURL(helpFilePath);
            Application.OpenURL($"https://assetstore.unity.com/publishers/" + myPubID + "?aid=1011lds77");
        }


    }
}