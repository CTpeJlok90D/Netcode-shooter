using UnityEngine;
using Core.Weapons;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.Netcode;
using Core.Items;

namespace View.Items
{
    public class ItemModelLoader : MonoBehaviour
    {
        [field: SerializeField] public UsebleReference UsebleReference { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }

        private GameObject _instance;
        private bool _isDestroyed;
        private AsyncOperationHandle<GameObject> _asyncOperationHandle;

        public Useble Useble => UsebleReference.Value;

        private void OnEnable()
        {
            UsebleReference.Changed += OnUsebleChange;
            NetworkManager.Singleton.OnConnectionEvent += OnConnect;
        }

        private void OnConnect(NetworkManager manager, ConnectionEventData data)
        {
            if (Useble != null)
            {
                LoadWeapon();
            }   
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        private void OnDisable()
        {
            UsebleReference.Changed -= OnUsebleChange;
            NetworkManager.Singleton.OnConnectionEvent -= OnConnect;
        }

        private void OnUsebleChange(Useble weapon) => LoadWeapon();
        private void LoadWeapon()
        {
            if (_asyncOperationHandle.IsDone == false)
            {
                _asyncOperationHandle.Task.Wait();
            }

            if (_instance != null)
            {
                Destroy(_instance);
            }

            if (Useble == null)
            {
                return;
            }

            _asyncOperationHandle = Addressables.InstantiateAsync(Useble.View.ViewReference, Parent);
            _asyncOperationHandle.Completed += (handle) => 
            {
                if (_isDestroyed)
                {
                    return;
                }
                _instance = handle.Result;
                if (_instance.TryGetComponent(out WeaponLocalReference reference) && Useble.TryGetComponent(out Firearm weapon))
                {    
                    reference.Value = weapon;
                }
            };
        }
    }
}
