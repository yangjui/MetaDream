using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : Singleton<ActionController>
{
    [SerializeField]
    private InventoryManager inventoryManager;

    [SerializeField]
    private float itemMaxRange = 5f;  // 아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false;  // 아이템 습득 가능할시 True 

    private RaycastHit hitInfo;  // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask;  // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.

    [SerializeField]
    private Text actionText;  // 행동을 보여 줄 텍스트

    private void Update()
    {
        if (SettingManager.Instance.isSettingMenuOn == false)
        {
            CheckItem();
            TryAction();
        }
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, itemMaxRange, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                Debug.Log("Q");
                ItemInfoAppear();
            }
        }
        else
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "[F]" + "</color>";
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");  // 인벤토리 넣기
                if (Item.ItemType.Equipment == hitInfo.transform.GetComponent<ItemPickUp>().item.itemType)
                {
                    inventoryManager.AcquireItemKey(hitInfo.transform.GetComponent<ItemPickUp>().item);
                }
                else
                {
                    inventoryManager.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                }

                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}