using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatternData))]
public class PatternDataInspector : Editor
{
   private PatternData patternData;
   private List<Vector2Int> selectedPositions = new List<Vector2Int>();
   private Texture2D whiteTexture;
   private Texture2D greenTexture;
   private Texture2D grayTexture;
   private bool m_hasChanged;

   private void OnEnable()
   {
      patternData = (PatternData)target;
      whiteTexture = CreateTexture(Color.white);
      greenTexture = CreateTexture(Color.green);
      grayTexture = CreateTexture(Color.gray);
      selectedPositions = new List<Vector2Int>(patternData.Pattern);
   }

   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      RenderInfo();
      EditorGUILayout.Space();
      RenderVisualGrid();
      EditorGUILayout.Space();
      EditorGUILayout.Space();
      if (GUILayout.Button("Save",GUILayout.Height(30)))
      {
         Save();
      }
      EditorGUILayout.Space();
      if (GUILayout.Button("Clear",GUILayout.Height(30)))
      {
         Clear();
      }
   }

   private void RenderInfo()
   {
      EditorGUILayout.HelpBox("The grey center cell is the host of this pattern.\n It will be automatically added to the list.", MessageType.Info);
   }
   
   private void RenderVisualGrid()
   {
      m_hasChanged = false;
      
      Vector2Int centerPos = new Vector2Int(patternData.GridWidth / 2, patternData.GridHeight / 2);
      if (!selectedPositions.Contains(centerPos))
      {
         selectedPositions.Add(centerPos);
         m_hasChanged = true;
      }
      
      for (int j = 0; j < patternData.GridHeight; j++) // Outer: Rows (top to bottom)
      {
         EditorGUILayout.BeginHorizontal();
         for (int i = 0; i < patternData.GridWidth; i++) // Inner: Columns (left to right)
         {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.background = Texture2D.whiteTexture;
      
            var pos = new Vector2Int(i, j);
            bool isSelected = selectedPositions.Contains(pos);
            bool isCenter = pos == centerPos;
            if (isCenter) style.normal.background = grayTexture;
            else if (isSelected) style.normal.background = greenTexture;
            else style.normal.background = whiteTexture;
      
            if (GUILayout.Button("", style, GUILayout.Width(30), GUILayout.Height(30)))
            {
               if (!isCenter)
               {
                  if (isSelected)
                  {
                     selectedPositions.Remove(pos);
                  }
                  else
                  {
                     selectedPositions.Add(pos);
                  }

                  m_hasChanged = true;
               }
            }
         }
         EditorGUILayout.EndHorizontal();
      }
      
      if (m_hasChanged)
      {
         Undo.RecordObject(patternData, "Pattern Change");
         List<Vector2Int> sortedPositions = new List<Vector2Int>(selectedPositions);
         sortedPositions.Sort((a, b) =>
         {
            int rowComparison = a.y.CompareTo(b.y);
            return rowComparison != 0 ? rowComparison : a.x.CompareTo(b.x);
         });
         patternData.Pattern.Clear();
         patternData.Pattern.AddRange(sortedPositions);
      }
   }

   private void Clear()
   {
      for (int j = 0; j < patternData.GridHeight; j++) // Outer: Rows (top to bottom)
      {
         EditorGUILayout.BeginHorizontal();
         for (int i = 0; i < patternData.GridWidth; i++) // Inner: Columns (left to right)
         {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.background = Texture2D.whiteTexture;
            if (GUILayout.Button("", style, GUILayout.Width(30), GUILayout.Height(30))) { }
         }
         EditorGUILayout.EndHorizontal();
      }
         
      selectedPositions.Clear();
      patternData.Pattern.Clear();
      
      Save();
   }
   
   private void Save()
   {
      EditorUtility.SetDirty(patternData);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh(); 
   }
   

   private Texture2D CreateTexture(Color color)
   {
      Texture2D tex = new Texture2D(1, 1);
      tex.SetPixel(0, 0, color);
      tex.Apply();
      return tex;
   }
}
