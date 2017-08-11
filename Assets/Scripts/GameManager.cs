using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour
{
	private static Puzzle[] _puzzles;
	public static ReadOnlyCollection<Puzzle> AllPuzzles => System.Array.AsReadOnly(_puzzles);
    public static PlayerControlManager playerControlManager;
    public static FirstPersonController fpsController;
    public static ControllerManager controllerManager;
    public static Brightness brightness;
    public static PostProcessingManager postProcessingManager;
    public static PostProcessingBehaviour postProcessingBehaviour;
    public static GameObject blackScreen;
    public static Power power;
    public static CursorManager cursorManager;
    public static GameplayUIManager gameplayUIManager;
    public static VideoSettingsManager videoSettingsManager;
    public static LoadingScreen loadingScreen;
    public static AudioSettingsManager audioSettingsManager;
    public static ControlSettingsManager controlSettingsManager;

	private void Awake()
	{
		_puzzles = FindObjectsOfType<Puzzle>();
        playerControlManager = GetComponent<PlayerControlManager>();
        fpsController = FindObjectOfType<FirstPersonController>();
        controllerManager = GetComponent<ControllerManager>();
        brightness = FindObjectOfType<Brightness>();
        postProcessingManager = GetComponent<PostProcessingManager>();
        postProcessingBehaviour = FindObjectOfType<PostProcessingBehaviour>();
        blackScreen = GameObject.Find("BlackFade");
        power = GetComponent<Power>();
        cursorManager = GetComponent<CursorManager>();
        gameplayUIManager = FindObjectOfType<GameplayUIManager>();
        videoSettingsManager = FindObjectOfType<VideoSettingsManager>();
        loadingScreen = FindObjectOfType<LoadingScreen>();
        audioSettingsManager = FindObjectOfType<AudioSettingsManager>();
        controlSettingsManager = FindObjectOfType<ControlSettingsManager>();
    }
}
