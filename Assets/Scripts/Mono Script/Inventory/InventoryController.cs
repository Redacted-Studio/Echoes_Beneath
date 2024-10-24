using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }
    //Other
    InventoryItem selectedItem;
    InventoryItem OverlapItem;
    RectTransform rectTransform;
    [SerializeField] List<ItemSize> items;
    [SerializeField] private GameObject gameObject1;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }
    private void Update()
    {
        //Buat Ngebantu Debug aja
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RandomItemSpawn();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InsertRandomItem();
        }
        */
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            return;
        }
        //Buat Ngedrag Item
        ItemIconDrag();

        if (SelectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        //Buat Highlight Item
        HandleHighlight();
        //Ketika Klik kiri mouse
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonclick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickButtonClick();
        }

    }

    //Buat Rotate Item
    private void RotateItem()
    {
        if (selectedItem == null) return;

        selectedItem.Rotate();

    }
    //Buat Bantu Debug aja
    private void InsertRandomItem()
    {
        if (selectedItemGrid == null) return;
        RandomItemSpawn();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }
    //Insert Item Otomatis ke Item Grid
    private void InsertItem(InventoryItem itemToInsert)
    {
        //Cari Posisi Kosong yang muat di Grid
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        //Kalo gk ada return
        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            return;
        }
        
        //kalo ada di masukin ke posisi yang udh didapetin
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);

    }
    public void RandomItemSpawn()
    {
        InventoryItem inventoryItem = Instantiate(gameObject1).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);

    }

    Vector2Int Oldposition;
    InventoryItem ItemtoHighlight;

    //Buat ngurus HighLight
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (Oldposition == positionOnGrid) return;
        Oldposition = positionOnGrid;

        if(selectedItem == null)
        {
            ItemtoHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if(ItemtoHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.Setsize(ItemtoHighlight);
                inventoryHighlight.SetPostion(SelectedItemGrid, ItemtoHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }

        }
        else
        {
            inventoryHighlight.Show(SelectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH, 
                selectedItem.HEIGHT));
            inventoryHighlight.Setsize(selectedItem);
            inventoryHighlight.SetPosition(SelectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }
    
    //Ketika klik kiri
    private void LeftMouseButtonclick()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }

    }

    private void RightClickButtonClick()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            UseItem(tileGridPosition);
        }
        else
        {
            return;
        }
    }

    //Buat dapet posisi TileGrid
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.TileSizeHeight / 2;
        }

        return SelectedItemGrid.GetTileGridPosition(position);
    }

    //Buat Input PickUp Item
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void UseItem(Vector2Int tileGridPosition)
    {
        SelectedItemGrid.UseItem(tileGridPosition.x, tileGridPosition.y);
    }

    //Buat Input Place Item
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref OverlapItem);
        if (complete == true)
        {
            selectedItem = null;
            if (OverlapItem != null)
            {
                selectedItem = OverlapItem;
                OverlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }

    //Buat Ngedrag Item
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
