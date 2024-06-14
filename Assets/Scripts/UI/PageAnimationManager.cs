using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class PageAnimationManager : MonoBehaviour
{
    public GameObject PageFlipL2RPrefab, PageFlipR2LPrefab;
    public RenderTexture RenderTexture;
    public Camera pageCapturer;
    public AudioSource PageFlipSource;
    public Transform BookStandTransform;
    public Texture2D CapturedPage;

    private List<GameObject> _activeAnimations = new();

    void Update()
    {
        var toBeDestroyed = new List<GameObject>();
        foreach (var flip in _activeAnimations)
        {
            if (flip.GetComponent<PlayableDirector>().state == PlayState.Paused)
                toBeDestroyed.Add(flip);
        }
        foreach (var flip in toBeDestroyed)
        {
            _activeAnimations.Remove(flip);
            Destroy(flip);
        }
    }

    public void AddLeftFlip()
    {
        //bool beginningReached = !book.PrevPage();
        //if (!beginningReached)
        {
            //isFlipping = true;
            //pageCapturer.Render();
            //RenderTexture.active = RenderTexture;
            //Graphics.CopyTexture(RenderTexture, CapturedPage);
            //CapturedPage.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0, false);
            //CapturedPage.Apply();

            PageFlipSource.Play();
            var flipInstance = Instantiate(PageFlipL2RPrefab, BookStandTransform);

            //var material = flipInstance.GetComponentsInChildren<MeshRenderer>().First().material;
            //material.mainTexture = CapturedPage;
            //Debug.Log(CapturedPage.EncodeToPNG());
            //System.IO.File.WriteAllBytes("C:\\Users\\Julius\\Downloads" + ".png", CapturedPage.EncodeToPNG());
            //isFlipping = false;
            //RenderTexture.active = null;

            _activeAnimations.Add(flipInstance);
        }
    }

    public void AddRightFlip()
    {
        //isFlipping = true;
        //pageCapturer.Render();
        //RenderTexture.active = RenderTexture;
        //Graphics.CopyTexture(RenderTexture, CapturedPage);
        //CapturedPage.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0, false);
        //CapturedPage.Apply();
        PageFlipSource.Play();
        var flipInstance = Instantiate(PageFlipR2LPrefab, BookStandTransform);
        //var material = flipInstance.GetComponentsInChildren<MeshRenderer>().First().material;
        //material.mainTexture = CapturedPage;
        //Debug.Log(CapturedPage.EncodeToPNG());
        //System.IO.File.WriteAllBytes("C:\\Users\\Julius\\Downloads" + ".png", CapturedPage.EncodeToPNG());
        //isFlipping = false;
        //RenderTexture.active = null;

        _activeAnimations.Add(flipInstance);
    }
}
