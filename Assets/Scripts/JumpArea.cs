using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JumpArea : MonoBehaviour
{
    public Transform JumpPointA;
    public Transform JumpPointB;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Luna"))
        {
            LunaController controller = collider.GetComponent<LunaController>();
            controller.Jump(true);
            float disA = Vector3.Distance(collider.transform.position, JumpPointA.position);
            float disB = Vector3.Distance(collider.transform.position, JumpPointB.position);
            Transform target;
            Transform lunaJump = controller.transform.GetChild(0);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(lunaJump.DOLocalMoveY(1.5f, 0.25f).SetEase(Ease.InOutSine));
            sequence.Append(lunaJump.DOLocalMoveY(0.4f, 0.25f).SetEase(Ease.InQuad));

            if (disA > disB)
            {
                target = JumpPointA;
            }
            else
            {
                target = JumpPointB;
            }
            controller.transform.DOMove(target.position, 0.5f).SetEase(Ease.Linear).OnComplete(() => { _endJump(controller); });
            sequence.Play();
        }
    }
    private void _endJump(LunaController controller)
    {
        controller.Jump(false);
    }
}
