using UnityEngine;
using System.IO.Ports;
using System.Collections.Generic;

public class ArduinoConection : Singleton<ArduinoConection>
{
    [Header("Configuración del Puerto Serial")]
    [SerializeField] private string selectedPort;
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private bool autoConnect = true;
    [SerializeField] private int readTimeout = 50;

    [Header("Estado")]
    [SerializeField] private bool isConnected = false;

    private SerialPort sp;
    private string[] availablePorts;
    private float reconnectInterval = 2f;
    private float lastReconnectAttempt = 0f;

    void OnEnable(){
        RefreshPortList();
        InitializeConnection();
    }

    void Update(){
        if (isConnected && !CheckConnectivity()){
            Debug.Log("Se perdió la conexión. Preparando reconexión...");
        }
        
        if (!isConnected && autoConnect && Time.time > lastReconnectAttempt + reconnectInterval){
            lastReconnectAttempt = Time.time;
            InitializeConnection();
        }
    }

    private void InitializeConnection(){
        RefreshPortList();
        if (autoConnect && availablePorts.Length > 0){
            TryConnectToAnyAvailablePort();
        }
    }

    public bool CheckConnectivity(){
        if (sp == null){
            isConnected = false;
            return false;
        }

        try{
            if (!sp.IsOpen){
                isConnected = false;
                return false;
            }

            sp.Write(new byte[] { 0 }, 0, 0);

            return true;
        }
        catch (System.IO.IOException){
            Debug.LogWarning("Dispositivo físicamente desconectado");
            ClosePortSafely();
            isConnected = false;
            return false;
        }
        catch (System.Exception e){
            Debug.LogError("Error de conectividad: " + e.Message);
            ClosePortSafely();
            isConnected = false;
            return false;
        }
    }

    private void ClosePortSafely(){
        if (sp != null && sp.IsOpen){
            try{
                sp.Close();
            }
            catch (System.Exception e){
                Debug.LogWarning("Error al cerrar el puerto: " + e.Message);
            }
        }
    }
    

    public void RefreshPortList(){
        availablePorts = SerialPort.GetPortNames();
        
        if (Debug.isDebugBuild){
            string portsInfo = "Puertos disponibles: " + (availablePorts.Length == 0 ? "Ninguno" : string.Join(", ", availablePorts));
            Debug.Log(portsInfo);
        }
    }

    public void TryConnectToAnyAvailablePort(){
        if (availablePorts.Length == 0){
            Debug.LogWarning("No hay puertos COM disponibles");
            return;
        }

        ClosePortSafely();
        isConnected = false;

        foreach (string port in availablePorts){
            if (ConnectToPortInternal(port)){
                return;
            }
        }

        Debug.LogError("No se pudo conectar a ningún puerto COM disponible");
    }

    public void ConnectToPort(string portName){
        ClosePortSafely();
        isConnected = false;
        
        ConnectToPortInternal(portName);
    }

    private bool ConnectToPortInternal(string portName){
        try{
            Debug.Log("Intentando conectar al puerto " + portName);
            
            sp = new SerialPort(portName, baudRate);
            sp.ReadTimeout = readTimeout;

            sp.Open();
            selectedPort = portName;
            isConnected = true;
            
            Debug.Log("Conectado exitosamente al puerto " + portName);
            
            return true;
        }
        catch (System.Exception e){
            Debug.LogWarning("No se pudo conectar al puerto " + portName + ": " + e.Message);
            return false;
        }
    }

    public void SendCommand(string comando){
        if (CheckConnectivity()){
            try{
                sp.Write(comando);
                Debug.Log("Comando enviado: " + comando);
            }
            catch (System.Exception e){
                Debug.LogError("Error al enviar comando: " + e.Message);
                isConnected = false;
            }
        }
        else{
            Debug.LogWarning("No se puede enviar el comando. Puerto serial no disponible.");
        }
    }

    public string ReadCommand(){
        if (CheckConnectivity() && sp.BytesToRead > 0){
            try{
                return sp.ReadLine();
            }
            catch (System.Exception e){
                Debug.LogWarning("Error al leer datos: " + e.Message);
                return null;
            }
        }
        return null;
    }

    public string[] GetAvailablePorts(){
        return availablePorts;
    }

    public bool IsConnected(){
        return isConnected && CheckConnectivity();
    }

    public string GetSelectedPort(){
        return selectedPort;
    }

    void OnApplicationQuit(){
        ClosePortSafely();
        isConnected = false;
    }
}