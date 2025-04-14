using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace GamePlay.Health
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int health = 100;
        public event EventHandler OnDead;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0)
            {
                health = 0;
            }

            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }
    }
}