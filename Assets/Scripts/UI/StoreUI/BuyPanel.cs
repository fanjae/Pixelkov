using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] private ProductUI product;
    [SerializeField] private RectTransform content;

    private List<ProductUI> productUIs = new List<ProductUI>();

    /// <summary>
    /// ItemId와 구매 메서드를 파라미터로 받아서 생성한 product객체를 초기화 합니다.
    /// </summary>
    public void AddProduct(int itemId, Action<int> buyAction)
    {
        if (product == null || content == null) return;

        ProductUI newProduct = Instantiate(product, content);
        newProduct.Init(itemId);
        newProduct.OnBuyAction += buyAction;
        productUIs.Add(newProduct);
        // content 사이즈를 동적으로 조절
        content.sizeDelta = new Vector2(content.sizeDelta.x, productUIs.Count * 130);
    }
    /// <summary>
    /// 생성했던 product에 할당된 이벤트들을 취소하는 메서드입니다.
    /// </summary>
    public void AllocEvent(Action<int> buyAction)
    {
        foreach(var product in productUIs)
        {
            product.OnBuyAction -= buyAction;
        }
    }
}
