using System.Reflection;
using SGGames.Scripts.Data;
using SGGames.Scripts.HealthSystem;
using SGGames.Scripts.Weapons;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SGGames.Scripts.EditorExtensions
{
    public class PlayerProjectileWizard : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private Texture2D m_previewTexture;
        
        [MenuItem("SGGames/Player Projectile Wizard")]
        public static void ShowWindow()
        {
            PlayerProjectileWizard wnd = GetWindow<PlayerProjectileWizard>();
            wnd.titleContent = new GUIContent("Projectile Wizard");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            
            // Instantiate UXML
            VisualElement visualTree = m_VisualTreeAsset.Instantiate();
            root.Add(visualTree);

            var spriteData = root.Q("sprite_field") as ObjectField;
            spriteData.RegisterValueChangedCallback(ChangeSprite);
            
            var createButton = root.Q("create_button") as Button;
            createButton.RegisterCallback<PointerDownEvent>(CreateButtonClicked,TrickleDown.TrickleDown);
        }

        private void ChangeSprite(ChangeEvent<Object> changeEvent)
        {
            var newSprite = changeEvent.newValue as Sprite;

            if (newSprite == null) return;
            
            var spriteReview = rootVisualElement.Q("sprite_review");
            // Create a new texture from the sprite's rectangle
            //Here make sure the texture which the sprite is from, is a readable texture.
            
            //TODO: Re-visit this code after having all monster in big atlas
            m_previewTexture = new Texture2D((int)newSprite.rect.width, (int)newSprite.rect.height);
            var pixels = newSprite.texture.GetPixels(
                (int)newSprite.textureRect.x, 
                (int)newSprite.textureRect.y, 
                (int)newSprite.textureRect.width, 
                (int)newSprite.textureRect.height);
            m_previewTexture.SetPixels(pixels);
            m_previewTexture.Apply();
            
            spriteReview.style.backgroundImage = m_previewTexture;
        }

        private void CreateButtonClicked(PointerDownEvent evt)
        {
            var dataValue = (rootVisualElement.Q("data_field") as ObjectField).value as ProjectileData;
            var projectileName = (rootVisualElement.Q("projectile_name") as TextField).text;
            
            GameObject projectile = new GameObject();
            projectile.name = projectileName;
            
            //BoxCollider 2D Componenet
            var projectileCollider = projectile.AddComponent<BoxCollider2D>();
            projectileCollider.isTrigger = true;
            
            //Damage Handler Component
            var damageHandler = projectile.AddComponent<DamageHandler>();
            var targetMaskField = typeof(DamageHandler).GetField("m_targetMask", BindingFlags.Instance | BindingFlags.NonPublic);
            targetMaskField.SetValue(damageHandler, ((LayerMask)LayerMask.GetMask("Enemy")));
            
            //Model and its sprite renderer
            GameObject model = new GameObject();
            model.name = "Model";
            model.transform.SetParent(projectile.transform);
            
            var renderer = model.AddComponent<SpriteRenderer>();
            var spriteData = rootVisualElement.Q("sprite_field") as ObjectField;
            renderer.sprite = spriteData.value as Sprite;
            renderer.sortingLayerName = "Player";
            renderer.sortingOrder = 3;
            
            //Player Projectile Component
            var playerProjectile = projectile.AddComponent<PlayerProjectile>();
            var dataField = typeof(PlayerProjectile).GetField("m_projectileData", BindingFlags.Instance | BindingFlags.NonPublic);
            dataField.SetValue(playerProjectile,dataValue);
            var colliderRefField = typeof(PlayerProjectile).GetField("m_projectileCollider", BindingFlags.Instance | BindingFlags.NonPublic);
            colliderRefField.SetValue(playerProjectile, projectileCollider);
            var dmgHandlerRefField = typeof(PlayerProjectile).GetField("m_damageHandler", BindingFlags.Instance | BindingFlags.NonPublic);
            dmgHandlerRefField.SetValue(playerProjectile, damageHandler);
            var modelRefField = typeof(PlayerProjectile).GetField("m_model", BindingFlags.Instance | BindingFlags.NonPublic);
            modelRefField.SetValue(playerProjectile, model);
            var obstacleMaskField = typeof(PlayerProjectile).GetField("m_obstacleLayerMask", BindingFlags.Instance | BindingFlags.NonPublic);
            obstacleMaskField.SetValue(playerProjectile, ((LayerMask)LayerMask.GetMask("Door","Obstacle")));
            
            PrefabUtility.SaveAsPrefabAsset(projectile,"Assets/Prefab/Player/Weapon/"+ projectile.name + ".prefab");
            DestroyImmediate(projectile);
            DestroyImmediate(m_previewTexture);
            m_previewTexture = null;
            ResetAllFields();
        }

        private void ResetAllFields()
        {
            var textField = rootVisualElement.Q("projectile_name") as TextField;
            textField.value = string.Empty;
            
            var dataField = rootVisualElement.Q("data_field") as ObjectField;
            dataField.value = null;
            
            var spriteField = rootVisualElement.Q("sprite_field") as ObjectField;
            spriteField.value = null;
            
            var spriteReview = rootVisualElement.Q("sprite_review");
            spriteReview.style.backgroundImage = null;
        }
    }
}

