using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRomoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        timeElapsed = 0f; // thoi` gian da troi qua
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color; // lay mau goc cua nhan vat
        objToRemove = animator.gameObject; // chon doi tuong de lam vat bi bien mat
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime; // tinh thoi gian da troi qua
        float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
        // đại lượng a đưa ra giá trị trong khoảng từ 0 - 1, 0 là biến mất hẳn còn 1 là rõ nhất
        // khi mà giá trị timeElapsed gần bằng gt fade time thì bên trong ngoặc (1 - (timeElapsed / fadeTime)) sẽ cho ra gt nhỏ nhất, lúc này giá trị startColor.a sẽ nhỏ nhất và đạt gần ngưỡng 0 để biến mất
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        //từng frame sẽ cập nhật lại màu
        if(timeElapsed > fadeTime)
        {
            Destroy(objToRemove);
        }
    }


}
