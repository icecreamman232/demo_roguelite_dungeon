using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SGGames.Scripts.Data
{
    [CreateAssetMenu(menuName = "SGGames/Bootstrap Profile", fileName = "Bootstrap Profile")]
    public class BootstrapProfile : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject[] m_assetList;

        public AssetReferenceGameObject[] AssetList => m_assetList;
        
    }
}
