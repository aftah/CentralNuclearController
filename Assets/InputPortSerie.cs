using System;
using System.IO.Ports;
using UnityEngine;



public class InputPortSerie : MonoBehaviour
{
    [SerializeField]
    private static string COMPort = "COM4"; //Define the Comport Unity should listen to
    [SerializeField]
    private bool portOpen = false; //If the port is open this boolean is true

    private SerialPort serialPort = new SerialPort(COMPort, 9600);

   
    [SerializeField]
    private string incomingLineData = ""; //The Incoming data will be stored in this variable
   


    public event EventHandler<OnButtonJumpEventArgs> onButtonJumpEvent;

   

    private void OnButtonJumpEvent(OnButtonJumpEventArgs e)
    {
        onButtonJumpEvent?.Invoke(this, e); 
    }



    // Use this for initialization
    void Start()
    {
        OpenConnection(); // Open the connection
        if (serialPort.IsOpen)
        {
            Debug.Log("I'm open");
        }
        else
        {
            Debug.Log("Please Conected....");
        }
    }

    // Update is called once per frame
    void Update()
    {
       // updateSlower++; //Recieve data every 4 frames
        if (portOpen && serialPort.IsOpen) //&& updateSlower >= updateInterval
        { //
             
            RecieveInput();
           // updateSlower = 0;
        }
    }

    public void OpenConnection()
    {
        if (serialPort != null)
        { 
            if (serialPort.IsOpen)
            { 
                serialPort.Close();

                Debug.Log("Closing port as it was already open");
            }
            else
            {
                serialPort.Open(); 
                serialPort.ReadTimeout = 1; 
                portOpen = true;

            }
        }
        else
        { //errorcodes
            if (serialPort.IsOpen)
            {
                Debug.Log("Port is already open");
            }
            else
            {
                Debug.Log("Port == Null");
            }
        }
    }

    void RecieveInput()
    {
        try
        {
            

           incomingLineData = serialPort.ReadLine();
          //  int jumpData = int.Parse(incomingLineData);
           
            //test button jump
            JumpPlayer(incomingLineData);


            //serialPort.DataReceived += (s, e) =>
            //{
            //    var c = serialPort.ReadExisting();
            //   Debug.Log(c);
            //};

           
          
        }
        catch (Exception errorpiece)
        {
           
                Debug.Log("Error 1: " + errorpiece);
           
        }


    }

    private void JumpPlayer(string jumpData)
    {
        if (jumpData == "SPACE")
        {
            OnButtonJumpEvent(new OnButtonJumpEventArgs() { Jump = true });
        }
    }

    public class OnButtonJumpEventArgs : EventArgs 
    {
        public bool Jump { get; set; }
    }
}

