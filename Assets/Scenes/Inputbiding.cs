using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Windows.Input;


public class InputBinding : MonoBehaviour
{
    [SerializeField] private InputInfos[] inputs;
    private Dictionary<string, char> inputsDictionnary = new Dictionary<string, char>();

    private string BindingAxis = "";
    void Start()
    {
        foreach (InputInfos _input in inputs)
        {
            inputsDictionnary.Add(_input.Name, _input.Key);
        }
    }

    void Update()
    {
        if (BindingAxis != "")
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                foreach (KeyCode _keycode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (UnityEngine.Input.GetKey(_keycode))
                    {
                        inputsDictionnary[BindingAxis] = (char)_keycode;
                        BindingAxis = "";
                        return;
                    }
                }
            }
        }
        foreach (var _inputAxis in inputsDictionnary.Keys)
        {
            TestInput(_inputAxis);
        }
    }
    
    public void Bind(string _axis) => BindingAxis = _axis;

    private void TestInput(string _inputAxis)
    {
        bool _input = UnityEngine.Input.GetKey((KeyCode)inputsDictionnary[_inputAxis]);
        if (_input)
        {
            Debug.Log(_inputAxis + " : " + inputsDictionnary[_inputAxis]);
        }
    }
}

[System.Serializable]
public struct InputInfos
{ 
    [SerializeField] private string inputName;

    [SerializeField] private char key;
    
    public string Name => inputName;
    
    public char Key => key;
        
        
}