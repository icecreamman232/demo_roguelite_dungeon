using System.Reflection;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Entities;
using SGGames.Scripts.Events;
using SGGames.Scripts.HealthSystem;
using UnityEditor;
using UnityEngine;

namespace SGGames.Scripts.EditorExtensions
{
    public partial class QuickMenuEditor
    {
        #region Create Enemy Wizard

        [MenuItem("GameObject/Create Enemy Wizard",priority = 1)]
        public static void CreateEnemyPlaceHolderInScene()
        {
            GameObject newEnemyGO = new GameObject();
            newEnemyGO.name = "New Enemy";
            newEnemyGO.layer = LayerManager.EnemyLayer;

            AddMainCollisionComponents(newEnemyGO);
            AddEnemyController(newEnemyGO);
            AddHealthComponent(newEnemyGO);
            AddMovementComponent(newEnemyGO);

            AddModel(newEnemyGO);
            AddBodyDamageGameObject(newEnemyGO);

            var pathContainer = AssetDatabase.LoadAssetAtPath<AssetPathContainer>("Assets/Data/Asset Path Container.asset");
            AddSharedPrefab(newEnemyGO, pathContainer.EnemyHPBarPath);
            AddSharedPrefab(newEnemyGO, pathContainer.FillColorSpritePath);
            AddSharedPrefab(newEnemyGO,pathContainer.LootTablePath);
        }

        private static void AddMainCollisionComponents(GameObject main)
        {
            //Collision components
            main.AddComponent<BoxCollider2D>();
            var rigidBody2D = main.AddComponent<Rigidbody2D>();
            rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidBody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidBody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        private static void AddEnemyController(GameObject main)
        {
            var controller = main.AddComponent<EnemyController>();
            var gameEventAsset =
                AssetDatabase.LoadAssetAtPath("Assets/Data/Events/Game Event.asset", typeof(GameEvent));
            var gameEventField =
                typeof(EnemyController).GetField("m_gameEvent", BindingFlags.NonPublic | BindingFlags.Instance);
            gameEventField.SetValue(controller, gameEventAsset);

            var permissionField =
                typeof(EnemyController).GetField("m_isPermit", BindingFlags.NonPublic | BindingFlags.Instance);
            permissionField.SetValue(controller, true);
        }

        private static void AddHealthComponent(GameObject main)
        {
            var health = main.AddComponent<EnemyHealth>();
            var healthPermissionField =
                typeof(EnemyHealth).GetField("m_isPermit", BindingFlags.NonPublic | BindingFlags.Instance);
            healthPermissionField.SetValue(health, true);
        }

        private static void AddMovementComponent(GameObject main)
        {
            var movement = main.AddComponent<EnemyMovement>();
            var movementPermissionField =
                typeof(EnemyMovement).GetField("m_isPermit", BindingFlags.NonPublic | BindingFlags.Instance);
            movementPermissionField.SetValue(movement, true);
        }

        private static void AddModel(GameObject main)
        {
            GameObject enemyModelGO = new GameObject();
            enemyModelGO.name = "Model";
            enemyModelGO.transform.parent = main.transform;
            var spriteRenderer = enemyModelGO.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Enemy";
        }

        private static void AddBodyDamageGameObject(GameObject main)
        {
            GameObject bodyDamageGO = new GameObject();
            bodyDamageGO.name = "BodyDamage";
            bodyDamageGO.transform.parent = main.transform;
            bodyDamageGO.layer = LayerManager.EnemyLayer;

            var dmgHandler = bodyDamageGO.AddComponent<DamageHandler>();
            var targetMaskField =
                typeof(DamageHandler).GetField("m_targetMask", BindingFlags.NonPublic | BindingFlags.Instance);
            targetMaskField.SetValue(dmgHandler, (LayerMask)LayerManager.PlayerMask);

            var bodyCollider = bodyDamageGO.AddComponent<BoxCollider2D>();
            bodyCollider.isTrigger = true;
        }

        private static void AddSharedPrefab(GameObject main, string path)
        {
            var enemyHPBarPrefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            var enemyHPBar = PrefabUtility.InstantiatePrefab(enemyHPBarPrefab);
            ((GameObject)enemyHPBar).transform.parent = main.transform;
        }

        #endregion
    }
}