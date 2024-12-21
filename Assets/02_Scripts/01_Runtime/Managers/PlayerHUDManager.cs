using System.Collections;
using System.Numerics;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;
using UnityEngine.Playables;
using NotImplementedException = System.NotImplementedException;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace MinD.Runtime.Managers {

public class PlayerHUDManager : Singleton<PlayerHUDManager> {

	
	public static PlayerHUD playerHUD {
		get {
			if (playerHUD_ == null) {
				playerHUD_ = FindObjectOfType<PlayerHUD>();
			} else if (!playerHUD_.gameObject.activeSelf) {
				playerHUD_ = FindObjectOfType<PlayerHUD>();
			}

			return playerHUD_;
		}
	}
	private static PlayerHUD playerHUD_;
	
	public Player player;

	public bool isLockOnSpotEnable;
	public bool isPlayingBurstPopup;
	public bool isFadingWithBlack;
	
	private Coroutine fadingBlackScreenCoroutine;

	public PlayerMenu currentShowingMenu;
	


	public void Update() {

		if (player == null)
			return;

		HandleStatusBar();
		HandleLockOnSpot();
		HandleMenuInput();
	}
	
	
	
	private void HandleStatusBar() {

		playerHUD.hpBar.HandleTrailFollowing();
		playerHUD.mpBar.HandleTrailFollowing();
		playerHUD.staminaBar.HandleTrailFollowing();

	}

	private void HandleLockOnSpot() {
		if (!isLockOnSpotEnable) {
			return;
		}

		playerHUD.lockOnSpot.position = player.camera.camera.WorldToScreenPoint(player.camera.currentTargetOption.position);
		if (Vector3.Angle(player.camera.transform.forward, player.camera.currentTargetOption.position - player.camera.transform.position) > 90) {
			playerHUD.lockOnSpot.gameObject.SetActive(false);
		}
	}

	private void HandleMenuInput() {
		
		// MENU QUIT INPUT
		if (PlayerInputManager.Instance.menuQuitInput) {
			
			if (currentShowingMenu != null) {
				currentShowingMenu.OnQuitInput();
			}
			PlayerInputManager.Instance.menuQuitInput = false;
		}
		
		// MENU SELECT INPUT
		if (PlayerInputManager.Instance.menuSelectInput) {
			
			if (currentShowingMenu != null) {
				currentShowingMenu.OnSelectInput();
			}
			PlayerInputManager.Instance.menuSelectInput = false;
		}
		
		
		// MENU DIRECTION INPUT
		Vector2 dirxInput = PlayerInputManager.Instance.menuDirectionInput;
		if (dirxInput.magnitude != 0 && currentShowingMenu != null) {
			currentShowingMenu.OnInputWithDirection(dirxInput);
		}
		PlayerInputManager.Instance.menuDirectionInput = Vector2.zero;
	}
	
	
	
	public void RefreshAllStatusBar() {
		RefreshHPBar();
		RefreshMPBar();
		RefreshStaminaBar();
	}
	
	public void RefreshHPBar() {
		playerHUD.hpBar.SetMaxValue(player.attribute.maxHp);
		playerHUD.hpBar.SetValue(player.CurHp);
	}
	public void RefreshMPBar() {
		playerHUD.mpBar.SetMaxValue(player.attribute.maxMp);
		playerHUD.mpBar.SetValue(player.CurMp);
	}
	public void RefreshStaminaBar() {
		playerHUD.staminaBar.SetMaxValue(player.attribute.maxStamina);
		playerHUD.staminaBar.SetValue(player.CurStamina);
	}

	
	
	public void SetLockOnSpotActive(bool value) {
		isLockOnSpotEnable = value;
		playerHUD.lockOnSpot.gameObject.SetActive(value);
	}



	public void PlayBurstPopup(PlayableDirector burstPopupDirector, bool playWithForce = false) {

		if (isPlayingBurstPopup) {
			
			if (playWithForce) {
				StartCoroutine(PlayBurstPopupCoroutine(burstPopupDirector));
			} else {
				throw new UnityException("!! BURST POPUP IS ALREADY PLAYING!");
			}
			
		} else {
			StartCoroutine(PlayBurstPopupCoroutine(burstPopupDirector));
			
		}
		
	}

	private IEnumerator PlayBurstPopupCoroutine(PlayableDirector burstPopupDirector) {

		burstPopupDirector.gameObject.SetActive(true);
		burstPopupDirector.Play();

		yield return new WaitForSeconds((float)burstPopupDirector.duration);
		
		burstPopupDirector.gameObject.SetActive(false);

	}



	public void FadeInToBlack(float duration) {

		if (isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}

		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, true));
	}

	public void FadeOutFromBlack(float duration) {
		
		if (isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}
		
		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, false));
	}

	private IEnumerator FadeBlackScreen(float duration, bool fadeDirection) {
		
		isFadingWithBlack = true;
		playerHUD.blackScreen.gameObject.SetActive(true);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 0 : 1);
		
		
		float elapsedTime = 0;
		while (true) {

			elapsedTime += Time.deltaTime;
			
			playerHUD.blackScreen.color = new Color(0, 0, 0, (fadeDirection ? (elapsedTime / duration) : (1-elapsedTime / duration))); 
			yield return null;

			if (elapsedTime > duration) { 
				break;
			}
		}
		
		
		isFadingWithBlack = false;
		playerHUD.blackScreen.gameObject.SetActive(fadeDirection);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 1 : 0);
		
	}



	public void OpenMenu(PlayerMenu menu, bool openWithForce = false) {

		if (currentShowingMenu != null) {
			
			if (!openWithForce) {
				// OTHER MENU IS ALREADY SHOWING 
				throw new UnityException("!! " + menu.name + " TRIED TO OPEN WHILE " + currentShowingMenu + " IS SHOWING");
				
			} else {
				menu.Close();
				StartCoroutine(currentShowingMenu.FadeOpenAndClose(0, false));
			}
		}
		
		menu.Open();
		StartCoroutine(menu.FadeOpenAndClose(menu.fadeInTime, true));

		currentShowingMenu = menu;
	}
	
	public void CloseMenu(PlayerMenu menu) {

		if (!menu.Equals(currentShowingMenu)) {
			throw new UnityException("!! " + menu.name + " IS NOT CURRENT MENU! \n" + "CURRENT MENU IS " + currentShowingMenu.name);
		}
		
		menu.Close();
		StartCoroutine(menu.FadeOpenAndClose(menu.fadeOutTime, false));

		currentShowingMenu = null;
	}
}

}