using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    
    public float rotationSpeed ; // Prêdkoœæ obrotu
    public float movingSpeed; // Prêdkoœæ obrotu
    public float rotationThreshold; // Próg, kiedy uznajemy obrót za zakoñczony
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

        // Ró¿nica w pozycjach miêdzy targetFigure a movingFigure
        Vector3 direction = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysokoœæ nie zmienia siê
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Wykorzystanie funkcji LookRotation do obliczenia celu obrotu
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // P³ynne obracanie obiektu w stronê targetFigure
        while (Quaternion.Angle(movingFigure.transform.rotation, targetRotation) > rotationThreshold)
        {
            movingFigure.transform.rotation = Quaternion.Slerp(movingFigure.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // Czekamy na kolejny frame
        }

        // Obrót zakoñczony, wykonujemy akcjê
        Debug.Log("Obrót zakoñczony!");

            StartCoroutine(MoveAniamtion());
       
       
       

    }

    IEnumerator MoveAniamtion()
    {
        Debug.Log("second step animation");
        if (targetFigure == null) yield break;

        // Obliczanie ró¿nicy w pozycjach (pozostawiamy Z i X do przesuniêcia)
        Vector3 difference = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysokoœæ nie zmienia siê
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Sprawdzamy po której stronie znajduje siê movingFigure wzglêdem targetFigure
        Vector3 direction = difference.normalized;

        // Ustalanie, po której stronie jest movingFigure:
        // Jeœli movingFigure jest po prawej stronie targetFigure na osi X
        bool isRightSide = difference.x > 0;
        bool isUpSide = difference.z > 0;

        // Obliczamy przesuniêcie, uwzglêdniaj¹c offset
        Vector3 QuatersLogics = new Vector3(
            (Mathf.Abs(difference.x) - offsetStopMoving) * (isRightSide ? 1 : -1),  // Kierunek zale¿ny od strony (lewa/prawa)
            0,  // Wysokoœæ (Y) nie zmienia siê
            (Mathf.Abs(difference.z) - offsetStopMoving) * (isUpSide ? 1 : -1) // Kierunek w osi Z
        );

        Debug.Log("quaterlogics: " + QuatersLogics);

        // Obliczenie nowej pozycji docelowej z uwzglêdnieniem offsetu
        Vector3 preTargetPosition = movingFigure.transform.position + QuatersLogics;
        Debug.Log("pretargetpos: " + preTargetPosition);

        // P³ynne przesuwanie
        while (Vector3.Distance(movingFigure.transform.position, preTargetPosition) > distanceThreshold)
        {
            // Debugowanie pozycji aktualnej obiektu i docelowej
            Debug.Log("Current Position: " + movingFigure.transform.position);
            Debug.Log("Target Position: " + preTargetPosition);

            // U¿ycie Vector3.MoveTowards do p³ynnego przesuwania
            movingFigure.transform.position = Vector3.MoveTowards(
                movingFigure.transform.position,
                preTargetPosition,
                movingSpeed * Time.deltaTime
            );

            yield return null;  // Czekaj na kolejny frame
        }

        // Gdy obiekt dotrze do docelowej pozycji
        Debug.Log("ruch zakoñczony!" );
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


