using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : Singleton<ActionController>
{
    [SerializeField]
    private InventoryManager inventoryManager;

    [SerializeField]
    private float itemMaxRange = 5f;  // ������ ������ ������ �ִ� �Ÿ�

    private bool pickupActivated = false;  // ������ ���� �����ҽ� True 

    private RaycastHit hitInfo;  // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask;  // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.

    [SerializeField]
    private Text actionText;  // �ൿ�� ���� �� �ؽ�Ʈ

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
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>" + "[F]" + "</color>";
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");  // �κ��丮 �ֱ�
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