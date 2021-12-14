using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BGScroller : MonoBehaviour
{
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Image>().material;
        StartCoroutine("BGScrollCoroutine");
    }

    private IEnumerator BGScrollCoroutine()
    {
        while(true)
        {
            _material.mainTextureOffset += Time.deltaTime * GameManager.Instance.GameConfig.GetGameBGSpeed() * Vector2.up;
            yield return new WaitForEndOfFrame();
        }
    }
}
