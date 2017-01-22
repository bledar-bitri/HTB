using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using MapPoint;

namespace MapPointServer
{
    public class Server
    {
        public Server()
        {
            TcpListener serverSocket = new TcpListener(MapPointConstants.PORT);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                HandleClinet client = new HandleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }
    
    //Class to handle each client request separatly
    class HandleClinet
    {
        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(ProcessRequest);
            ctThread.Start();
        }
            
        private void ProcessRequest()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[MapPointConstants.BUFFER_SIZE];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;
            bool hasErrors = false;
            while ((!hasErrors))
            {
                NetworkStream networkStream = null;
                try
                {
                    requestCount = requestCount + 1;
                    networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf(MapPointConstants.REQUEST_END));

                    string command = dataFromClient.Substring(0, dataFromClient.IndexOf(MapPointConstants.COMMAND_END));
                    string response = "";
                    switch (command)
                    {
                        case MapPointConstants.COMMAND_GetCalculatedRoute:
                            response = GetCalculatedRoute(dataFromClient.Substring(dataFromClient.IndexOf(MapPointConstants.COMMAND_END) + 1));
                            Console.WriteLine(" >> RESPONSE: " + response);
                            break;
                        default:
                            Console.WriteLine(" >> UNKNOWN COMMAND: " + command);
                            response = "UNKNKOWN COMMAND";
                            break;
                    }
                    Console.WriteLine(" >> From client- " + clNo + dataFromClient);

                    rCount = Convert.ToString(requestCount);
                    serverResponse = "Server to clinet( " + response + " ) " + rCount;
                    sendBytes = Encoding.ASCII.GetBytes(response);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex)
                {
                    hasErrors = true;
                    Console.WriteLine(" >> " + ex.ToString());
                    if (networkStream != null)
                    {
                        try
                        {
                            serverResponse = "ERROR" + MapPointConstants.DELIM.ToString() + ex.ToString();
                            sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                            networkStream.Flush();
                        }
                        catch(Exception exx) {
                            Console.WriteLine("ERROR 2: " + exx.ToString());
                        }
                    }
                }
            }
        }

        private string GetCalculatedRoute(string data)
        {
            string[] addresses = data.Split(MapPointConstants.DELIM);
            ArrayList nonEmptyAddresses = new ArrayList();
            
            for (int i = 0; i < addresses.Count(); i++)
                if (addresses[i].Trim() != string.Empty)
                    nonEmptyAddresses.Add(addresses[i]);
            

            ArrayList addrList = new ArrayList();
            for (int i = 1; i < nonEmptyAddresses.Count - 1; i++)
            {
                addrList.Add(nonEmptyAddresses[i]);
            }
            MapPointManager.GetCalculatedRoute((string)nonEmptyAddresses[0], (string)nonEmptyAddresses[nonEmptyAddresses.Count - 1], addrList);
            Console.WriteLine(data);
            return "got here";

        }
    }
}
