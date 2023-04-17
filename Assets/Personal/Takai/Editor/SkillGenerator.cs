using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SkillBase))]
public class SkillGenerator : EditorWindow
{
    private string _skillName = "";
    private int _damage = 0;
    private WeaponType _weapon;
    private OreRarity _rarity;
    private SkillType _type;
    private PlayerSkillDataManagement _playerSkillDataManagement;

    private string _className = "";

    [MenuItem("Window/SkillGenerator")]
    static void Open()
    {
        var window = GetWindow<SkillGenerator>();
        window.titleContent = new GUIContent("SkillGenerator");
    }

    void OnGUI()
    {
        GUILayout.Label("�X�L���X�e�[�^�X�ݒ�");

        EditorGUILayout.BeginVertical(GUI.skin.box);
        _skillName = EditorGUILayout.TextField("�X�L����", _skillName);
        _damage = EditorGUILayout.IntField("�_���[�W", _damage);
        _weapon = (WeaponType)EditorGUILayout.EnumPopup("������", _weapon);
        _rarity = (OreRarity)EditorGUILayout.EnumPopup("���A���e�B", _rarity);
        _type = (SkillType)EditorGUILayout.EnumPopup("�^�C�v", _type);
        _className = EditorGUILayout.TextField("�N���X��", _className);
        if (GUILayout.Button("���Z�b�g"))
        {
            Reset();
        }

        if (GUILayout.Button("�쐬"))
        {
            CreatSkill();
        }

        EditorGUILayout.EndVertical();
    }

    private void Reset()
    {
        _skillName = "";
        _damage = 0;
        _skillName = "";
    }

    private void CreatSkill()
    {
        if (_skillName == "" || _skillName == "") { return; }
        string prefabPath = $"Assets/Resources/Skills/{_className}.prefab";
        GameObject newPrefab = new GameObject(_className);

        CreateClass();
        while(EditorApplication.isCompiling)
        {
            System.Threading.Thread.Sleep(100);
        }

        newPrefab.AddComponent(Type.GetType(_className));
        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        GameObject.DestroyImmediate(newPrefab);
        AssetDatabase.Refresh();
    }

    private void CreateClass()
    {
        string path = "";

        switch (_weapon)
        {
            case WeaponType.GreatSword:
                path = $"Assets/Personal/Takai/Script/Skills/GreatSword/{_className}.cs";
                break;
            case WeaponType.DualBlades:
                path = $"Assets/Personal/Takai/Script/Skills/DualBlades/{_className}.cs";
                break;
            case WeaponType.Hammer:
                path = $"Assets/Personal/Takai/Script/Skills/Hammer/{_className}.cs";
                break;
            case WeaponType.Spear:
                path = $"Assets/Personal/Takai/Script/Skills/Spear/{_className}.cs";
                break;
        }

        string classCode = "using System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing Cysharp.Threading.Tasks;\r\nusing UnityEngine.Playables;\r\n\r\npublic  class " + _className + " : SkillBase \r\n{\r\n    public string SkillName { get; set; }\r\n    public int Damage { get; set; }\r\n    public WeaponType Weapon { get; set; }\r\n    public OreRarity Rarity { get; set; }\r\n    public SkillType Type  { get; set; }\r\n    \r\n    private PlayableDirector _anim;\r\n\r\n    public override async UniTask UseSkill()\r\n    {\r\n        Debug.Log(\"Use Skill\");\r\n        _anim = GetComponent<PlayableDirector>();\r\n        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);\r\n        Debug.Log(\"Anim End\");\r\n    }\r\n\r\n    protected override void SkillEffect()\r\n    {\r\n        Debug.Log(\"Skill Effect\");\r\n    }\r\n}";
        File.WriteAllText(path, classCode);
    }
}