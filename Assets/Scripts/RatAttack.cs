using System.Collections;
using UnityEngine;

public class MouseAttack : MonoBehaviour
{
    public float attackDistance = 50f; // Distancia máxima para atacar al monstruo
    public float attackSpeed = 10f; // Velocidad de ataque del ratón
    public float returnSpeed = 5f; // Velocidad de regreso al personaje
    public int damage = 25; // Daño que causa al monstruo
    public float distanciaMinima = 4f; // Distancia mínima para atacar
    public float attackCooldown = 2f; // Tiempo de espera antes de volver a atacar

    private GameObject target; // Referencia al monstruo
    private Vector3 startPosition; // Posición inicial del ratón
    private Rigidbody2D rb; // Componente Rigidbody2D

    private enum State
    {
        Idle,
        Attacking,
        Returning
    }

    private State currentState = State.Idle; // Estado actual del ratón

    void Start()
    {
        startPosition = transform.position; // Guardar la posición inicial
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                FindTarget();
                break;

            case State.Attacking:
                MoveTowardsTarget();
                break;

            case State.Returning:
                ReturnToStart();
                break;
        }
    }

    void FindTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // Asegúrate de etiquetar a tus monstruos

        // Encontrar el monstruo más cercano
        float closestDistance = attackDistance;
        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = monster;
            }
        }

        // Si se encuentra un objetivo, cambiar al estado de ataque
        if (target != null)
        {
            currentState = State.Attacking;
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null) return; // Si no hay objetivo, salir

        // Mover el ratón hacia el monstruo, restringiendo el movimiento al eje X
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = direction * attackSpeed; // Usar Rigidbody2D para mover el ratón

        // Verificar si ha alcanzado al monstruo
        if (Vector3.Distance(transform.position, target.transform.position) < distanciaMinima)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Aplicar daño al monstruo
        Monster monster = target.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage); // Método para aplicar daño
        }

        // Iniciar el regreso
        rb.velocity = Vector2.zero; // Detener el ratón al atacar
        currentState = State.Returning; // Cambiar al estado de regreso
        StartCoroutine(AttackCooldownCoroutine()); // Iniciar el cooldown antes de permitir otro ataque
    }

    void ReturnToStart()
    {
        // Regresar a la posición inicial
        Vector2 direction = (startPosition - transform.position).normalized;
        rb.velocity = direction * returnSpeed; // Usar Rigidbody2D para regresar

        // Verificar si ha vuelto a la posición original
        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            rb.velocity = Vector2.zero; // Detener el ratón al regresar
            target = null; // Reiniciar el objetivo
            currentState = State.Idle; // Regresar al estado de espera
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown); // Esperar el tiempo de cooldown
        currentState = State.Idle; // Regresar al estado de espera
    }
}
