using Unity.Netcode;
using Unity.Netcode.Custom;
using Random = System.Random;

namespace Core.Netrandom
{
    public class NetRandom : NetworkBehaviour
    {
        private NetVariable<int> _seed;
        private static Random _random;

        public static Random Random => _random;
        public static int Range(int min, int max) => Random.Next(min, max);
        public static float Range(float min, float max) => Random.Next((int)(min*1000), (int)(max*1000))/1000f;

        private void Awake()
        {
            _seed = new();
        }

        private void Start()
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnect;
        }

        private void OnEnable()
        {
            if (didStart)
            {
                NetworkManager.OnClientConnectedCallback += OnClientConnect;
            }
            _seed.ValueChanged += OnValueChange;
        }

        private void OnDisable()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnect;
            }
            _seed.ValueChanged -= OnValueChange;
        }

        private void OnValueChange(int previousValue, int newValue)
        {
            _random = new(newValue);
        }

        private void OnClientConnect(ulong obj)
        {
            if (IsServer)
            {
                _seed.Value = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }
        }
    }
}
