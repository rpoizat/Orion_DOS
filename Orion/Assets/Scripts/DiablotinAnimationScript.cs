using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiablotinAnimationScript : MonoBehaviour
{
    [SerializeField] private Animator diablotinAnimator;

    public bool runForward;
    public bool runBackward;
    public bool attack;
    
    // Start is called before the first frame update
    void Start()
    {
        diablotinAnimator.SetBool("Run Forward", false);
        diablotinAnimator.ResetTrigger("Attack 01");
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            diablotinAnimator.SetTrigger("Attack 01");
            attack = false;
        }
        else if (runForward)
        {
            diablotinAnimator.SetBool("Run Forward", true);
            diablotinAnimator.SetFloat("direction", 1.0f);
        }
            
        else if (runBackward)
        {
            diablotinAnimator.SetBool("Run Forward", true);
            diablotinAnimator.SetFloat("direction", -1.0f);
        }
            
        else
            diablotinAnimator.SetBool("Run Forward", false);


    }
}
