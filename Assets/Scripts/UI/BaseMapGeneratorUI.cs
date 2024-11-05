using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseMapGeneratorUI : MonoBehaviour
{
    [SerializeField]private TMP_InputField inputField_width;
    [SerializeField]private TMP_InputField inputField_height;
    [SerializeField]private TMP_Dropdown dropdown_type;
    
    private int width;
    private int height;
    private MapType type;
    
    
    public void GenerateBaseMap()
    {
        
        if (!int.TryParse(inputField_width.text, out width))
        {
            width = 8;
        }
        
        else if (!int.TryParse(inputField_height.text, out height))
        {
            height = 8;
        }

        if (width == 0)
        {
            width = 8;
        }

        if (height == 0)
        {
            height = 8;
        }
        

        switch (dropdown_type.value)
        {
            case 0:
                type = MapType.Space;
                break;
            case 1:
                type = MapType.Dungeon;
                break;
            case 2:
                type = MapType.Maze;
                break;
        }
        
        MapEditor.Instance.generator.GenerateDefaultMap(type,width,height);
        Destroy(gameObject);
    }

}
