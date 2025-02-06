using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    
    public float rotationSpeed ; // Pr�dko�� obrotu
    public float movingSpeed; // Pr�dko�� obrotu
    public float rotationThreshold; // Pr�g, kiedy uznajemy obr�t za zako�czony
    public float distanceThreshold; 
    public float offsetStopMoving ; 

    public ChessPiece movingFigure, targetFigure;
    public bool looking = false;
    public void StartAnimation(ChessPiece _movingFigure, ChessPiece _targetFigure)
    {
        movingFigure = _movingFigure;
        targetFigure = _targetFigure;
        StartCoroutine(RotateTowardsTarget());

    }
        IEnumerator RotateTowardsTarget()
    {
        Debug.Log("first step animation");
        if (targetFigure == null) yield break;

        // R�nica w pozycjach mi�dzy targetFigure a movingFigure
        Vector3 direction = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysoko�� nie zmienia si�
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Wykorzystanie funkcji LookRotation do obliczenia celu obrotu
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // P�ynne obracanie obiektu w stron� targetFigure
        while (Quaternion.Angle(movingFigure.transform.rotation, targetRotation) > rotationThreshold)
        {
            movingFigure.transform.rotation = Quaternion.Slerp(movingFigure.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // Czekamy na kolejny frame
        }

        // Obr�t zako�czony, wykonujemy akcj�
        Debug.Log("Obr�t zako�czony!");

            StartCoroutine(MoveAniamtion());
       
       
       

    }

    IEnumerator MoveAniamtion()
    {
        Debug.Log("second step animation");
        if (targetFigure == null) yield break;

        // Obliczanie r�nicy w pozycjach (pozostawiamy Z i X do przesuni�cia)
        Vector3 difference = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysoko�� nie zmienia si�
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Sprawdzamy po kt�rej stronie znajduje si� movingFigure wzgl�dem targetFigure
        Vector3 direction = difference.normalized;

        // Ustalanie, po kt�rej stronie jest movingFigure:
        // Je�li movingFigure jest po prawej stronie targetFigure na osi X
        bool isRightSide = difference.x > 0;
        bool isUpSide = difference.z > 0;

        // Obliczamy przesuni�cie, uwzgl�dniaj�c offset
        Vector3 QuatersLogics = new Vector3(
            (Mathf.Abs(difference.x) - offsetStopMoving) * (isRightSide ? 1 : -1),  // Kierunek zale�ny od strony (lewa/prawa)
            0,  // Wysoko�� (Y) nie zmienia si�
            (Mathf.Abs(difference.z) - offsetStopMoving) * (isUpSide ? 1 : -1) // Kierunek w osi Z
        );

        Debug.Log("quaterlogics: " + QuatersLogics);

        // Obliczenie nowej pozycji docelowej z uwzgl�dnieniem offsetu
        Vector3 preTargetPosition = movingFigure.transform.position + QuatersLogics;
        Debug.Log("pretargetpos: " + preTargetPosition);

        // P�ynne przesuwanie
        while (Vector3.Distance(movingFigure.transform.position, preTargetPosition) > distanceThreshold)
        {
            // Debugowanie pozycji aktualnej obiektu i docelowej
            Debug.Log("Current Position: " + movingFigure.transform.position);
            Debug.Log("Target Position: " + preTargetPosition);

            // U�ycie Vector3.MoveTowards do p�ynnego przesuwania
            movingFigure.transform.position = Vector3.MoveTowards(
                movingFigure.transform.position,
                preTargetPosition,
                movingSpeed * Time.deltaTime
            );

            yield return null;  // Czekaj na kolejny frame
        }

        // Gdy obiekt dotrze do docelowej pozycji
        Debug.Log("ruch zako�czony!" );
        looking = true;
        PlayingAnimation();


    }
    void Update()
    {
        
        if (targetFigure != null && looking)
        {
            Vector3 directionToTarget = targetFigure.transform.position - movingFigure.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            movingFigure.transform.rotation = Quaternion.Lerp(movingFigure.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }



    public void PlayingAnimation()
    {
        Debug.Log(movingFigure.transform.position);
        movingFigure.GetComponent<Animator>().SetBool("MoveAnimation", true);
        
        Debug.Log("animacja !");


        Invoke("end", 1.8f);
        
    }
    public void end() {
        looking = false;
    }
    


}


