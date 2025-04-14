using System;
using GamePlay.Health;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Unit
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private Transform ragdollPrefab;
        [SerializeField] private Transform unitGun;
        [SerializeField] private Transform originalRagDollRootBone;

        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        private void Start()
        {
            _healthSystem.OnDead += HealthSystemOnDead;
        }

        private void HealthSystemOnDead(object sender, EventArgs e)
        {
            Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, rotation: transform.rotation);
            UnitRagDoll unitRagDoll = ragdollTransform.GetComponent<UnitRagDoll>();
            unitRagDoll.SetUp(originalRagDollRootBone);
        }
    }
}