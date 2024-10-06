using UnityEngine;

public class SingleBackgroundScroller : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private Vector2 scrollSpeed = new Vector2(-2f, -1f);
    
    private Vector2 startPosition;
    public float repositionThreshold = 4f; // Qué tanto se mueve antes de reposicionar
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Asegurarnos de que el sprite es lo suficientemente grande
        EnsureBackgroundSize();
        
        // Guardar la posición inicial
        startPosition = transform.position;
    }

    void EnsureBackgroundSize()
    {
        if (backgroundSprite == null) return;

        // Obtener las dimensiones de la pantalla en unidades del mundo
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;
        
        // Hacer el sprite significativamente más grande que la pantalla
        Vector2 spriteSize = backgroundSprite.size;
        float scaleX = (screenWidth * 3) / spriteSize.x;
        float scaleY = (screenHeight * 3) / spriteSize.y;
        
        // Usar la escala mayor para mantener la proporción
        float maxScale = Mathf.Max(scaleX, scaleY);
        backgroundSprite.transform.localScale = new Vector3(maxScale, maxScale, 1f);
    }

    void Update()
    {
        // Mover el fondo
        transform.Translate(scrollSpeed * Time.deltaTime);
        
        // Calcular cuánto nos hemos movido desde la posición inicial
        Vector2 movement = (Vector2)transform.position - startPosition;
        
        // Si nos hemos movido más del threshold en cualquier dirección, reposicionar
        if (Mathf.Abs(movement.x) >= repositionThreshold || Mathf.Abs(movement.y) >= repositionThreshold)
        {
            // Calculamos el offset para que el movimiento sea suave
            Vector2 offset = new Vector2(
                movement.x - Mathf.Sign(movement.x) * repositionThreshold,
                movement.y - Mathf.Sign(movement.y) * repositionThreshold
            );
            
            // Reposicionar al punto de inicio más el offset residual
            transform.position = startPosition + offset;
        }
    }
}