using QZGameFramework.GameTool;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class QZGameframeworkWindow : OdinMenuEditorWindow
{
    [MenuItem("GameTool/SettingsWindow")]
    private static void OpenQZGameframeworkSettingWindow()
    {
        QZGameframeworkWindow window = GetWindow<QZGameframeworkWindow>("Settings Window");
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    [SerializeField]
    private ExcelTool excelTool = new ExcelTool();

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Settings",                           this,                           EditorIcons.House                       },
                { "Odin Settings",                  null,                           SdfIconType.GearFill                    },
                { "Odin Settings/Color Palettes",   ColorPaletteManager.Instance,   SdfIconType.PaletteFill                 },
                { "Odin Settings/AOT Generation",   AOTGenerationConfig.Instance,   EditorIcons.SmartPhone                  },
            };

        return tree;
    }

    protected override void OnImGUI()
    {
        base.OnImGUI();
    }
}