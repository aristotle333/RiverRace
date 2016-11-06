using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationSprites {
    public Sprite[] LeftWalk;
    public Sprite[] RightWalk;
    public Sprite[] DownWalk;
    public Sprite[] UpWalk;
}

public enum AnimationState {
    leftWalk, rightWalk, downWalk, upWalk, idle
}

public enum PlayerColor {
    blue, green, orange, grey
}

public class BasicMovementAnimator : MonoBehaviour {

    public PlayerColor          currentColor;
    public AnimationState       animationState;
    public int                  playerNum;           // Must be a number between 1-4
    public float                updateRate;          // How frequently to check to update the animation state
    public bool                 animatorIsActive;    // Set it to true for as long as you want to update sprites and have animation

    [Header("List of all Sprites: 0 blue, 1 green, 2 orange, 3 grey")]
    public List<AnimationSprites> pirateSprites = new List<AnimationSprites>();


    private int animationVal;                   // This should be a number between 0-3 since we have 3 animations for each direction plus 1 to loop so 4 in total
    private int idleAnimationVal = 1;           // The idle animation value (in our case this is the second element in the list, or index 1)
    private SpriteRenderer sr;                  // Sprite Renderer reference of attacked object

    private const int numOfAnimationValues = 3;      // Maximum index number of animation values. Animation loop should be 0->1->2->1->0->1->2 and so on

    // Axis private variables
    private string xAxisName;
    private string yAxisName;
    private float xAxisVal = 0f;
    private float yAxisVal = 0f;

    void Awake() {
        xAxisName = "Player" + playerNum + "_axisX";
        yAxisName = "Player" + playerNum + "_axisY";
        sr = this.GetComponent<SpriteRenderer>();
    }

    void Start() {
        animationState = AnimationState.idle;
        StartCoroutine(AnimateCoroutine());
    }

    void Update() {
        xAxisVal = Input.GetAxis(xAxisName);
        yAxisVal = Input.GetAxis(yAxisName);
    }

    public void activateAnimator() {
        animatorIsActive = true;
        StartCoroutine(AnimateCoroutine());
    }

    public void deactivateAnimator() {
        animatorIsActive = false;
    }

    private IEnumerator AnimateCoroutine() {
        while(animatorIsActive) {
            AnimationState nextState = getNextAnimationState();

            // Same animation state so continue looping
            if (nextState == animationState && nextState != AnimationState.idle) {
                updateAnimationValue();
            }
            else if (nextState == AnimationState.idle) {
                animationVal = idleAnimationVal;
            }
            else if (nextState != animationState && nextState != AnimationState.idle) {
                animationState = nextState;
                animationVal = idleAnimationVal;
            }

            // Update actual pirate sprite
            updateSprite();
            yield return new WaitForSeconds(updateRate);
        }
    }

    private AnimationState getNextAnimationState() {
        AnimationState temp = AnimationState.idle;          // Default value animation state
        if (xAxisVal > 0f) {
            if (Mathf.Abs(yAxisVal) < 0.5f) {
                temp = AnimationState.rightWalk;
            }
            else if (yAxisVal >= 0.5f) {
                temp = AnimationState.upWalk;
            }
            else if (yAxisVal <= 0.5f) {
                temp = AnimationState.downWalk;
            }
        }
        else if (xAxisVal < 0f) {
            if (Mathf.Abs(yAxisVal) < 0.5f) {
                temp = AnimationState.leftWalk;
            }
            else if (yAxisVal >= 0.5f) {
                temp = AnimationState.upWalk;
            }
            else if (yAxisVal <= 0.5f) {
                temp = AnimationState.downWalk;
            }
        }
        else if (xAxisVal == 0f) {
            if (yAxisVal > 0f) {
                temp = AnimationState.upWalk;
            }
            else if (yAxisVal < 0f) {
                temp = AnimationState.downWalk;
            }
            else if (yAxisVal == 0f) {
                temp = AnimationState.idle;
            }
        }
        return temp;
    }

    private void updateAnimationValue() {
        if (animationVal == numOfAnimationValues) {
            animationVal = 0;
        }
        else {
            ++animationVal;
        }
    }

    private void updateSprite() {
        switch(animationState) {
            case AnimationState.downWalk:
                sr.sprite = pirateSprites[(int)currentColor].DownWalk[animationVal];
                break;
            case AnimationState.upWalk:
                sr.sprite = pirateSprites[(int)currentColor].UpWalk[animationVal];
                break;
            case AnimationState.leftWalk:
                sr.sprite = pirateSprites[(int)currentColor].LeftWalk[animationVal];
                break;
            case AnimationState.rightWalk:
                sr.sprite = pirateSprites[(int)currentColor].RightWalk[animationVal];
                break;
            default:
                // staying idle (do nothing)
                break;
        }
    }


}
