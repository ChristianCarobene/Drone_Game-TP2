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
        // Si no se asign� una c�mara en el Inspector, intenta encontrar la c�mara principal
        if (playerCamera == null)
            {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("PlayerShoot: �No se encontr� una c�mara principal (Camera.main) y no se asign� ninguna en el Inspector!", this);
                this.enabled = false; // Desactivar el script si no hay c�mara
            }
        }

        // Si no se asign� un punto de origen, usa la posici�n de la c�mara como fallback
        if (muzzlePoint == null)
        {
            muzzlePoint = playerCamera.transform; // Disparar desde la c�mara si no hay muzzle espec�fico
        }
    }

    void Update()
    {
        // Detectar el bot�n izquierdo del rat�n (o el bot�n "Fire1" definido en el Input Manager)
        if (Input.GetButtonDown("Fire1")) // "Fire1" es el mapeo est�ndar para el clic izquierdo
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Determinar origen y direcci�n del rayo
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        // Variable para almacenar la informaci�n de la colisi�n del raycast
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
            // Tambi�n podr�as usar GetComponentInParent<EnemyHealth>() o GetComponentInChildren<EnemyHealth>()
            // si el script no est� directamente en el objeto con el Collider.

            if (enemy != null)
            {
                // �Encontrado! Llamar al m�todo TakeDamage en el script del enemigo
                Debug.Log("Enemy detected! Applying damage to " + hitInfo.collider.gameObject.name);
                enemy.TakeDamage(damageAmount);
            }
            else
            {
                // El objeto golpeado no tiene el script de enemigo
                Debug.Log(hitInfo.collider.gameObject.name + " hit, but it's not an enemy.");
            }

            // Instanciar efecto de impacto (si se asign� uno)
            if (hitEffectPrefab != null)
            {
                // Instanciar en el punto de colisi�n, orientado seg�n la normal de la superficie golpeada
                Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
        }
        else
        {
            // El raycast no golpe� nada dentro del rango y capas especificadas
            Debug.Log("Raycast did not hit any object.");
        }

        // Podr�as a�adir aqu� un efecto de fogonazo (muzzle flash) en muzzlePoint si lo deseas
        // if (muzzleFlashPrefab != null) { Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation); }
    }
}
