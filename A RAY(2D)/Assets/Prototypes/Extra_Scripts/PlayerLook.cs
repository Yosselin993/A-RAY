using UnityEngine;
// Simply focused on changing the player's outfit (or in this case the spritesheet)
public class PlayerLook : MonoBehaviour

{
    
    public Animator animator; // refering to the component Animator
    public RuntimeAnimatorController outfit1;
    public RuntimeAnimatorController outfit2;

    private bool usingOutfit1 = true;
    // This will focus on updatin
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            usingOutfit1 = !usingOutfit1; //this allows to toggle
            animator.runtimeAnimatorController = usingOutfit1
                ? outfit1 : outfit2;
        }
    }
}
