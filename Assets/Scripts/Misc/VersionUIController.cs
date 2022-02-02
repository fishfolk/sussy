using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionUIController : MonoBehaviour
{
    public static VersionUIController Instance;
    [SerializeField] TextMeshProUGUI versionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            versionText.text = Application.productName + " " + Application.version;
        }
        else
            Destroy(this.gameObject);
    }
}
