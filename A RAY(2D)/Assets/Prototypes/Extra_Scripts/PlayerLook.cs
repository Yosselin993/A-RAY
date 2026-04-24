using UnityEngine;

[System.Serializable]
public class GenreOutfitSet
{
    //for the genres
    public string genreName;
    public RuntimeAnimatorController normaloutfit_normal; // this will be for the normal outfit + normal weapon
    public RuntimeAnimatorController normaloutfit_terrible; //normal outfit + terrible weapon
    public RuntimeAnimatorController terribleoutfit_normal; //terrible outfit and normal weapon
    public RuntimeAnimatorController terribleoutfit_terrible; // terrible outfit + weapon
}


// Simply focused on changing the player's outfit (or in this case the spritesheet)
public class PlayerLook : MonoBehaviour

{
    public Animator animator; // refering to the component Animator


    // the possible genres will be focused here
    public GenreOutfitSet[] genreOutfits;
    private GenreOutfitSet currentGenre; //focused on the one currently being used
    //might remove since we don't have the genre system yet. 

    // PUNISHMENT STATE HERE 
    // talked about punishments for users lower than the average score, so making bool
    private bool isPunished = false; //punishment 
    private bool secondStageApplied = false; // if punishment still passes and finally at the /second stage. 

    private int firstPunishment = -1; //0 is outfit, 1 is weapond
    private float punishmentTimer = 0f; //WILL CHANGE LATER

    public float timetoFullPunishment = 20f; //20 seconds before the other penalty happens. 
                                             // I didn't want it showing all at once, but one by one. 
                                             // doing changes based on reward/punishment system mentioned in meeting


    void Start()
    {
        if (genreOutfits != null && genreOutfits.Length > 0)
            currentGenre = genreOutfits[0];
        ApplyNormalState();
    }

    // This will focus on updatin
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        if (GameManager.Instance == null) return;

        int score = GameManager.Instance.currentScore;

        HandlePunishmentTrigger(score);
        HandleSecondStage();
        HandleReset(score);
    }

    void HandlePunishmentTrigger(int score)
    {
        if (score < 100 && !isPunished)
        {
            isPunished = true; //when true, will go to punishment state
            secondStageApplied = false;

            firstPunishment = Random.Range(0, 2); //either picks a weapon or a outfit
            punishmentTimer = 0f;
            ApplyFirstPunishment();
        }
    }


    void HandleSecondStage()
    {
        if (!isPunished) return;

        punishmentTimer += Time.deltaTime;
        // after the chosen amount of seconds (20), apply the second punishment state
        if (!secondStageApplied && punishmentTimer >= timetoFullPunishment)
        {
            ApplySecondPunishment();
            secondStageApplied = true;
        }
    }
    void HandleReset(int score)
    {
        //THIS IS RESET
        if (score >= 100 && isPunished)
        {
            ResetPlayer(); // happens only if the player recovers
        }
    }

    void ApplyFirstPunishment()
    {
        if (firstPunishment == 0)
        {
            // Outfit WILL be terrible
            animator.runtimeAnimatorController = currentGenre.terribleoutfit_normal;
        }
        else
        {
            //
            animator.runtimeAnimatorController = currentGenre.normaloutfit_terrible;
        }
    }

    void ApplySecondPunishment()
    {
        animator.runtimeAnimatorController = currentGenre.terribleoutfit_terrible;
    }

    void ResetPlayer()
    {
        isPunished = false;
        secondStageApplied = false;
        firstPunishment = -1;
        punishmentTimer = 0f;

        ApplyNormalState();
    }

    void ApplyNormalState()
    {
        if (currentGenre != null)
            animator.runtimeAnimatorController = currentGenre.normaloutfit_normal;
        // restoring normal outfit + weapon

    }
}

