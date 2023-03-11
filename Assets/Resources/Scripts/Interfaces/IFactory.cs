using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of IFactory script: 
///ENDINFO

public interface IFactory
{
    public GameObject Produce();
}
