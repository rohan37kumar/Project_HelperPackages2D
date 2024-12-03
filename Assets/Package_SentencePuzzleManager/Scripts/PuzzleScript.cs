using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//script is roughly hardcoded and needs refactoring -rohan37kumar
public class PuzzleScript : MonoBehaviour
{
    [SerializeField] private Sprite questionImage;
    [SerializeField] private Sprite solvedImage;
    [SerializeField] private Sprite sadDoodle;
    [SerializeField] private Sprite happyDoodle;

    //gameobject references
    [SerializeField] private Image questionImageHolder;
    [SerializeField] private GameObject answerImageHolder;
    [SerializeField] private GameObject targetPlace;

    private Vector3 originalAnswerPosition;
    private Vector3 targetPosition;
    private Animator animator;

    private void Start()
    {
        originalAnswerPosition = answerImageHolder.transform.position;
        targetPosition = targetPlace.transform.position;
        animator=answerImageHolder.GetComponent<Animator>();
    }

    public void QuestionSolved()
    {
        questionImageHolder.sprite = solvedImage;
        //SpriteRenderer spriteRenderer = answerImageHolder.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = happyDoodle;
        AnimateWalking();
        LeanTween.move(answerImageHolder.gameObject, targetPosition, 2f).setEase(LeanTweenType.easeInOutQuad);

    }

    public void ReloadPuzzle()
    {
        questionImageHolder.sprite = questionImage;
        answerImageHolder.transform.position = originalAnswerPosition;
        animator.SetBool("toWalk", false);
    }

    public void AnimateWalking()
    {
        //Debug.Log("animating walking animation");
        animator.SetBool("toWalk", true);
        StartCoroutine(WaitAndIdle());
    }

    private IEnumerator WaitAndIdle()
    {
        yield return new WaitForSeconds(2.0f);
        animator.SetBool("toWalk", false);
    }
}
