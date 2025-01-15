using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuResumeButton : MonoBehaviour
{
    public static MenuResumeButton Instance;
    public bool resumeButtonEnabled = false;
    public GameObject resumeButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (resumeButtonEnabled == true) {
            resumeButton.SetActive(true);
        }
        else {
            resumeButton.SetActive(false);
        }
    }

    public void EnableResumeButton() {
        resumeButtonEnabled = true;
    }

    public void DisableResumeButton() {
        resumeButtonEnabled = false;
    }
}
