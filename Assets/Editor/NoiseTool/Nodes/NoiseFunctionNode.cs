using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Codice.CM.Common.Serialization;
using Packages.FastNoise2;
using QFSW.QC;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using World.WorldGeneration.DensityFunctions;
using XNode;
using XNodeEditor;
using Fractal = World.WorldGeneration.Noise.Fractal;

namespace Editor.NoiseTool.Nodes
{
    public class NoiseFunctionNode : Node
    {
        [SerializeReference]
        public NoiseFunction noiseFunction;
        public List<FieldInfo> ReadOnlyFields = new List<FieldInfo>();
        public float output;
        private FastNoise _fastNoise;
        private List<FieldInfo> _fields = new();
        private bool _initialized = false;
        
        public void Initialize()
        {
            if (_initialized) return;
            _fastNoise = new FastNoise(noiseFunction.GetType().Name.Replace("_", ""));
            foreach (var field in noiseFunction.GetType().GetFields())
                _fields.Add(field);
            SetValues();
            _initialized = true;
            
        }

        private void SetValues()
        {
            foreach (var field in _fields)
            {
                var fieldType = field.FieldType;
                if (fieldType == typeof(float))
                {
                    _fastNoise.Set(field.Name, (float)field.GetValue(noiseFunction));
                }
                else if (fieldType == typeof(int))
                {
                    _fastNoise.Set(field.Name, (int)field.GetValue(noiseFunction));
                }else if (fieldType == typeof(string))
                {
                    _fastNoise.Set(field.Name, (string)field.GetValue(noiseFunction));
                }
            }

        }
        public override object GetValue(NodePort port)
        {
                
            return _fastNoise;
        }

        public Texture2D CreateNoiseMap(float frequency)
        {
            if (!_initialized)
                Initialize();
            if (_fastNoise is null)
                return null;
            var output = new float[256*256];
            try
            {
                _fastNoise.GenUniformGrid2D(output, 0, 0, 256,256, frequency, 0);
                
            }catch(Exception e)
            {
                Debug.Log(e);
            }
            var texture = new Texture2D(256, 256);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    var value = (1 + output[i * 256 + j]) / 2;
                    texture.SetPixel(i, j, new Color(value, value, value));
                }
            }
            texture.Apply();
            return texture;
        }
    }
    
    [CustomNodeEditor(typeof(NoiseFunctionNode))]
    public class DensityFunctionNodeEditor : NodeEditor
    {
        public static readonly HashSet<Type> StandAlone = new HashSet<Type>
        {
            typeof(CoherentNoise),
            typeof(BasicGenerators)
        };
        private NoiseFunctionNode _node;
        private bool _changed = true;
        private Texture2D _texture;
        private uint _hash = 2;
        private bool _hasSource = false;
        public override void OnBodyGUI()
        {
            if (_node is null) _node = target as NoiseFunctionNode;
            serializedObject.Update();
            
            var obj = serializedObject.FindProperty("noiseFunction");
            uint hash = 0;
            foreach (var field in _node.ReadOnlyFields)
            {
                var property = obj.FindPropertyRelative(field.Name);
                NodeEditorGUILayout.PropertyField(property);
                hash = (uint) (hash * 397) ^ (uint) property.contentHash;
            }

            foreach (var port in _node.DynamicInputs)
            {
                var property = obj.FindPropertyRelative(port.fieldName);
                Debug.Log(property.propertyType.GetType());
                NodeEditorGUILayout.PropertyField(property, port);
                hash = (uint) (hash * 397) ^ (uint) property.contentHash;                
            }

            foreach (var port in _node.DynamicOutputs)
            {
                var property = serializedObject.FindProperty(port.fieldName);
                NodeEditorGUILayout.PropertyField(property, port);
                hash = (uint) (hash * 397) ^ (uint) property.contentHash;
            }

            // Debug.Log(hash);
            // if (StandAlone.Contains(_node.noiseFunction.GetType().DeclaringType))
            // {
            //     if(hash != _hash)
            //         _texture = _node.CreateNoiseMap(0.1f);
            //     GUILayout.Label(_texture);
            // }
            // _hash = hash;

            _changed = false;
            serializedObject.ApplyModifiedProperties();
        }
    }
}