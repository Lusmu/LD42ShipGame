﻿using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    public class PlayerController : MonoBehaviour
    {
        public ShipController ship;

        public Transform harbour;

        public ParticleSystem homeFlare;

        public ParticleSystem levelUpParticles;

        public int Score { get; set; }

        float lastFiredHomeFlare = 0;

        int currentBeaconStage = 1;

        [SerializeField]
        private Beacon beacon;

        // Use this for initialization
        void Start()
        {
            ship.Thrust = 1;
        }

        void Update()
        {
			if (Input.touchCount == 1) 
			{
				ship.Rudder = Input.touches [0].position.x < Screen.width * 0.5f ? -1 : 1;
			}
			else 
			{
				ship.Rudder = Input.GetAxis("Horizontal");
			}

            if (ship.isActiveAndEnabled && !ship.IsSinking && harbour && homeFlare)
            {
                var distSqr = (ship.transform.position - harbour.position).sqrMagnitude;
                if (distSqr > 100 * 100 && Time.time > lastFiredHomeFlare + 10)
                {
                    lastFiredHomeFlare = Time.time;
                    homeFlare.Play();
                }
            }
        }

        public void SetBeaconStage(int stage)
        {
            if (stage <= currentBeaconStage) return;

            bool updated = false;
            switch (stage)
            {
                case 2:
                    updated = true;
                    beacon.GetComponent<Light>().range = 15;
                    beacon.GetComponent<Light>().intensity = 1f;
                    beacon.GetComponent<SphereCollider>().radius = 8;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 1.2f;
                    break;
                case 3:
                    updated = true;
                    beacon.GetComponent<Light>().range = 20;
                    beacon.GetComponent<Light>().intensity = 1.2f;
                    beacon.GetComponent<SphereCollider>().radius = 1;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 1.4f;
                    break;
                case 4:
                    updated = true;
                    beacon.GetComponent<Light>().range = 30;
                    beacon.GetComponent<SphereCollider>().radius = 13;
                    beacon.GetComponent<Light>().intensity = 1.3f;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 2f;
                    break;
                case 5:
                    updated = true;
                    beacon.GetComponent<Light>().range = 36;
                    beacon.GetComponent<SphereCollider>().radius = 15;
                    beacon.GetComponent<Light>().intensity = 1.5f;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 2.5f;
                    break;
            }

            if (updated)
            {
                MessageManager.Instance.ShowMessage("Guiding Light Upgraded to Level " + stage);
                levelUpParticles.Play();
            }
        }
    }
}
