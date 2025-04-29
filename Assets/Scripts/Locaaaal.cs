using Assembly_CSharp;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Locaaaal", menuName = "Scriptable Objects/Locaaaal")]
public class Locaaaal : ScriptableObject
{
    public List<LocalizationPair> Russian;
    public List<LocalizationPair> English;
}
