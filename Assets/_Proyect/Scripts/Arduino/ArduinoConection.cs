using UnityEngine;
using System.IO.Ports;
using System.Collections.Generic;

public class ArduinoConection : MonoBehaviour
{
    [Header("Configuración del Puerto Serial")]
    [SerializeField] private string selectedPort;
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private bool autoConnect = true;
    
    [Header("Estado")]
    [SerializeField] private bool isConnected = false;
    
    private SerialPort sp;
    private string[] availablePorts;
    
    void Update(){
        if (!isConnected){
            RefreshPortList();
            TryConnectToAnyAvailablePort();
        }
    }

    void Start()
    {
        RefreshPortList();
        
        if (autoConnect)
        {
            TryConnectToAnyAvailablePort();
        }
    }
    
    void OnEnable()
    {
        RefreshPortList();
    }
    
    public void RefreshPortList()
    {
        availablePorts = SerialPort.GetPortNames();
        string portsInfo = "Puertos disponibles: ";
        
        if (availablePorts.Length == 0)
        {
            portsInfo += "Ninguno";
        }
        else
        {
            portsInfo += string.Join(", ", availablePorts);
        }
        
        Debug.Log(portsInfo);
    }
    
    public void TryConnectToAnyAvailablePort()
    {
        if (availablePorts.Length == 0)
        {
            Debug.LogWarning("No hay puertos COM disponibles");
            return;
        }
        
        bool connected = false;
        
        foreach (string port in availablePorts)
        {
            try
            {
                Debug.Log("Intentando conectar al puerto " + port);
                
                if (sp != null && sp.IsOpen)
                {
                    sp.Close();
                }
                
                sp = new SerialPort(port, baudRate);
                sp.ReadTimeout = 50;
                sp.Open();
                
                selectedPort = port;
                isConnected = true;
                connected = true;
                Debug.Log("Conectado exitosamente al puerto " + port);
                break;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("No se pudo conectar al puerto " + port + ": " + e.Message);
            }
        }
        
        if (!connected)
        {
            Debug.LogError("No se pudo conectar a ningún puerto COM disponible");
        }
    }
    
    public void ConnectToPort(string portName)
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
            isConnected = false;
        }
        
        try
        {
            sp = new SerialPort(portName, baudRate);
            sp.ReadTimeout = 50;
            sp.Open();
            selectedPort = portName;
            isConnected = true;
            Debug.Log("Conectado exitosamente al puerto " + portName);
        }
        catch (System.Exception e)
        {
            isConnected = false;
            Debug.LogError("Error al conectar al puerto " + portName + ": " + e.Message);
        }
    }
    
    // Método para enviar comandos
    public void SendCommand(string comando)
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Write(comando);
            Debug.Log("Comando enviado: " + comando);
        }
        else
        {
            Debug.LogWarning("El puerto serial no está abierto.");
        }
    }
    
    public string ReadCommand()
    {
        if (sp != null && sp.IsOpen && sp.BytesToRead > 0)
        {
            try
            {
                return sp.ReadLine();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error al leer datos: " + e.Message);
            }
        }
        return null;
    }
    
    public string[] GetAvailablePorts()
    {
        return availablePorts;
    }
    
    public bool IsConnected()
    {
        return isConnected;
    }
    
    void OnApplicationQuit()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
            isConnected = false;
        }
    }
    
    [ContextMenu("RefreshPorts()")]
    void EditorRefreshPorts()
    {
        RefreshPortList();
    }
    
    [ContextMenu("ConnectToAvailablePorts()")]
    void EditorConnectToAvailablePort()
    {
        TryConnectToAnyAvailablePort();
    }
}