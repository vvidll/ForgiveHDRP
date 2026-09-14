using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField] RectTransform markerImage;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform objectTransform;
    
    [SerializeField] float compassWidth = 400f;

    float targetAngle;
    float playerAngle; 
    float relativeAngle; // относительный угол 
    float normalized; // расчетный угол
    float xPos; // позиция метки относительно X-координате
    
    void Update()
    {
        Vector3 toTarget = objectTransform.position - playerTransform.position;
        
        // угол направления на объект от 0 градусов до 360
        targetAngle = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
        if (targetAngle < 0f) targetAngle += 360f;
        
        // угол поворота игрока от 0 градусов до 360
        playerAngle = playerTransform.eulerAngles.y;
        
        // относительный угол между направлением игрока и объекта, DeltaAngle - корректно вычисляет разницу между двумя углами
        relativeAngle = Mathf.DeltaAngle(playerAngle, targetAngle);
        
        // преобразование угла в X-координату на компасе
        // -45 градусов - левый край компаса, 0 градусов - центр компаса, +45 градусов - правый край компаса 
        normalized = relativeAngle / 90f;
        xPos = normalized * compassWidth;
        
        // меняем позицию маркера относительно компаса ( локальной позиции ), где новая позиция будет - xPos
        markerImage.localPosition = new Vector3(xPos, markerImage.localPosition.y, 0f);
    }
}
