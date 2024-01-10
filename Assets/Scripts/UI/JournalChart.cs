using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalChart : MonoBehaviour {

    [SerializeField] private float normaliseFactor = 1f;
    [SerializeField] private GameObject[] lines = new GameObject[12];
    
    void Start() {
        
    }

    public void updateChart(int[] vals) {
        for (int i = 0; i < vals.Length; i++) {
            RectTransform rect = lines[i].GetComponent<RectTransform>();
            rect.localScale = new Vector3(1f, vals[i] * normaliseFactor, 0f);
            rect.localEulerAngles = new Vector3(0, 0, (i * -30));
        }
    }

    void Update() {

    }
}
