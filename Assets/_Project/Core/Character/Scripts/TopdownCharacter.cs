using System;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEditor;
using UnityEngine;

namespace Core.Characters
{
    [RequireComponent(typeof(DestroyEvent))]
    public class TopdownCharacter : NetworkBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterHeightParametrs _heightParametrs;
        [SerializeField] public float MaxEndurance { get; private set; } = 100;
        [SerializeField] private AnimationCurve _enduranceRecovery;
        [SerializeField] private float _enduranceExpenses = 25;
        [SerializeField] private SpeedModificator _defualtSpeed = new() 
        {
             AccelerationClamp = 8,
             MaxSpeedClamp = 3.5f, 
        };
        [SerializeField] private SpeedModificator _crouchingSpeedModifier = new() 
        {
             AccelerationClamp = 8,
             MaxSpeedClamp = 1.75f, 
        };
        [SerializeField] private SpeedModificator _sprintModifier = new()
        {
            AccelerationClamp = 8,
            MaxSpeedClamp = 7f, 
        };
        [field: SerializeField] public float StopSpeed { get; private set; } = 16f;

        private NetVariable<Vector3> _moveDirection;
        private NetVariable<Vector3> _lookPosition;
        private NetVariable<bool> _isCrouching;
        private NetVariable<bool> _isSprinting;
        private NetVariable<Vector3> _ownerPosition;
        public NetVariable<float> Endurance { get; private set; }
        private float _positionInaccuracy = 0.75f;
        private const float _maxPositionIcacurracy = 1.5f;
        public float MaxSpeed { get; private set; }
        public float MaxAcceleration { get; private set; }
        public Vector3 Velocity { get; private set; }
        public NetworkList<SpeedModificator> SpeedModificators { get; private set; }
        public Vector3 LookPoint => _lookPosition.Value;
        

        public bool IsCrouching
        {
            get 
            {
                return _isCrouching.Value;
            } 
            set
            {
                if (_isCrouching.Value == value)
                {
                    return;
                }
                _isCrouching.Value = value;
            } 
        }

        public bool IsSprinting
        {
            get
            {
                return _isSprinting.Value;
            }
            set
            {
                if (_isSprinting.Value == value)
                {
                    return;
                }
                _isSprinting.Value = value;
            }
        }

        public float Speed => Velocity.magnitude;

        private void Awake()
        {
            _moveDirection = new(writePerm: NetworkVariableWritePermission.Owner);
            _lookPosition = new(writePerm: NetworkVariableWritePermission.Owner);
            _isCrouching = new(writePerm: NetworkVariableWritePermission.Owner);
            _isSprinting = new(writePerm: NetworkVariableWritePermission.Owner);
            SpeedModificators = new(writePerm: NetworkVariableWritePermission.Owner);
            _ownerPosition = new(transform.position, writePerm: NetworkVariableWritePermission.Owner);
            Endurance = new(MaxEndurance, writePerm: NetworkVariableWritePermission.Owner);
        }

        private void OnEnable()
        {
            _isCrouching.ValueChanged += OnCrouchingValueChange;
            _isSprinting.ValueChanged += OnSprintChange;
            SpeedModificators.OnListChanged += OnListChange;   
        }

        private void OnDisable()
        {
            _isCrouching.ValueChanged -= OnCrouchingValueChange;
            _isSprinting.ValueChanged -= OnSprintChange;
            SpeedModificators.OnListChanged -= OnListChange;
        }

        private void Start()
        {
            if (IsOwner)
            {
                SpeedModificators.Add(_defualtSpeed);
            }
        }

        private void OnListChange(NetworkListEvent<SpeedModificator> changeEvent)
        {
            float MaxSpeed = Mathf.Infinity;
            float MaxAcceleration = Mathf.Infinity;

            for (int i = 0; i < SpeedModificators.Count; i++)
            {
                SpeedModificator iterator = SpeedModificators[i];
                if (iterator.MaxSpeedClamp < MaxSpeed)
                {
                    MaxSpeed = iterator.MaxSpeedClamp;
                }

                if (iterator.AccelerationClamp < MaxAcceleration)
                {
                    MaxAcceleration = iterator.AccelerationClamp;
                }
            }

            this.MaxSpeed = MaxSpeed;
            this.MaxAcceleration = MaxAcceleration;
        }

        private void OnSprintChange(bool previousValue, bool newValue)
        {
            if (IsOwner == false)
            {
                return;   
            }

            if (_isSprinting.Value)
            {
                SpeedModificators.Add(_sprintModifier);
                SpeedModificators.Remove(_defualtSpeed);
                return;
            }
            
            SpeedModificators.Add(_defualtSpeed);
            SpeedModificators.Remove(_sprintModifier);
        }

