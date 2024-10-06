using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monster : MonoBehaviour
{
    public int health = 100;
    public int clickDamage = 25; 
    public GameObject damageTextPrefab; 
    public Transform canvasTransform;
    public Animator animator; 
    public AudioSource destroySound; 
    public AudioSource hitSound;
    public float clickDelay = 0.05f; 
    private float lastClickTime;
    private int damageTextOffset = 50;
    private int damageTextCount = 0;
    public Color hitColor = Color.red;
    public float hitColorDuration = 0.1f; 
    public float shakeDuration = 0.1f; 
    public float shakeMagnitude = 0.1f; 

    void OnMouseDown()
    {
        // Esto sigue permitiendo el daño al hacer clic
        if (Time.time - lastClickTime >= clickDelay)
        {
            lastClickTime = Time.time; 
            TakeDamage(clickDamage); // Usar un método para aplicar daño
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Reducir la salud del monstruo
        ShowDamageText(damage); // Mostrar el texto de daño
        hitSound.Play(); // Reproducir sonido de golpe
        StartCoroutine(ChangeColorOnHit(hitColor, hitColorDuration));
        StartCoroutine(ShakeMonster());

        if (health <= 0)
        {
            destroySound.Play();
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }
    }

    public void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null && canvasTransform != null)
        {
            GameObject damageTextInstance = Instantiate(damageTextPrefab, canvasTransform);
            TextMeshProUGUI tmp = damageTextInstance.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = "-" + damage.ToString();
                tmp.fontSize = 50f;
            }

            Vector3 worldPosition = transform.position + new Vector3(0.5f, 2.3f, 0);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            Vector3 offsetPosition = new Vector3(screenPosition.x, screenPosition.y + damageTextOffset * damageTextCount, screenPosition.z);
            damageTextInstance.transform.position = offsetPosition;

            damageTextCount++;

            Destroy(damageTextInstance, 1.0f);

            if (damageTextCount > 5)
            {
                damageTextCount = 0;
            }
        }
    }

    public IEnumerator ChangeColorOnHit(Color color, float duration)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = color;
            yield return new WaitForSeconds(duration);
            renderer.material.color = originalColor;
        }
    }

    private IEnumerator ShakeMonster()
    {
        Vector3 originalPosition = transform.position;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition; 
    }
}
