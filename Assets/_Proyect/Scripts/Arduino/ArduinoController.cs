using UnityEngine;

[CreateAssetMenu(fileName = "ArduinoController", menuName = "Controllers/Arduino")]
public class ArduinoController : ScriptableObject, IPaddleController
{
    public bool GetLaunchInput()
    {
        throw new System.NotImplementedException();
    }

    public float GetMovementInput()
    {
        throw new System.NotImplementedException();
    }

    public void Movement(GameObject Paddle, float leftBoundary, float rightBoundary)
    {
        Debug.Log("MOVIENDO");
        if (ArduinoConection.Instance.IsConnected())
        {
            try
            {
                string line = ArduinoConection.Instance.ReadCommand();
                Debug.Log("Dato recibido: " + line);
                if (line.StartsWith("P:"))
                {
                    string valueStr = line.Substring(2).Trim();
                    int potValue;
                    if (int.TryParse(valueStr, out potValue))
                    {
                        float normalized = (float)potValue / 1023f;
                        float newX = Mathf.Lerp(leftBoundary, rightBoundary, normalized);
                        if (Paddle != null)
                        {
                            Vector3 pos = Paddle.transform.position;
                            pos.x = newX;
                            Paddle.transform.position = pos;
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

    public void SetSpeed(float value)
    {
        throw new System.NotImplementedException();
    }
}
