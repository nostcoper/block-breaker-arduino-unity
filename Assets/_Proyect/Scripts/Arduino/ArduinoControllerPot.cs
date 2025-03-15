using UnityEngine;
using System.IO.Ports;

public class ArduinoControllerPot : Singleton<ArduinoControllerPot>
{   

    public string portName = "COM4";
    public int baudRate = 9600;
    private SerialPort serialPort;

    public float minX = -8f;
    public float maxX = 8f;

    public GameObject paddle;


    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 50;
        try
        {
            serialPort.Open();
            Debug.Log("Puerto serial abierto en " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al abrir el puerto serial: " + e.Message);
        }

        if (paddle == null)
        {
            paddle = GameObject.FindWithTag("Paddle");
            if (paddle != null)
                Debug.Log("Paddle encontrado autom�ticamente.");
            else
                Debug.LogWarning("No se encontr� ning�n GameObject con la etiqueta 'Paddle'.");
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine();
                Debug.Log("Dato recibido: " + line);
                if (line.StartsWith("P:"))
                {
                    string valueStr = line.Substring(2).Trim();
                    int potValue;
                    if (int.TryParse(valueStr, out potValue))
                    {
                        float normalized = (float)potValue / 1023f;
                        float newX = Mathf.Lerp(minX, maxX, normalized);
                        if (paddle != null)
                        {
                            Vector3 pos = paddle.transform.position;
                            pos.x = newX;
                            paddle.transform.position = pos;
                            Debug.Log("Moviendo paddle a X = " + newX);
                        }
                        else
                        {
                            Debug.LogWarning("Paddle no asignado en el Inspector.");
                        }
                    }
                }
            }
            catch (System.TimeoutException)
            {
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error leyendo del serial: " + e.Message);
            }
        }
    }

    // M�todo para enviar mensajes al Arduino
    public void SendToArduino(string message)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.WriteLine(message);
            Debug.Log("Mensaje enviado a Arduino: " + message);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
