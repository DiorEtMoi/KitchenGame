using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectUI : MonoBehaviour
{
    [SerializeField] private int colorID;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedObject;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenObjectNetworkManager.instance.ChangePlayerColor(colorID);
        });
    }
    private void Start()
    {
        KitchenObjectNetworkManager.instance.OnListDataPlayerChanged += Instance_OnListDataPlayerChanged;
        image.color = KitchenObjectNetworkManager.instance.GetPlayerColor(colorID);
        UpdateIsSelected();
    }

    private void Instance_OnListDataPlayerChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if(KitchenObjectNetworkManager.instance.GetPlayerData().colorId == colorID)
        {
            selectedObject.SetActive(true);
        }
        else
        {
            selectedObject.SetActive(false);
        }
    }
    public void OnDestroy()
    {
        KitchenObjectNetworkManager.instance.OnListDataPlayerChanged -= Instance_OnListDataPlayerChanged;
    }
}
