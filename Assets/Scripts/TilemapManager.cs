using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField]
    Tilemap floor;
    [SerializeField]
    Tilemap levelBounds;
    [SerializeField]
    Tilemap columns;
    [SerializeField]
    Tilemap obstacles;

    [SerializeField]
    Tile unbreakableWall;

    [SerializeField]
    Tile wall3HP;
    [SerializeField]
    Tile wall2HP;
    [SerializeField]
    Tile wall1HP;

  

    //����� �� ������ ���� ������, ������ ��� ������� �� ��� ������ ��������
    public Tilemap FloorTilemap
    {
        get
        {
            return floor;
        }
    }

    public Tilemap Walls
    {
        get
        {
            return levelBounds;
        }
    }


    public Tilemap Columns
    {
        get
        {
            return columns;
        }
    }

    public Tilemap Obstacles
    {
        get
        {
            return obstacles;
        }
    }

    //���������� true, ���� ����� ��� ����� ����
    public bool hitCell(Vector3Int cell)
    {
        TileBase tile = obstacles.GetTile(cell);
        //switch case ����� ������������ ������ ��� ���������� ��������, � ������������� ���� �������� �� ��������, ������� if else 
        if (tile == wall3HP)
        {
            obstacles.SetTile(cell, wall2HP);
            return true;
        }

        if (tile == wall2HP)
        {
            obstacles.SetTile(cell, wall1HP);
            return true;
        }
        if (tile == wall1HP)
        {
            obstacles.SetTile(cell, null);
            return true;
        }
        return false;
    }
   

    public static TilemapManager StaticInstance
    {
        get; private set;
    }

    private void Awake()
    {
        StaticInstance = this;


    }


}
