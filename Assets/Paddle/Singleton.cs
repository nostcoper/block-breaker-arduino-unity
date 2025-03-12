using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component // T es un simbolo generico que es heredado a un componente
{
    private static T _intance;
    public static T Intance
    {
        get
        {
            if(_intance == null) // sinifica que no esta instaceado
            {
                _intance = FindAnyObjectByType<T>();//busca el a T
                if(_intance == null)
                {
                    GameObject nuevoGameObjet = new GameObject();
                    _intance = nuevoGameObjet.AddComponent<T>();
                }
            }

            return _intance;

        }
    }

    private void Awake()
    {
        _intance = this as T;   
    }

}
