using UnityEngine;
using System.IO.Ports;

public class ArduinoBotones : MonoBehaviour
{
    [Header("Configuraci�n del Puerto Serial")]
    // Cambia "COM3" por el puerto al que est� conectado tu Arduino
    SerialPort sp = new SerialPort("COM4", 9600);    

    void Start()
    {
        // Configuramos el tiempo de espera de lectura y abrimos el puerto
        sp.ReadTimeout = 50;
        try
        {
            sp.Open();
            Debug.Log("Puerto serial abierto correctamente.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al abrir el puerto serial: " + e.Message);
        }
    }

    // Funci�n gen�rica para enviar comandos, que puede ser llamada desde la UI
    public void EnviarComando(string comando)
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Write(comando);
            Debug.Log("Comando enviado: " + comando);
        }
        else
        {
            Debug.LogWarning("El puerto serial no est� abierto.");
        }
    }

    void OnApplicationQuit()
    {
        if (sp != null && sp.IsOpen)
            sp.Close();
    }
}
