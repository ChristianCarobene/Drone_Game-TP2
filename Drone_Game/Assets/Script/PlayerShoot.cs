using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float maxShootDistance = 100f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask shootableLayers; // Configura esto en el Inspector

    [Header("Visual Effects (Optional)")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private EnemyHealt enemy;


    void Awake()
    {
        // Si no se asignó una cámara en el Inspector, intenta encontrar la cámara principal
        if (playerCamera == null)
            {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("PlayerShoot: ¡No se encontró una cámara principal (Camera.main) y no se asignó ninguna en el Inspector!", this);
                this.enabled = false; // Desactivar el script si no hay cámara
            }
        }

        // Si no se asignó un punto de origen, usa la posición de la cámara como fallback
        if (muzzlePoint == null)
        {
            muzzlePoint = playerCamera.transform; // Disparar desde la cámara si no hay muzzle específico
        }
    }

    void Update()
    {
        // Detectar el botón izquierdo del ratón (o el botón "Fire1" definido en el Input Manager)
        if (Input.GetButtonDown("Fire1")) // "Fire1" es el mapeo estándar para el clic izquierdo
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Determinar origen y dirección del rayo
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        // Variable para almacenar la información de la colisión del raycast
        RaycastHit hitInfo;

        // Visualizar el rayo en la vista de Escena (solo visible en el editor)
        Debug.DrawRay(rayOrigin, rayDirection * maxShootDistance, Color.red, 1.0f);

        // Lanzar el Raycast
        // Physics.Raycast devuelve true si colisiona con algo dentro de maxShootDistance en las shootableLayers
        bool hitDetected = Physics.Raycast(rayOrigin, rayDirection, out hitInfo, maxShootDistance, shootableLayers);

        if (hitDetected)
        {
            // --- Impacto Detectado ---
            Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);

            // Intentar obtener el componente del script del enemigo del objeto colisionado
            // !!! IMPORTANTE: Reemplaza 'EnemyHealth' con el nombre REAL de tu script de enemigo !!!
            enemy = hitInfo.collider.GetComponent<EnemyHealt>();
            // También podrías usar GetComponentInParent<EnemyHealth>() o GetComponentInChildren<EnemyHealth>()
            // si el script no está directamente en el objeto con el Collider.

            if (enemy != null)
            {
                // ¡Encontrado! Llamar al método TakeDamage en el script del enemigo
                Debug.Log("Enemy detected! Applying damage to " + hitInfo.collider.gameObject.name);
                enemy.TakeDamage(damageAmount);
            }
            else
            {
                // El objeto golpeado no tiene el script de enemigo
                Debug.Log(hitInfo.collider.gameObject.name + " hit, but it's not an enemy.");
            }

            // Instanciar efecto de impacto (si se asignó uno)
            if (hitEffectPrefab != null)
            {
                // Instanciar en el punto de colisión, orientado según la normal de la superficie golpeada
                Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
        }
        else
        {
            // El raycast no golpeó nada dentro del rango y capas especificadas
            Debug.Log("Raycast did not hit any object.");
        }

        // Podrías añadir aquí un efecto de fogonazo (muzzle flash) en muzzlePoint si lo deseas
        // if (muzzleFlashPrefab != null) { Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation); }
    }
}
