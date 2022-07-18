using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AsteroiderCore.Settings.Editor
{
    [CustomEditor(typeof(LayerCollisionMatrix))]
    public sealed class LayerCollisionMatrixEditor : UnityEditor.Editor
    {
        private class Styles
        {
            public static readonly GUIStyle RightLabel = new("RightLabel");
            public static readonly GUIStyle HoverStyle = GetHoverStyle();
            private static readonly Color TransparentColor = new(1, 1, 1, 0);
            private static readonly Color HighlightColor = EditorGUIUtility.isProSkin ? 
                new Color(1, 1, 1, 0.2f) : new Color(0,0,0, 0.2f);
            private static GUIStyle GetHoverStyle()
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);

                Texture2D texNormal = new Texture2D(1,1){alphaIsTransparency = true};
                texNormal.SetPixel(1,1, TransparentColor);
                texNormal.Apply();

                Texture2D texHover = new Texture2D(1,1){alphaIsTransparency = true};
                texHover.SetPixel(1,1, HighlightColor);
                texHover.Apply();

                style.normal.background = texNormal;
                style.hover.background = texHover;

                return style;
            }
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            serializedObject.Update();
            
            const int checkboxSize = 16;
            int labelSize = 110;
            const int indent = 30;
            var enumNames = Enum.GetNames(typeof(CollisionLayer));
            var enumValues = Enum.GetValues(typeof(CollisionLayer)).Cast<int>().ToArray();
            int numLayers = enumNames.Length;
            
            //ищем самый длинный лейбл
            for (int i = 0; i < enumNames.Length; i++)
            {
                var textDimensions = GUI.skin.label.CalcSize(new GUIContent(enumNames[i]));
                if (labelSize < textDimensions.x)
                    labelSize = (int)textDimensions.x;
            }
            
            var topLabelRect = GUILayoutUtility.GetRect(checkboxSize * numLayers + labelSize, labelSize);
            var topLeft = new Vector2(topLabelRect.x - 10, topLabelRect.y);
            var y = 0;
            foreach (var t in enumNames)
            {
                var translate = new Vector3(labelSize + indent + checkboxSize * (numLayers - y) + topLeft.x + 10, topLeft.y, 0);
                
                GUI.matrix = Matrix4x4.TRS(translate, Quaternion.identity, Vector3.one) 
                             * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
                
                var labelRect = new Rect(2 - topLeft.x, 0, labelSize, checkboxSize + 5);
                GUI.Label(labelRect, t, Styles.RightLabel);
                y++;
            }
            
            GUI.matrix = Matrix4x4.identity;
            y = 0;
            for (int i = 0; i < enumNames.Length; i++)
            {
                int x = 0;
                var r = GUILayoutUtility.GetRect(indent + checkboxSize * numLayers + labelSize, checkboxSize);
                var labelRect = new Rect(r.x + indent, r.y, labelSize, checkboxSize + 5);
                GUI.Label(labelRect, enumNames[i], Styles.RightLabel);
                
                var checkRect = new Rect(r.x + indent, r.y, labelSize + (numLayers - y) * checkboxSize, checkboxSize);
                GUI.Label(checkRect, GUIContent.none, Styles.HoverStyle);

                for (int j = enumNames.Length - 1; j >= 0; j--)
                {
                    if (x < numLayers - y)
                    {
                        var tooltip = new GUIContent("", enumNames[i] + "/" + enumNames[j]);
                        bool val = GetValue(enumValues[i], enumValues[j]);
                        bool toggle = GUI.Toggle(new Rect(labelSize + indent + r.x + x * checkboxSize, 
                            r.y, checkboxSize, checkboxSize), val, tooltip);
                        if (toggle != val)
                            SetValue(enumValues[i], enumValues[j], toggle);
                    }
                    x++;
                }
                y++;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void SetValue(int firstValue, int secondValue, bool toggle)
        {
            if (firstValue == secondValue)
            {
                var enumProperty = GetEnumProperty(firstValue);
                var hasFlag = HasFlag(enumProperty, secondValue);
                if(toggle && !hasFlag)
                {
                    enumProperty.intValue += secondValue;
                }
                else if(!toggle && hasFlag)
                {
                    enumProperty.intValue -= secondValue;
                }
                return;
            }
            
            var firstEnumProperty = GetEnumProperty(firstValue);
            var secondEnumProperty = GetEnumProperty(secondValue);
            var hasFlagFirst = HasFlag(firstEnumProperty, secondValue);
            var hasFlagSecond = HasFlag(secondEnumProperty, firstValue);

            if(toggle && !hasFlagFirst && !hasFlagSecond)
            {
                firstEnumProperty.intValue += secondValue;
                secondEnumProperty.intValue += firstValue;
            }
            else if(!toggle && hasFlagFirst && hasFlagSecond)
            {
                firstEnumProperty.intValue -= secondValue;
                secondEnumProperty.intValue -= firstValue;
            }
        }

        private bool GetValue(int firstValue, int secondValue)
        {
            if (firstValue == secondValue)
            {
                var enumProperty = GetEnumProperty(firstValue);
                var hasFlag = HasFlag(enumProperty, secondValue);
                return hasFlag;
            }

            var firstEnumProperty = GetEnumProperty(firstValue);
            var secondEnumProperty = GetEnumProperty(secondValue);
            var hasFlagFirst = HasFlag(firstEnumProperty, secondValue);
            var hasFlagSecond = HasFlag(secondEnumProperty, firstValue);
            
            return hasFlagFirst && hasFlagSecond;
        }

        private bool HasFlag(SerializedProperty enumProperty, int compareValue)
        {
            var enumValue = enumProperty.intValue;
            var elementEnum = (CollisionLayer) enumValue;
            var checkEnum = (CollisionLayer) compareValue;
            return elementEnum.HasFlag(checkEnum);
        }

        private SerializedProperty GetEnumProperty(int value)
        {
            var enumValue = (CollisionLayer) value;
            return enumValue switch
            {
                CollisionLayer.Player => serializedObject.FindProperty("player"),
                CollisionLayer.Asteroid => serializedObject.FindProperty("asteroid"),
                CollisionLayer.Bullet => serializedObject.FindProperty("bullet"),
                CollisionLayer.UFO => serializedObject.FindProperty("ufo"),
                _ => throw new Exception($"Can't find element in {nameof(CollisionLayer)} with {value} value")
            };
        }
    }
}
