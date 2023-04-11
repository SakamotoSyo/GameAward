using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OreUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Image _oreImage;
    [SerializeField] private Text _rearityText;
    [SerializeField] private Text _enhanceText;
    private OreData _data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _infoPanel.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _infoPanel.SetActive(false);
    }

    public void SetData(OreData oreData) 
    {
        _rearityText.text = oreData.Rarity.ToString();
        for (int i = 0; i < oreData.EnhancedData.Length; i++) 
        {
            _enhanceText.text += oreData.EnhancedData[i].EnhanceDescription;
            _enhanceText.text += " ";
        }
        _oreImage = oreData.OreImage;
    }
}