using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField, Min(0.001f)] private float _speed;
        [SerializeField] private bool _homing = true;
        [SerializeField] private GameObject _impactEffectPrefab;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private GameObject[] _destroyOnHit;
        [SerializeField] private float _lifeAfterImpact = 2;

        private Health _target;
        private float _damage;
        private GameObject _intigator;
        
        private void Start()
        {
            transform.LookAt(GetAimLocation(_target));
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            if(_homing && !_target.HasDied)
            {
                transform.LookAt(GetAimLocation(_target));
            }
            
            var translateDelta = Vector3.forward * Time.deltaTime * _speed;
            transform.Translate(translateDelta);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _intigator = instigator;
            _damage = damage;

            Destroy(gameObject, _maxLifeTime);
        }

        private Vector3 GetAimLocation(Health target)
        {
            if (target.TryGetComponent<CapsuleCollider>(out var targetCollider))
            {
                return target.transform.position + Vector3.up * targetCollider.height / 2;
            }
            
            return target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Health>(out var health) || health != _target)
            {
                return;
            }

            if (_target.HasDied)
            {
                return;
            }
            
            if(_impactEffectPrefab != null)
            {
                Instantiate(_impactEffectPrefab, GetAimLocation(_target), transform.rotation);
            }
            
            _target.TakeDamage(_intigator, _damage);
            _speed = 0;

            foreach (var objectToDestroy in _destroyOnHit)
            {
                Destroy(objectToDestroy);
            }
            
            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}