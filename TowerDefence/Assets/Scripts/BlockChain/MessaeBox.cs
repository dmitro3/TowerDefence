using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessaeBox : MonoBehaviour
{
    public static MessaeBox insta;
    [SerializeField] GameObject msgBoxUI;
    [SerializeField] GameObject okBtn;
    [SerializeField] TMP_Text msgText;



    private void Awake()
    {
        insta = this;
        msgBoxUI.SetActive(false);
    }

    Coroutine coroutine;
    public void showMsg(string _msg, bool showBtn)
    {
        StopAllCoroutines();

        msgBoxUI.SetActive(true);
        if (showBtn) okBtn.SetActive(true);
        else okBtn.SetActive(false);

        msgText.text = _msg;

        coroutine=StartCoroutine(WaitToShowOk());
    }

   

    IEnumerator WaitToShowOk()
    {
        if (!okBtn.activeSelf)
        {
            yield return new WaitForSecondsRealtime(40);
            okBtn.SetActive(true);
        }
    }

    public void OkButton()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }        
        msgBoxUI.SetActive(false);
    }

    [Header("Information References")]
    [SerializeField] Image img_info;
    [SerializeField] TMP_Text info_text;
    [SerializeField] GameObject infoObject;
    [SerializeField] Sprite defaultSprite;
    Coroutine coroutine2;

    public void ShowInformationMsg(Color msg_sprite_color,string text,Sprite sprite=null)
    {
        if (sprite != null)
        {
            img_info.sprite = sprite;
        }
        else
        {
            img_info.sprite = defaultSprite;
        }

        img_info.color = msg_sprite_color;
        info_text.text = text;
        infoObject.SetActive(true);
        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        coroutine2 = StartCoroutine(closeInfo());
    }
    IEnumerator closeInfo()
    {
        yield return new WaitForSeconds(5f);
        infoObject.SetActive(false);
    }
}
