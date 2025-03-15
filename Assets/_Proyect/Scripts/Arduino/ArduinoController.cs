using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
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

    void Update()
    {
        // Ejemplo de control mediante pulsaciones de teclas:
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnviarComando("R");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            EnviarComando("G");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EnviarComando("Y");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnviarComando("O");
        }
    }

    // Funci�n para enviar comandos al Arduino
    void EnviarComando(string comando)
    {
        if (sp.IsOpen)
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
        // Al cerrar la aplicaci�n, se cierra el puerto serial
        if (sp.IsOpen)
        {
            sp.Close();
        }
    }
}
