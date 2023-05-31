using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTextUI : MonoBehaviour {

    private const string COUNTDOWN = "Countdown";
    private const string POPUP = "Popup";

    public static event EventHandler<OnHintTextShownEventArgs> OnHintTextShown;
    public class OnHintTextShownEventArgs : EventArgs {
        public Vector3 hintPosition;
    }

    private Animator animator;

    [SerializeField] private int secondsBeforeHint = 10;


    private void Start() {
        SmashableBird.OnAnySmash += SmashableBird_OnAnySmash;

        animator = GetComponent<Animator>();
        InvokeRepeating(COUNTDOWN, 1f, 1f);
        gameObject.SetActive(false);
    }

    private void SmashableBird_OnAnySmash(object sender, SmashableBird.OnAnySmashEventArgs e) {
        CancelInvoke(COUNTDOWN);
        gameObject.SetActive(false);
        SmashableBird.OnAnySmash -= SmashableBird_OnAnySmash;
    }

    private void Countdown() {
        secondsBeforeHint--;

        if (secondsBeforeHint <= 0) {
            CancelInvoke(COUNTDOWN);
            gameObject.SetActive(true);

            animator.SetTrigger(POPUP);
            OnHintTextShown?.Invoke(this, new OnHintTextShownEventArgs {
                hintPosition = transform.position
            });
        }
    }

    public static void ResetStaticData() {
        OnHintTextShown = null;
    }
}
