using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	internal class AssetPopupBackend
	{
		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache0;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache1;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache2;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache3;

		public static void AssetPopup<T>(SerializedProperty serializedProperty, GUIContent label, string fileExtension, string defaultFieldName) where T : UnityEngine.Object, new()
		{
			bool showMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = serializedProperty.hasMultipleDifferentValues;
			string objectReferenceTypeString = serializedProperty.objectReferenceTypeString;
			GUIContent buttonContent;
			if (serializedProperty.hasMultipleDifferentValues)
			{
				buttonContent = EditorGUI.mixedValueContent;
			}
			else if (serializedProperty.objectReferenceValue != null)
			{
				buttonContent = GUIContent.Temp(serializedProperty.objectReferenceStringValue);
			}
			else
			{
				buttonContent = GUIContent.Temp(defaultFieldName);
			}
			Rect buttonRect;
			if (AudioMixerEffectGUI.PopupButton(label, buttonContent, EditorStyles.popup, out buttonRect, new GUILayoutOption[0]))
			{
				AssetPopupBackend.ShowAssetsPopupMenu<T>(buttonRect, objectReferenceTypeString, serializedProperty, fileExtension, defaultFieldName);
			}
			EditorGUI.showMixedValue = showMixedValue;
		}

		public static void AssetPopup<T>(SerializedProperty serializedProperty, GUIContent label, string fileExtension) where T : UnityEngine.Object, new()
		{
			AssetPopupBackend.AssetPopup<T>(serializedProperty, label, fileExtension, "Default");
		}

		private static void ShowAssetsPopupMenu<T>(Rect buttonRect, string typeName, SerializedProperty serializedProperty, string fileExtension, string defaultFieldName) where T : UnityEngine.Object, new()
		{
			GenericMenu genericMenu = new GenericMenu();
			int num = (!(serializedProperty.objectReferenceValue != null)) ? 0 : serializedProperty.objectReferenceValue.GetInstanceID();
			bool flag = false;
			UnityType unityType = UnityType.FindTypeByName(typeName);
			int num2 = (unityType == null) ? 0 : unityType.persistentTypeID;
			BuiltinResource[] array = null;
			if (num2 > 0)
			{
				array = EditorGUIUtility.GetBuiltinResourceList(num2);
				BuiltinResource[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					BuiltinResource resource = array2[i];
					if (resource.m_Name == defaultFieldName)
					{
						GenericMenu arg_124_0 = genericMenu;
						GUIContent arg_124_1 = new GUIContent(resource.m_Name);
						bool arg_124_2 = resource.m_InstanceID == num;
						if (AssetPopupBackend.<>f__mg$cache0 == null)
						{
							AssetPopupBackend.<>f__mg$cache0 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
						}
						arg_124_0.AddItem(arg_124_1, arg_124_2, AssetPopupBackend.<>f__mg$cache0, new object[]
						{
							resource.m_InstanceID,
							serializedProperty
						});
						array = (from x in array
						where x != resource
						select x).ToArray<BuiltinResource>();
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				GenericMenu arg_1A6_0 = genericMenu;
				GUIContent arg_1A6_1 = new GUIContent(defaultFieldName);
				bool arg_1A6_2 = num == 0;
				if (AssetPopupBackend.<>f__mg$cache1 == null)
				{
					AssetPopupBackend.<>f__mg$cache1 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
				}
				arg_1A6_0.AddItem(arg_1A6_1, arg_1A6_2, AssetPopupBackend.<>f__mg$cache1, new object[]
				{
					0,
					serializedProperty
				});
			}
			HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
			SearchFilter searchFilter = new SearchFilter
			{
				classNames = new string[]
				{
					typeName
				}
			};
			hierarchyProperty.SetSearchFilter(searchFilter);
			hierarchyProperty.Reset();
			while (hierarchyProperty.Next(null))
			{
				GenericMenu arg_23D_0 = genericMenu;
				GUIContent arg_23D_1 = new GUIContent(hierarchyProperty.name);
				bool arg_23D_2 = hierarchyProperty.instanceID == num;
				if (AssetPopupBackend.<>f__mg$cache2 == null)
				{
					AssetPopupBackend.<>f__mg$cache2 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
				}
				arg_23D_0.AddItem(arg_23D_1, arg_23D_2, AssetPopupBackend.<>f__mg$cache2, new object[]
				{
					hierarchyProperty.instanceID,
					serializedProperty
				});
			}
			if (num2 > 0 && array != null)
			{
				BuiltinResource[] array3 = array;
				for (int j = 0; j < array3.Length; j++)
				{
					BuiltinResource builtinResource = array3[j];
					GenericMenu arg_2C7_0 = genericMenu;
					GUIContent arg_2C7_1 = new GUIContent(builtinResource.m_Name);
					bool arg_2C7_2 = builtinResource.m_InstanceID == num;
					if (AssetPopupBackend.<>f__mg$cache3 == null)
					{
						AssetPopupBackend.<>f__mg$cache3 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
					}
					arg_2C7_0.AddItem(arg_2C7_1, arg_2C7_2, AssetPopupBackend.<>f__mg$cache3, new object[]
					{
						builtinResource.m_InstanceID,
						serializedProperty
					});
				}
			}
			genericMenu.AddSeparator("");
			genericMenu.AddItem(new GUIContent("Create New..."), false, delegate
			{
				T t = Activator.CreateInstance<T>();
				ProjectWindowUtil.CreateAsset(t, "New " + typeName + "." + fileExtension);
				serializedProperty.objectReferenceValue = t;
				serializedProperty.m_SerializedObject.ApplyModifiedProperties();
			});
			genericMenu.DropDown(buttonRect);
		}

		private static void ShowAssetsPopupMenu<T>(Rect buttonRect, string typeName, SerializedProperty serializedProperty, string fileExtension) where T : UnityEngine.Object, new()
		{
			AssetPopupBackend.ShowAssetsPopupMenu<T>(buttonRect, typeName, serializedProperty, fileExtension, "Default");
		}

		private static void AssetPopupMenuCallback(object userData)
		{
			object[] array = userData as object[];
			int instanceID = (int)array[0];
			SerializedProperty serializedProperty = (SerializedProperty)array[1];
			serializedProperty.objectReferenceValue = EditorUtility.InstanceIDToObject(instanceID);
			serializedProperty.m_SerializedObject.ApplyModifiedProperties();
		}
	}
}
