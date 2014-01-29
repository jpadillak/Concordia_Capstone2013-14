﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MarsRover
{
    public class UDPListener
    {
        public delegate void ReceivedDataCBType(int NbAvailableData);
        private event ReceivedDataCBType ReceivedDataCB;

        #region Attributes
        
       // private Logger logger;
        private int port;
        private UdpClient listener;
        private IPEndPoint groupEP;
      
        private ConcurrentQueue<byte> ReceivingQueue;
        private Thread ListeningThread;
        private int TotalNbDataReceived;
        #endregion


        public UDPListener(int aPort, ReceivedDataCBType aReceivedDataCB)
        {
            port = aPort;
            
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);

            //logger = NLog.LogManager.GetCurrentClassLogger();

            ReceivedDataCB += aReceivedDataCB;

            ReceivingQueue = new ConcurrentQueue<byte>();
            ListeningThread = new Thread(StartListening);

            ListeningThread.IsBackground = false;
            ListeningThread.Priority = ThreadPriority.Highest;
            ListeningThread.Start();
        }

        public int GetTotalNbDataINOUT()
        {
            return TotalNbDataReceived;
        }

        public int GetNumberOfReceivedData()
        {
            return ReceivingQueue.Count;

        }
        
        public int  ReceiveData( ref byte[] data, int size )
        {
            int NbOfDataToRead = Math.Min(size, ReceivingQueue.Count);
          
            if (NbOfDataToRead > 0)
            {
                for (int i = 0; i < NbOfDataToRead; i++)
                {
                    ReceivingQueue.TryDequeue(out data[i]);
                }
            }

            return NbOfDataToRead;
          
        }

        private void StartListening()
        {
          
            try
            {
                while (true)
                {
                    
                    //BLOCKING CALL                    
                    byte[] bytes = listener.Receive(ref groupEP); 
                
                    TotalNbDataReceived += bytes.Length;
                   
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        ReceivingQueue.Enqueue(bytes[i]);                        
                    }
                   
                    if(ReceivedDataCB != null)
                    {
                        ReceivedDataCB(ReceivingQueue.Count);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
