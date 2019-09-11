using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;



public class InputPortSerie : MonoBehaviour
{

    public static string COMPort = "COM4"; //Define the Comport Unity should listen to
    [SerializeField]
    private bool portOpen = false; //If the port is open this boolean is true

    private SerialPort serialPort = new SerialPort(COMPort, 9600);



    [SerializeField]
    private string incomingLineData = ""; //The Incoming data will be stored in this variable


    //Event
    public event EventHandler<OnButtonJumpEventArgs> onButtonJumpEvent;
    public event EventHandler<OnSlideEventArgs> onSlideEvent;




   

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

        if (portOpen && serialPort.IsOpen)
        {

            // RecieveInput();
            ParseIncomingData(incomingLineData);

        }
    }

    //invoke event jump
    private void OnButtonJumpEvent(OnButtonJumpEventArgs e)
    {
        onButtonJumpEvent?.Invoke(this, e);
    }

    //invoke event Slide
    private void OnSlideEvent(OnSlideEventArgs e)
    {

        onSlideEvent?.Invoke(this, e);
    }

    //Connection serial port
    public void OpenConnection()
    {
        if (serialPort != null)
        {
            Thread sampleThread = new Thread(new ThreadStart(RecieveInput));
            sampleThread.IsBackground = true;

            if (serialPort.IsOpen)
            {
                serialPort.Close();
                

                Debug.Log("Closing port as it was already open");
            }
            else
            {
                serialPort.Open();
                serialPort.ReadTimeout = 100;
                portOpen = true;
               
                // start thread
                sampleThread.Start();

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
        while (true)
        {
            if (serialPort.IsOpen)
            {


                try
                {


                    incomingLineData = serialPort.ReadLine();
                     ParseIncomingData(incomingLineData);



                }
                catch (Exception errorpiece)
                {

                    Debug.Log("Error 1: " + errorpiece);

                }

            }
            else
            {

                //something to do for the thread
            }
        }
    }

    private void ParseIncomingData(string incomingLineData)
    {
        string[] buffer;
        string[] tempBuffer;
        int valButton;
        bool is_Jump;
        int valSlider;
        bool is_Sliding;

        if (incomingLineData.Length > 0)
        {
            buffer = incomingLineData.Split(',');

                      
            tempBuffer = buffer[0].Split('=');
            is_Jump = int.TryParse(tempBuffer[1], out valButton);
            Debug.Log(valButton);

            tempBuffer = buffer[1].Split('=');
            is_Sliding = int.TryParse(tempBuffer[1], out valSlider);
            Debug.Log(valSlider);

            if (is_Jump) //jump
            {
                JumpPlayer(valButton);
            }

            if (is_Sliding) //move
                MoveSlide(valSlider);

        }
    }

    private void MoveSlide(int valSlider)
    {
        if (valSlider >= 0)
        {
            OnSlideEvent(new OnSlideEventArgs() { ValueSlide = valSlider }); 
        }
    }

    //Jump
    private void JumpPlayer(int jump)
    {
        if (jump == 1)
            OnButtonJumpEvent(new OnButtonJumpEventArgs() { Jump = true });

    }

    public class OnButtonJumpEventArgs : EventArgs
    {
        public bool Jump { get; set; }
    }
}

public class OnSlideEventArgs
{
    public int ValueSlide { get; set; }
}