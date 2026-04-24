using UnityEngine;
using System.Collections;

public class Meat : MonoBehaviour
{
    [Header("References")]
    public Animator meatAnimator;
    public Dog dog; 

    private bool isActive;

    void Awake()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public void ShowMeat()
    {
        if (isActive) return;

        // Wake up the object so Coroutines can run
        gameObject.SetActive(true);

        // --- Suspicion Logic (Keep your existing check) ---
        bool dogIsCurrentlyAThreat = (dog != null && dog.IsMovingToOrInspectingKing());
        if (!dogIsCurrentlyAThreat)
        {
            SuspicionSystem.Instance.AddSuspicion(15f);
            NPC[] allNPCs = UnityEngine.Object.FindObjectsByType<NPC>(FindObjectsSortMode.None);
            foreach (NPC npc in allNPCs)
            {
                npc.StartInspection("What was that noise? Is that... meat?");
                StartCoroutine(DismissNPCAfterDelay(npc, 4f));
            }
        }
        else
        {
            // Reward for reacting correctly to the dog!
            SuspicionSystem.Instance.ReduceSuspicion(3f); 
        }

        StopAllCoroutines();
        StartCoroutine(SingleAnimationSequence());
    }

    private IEnumerator SingleAnimationSequence()
    {
        isActive = true;

        if (dog != null) dog.Distract();

        // 1. Play the one and only animation
        if (meatAnimator != null)
        {
            // Trigger the animation (Make sure the name matches your Animator!)
            meatAnimator.SetTrigger("PlayMeatAnim");
            
            // 2. Wait for the EXACT length of the animation
            // This pulls the duration directly from the Animator's current clip
            yield return new WaitForEndOfFrame(); // Wait a frame for the state to change
            float animLength = meatAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
        }
        else
        {
            // Fallback if no animator found
            yield return new WaitForSeconds(2f);
        }

        // 3. Clean up
        if (dog != null) dog.ResetDistraction();
        isActive = false;
        gameObject.SetActive(false);
    }

    private IEnumerator DismissNPCAfterDelay(NPC npc, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (npc != null) npc.EndInspection("How unkingly...");
    }
}