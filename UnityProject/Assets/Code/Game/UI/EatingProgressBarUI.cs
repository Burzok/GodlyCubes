using UnityEngine;
using System.Collections;

public class EatingProgressBarUI : MonoBehaviour {

	private UISprite topSprite;
	private Animator eatingAnimation;
	AnimatorStateInfo currentState;

	private void Awake()
	{
		topSprite = transform.FindChild("EatingProgressBarTop").GetComponent<UISprite>();
	//	eatingAnimation = GameObject.FindGameObjectWithTag(Tags.player).transform.FindChild("Animator").GetComponent<Animator>();
	}

	private void Start ()
	{
		topSprite.transform.parent.gameObject.SetActive(false);
	}

	private void Update ()
	{
/*		currentState = eatingAnimation.GetCurrentAnimatorStateInfo(0);
		Debug.Log(currentState.IsName("Base Layer.Eating"));
		topSprite.fillAmount = currentState.normalizedTime % 1;
*/	}
}
