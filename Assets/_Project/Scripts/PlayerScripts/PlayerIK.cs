using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    /*[SerializeField] Animator playerAnimator;

    [SerializeField] bool isIKActive = true;

    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;

    float leftFootWeight;
    float rightFootWeight;

    private void Start()
    {
        leftFoot = playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
    }

    private void OnAnimatorIK()
    {
        if (isIKActive == true)
        {
            leftFootWeight = playerAnimator.GetFloat("LeftFoot");

            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot.position);

            rightFootWeight = playerAnimator.GetFloat("RightFoot");

            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot.position);
        }
    }*/
}
