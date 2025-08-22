using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OffLayoutGroup : MonoBehaviour
{

    private void Start()
    {
        if (TryGetComponent(out LayoutGroup layout))
        {
            StartCoroutine(DisableLayout(layout));
        }
    }
    private IEnumerator DisableLayout(LayoutGroup layout)
    {
        yield return new WaitForSeconds(1f);
        layout.enabled = false;
    }
}
