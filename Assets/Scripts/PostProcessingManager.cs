using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    private PostProcessingBehaviour _postProcessingBehaviour;

	void Start ()
    {
        _postProcessingBehaviour = GameManager.postProcessingBehaviour;
    }

    public void SetPostProcessing(bool postProcessEnabled)
    {
        _postProcessingBehaviour.enabled = postProcessEnabled;
    }
}