        private void OnCrouchingValueChange(bool previousValue, bool newValue)
        {
            if (_isCrouching.Value)
            {
                if (IsOwner)
                {
                    SpeedModificators.Add(_crouchingSpeedModifier);
                }
                _characterController.height = _heightParametrs.SittingHeight;
                _characterController.center = new(0, _characterController.height/2, 0);
                return;
            }

            if (IsOwner)
            {
                SpeedModificators.Remove(_crouchingSpeedModifier);
            }
            _characterController.height = _heightParametrs.DefualtHeight;
            _characterController.center = new(0, _characterController.height/2, 0);
        }


        private void Update()
        {
            ValidatePosition();
            ValidateRotation();
            if (IsOwner) 
            {
                ValidateEndurance();
            }
        }

        private void ValidateEndurance() 
        {
            if (IsSprinting) 
            {
                Endurance.Value = Mathf.Clamp(Endurance.Value - _enduranceExpenses * Time.deltaTime, 0, MaxEndurance);

                if (Endurance.Value == 0) 
                {
                    IsSprinting = false;
                }

                return;
            }

            Endurance.Value = Mathf.Clamp(Endurance.Value + _enduranceRecovery.Evaluate(Endurance.Value) * Time.deltaTime, 0, MaxEndurance);
        }

        private void ValidateRotation()
        {
            Vector3 lookDirection = _lookPosition.Value - _characterController.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            float newRotation = lookRotation.eulerAngles.y;
            _characterController.transform.eulerAngles = new(0, newRotation, 0);
        }

        private void ValidatePosition()
        {
            _characterController.Move(Velocity * Time.deltaTime);
            _characterController.Move(Physics.gravity * Time.deltaTime);
            Vector3 acceleration = GetAcceleration();
            CalculateVelocity(acceleration);

            if (IsOwner)
            {
                _ownerPosition.Value = transform.position;
            }
            else 
            {
                float inacurracy = Mathf.Clamp(_positionInaccuracy * Velocity.magnitude, _positionInaccuracy, _maxPositionIcacurracy);
                if (Vector3.Distance(_ownerPosition.Value, transform.position) > inacurracy)
                {
                    transform.position = _ownerPosition.Value;
                }
                else
                {
                    float delta = Vector3.Distance(transform.position, _ownerPosition.Value) * Time.deltaTime * Velocity.magnitude;
                    transform.position = Vector3.MoveTowards(transform.position, _ownerPosition.Value, delta);
                }
            }
        }

        private Vector3 GetAcceleration()
        {
            Vector3 acceleration;
            if (_moveDirection.Value == Vector3.zero)
            {
                acceleration = Velocity.normalized * -1 * MaxAcceleration;
            }
            else
            {
                acceleration = _moveDirection.Value.normalized * MaxAcceleration;
            }
            return acceleration;
        }

        private void CalculateVelocity(Vector3 acceleration)
        {
            Velocity += acceleration * Time.deltaTime;
            if (_moveDirection.Value == Vector3.zero)
            {
                Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, Time.deltaTime * StopSpeed);
            }
            if (Velocity.magnitude > MaxSpeed)
            {
                Velocity = Velocity.normalized * MaxSpeed;
            }
        }

        public void Move(Vector3 destination)
        {
            _moveDirection.Value = destination;
        }

        public void Warp(Vector3 destination) 
        {
            _ownerPosition.Value = destination;
            transform.position = destination;
        }

        public void SetLookPosition(Vector3 lookPosition)
        {
            _lookPosition.Value = lookPosition;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(TopdownCharacter))]
        private class CEditor : Editor
        {
            private TopdownCharacter NavMeshAgentNetworkTarget => target as TopdownCharacter;
            private Vector3 _destination;
            private Vector3 _rotation;
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (Application.IsPlaying(target) == false)
                {
                    return;
                }

                if (NavMeshAgentNetworkTarget.NetworkManager != null && NavMeshAgentNetworkTarget.NetworkManager.LocalClientId == NavMeshAgentNetworkTarget.NetworkObject.OwnerClientId)
                {
                    _destination = EditorGUILayout.Vector3Field("", _destination);
                    if (GUILayout.Button("Set destination"))
                    {
                        NavMeshAgentNetworkTarget.Move(_destination);
                    }

                    _rotation = EditorGUILayout.Vector3Field("", _rotation);
                    if (GUILayout.Button("Look rotation"))
                    {
                        NavMeshAgentNetworkTarget.SetLookPosition(_rotation);
                    }
                }
            }
        }
#endif
    }
}
